using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Application.Common.Utilities
{
    public static class CsvReaderUtility<T, TMap>
       where TMap : ClassMap<T>
    {
        public static List<T> ReadCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8")))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                // Apply the mapping
                csv.Context.RegisterClassMap<TMap>();
                return csv.GetRecords<T>().ToList();
            }
        }
    }
}
