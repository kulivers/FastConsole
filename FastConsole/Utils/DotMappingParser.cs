using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

namespace Comindware.Bootloading.Core.Configuration.Utils
{
    internal class DotMappingParser : IParser
    {
        private readonly IEnumerator<ParsingEvent> _enumerator;

        public DotMappingParser(string content)
        {
            var stream = new YamlStream();
            stream.Load(new StringReader(content));

            var events = YamlStreamConverter.ConvertFromDotMapping(stream);
            _enumerator = events.GetEnumerator();
        }

        public ParsingEvent Current
        {
            get
            {
                return _enumerator.Current;
            }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }
    }
}
