using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Stocks.Pipelines.Providers
{
    class XmlPipelineInfoProvider : IPipelineInfoProvider
    {
        private readonly string _configPath;

        public XmlPipelineInfoProvider(string configPath)
        {
            _configPath = configPath;
        }

        public IEnumerable<ProcessorInfo> GetProcessors(string pipelineName)
        {
            var pipelineNode = GetPipelineNode(pipelineName);
            XmlNodeList processorsNodes = pipelineNode.SelectNodes("processor");
            var processors = new List<ProcessorInfo>();
            foreach (XmlNode processorNode in processorsNodes)
            {
                var typeAttribute = processorNode.Attributes["type"];
                if (typeAttribute == null)
                {
                    throw new Exception("'processor' XML element doesn't have 'type' attribute");
                }

                string typeName = typeAttribute.Value;
                var processorType = Type.GetType(typeName) ?? throw new Exception($"'processor' XML element contains invalid type name in 'type' attribute: {typeName}");
                processors.Add(new ProcessorInfo(processorType));
            }
            return processors;
        }

        private XmlNode GetPipelineNode(string nodeName)
        {
            var doc = new XmlDocument();
            doc.Load(_configPath);
            string pipelineXpath = $"/pipelines/{nodeName}";
            var pipelineNode = doc.SelectSingleNode(pipelineXpath);
            if (pipelineNode == null)
            {
                throw new Exception($"File {_configPath} doesn't have {pipelineXpath} section");
            }
            return pipelineNode;
        }
    }
}
