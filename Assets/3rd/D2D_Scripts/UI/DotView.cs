using D2D.Common;
using D2D.Utilities;
using UnityEngine;

namespace D2D.UI
{
    public abstract class DotView : MonoBehaviour
    {
        public abstract TrackableValue<int> Trackable { get; }

        private Dot[] _dots;

        private void OnEnable()
        {
            _dots = GetComponentsInChildren<Dot>();
            
            Trackable.Changed += Redraw;
            
            Redraw(Trackable.Value);
        }

        private void OnDisable()
        {
            Trackable.Changed -= Redraw;
        }

        private void Redraw(int count)
        {
            _dots.ForEach(d => d.IsFilled = false);

            for (int i = 0; i < count; i++)
                _dots[i].IsFilled = true;
        }
    }
}