using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Stocks.Ameritrade.Tests
{
    public static class GenericExtensions
    {
        public static string ToJson<T>(this T instance)
        {
            return JsonSerializer.Serialize(instance, new JsonSerializerOptions() { WriteIndented = true});
        }
    }
}
