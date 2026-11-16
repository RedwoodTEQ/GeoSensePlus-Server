using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

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

    //[Column("is_deleted")]
    //public bool IsDeleted { get; set; } = false;   // if don't want to cascade delete all children, just set this to true
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
        builder.HasKey(x => x.Id).HasName("pk_id");
    }
}