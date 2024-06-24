using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Comindware.Bootloading.Core.Configuration.Utils
{
    internal class DotMappingEmitter : IEmitter
    {
        private List<ParsingEvent> _originEvents = new List<ParsingEvent>();
        private Emitter _innerEmitter;
        private StringBuilder _stringBuilder;

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
