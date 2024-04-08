using D2D.Utilities;
using D2D.Tools;
using UnityEngine;

namespace D2D.Core
{
    public class PausablesMember : MonoBehaviour
    {
        private void Awake()
        {
            var hub = this.FindLazy<PausablesHub>();
            hub.AddPausable(this);
        }
    }
}
