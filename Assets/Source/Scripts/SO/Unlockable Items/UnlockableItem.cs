using UnityEngine;

public enum UnlockableType
{
    Member = 1
}

public class UnlockableItem : ScriptableObject
{
    [SerializeField] private UnlockableType unlockableType = UnlockableType.Member;
    [SerializeField] private float unlockStep;
    
    [Header("Visual")]
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite backIcon;
    [SerializeField] private string showName;

    public Sprite Icon => icon;
    public Sprite BackIcon => backIcon;
    public string ShowName => showName;
    public float UnlockStep => unlockStep;
    public UnlockableType UnlockableType => unlockableType;
}