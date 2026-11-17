using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GeoSensePlus.Data.DatabaseModels;

public class BaseEntity<T>
{
    [Column("id")]
    public T Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;   // if don't want to cascade delete all children, just set this to true
}

/// <summary>
/// Base EF Core configuration for entities inheriting <see cref="BaseEntity{T}"/>.
/// Column mappings come from the [Column] attributes on BaseEntity by convention;
/// derived configurations add entity-specific mappings (table name, indexes, relationships).
/// </summary>
public abstract class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        string TableName = typeof(TEntity).Name.ToSnakeCase();
        builder.HasKey(x => x.Id).HasName($"pk_{TableName}");   // PostgreSQL needs unique constraint name for primary key in the same schema

        //builder.HasIndex(x => x.Name).HasDatabaseName($"idx_{TableName}_name");
        //builder.HasIndex(x => x.UpdatedAt).HasDatabaseName($"idx_{TableName}_updated_at");
    }
}

public static class StringExtensions
{
    public static string ToSnakeCase(this string s) =>
        string.Concat(s.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + char.ToLowerInvariant(c) : char.ToLowerInvariant(c).ToString()));
}