using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave Settings")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float duration;
    [SerializeField] private int minLevel;

    public GameObject[] Enemies => enemies;
    public float Duration => duration;
    public int MinLevel => minLevel;
}