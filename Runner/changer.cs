using System;
using System.IO;
using System.Text;

namespace Runner
{
    public class ConfigChanger
    {
        public void RewriteFile(string path, YamlFieldCollection changes)
        {
            var content = File.ReadAllBytes(path);
            var yamlStream = new YamlStream();
            yamlStream.Load(new StreamReader(new MemoryStream(content)));
            var visitor = new MyVisitor(changes);
            yamlStream.Accept(visitor);

            var marks = visitor.GetResult();
            var edits = marks.Edit.ToDictionary(edit => edit.Line, change => change.Value);

            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(new MemoryStream(content)))
            {
                var line = reader.ReadLine();
                var lineNum = 1;
                while (line != null)
                {
                    if (edits.TryGetValue(lineNum, out var newValue))
                    {
                        var key = GetKey(line);
                        var comment = GetComment(line);
                        line = comment == null ? $"{key}: {newValue}" : $"{key}: {newValue} #{comment}";
                    }

                    stringBuilder.AppendLine(line);
                    line = reader.ReadLine();
                    lineNum++;
                }
            }

            using (var writer = new StreamWriter(path))
            {
                writer.Write(stringBuilder.ToString());
            }
        }
        public string RewriteContent(string content, YamlFieldCollection changes)
        {
            var yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(content));
            var visitor = new MyVisitor(changes);
            yamlStream.Accept(visitor);

            var marks = visitor.GetResult();
            var edits = marks.Edit.ToDictionary(edit => edit.Line, change => change.Value);

            var stringBuilder = new StringBuilder();
            using (var reader = new StringReader(content))
            {
                var line = reader.ReadLine();
                var lineNum = 1;
                while (line != null)
                {
                    if (edits.TryGetValue(lineNum, out var newValue))
                    {
                        var key = GetKey(line);
                        var comment = GetComment(line);
                        line = comment == null ? $"{key}: {newValue}" : $"{key}: {newValue} #{comment}";
                    }

                    stringBuilder.AppendLine(line);
                    line = reader.ReadLine();
                    lineNum++;
                }
            }

            return stringBuilder.ToString();
        }

        private string GetKey(string line)
        {
            var separatorIndex = line.IndexOf(": ", StringComparison.Ordinal);
            var keyLength = separatorIndex;
            var key = line.Substring(0, keyLength);
            return key;
        }

        private string GetComment(string line)
        {
            var commentIndex = line.IndexOf(" #", StringComparison.Ordinal);
            if (commentIndex == -1)
            {
                return null;
            }

            return line.Substring(commentIndex + 2, line.Length - commentIndex - 2);
        }
    }
}