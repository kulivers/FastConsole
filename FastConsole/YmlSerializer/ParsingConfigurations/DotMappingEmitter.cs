using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Comindware.Configs.Core
{
    internal class DotMappingEmitter : IEmitter
    {
        private readonly List<ParsingEvent> _originEvents = new List<ParsingEvent>();
        private readonly Emitter _innerEmitter;
        private readonly StringBuilder _stringBuilder;

        public DotMappingEmitter()
        {
            _stringBuilder = new StringBuilder();
            _innerEmitter = new Emitter(new StringWriter(_stringBuilder));
        }

        public void Emit(ParsingEvent parsingEvent)
        {
            _originEvents.Add(parsingEvent);
        }

        public string GetSerializedObject()
        {
            var builder = new ParsingEventsConverter();
            var convertedEvents = builder.ConvertToDotMapping(_originEvents);
            foreach (var parsingEvent in convertedEvents)
            {
                _innerEmitter.Emit(parsingEvent);
            }

            return _stringBuilder.ToString();
        }
    }
}
