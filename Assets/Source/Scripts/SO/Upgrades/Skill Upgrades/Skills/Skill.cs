using System;
using UnityEngine;

[Serializable]
public abstract class Skill : ScriptableObject
{
    public abstract void Activate();  
}