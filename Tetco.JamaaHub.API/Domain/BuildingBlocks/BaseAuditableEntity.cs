namespace Domain.BuildingBlocks;

public abstract class BaseAuditableEntity : BaseEntity, IBaseAuditableEntity
   {
   public DateTimeOffset Created
      {
      get; set;
      }

   public string? CreatedBy
      {
      get; set;
      }

   public DateTimeOffset? LastModified
      {
      get; set;
      }

   public string? LastModifiedBy
      {
      get; set;
      }
   }

public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IBaseAuditableEntity<TKey>
   {
   protected BaseAuditableEntity ( )
      {
      }

   protected BaseAuditableEntity ( TKey id ) : base ( id )
      {
      }

   public DateTimeOffset Created
      {
      get; set;
      }

   public string? CreatedBy
      {
      get; set;
      }

   public DateTimeOffset? LastModified
      {
      get; set;
      }

   public string? LastModifiedBy
      {
      get; set;
      }
   }
