using D2D.Utilities;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "GeneralAISettings", menuName = "SO/GeneralAISettings", order = 0)]
    public class GeneralAISettings : SingletonData<GeneralAISettings>
    {
        public bool isAIEnabled = true;
        
        [Space]
        
        public bool fsmDebugEnabled;
        public GameObject debugCanvasPrefab;

        [Space]
        
        [Tooltip("Min speed from which we understand that the bot is got stuck")]
        public float stuckSpeed = .5f;

        [Tooltip("Used for periodically know that the bot is out of the map bounds => " +
                "he need to return back to the map center")]
        public float farAwayCheckDelay = 2f;
    }
}