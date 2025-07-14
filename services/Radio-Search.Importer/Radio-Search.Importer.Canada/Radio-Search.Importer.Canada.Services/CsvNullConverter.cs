using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Radio_Search.Importer.Canada.Services
{
    public class NullConverter<T> : ITypeConverter
    {
        private readonly ITypeConverter _inner;
        private static char[] _nullChars = ['-', '.'];

        public NullConverter(TypeConverterCache cache)
        {
            _inner = cache.GetConverter(typeof(T));
        }

        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text) || (text.Length == 1 && _nullChars.Contains(text[0])))
                return null;

            return _inner.ConvertFromString(text, row, memberMapData);
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return string.Empty;

            return _inner.ConvertToString(value, row, memberMapData);
        }
    }
}
