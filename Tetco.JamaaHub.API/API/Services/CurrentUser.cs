using Domain.BuildingBlocks;
using System.Security.Claims;

namespace API.Services;

public class CurrentUser : IUser
   {
   private readonly IHttpContextAccessor _httpContextAccessor;

   public CurrentUser ( IHttpContextAccessor httpContextAccessor )
      {
      _httpContextAccessor = httpContextAccessor;
      }

   public string Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue ( ClaimTypes.NameIdentifier );

   public string UserName
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string NormalizedUserName
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string Email
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string NormalizedEmail
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public bool EmailConfirmed
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string PasswordHash
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string SecurityStamp
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string ConcurrencyStamp
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public string PhoneNumber
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public bool PhoneNumberConfirmed
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public bool TwoFactorEnabled
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public DateTimeOffset? LockoutEnd
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public bool LockoutEnabled
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
   public int AccessFailedCount
      {
      get => throw new NotImplementedException ( );
      set => throw new NotImplementedException ( );
      }
    string IUser<string>.Id { get => Id; set { } }
}
