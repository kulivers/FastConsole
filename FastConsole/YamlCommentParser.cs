using Comindware.Configs.Core;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization.ObjectGraphVisitors;

public class YamlCommentParser
{
    public const string CommentSign = "#";
    public static string ChangeRawYmlValue(string sourceRaw, string key, string newValue)
    {
        if (sourceRaw.StartsWith(CommentSign))
        {
            return sourceRaw;
        }
        
        if (!sourceRaw.Contains($" {CommentSign}"))
        {
            var model = PlatformYmlSerializer.ReadContent<Dictionary<string, string>>(sourceRaw);
            if (model.ContainsKey(key))
            {
                model[key] = newValue;
            }

            return PlatformYmlSerializer.Serialize(model);
        }

        var comment = sourceRaw.Split($" {CommentSign}", 1);
        return null;
    }
    
    public void ParseYamlWithComments(string yamlContent)
    {
        var comments = new Dictionary<string, string>();
        using (var reader = new StringReader(yamlContent))
        {
            var parser = new MyParser(reader);
            string currentKey = null;

            while (parser.MoveNext())
            {
                var evt = parser.Current;

                if (evt is Comment comment)
                {
                    if (currentKey != null)
                    {
                        comments[currentKey] = comment.Value;
                    }
                }
                else if (evt is Scalar scalar)
                {
                    currentKey = scalar.Value;
                }

                // Handle other events like SequenceStart, SequenceEnd, MappingStart, MappingEnd as needed
            }
        }

        // Print comments
        foreach (var kvp in comments)
        {
            Console.WriteLine($"Key: {kvp.Key}, Comment: {kvp.Value}");
        }
    }

    //model -> file with coms
    //build file model
    //replace file model values
    //save back to file
    public void BuildFileModel(string yamlContent)
    {
        var yamlStream = new YamlStream();

        using (var reader = new StringReader(yamlContent))
        {
            var parser = new Parser(reader);
            yamlStream.Load(parser);
        }

        var doc = yamlStream.Documents[0];
    }
}