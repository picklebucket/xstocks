using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Stocks.Common.Tests
{
    public static class GenericExtensions
    {
        public static string ToPrettyJson<T>(this T instance)
        {
            return JsonSerializer.Serialize(instance, new JsonSerializerOptions() { WriteIndented = true });
        }
    }
}
