using System;
using System.Collections.Generic;
using YamlDotNet.Core.Events;

namespace Comindware.Configs.Core
{
    internal class ParsingEventsConverter
    {
        private const char AttributesSeparatorChar = '.';

        private IEnumerator<ParsingEvent> _enumerator;
        private ParsingStatus _status;
        private Action _action;
        private Stack<Scalar> _currentKeys;
        private List<ParsingEvent> _events;
        private ParsingEvent _current => _enumerator.Current;
        private int _nestedLevel;

        public IEnumerable<ParsingEvent> ConvertToDotMapping(IEnumerable<ParsingEvent> originEvents)
        {
            Reset(originEvents);
            while (MoveNext())
            {
                SetState();
                PerformAction();
            }

            return _events;
        }

        private bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        private void Reset(IEnumerable<ParsingEvent> events)
        {
            _enumerator = events.GetEnumerator();
            _events = new List<ParsingEvent>();
            if (_currentKeys != null)
            {
                _currentKeys.Clear();
            }
            else
            {
                _currentKeys = new Stack<Scalar>();
            }

            _status = ParsingStatus.Undefined;
            _action = NoAction;
            _nestedLevel = 0;
        }

        private string BuildKey()
        {
            var key = string.Empty;
            var array = _currentKeys.ToArray();
            for (int i = _currentKeys.Count - 1; i >= 0; i--)
            {
                key += array[i].Value + AttributesSeparatorChar;
            }

            key = key.Trim(AttributesSeparatorChar);

            return key;
        }

        private void PerformAction()
        {
            _action?.Invoke();
        }

        private void SetState()
        {
            if (IsScalar(out var _))
            {
                ProcessScalarStatus();
            }
            else if (IsMappingStart())
            {
                ProcessMappingStartStatus();
            }
            else if (IsMappingEnd())
            {
                ProcessMappingEndStatus();
            }
            else
            {
                ProcessOtherActionStatus();
            }
        }

        #region Actions
        private void AddKey()
        {
            AddCurrentKeyToContext();
        }

        private void AddValue()
        {
            if (IsScalar(out var scalar))
            {
                var key = BuildKey();
                RemoveLastKeyFromContext();
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(scalar.Value))
                {
                    return;
                }

                _events.Add(new Scalar(scalar.Anchor, scalar.Tag, key, scalar.Style, scalar.IsPlainImplicit, scalar.IsQuotedImplicit));
                _events.Add(scalar);
            }
        }

        private void AddCurrentKeyToContext()
        {
            if (IsScalar(out var scalar))
            {
                _currentKeys.Push(scalar);
            }
        }

        private void RemoveLastKeyFromContext()
        {
            if (_currentKeys.Count > 0)
            {
                _currentKeys.Pop();
            }
        }

        private void NoAction() { }

        private void AddEvent()
        {
            _events.Add(_current);
        }

        private void OnMappingEndAction()
        {
            if (_nestedLevel == 0)
            {
                AddEvent();
            }

            RemoveLastKeyFromContext();
        }
        #endregion Actions

        #region Status processing
        private void ProcessScalarStatus()
        {
            switch (_status)
            {
                case ParsingStatus.KeyAdd:
                    {
                        _status = ParsingStatus.ValueAdd;
                        _action = AddValue;
                        break;
                    }
                default:
                    {
                        _status = ParsingStatus.KeyAdd;
                        _action = AddKey;
                        break;
                    }
            }
        }

        private void ProcessOtherActionStatus()
        {
            _status = ParsingStatus.Undefined;
            _action = AddEvent;
        }

        private void ProcessMappingStartStatus()
        {
            _status = ParsingStatus.NestedLevelIncrese;
            if (_nestedLevel == 0)
            {
                _action = AddEvent;
            }
            else
            {
                _action = NoAction;
            }

            _nestedLevel++;
        }

        private void ProcessMappingEndStatus()
        {
            _status = ParsingStatus.NestedLevelDecrease;
            _action = OnMappingEndAction;
            _nestedLevel--;
        }
        #endregion Status processing

        #region Type check
        private bool IsScalar(out Scalar currentScalar)
        {
            if (_current is Scalar scalar)
            {
                currentScalar = scalar;
                return true;
            }
            else
            {
                currentScalar = null;
                return false;
            }
        }

        private bool IsMappingStart()
        {
            return _current is MappingStart;
        }

        private bool IsMappingEnd()
        {
            return _current is MappingEnd;
        }
        #endregion Type check
    }

    public enum ParsingStatus
    {
        Undefined,
        KeyAdd,
        ValueAdd,
        NestedLevelIncrese,
        NestedLevelDecrease
    }

}
