using System.ComponentModel.Design;
using System.Text;
using YamlDotNet.Core.Tokens;
using YamlDotNet.RepresentationModel;

namespace Comindware.Bootloading.Core.Configuration
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

        private string? GetComment(string line)
        {
            var commentIndex = line.IndexOf(" #", StringComparison.Ordinal);
            if (commentIndex == -1)
            {
                return null;
            }

            return line.Substring(commentIndex + 2, line.Length - commentIndex - 2);
        }
    }

    public class YamlLineChange
    {
        public int Line { get; set; }
        public string Value { get; set; }

        public YamlLineChange()
        {
        }

        public YamlLineChange(int line, string value)
        {
            Line = line;
            Value = value;
        }
    }

    public class YamlConfigChanges
    {
        public IEnumerable<YamlField> Add { get; set; }
        public IEnumerable<YamlLineChange> Edit { get; set; }
    }

    public class MyVisitor : IYamlVisitor
    {
        private List<YamlField> _add = new List<YamlField>();
        private List<YamlLineChange> _edit = new List<YamlLineChange>();

        private Dictionary<string, ComplexYamlField> _fields;

        private ComplexYamlField _currentField;
        private Dictionary<string, ComplexYamlField> _currentFieldsCollection;

        public MyVisitor(Dictionary<string, ComplexYamlField> changes)
        {
            _fields = changes;
        }

        public void Visit(YamlStream stream)
        {
            foreach (var doc in stream.Documents)
            {
                doc.Accept(this);
            }
        }

        public void Visit(YamlDocument document)
        {
            document.RootNode.Accept(this);
        }

        public void Visit(YamlScalarNode scalar)
        {
            _edit.Add(new YamlLineChange(scalar.Start.Line, _currentField.Value));
        }

        public void Visit(YamlSequenceNode sequence)
        {
            //throw new System.NotImplementedException();
        }

        public void Visit(YamlMappingNode mapping)
        {
            var lastCollection = _currentFieldsCollection;
            foreach (var pair in mapping)
            {
                var scalarNode = pair.Key as YamlScalarNode;
                var keys = scalarNode.Value.Split('.');

                var hasKey = true;
                var currentField = default(ComplexYamlField);
                var currentFields = _currentFieldsCollection ?? _fields;
                foreach (var key in keys)
                {
                    if (!currentFields.TryGetValue(key, out var field))
                    {
                        hasKey = false;
                        break;
                    }

                    currentField = field;
                    currentFields = field.Fields;
                }

                if (!hasKey)
                {
                    continue;
                }

                _currentField = currentField;
                _currentFieldsCollection = currentFields;
                pair.Value.Accept(this);
                _currentFieldsCollection = lastCollection;
            }

            _currentField = null;
            _currentFieldsCollection = null;
        }

        public YamlConfigChanges GetResult() => new YamlConfigChanges() { Add = _add, Edit = _edit };
    }

    public class YamlFieldCollection : Dictionary<string, ComplexYamlField>
    {
        public bool TrySetValue(string key, string value)
        {
            if (!this.TryGetValue(key, out var field))
            {
                return false;
            }

            field.Value = value;
            return true;
        }

        public bool TrySetValue(string[] propertyPath, string value)
        {
            var field = default(ComplexYamlField);
            var fields = this;
            foreach (var prop in propertyPath)
            {
                if (fields != null && !fields.TryGetValue(prop, out field))
                {
                    return false;
                }

                fields = field.Fields;
            }

            field.Value = value;
            return true;
        }

        public bool TryAddField(ComplexYamlField field)
        {
            if (this.ContainsKey(field.Name))
            {
                return false;
            }

            this.Add(field.Name, field);
            return true;
        }

        public ComplexYamlField CreateField(string[] propertyPath)
        {
            var collection = this;
            var field = default(ComplexYamlField);
            foreach (var prop in propertyPath)
            {
                if (collection.TryGetValue(prop, out field))
                {
                    collection = field.Fields;
                }
                else
                {
                    field = new ComplexYamlField(prop);
                    collection.Add(prop, field);
                    collection = field.Fields;
                }
            }

            return field;
        }

        public bool TryAddField(string[] propertyPath, ComplexYamlField field)
        {
            if (!TryFindField(propertyPath, out var existingField))
            {
                return false;
            }

            if (existingField.Fields != null && !existingField.Fields.ContainsKey(field.Name))
            {
                existingField.Fields.Add(field.Name, field);
                return true;
            }

            return false;
        }

        public bool TryFindField(string[] propertyPath, out ComplexYamlField field)
        {
            field = default(ComplexYamlField);
            var collection = this;
            foreach (var prop in propertyPath)
            {
                if (collection != null && !collection.TryGetValue(prop, out field))
                {
                    return false;
                }

                collection = field.Fields;
            }

            return true;
        }
    }

    public class YamlField
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public YamlField()
        {
        }

        public YamlField(string name)
        {
            Name = name;
        }

        public YamlField(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ComplexYamlField : YamlField
    {
        public YamlFieldCollection Fields { get; set; } = new YamlFieldCollection();

        public ComplexYamlField() : base()
        {
        }

        public ComplexYamlField(string name) : base(name)
        {
        }

        public ComplexYamlField(string name, string value) : base(name, value)
        {
        }

        public ComplexYamlField(string name, IEnumerable<ComplexYamlField> fields) : base(name)
        {
            Fields = ToDictionary(fields);
        }

        private YamlFieldCollection ToDictionary(IEnumerable<ComplexYamlField> fields)
        {
            var result = new YamlFieldCollection();
            foreach (var field in fields)
            {
                if (!result.TryGetValue(field.Name, out var dictField))
                {
                    result.Add(field.Name, field);
                }
            }

            return result;
        }
    }
}