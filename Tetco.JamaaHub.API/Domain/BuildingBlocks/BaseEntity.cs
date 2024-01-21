using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.BuildingBlocks;

public abstract class BaseEntity : IBaseEntity
{
    private readonly List<BaseEvent> _domainEvents = new();

    protected BaseEntity()
    {
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {string.Join(", ", GetKeys())}";
    }

    public abstract object[] GetKeys();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    //public bool EntityEquals(IBaseEntity other)
    //{
    //    return EntityHelper.EntityEquals(this, other);
    //}
}


public abstract class BaseEntity<TKey> : BaseEntity, IBaseEntity<TKey>
{
    public virtual TKey Id { get; protected set; }

    protected BaseEntity()
    {
    }

    protected BaseEntity(TKey id)
    {
        Id = id;
    }

    public override object[] GetKeys()
    {
        return new object[] { Id };
    }

    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }


}

