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

        public IEnumerable<ProcessorInfo> GetAllProcessors()
        {
            var processors = new List<ProcessorInfo>();
            foreach (XmlNode pipelineNode in GetAllPipelines())
            {
                processors.AddRange(GetProcessors(pipelineNode.Name));
            }
            return processors;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
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

                var asyncAttribute = processorNode.Attributes["async"];
                bool async = false;
                if(asyncAttribute != null)
                {
                    if(!bool.TryParse(asyncAttribute.Value, out async))
                    {
                        throw new ArgumentException($"Cannot parse 'async' attribute value of '{typeName}' processor");
                    }
                }

                var resolveAttribute = processorNode.Attributes["resolve"];
                bool resolve = false;
                if(resolveAttribute != null)
                {
                    if(!bool.TryParse(resolveAttribute.Value, out resolve))
                    {
                        throw new ArgumentException($"Cannot parse 'resolve' attribute value of '{typeName}' processor");
                    }
                }
                processors.Add(new ProcessorInfo(processorType, async, resolve));
            }
            return processors;
        }

        private XmlNodeList GetAllPipelines()
        {
            string pipelinesRootXpath = $"/pipelines";
            var pipelinesRoot = GetNode(pipelinesRootXpath);
            return pipelinesRoot.ChildNodes;
        }

        private XmlNode GetPipelineNode(string nodeName)
        {
            string pipelineXpath = $"/pipelines/{nodeName}";
            return GetNode(pipelineXpath);
        }

        private XmlNode GetNode(string xpath)
        {
            var doc = new XmlDocument();
            doc.Load(_configPath);
            var node = doc.SelectSingleNode(xpath);
            if (node == null)
            {
                throw new Exception($"File {_configPath} doesn't have {xpath} section");
            }
            return node;
        }
    }
}
