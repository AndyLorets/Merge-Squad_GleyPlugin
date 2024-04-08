using UnityEngine;
using D2D.Utilities;

namespace D2D.UI
{
    public class FPSLabel : LabelBase
    {
        protected override float UpdateRate => .25f;

        protected override string GetText() => $"{(1f / Time.deltaTime).Round()}";
    }
}