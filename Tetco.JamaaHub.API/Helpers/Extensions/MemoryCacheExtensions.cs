using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Helpers.Extensions;


//https://stackoverflow.com/questions/45597057/how-to-retrieve-a-list-of-memory-cache-keys-in-asp-net-core
public static class MemoryCacheExtensions
   {



   private static readonly Lazy<Func<MemoryCache , object>> GetCoherentState =
       new Lazy<Func<MemoryCache , object>> ( ( ) =>
           CreateGetter<MemoryCache , object> ( typeof ( MemoryCache )
               .GetField ( "_coherentState" , BindingFlags.NonPublic | BindingFlags.Instance ) ) );

   private static readonly Lazy<Func<object , IDictionary>> GetEntries7 =
       new Lazy<Func<object , IDictionary>> ( ( ) =>
           CreateGetter<object , IDictionary> ( typeof ( MemoryCache )
               .GetNestedType ( "CoherentState" , BindingFlags.NonPublic )
               .GetField ( "_entries" , BindingFlags.NonPublic | BindingFlags.Instance ) ) );

   private static Func<TParam , TReturn> CreateGetter<TParam, TReturn> ( FieldInfo field )
      {
      var methodName = $"{field.ReflectedType.FullName}.get_{field.Name}";
      var method = new DynamicMethod ( methodName , typeof ( TReturn ) , new [] { typeof ( TParam ) } , typeof ( TParam ) , true );
      var ilGen = method.GetILGenerator ( );
      ilGen.Emit ( OpCodes.Ldarg_0 );
      ilGen.Emit ( OpCodes.Ldfld , field );
      ilGen.Emit ( OpCodes.Ret );
      return ( Func<TParam , TReturn> ) method.CreateDelegate ( typeof ( Func<TParam , TReturn> ) );
      }


   private static readonly Func<MemoryCache , IDictionary> GetEntries = cache => GetEntries7.Value ( GetCoherentState.Value ( cache ) );

   public static ICollection GetKeys ( this IMemoryCache memoryCache ) =>
       GetEntries ( ( MemoryCache ) memoryCache ).Keys;

   public static IEnumerable<T> GetKeys<T> ( this IMemoryCache memoryCache ) =>
       memoryCache.GetKeys ( ).OfType<T> ( );
   }
