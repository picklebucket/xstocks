using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace Stocks.Pipelines.Models
{
    public class PipelineSettings
    {
        public string ConfigFilePath { get; set; }

        private PipelineInfoProviderType _configFileType;
        public PipelineInfoProviderType ConfigFileType 
        { 
            get 
            {
                if(_configFileType == PipelineInfoProviderType.Unknown && !string.IsNullOrEmpty(ConfigFilePath))
                {
                    string fileExtension = Path.GetExtension(ConfigFilePath).TrimStart('.');
                    _configFileType = PipelineInfoProviderTypeConverter.ConvertFrom(fileExtension);
                }
                return _configFileType; 
            } 
            set { _configFileType = value; } 
        }
    }
}
