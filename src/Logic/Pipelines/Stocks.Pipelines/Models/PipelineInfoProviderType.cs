using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Stocks.Pipelines.Models
{
    [TypeConverter(typeof(PipelineInfoProviderTypeConverterAttribute))]
    public enum PipelineInfoProviderType
    {
        Unknown,
        Xml
    }

    public class PipelineInfoProviderTypeConverterAttribute : StringConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string type = (string)value;
            return PipelineInfoProviderTypeConverter.ConvertFrom(type);
        }
    }

    public static class PipelineInfoProviderTypeConverter
    {
        public static PipelineInfoProviderType ConvertFrom(string value)
        {
            return (value.ToLower()) switch
            {
                "xml" => PipelineInfoProviderType.Xml,
                _ => PipelineInfoProviderType.Unknown,
            };
        }
    }
}
