// using YamlDotNet.Core;
// using YamlDotNet.Core.Events;
// using YamlDotNet.RepresentationModel;
//
// public class YamlCommentPreserver
// {
//     public static void Main2(string[] args)
//     {
//         string inputFilePath = "input.yaml";
//         string outputFilePath = "output.yaml";
//
//         var comments = new Dictionary<string, string>();
//         var yamlStream = new YamlStream();
//
//         // Load YAML file and preserve comments
//         using (var input = new StreamReader(inputFilePath))
//         {
//             var parser = new Parser(input);
//             var currentKey = string.Empty;
//
//             while (parser.MoveNext())
//             {
//                 var evt = parser.Current;
//
//                 if (evt is Comment comment)
//                 {
//                     if (!string.IsNullOrEmpty(currentKey))
//                     {
//                         comments[currentKey] = comment.Value;
//                     }
//                 }
//                 else if (evt is Scalar scalar)
//                 {
//                     currentKey = scalar.Value;
//                 }
//
//                 yamlStream.Load(parser);
//             }
//         }
//
//         // Modify the YAML model
//         var root = (YamlMappingNode)yamlStream.Documents[0].RootNode;
//         root.Children[new YamlScalarNode("key1")] = new YamlScalarNode("new_value1");
//         root.Children[new YamlScalarNode("key2")] = new YamlScalarNode("new_value2");
//
//         // Write the modified YAML back to a file while preserving comments
//         using (var output = new StreamWriter(outputFilePath))
//         {
//             var emitter = new Emitter(output);
//
//             foreach (var document in yamlStream.Documents)
//             {
//                 emitter.Emit(new DocumentStart());
//                 WriteNode(emitter, document.RootNode, comments);
//                 emitter.Emit(new DocumentEnd(true));
//             }
//         }
//     }
//
//     private static void WriteNode(IEmitter emitter, YamlNode node, Dictionary<string, string> comments)
//     {
//         switch (node.NodeType)
//         {
//             case YamlNodeType.Scalar:
//                 var scalar = (YamlScalarNode)node;
//                 if (comments.TryGetValue(scalar.Value, out var comment))
//                 {
//                     emitter.Emit(new Comment(comment, false));
//                 }
//                 emitter.Emit(new Scalar(scalar.Anchor, scalar.Tag, scalar.Value, scalar.Style, scalar.IsPlainImplicit, scalar.IsQuotedImplicit));
//                 break;
//
//             case YamlNodeType.Sequence:
//                 var sequence = (YamlSequenceNode)node;
//                 emitter.Emit(new SequenceStart(sequence.Anchor, sequence.Tag, sequence.IsImplicit, sequence.Style));
//                 foreach (var child in sequence.Children)
//                 {
//                     WriteNode(emitter, child, comments);
//                 }
//                 emitter.Emit(new SequenceEnd());
//                 break;
//
//             case YamlNodeType.Mapping:
//                 var mapping = (YamlMappingNode)node;
//                 emitter.Emit(new MappingStart(mapping.Anchor, mapping.Tag, mapping.IsImplicit, mapping.Style));
//                 foreach (var entry in mapping.Children)
//                 {
//                     WriteNode(emitter, entry.Key, comments);
//                     WriteNode(emitter, entry.Value, comments);
//                 }
//                 emitter.Emit(new MappingEnd());
//                 break;
//         }
//     }
// }