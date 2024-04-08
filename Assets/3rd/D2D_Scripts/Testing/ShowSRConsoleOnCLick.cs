using D2D.UI;
using UnityEngine;

namespace D2D
{
    public class ShowSRConsoleOnCLick : DButtonListener
    {
        protected override void OnClick()
        {
            SRDebug.Instance.ShowDebugPanel();
        }
    }
}