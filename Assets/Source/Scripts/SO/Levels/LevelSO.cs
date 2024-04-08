using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Level")]
public class LevelSO : ScriptableObject
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private float totalDuration = 300f;
    [SerializeField] private float baseXPToLevelUp = 100f;
    [SerializeField] private float stepXPOnLevelUp = 50f;

    public Wave[] Waves => waves;
    public float TotalDuration => totalDuration * (1 + _db.PassedLevels.Value / 10f);
    public float BaseXPToLevelUp => baseXPToLevelUp;
    public float StepXPOnLevelUp => stepXPOnLevelUp;
    public const int LevelUps = 50;

    /*public float LevelXp
    {
        get
        {
            var result = BaseXPToLevelUp * multiplier + (i * _levelSO.StepXPOnLevelUp * multiplier)
            return result;
        }
    }*/
}