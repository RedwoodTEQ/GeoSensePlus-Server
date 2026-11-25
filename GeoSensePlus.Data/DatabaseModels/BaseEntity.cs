using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoSensePlus.Data.DatabaseModels;

/// <summary>
/// Non-generic contract implemented by all <see cref="BaseEntity{T}"/>
/// entities, so the DbContext can apply timestamp auditing regardless of the
/// entity's key type.
/// 
/// <see cref="CreatedAt"/> is populated on insert and <see cref="UpdatedAt"/>
/// refreshed on update by <see cref="DbContexts.ApplicationDbContext.SaveChanges(bool)"/>
/// (all sync/async overloads route through it).
///
/// All values are UTC: written from <c>DateTimeOffset.UtcNow</c> and stored as
/// PostgreSQL <c>timestamptz</c>, which normalizes to a UTC instant and reads
/// back with a +00:00 offset. Converting to local time is the consumer's
/// responsibility.
/// </summary>
public interface IBaseEntity
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
}

public class BaseEntity<T> : IBaseEntity
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
