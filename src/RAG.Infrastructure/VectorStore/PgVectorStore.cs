using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using Pgvector;
using RAG.Application.Interfaces;
using RAG.Domain.Entities;

namespace RAG.Infrastructure.VectorStore;

public class PgVectorStore : IVectorStore
{
    private readonly PgVectorSettings _settings;
    private readonly ILogger<PgVectorStore> _logger;

    public PgVectorStore(IOptions<PgVectorSettings> settings, ILogger<PgVectorStore> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task StoreChunksAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken = default)
    {
        await using var connection = await CreateConnectionAsync(cancellationToken);

        foreach (var chunk in chunks)
        {
            await using var cmd = new NpgsqlCommand(@$"
                INSERT INTO {_settings.TableName} (id, tenant_id, document_id, content, embedding, chunk_index, metadata, created_at)
                VALUES (@id, @tenant_id, @document_id, @content, @embedding, @chunk_index, @metadata::jsonb, @created_at)
                ON CONFLICT (id) DO UPDATE SET
                    content = EXCLUDED.content,
                    embedding = EXCLUDED.embedding,
                    metadata = EXCLUDED.metadata", connection);

            cmd.Parameters.AddWithValue("id", chunk.Id);
            cmd.Parameters.AddWithValue("tenant_id", chunk.TenantId);
            cmd.Parameters.AddWithValue("document_id", chunk.DocumentId);
            cmd.Parameters.AddWithValue("content", chunk.Content);
            cmd.Parameters.AddWithValue("embedding", new Vector(chunk.Embedding));
            cmd.Parameters.AddWithValue("chunk_index", chunk.ChunkIndex);
            cmd.Parameters.AddWithValue("metadata", JsonSerializer.Serialize(chunk.Metadata));
            cmd.Parameters.AddWithValue("created_at", chunk.CreatedAt);

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        _logger.LogInformation("Stored {Count} chunks.", chunks.Count());
    }

    public async Task<IReadOnlyList<DocumentChunk>> SearchAsync(string tenantId, float[] queryEmbedding, int topK = 5, CancellationToken cancellationToken = default)
    {
        await using var connection = await CreateConnectionAsync(cancellationToken);

        await using var cmd = new NpgsqlCommand(@$"
            SELECT id, tenant_id, document_id, content, embedding, chunk_index, metadata, created_at
            FROM {_settings.TableName}
            WHERE tenant_id = @tenant_id
            ORDER BY embedding <=> @query_embedding
            LIMIT @top_k", connection);

        cmd.Parameters.AddWithValue("tenant_id", tenantId);
        cmd.Parameters.AddWithValue("query_embedding", new Vector(queryEmbedding));
        cmd.Parameters.AddWithValue("top_k", topK);

        var results = new List<DocumentChunk>();

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new DocumentChunk
            {
                Id = reader.GetGuid(0),
                TenantId = reader.GetString(1),
                DocumentId = reader.GetString(2),
                Content = reader.GetString(3),
                Embedding = reader.GetFieldValue<Vector>(4).ToArray(),
                ChunkIndex = reader.GetInt32(5),
                Metadata = JsonSerializer.Deserialize<Dictionary<string, string>>(reader.GetString(6)) ?? new(),
                CreatedAt = reader.GetDateTime(7)
            });
        }

        _logger.LogDebug("Found {Count} chunks for tenant {TenantId}.", results.Count, tenantId);
        return results;
    }

    public async Task DeleteByDocumentIdAsync(string tenantId, string documentId, CancellationToken cancellationToken = default)
    {
        await using var connection = await CreateConnectionAsync(cancellationToken);

        await using var cmd = new NpgsqlCommand(@$"
            DELETE FROM {_settings.TableName}
            WHERE tenant_id = @tenant_id AND document_id = @document_id", connection);

        cmd.Parameters.AddWithValue("tenant_id", tenantId);
        cmd.Parameters.AddWithValue("document_id", documentId);

        var deleted = await cmd.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Deleted {Count} chunks for document {DocumentId} in tenant {TenantId}.", deleted, documentId, tenantId);
    }

    private async Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken cancellationToken)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_settings.ConnectionString);
        dataSourceBuilder.UseVector();
        await using var dataSource = dataSourceBuilder.Build();

        var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        return connection;
    }
}
