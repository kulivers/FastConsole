using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

namespace Comindware.Configs.Core
{
    internal static class YamlStreamConverter
    {
        private const char AttributesSeparatorChar = '.';

        public static IEnumerable<ParsingEvent> ConvertFromDotMapping(YamlStream stream)
        {
            yield return new StreamStart();
            foreach (var document in stream.Documents)
            {
                foreach (var evt in ConvertDocument(document))
                {
                    yield return evt;
                }
            }

            yield return new StreamEnd();
        }

        private static IEnumerable<ParsingEvent> ConvertDocument(YamlDocument document)
        {
            yield return new DocumentStart();
            foreach (var evt in ConvertNode(document.RootNode))
            {
                yield return evt;
            }

            yield return new DocumentEnd(false);
        }

        private static IEnumerable<ParsingEvent> ConvertNode(YamlNode node)
        {
            if (node is YamlScalarNode scalar)
            {
                return ConvertScalarNode(scalar);
            }

            if (node is YamlSequenceNode sequence)
            {
                return ConvertSequenceNode(sequence);
            }

            if (node is YamlMappingNode mapping)
            {
                var newMap = RebuildMappingNode(mapping);
                return ConvertMappingNode(newMap);
            }

            throw new NotSupportedException(string.Format("Unsupported node type: {0}", node.GetType().Name));
        }

        private static IEnumerable<ParsingEvent> ConvertScalarNode(YamlScalarNode scalar)
        {
            yield return new Scalar(scalar.Anchor, scalar.Tag, scalar.Value, scalar.Style, false, false);
        }

        private static IEnumerable<ParsingEvent> ConvertSequenceNode(YamlSequenceNode sequence)
        {
            yield return new SequenceStart(sequence.Anchor, sequence.Tag, false, sequence.Style);
            foreach (var node in sequence.Children)
            {
                foreach (var evt in ConvertNode(node))
                {
                    yield return evt;
                }
            }

            yield return new SequenceEnd();
        }

        private static IEnumerable<ParsingEvent> ConvertMappingNode(YamlMappingNode mapping)
        {
            yield return new MappingStart(mapping.Anchor, mapping.Tag, false, mapping.Style);
            foreach (var pair in mapping.Children)
            {
                foreach (var evt in ConvertNode(pair.Key))
                {
                    yield return evt;
                }
                foreach (var evt in ConvertNode(pair.Value))
                {
                    yield return evt;
                }
            }

            yield return new MappingEnd();
        }

        private static YamlMappingNode RebuildMappingNode(YamlMappingNode mapping)
        {
            var newMap = new YamlMappingNode();
            foreach (var pair in mapping.Children)
            {
                if (pair.Key is YamlScalarNode scalarNode && scalarNode.Value.Contains(AttributesSeparatorChar))
                {
                    ConvertDotMapping(newMap, scalarNode, pair.Value);
                }
                else
                {
                    if (!newMap.Children.ContainsKey(pair.Key))
                    {
                        newMap.Add(pair.Key, pair.Value);
                    }
                }
            }

            return newMap;
        }

        private static void ConvertDotMapping(YamlMappingNode mapNode, YamlScalarNode key, YamlNode value)
        {
            var propertyNames = key.Value.Split(AttributesSeparatorChar).Select(s => new YamlScalarNode(s)).ToArray();
            var mappingNode = mapNode;
            for (int i = 0; i < propertyNames.Length; i++)
            {
                var propertyName = propertyNames[i];
                if (i == propertyNames.Length - 1)
                {
                    mappingNode.Children[propertyName] = value;
                }
                else
                {
                    mappingNode = mappingNode.GetOrAdd(propertyName);
                }
            }
        }
    }
}

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
