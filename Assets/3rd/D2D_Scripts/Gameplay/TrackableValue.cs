using System;

namespace D2D.Common
{
    /// <summary>
    /// The value which have on change event, so other scripts will know
    /// when value change
    /// </summary>
    public class TrackableValue<T>
    {
        public event Action<T> Changed;

        public virtual T Value
        {
            get
            {
                if (_firstGet != null && !_firstGetExecuted)
                {
                    _value = _firstGet();
                    _firstGetExecuted = true;
                }

                return _value;
            }
            set
            {
                if (_firstGet != null && !_firstGetExecuted)
                {
                    _value = _firstGet();
                    _firstGetExecuted = true;
                }
                
                if (!_value.Equals(value))
                {
                    _value = value;
                    Changed?.Invoke(value);
                }
            }
        }

        private T _value;
        private Func<T> _firstGet;
        
        // Mostly for money save to db
        private bool _firstGetExecuted;

        public TrackableValue(T value, Func<T> firstGet = null)
        {
            _value = value;
            _firstGet = firstGet;
        }
    }
}