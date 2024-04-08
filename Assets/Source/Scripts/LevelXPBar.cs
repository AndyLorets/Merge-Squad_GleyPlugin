using D2D.UI;

using static D2D.Utilities.CommonGameplayFacade;

public class LevelXPBar : FillBarBase
{
    protected override float Calculate()
{
    return _gameProgress.GetValueForLevelUPTime();
}
}
