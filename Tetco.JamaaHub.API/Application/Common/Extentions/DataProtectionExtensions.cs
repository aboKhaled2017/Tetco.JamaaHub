using Microsoft.AspNetCore.DataProtection;
using System.Text;
using System.Text.Json;

namespace Application.Common.Extentions
{
    public static class DataProtectionExtensions
    {
        public static string Hash(this IDataProtector protector,string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);

            return Encoding.ASCII.GetString(protector.Protect(bytes));
        }
        public static string Hash(this IDataProtector protector, object obj)
        {
            var bytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(obj));

            return Encoding.ASCII.GetString(protector.Protect(bytes));
        }
    }
}
