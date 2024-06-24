using YamlDotNet.RepresentationModel;

namespace Comindware.Bootloading.Core.Configuration.Utils
{
    internal static class YamlMappingNodeExtension
    {
        public static YamlMappingNode GetOrAdd(this YamlMappingNode node, YamlNode key)
        {
            var mappingNode = default(YamlMappingNode);
            if (node.Children.TryGetValue(key, out var value))
            {
                if (value is YamlMappingNode valueMappingNode)
                {
                    mappingNode = valueMappingNode;
                }
            }
            else
            {
                mappingNode = new YamlMappingNode();
                node.Add(key, mappingNode);
            }

            return mappingNode;
        }
    }
}
