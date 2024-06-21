using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Comindware.Configs.Core
{
    internal class DotMappingEmitter : IEmitter
    {
        public readonly List<ParsingEvent> OriginEvents = new List<ParsingEvent>();
        private readonly Emitter _innerEmitter;
        private readonly StringBuilder _stringBuilder;

        public DotMappingEmitter()
        {
            _stringBuilder = new StringBuilder();
            _innerEmitter = new Emitter(new StringWriter(_stringBuilder));
        }

        public void Emit(ParsingEvent parsingEvent)
        {
            OriginEvents.Add(parsingEvent);
        }

        public string GetSerializedObject()
        {
            var converter = new ParsingEventsConverter();
            var convertedEvents = converter.ConvertToDotMapping(OriginEvents);
            foreach (var parsingEvent in convertedEvents)
            {
                _innerEmitter.Emit(parsingEvent);
            }

            return _stringBuilder.ToString();
        }
    }
}