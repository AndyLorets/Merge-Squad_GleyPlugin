using D2D.Gameplay;
using D2D.Utilities;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.UI
{
    /// <summary>
    /// Shows player`s progress along Z axis.
    /// </summary>
    public class PlayerToFinishFillBar : FillBarBase
    {
        private Player _player;
        private Finish _finish;
        
        private void Start()
        {
            _player = FindObjectOfType<Player>();
            _finish = FindObjectOfType<Finish>();
        }

        protected override float Calculate()
        {
            var playerZ = _player.transform.position.z;
            var finishZ = _finish.transform.position.z;

            return playerZ.FactorRange(playerZ, finishZ);
        }
    }
}