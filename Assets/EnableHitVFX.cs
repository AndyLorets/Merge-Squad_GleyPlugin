using D2D;
using DG.Tweening;
using UnityEngine;

public class EnableHitVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem VFX;

    private Tween tween;

    private const float punchScale = 0.005f;

    private void OnTriggerEnter(Collider collision)
    {
        VFX?.Play();

        tween.KillTo0();

        tween = transform.DOPunchScale(Vector3.one * punchScale, .2f);
    }
}