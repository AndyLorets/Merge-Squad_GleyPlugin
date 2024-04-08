using UnityEngine;

[CreateAssetMenu(menuName = "Game/StickMan Animations")]
public class Animations : ScriptableObject
{
    public AnimationClip Idle;
    public AnimationClip FallingIdle;
    public AnimationClip Death;
    public AnimationClip UnarmedRun;
    public AnimationClip RunWithRifle;
    public AnimationClip RunWithDouble;
    public AnimationClip RunForward;
    public AnimationClip RunBackwards;
    public AnimationClip RunLeft;
    public AnimationClip RunRight;
    public AnimationClip HoldPistol;
    public AnimationClip HoldRifle;
    public AnimationClip HoldGrenadeLauncher;
    public AnimationClip HoldShotgun;
    public AnimationClip[] DanceAnimations;
}