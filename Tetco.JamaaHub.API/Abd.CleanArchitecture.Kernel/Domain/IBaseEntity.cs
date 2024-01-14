namespace Abd.CleanArchitecture.Kernel.Domain;

/// <summary>
/// Defines an entity. It's primary key may not be "Id" or it may have a composite primary key.
/// Use <see cref="IBaseEntity{TKey}"/> where possible for better integration to repositories and other structures in the framework.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Returns an array of ordered keys for this entity.
    /// </summary>
    /// <returns></returns>
    object[] GetKeys();

    IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    void AddDomainEvent(BaseEvent domainEvent);

    void RemoveDomainEvent(BaseEvent domainEvent);

    void ClearDomainEvents();
}

/// <summary>
/// Defines an entity with a single primary key with "Id" property.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
public interface IBaseEntity<TKey> : IBaseEntity
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>
    TKey Id { get; }
}
