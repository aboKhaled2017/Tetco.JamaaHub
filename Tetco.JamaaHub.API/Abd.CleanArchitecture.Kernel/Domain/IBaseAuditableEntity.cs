namespace Abd.CleanArchitecture.Kernel.Domain;

public interface IBaseAuditableEntity : IBaseEntity
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

public interface IBaseAuditableEntity<TKey> : IBaseAuditableEntity, IBaseEntity<TKey>
   {

   }