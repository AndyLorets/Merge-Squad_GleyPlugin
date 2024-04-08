using D2D;
using D2D.Gameplay;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamageNumbers : Unit
{
    [SerializeField] private GameObject damageText;

    private const float duration = 0.4f;

    private DamageTextRoot textRoot;

    private Camera currentCamera;

    private void Awake()
    {
        GetComponent<Health>().Damaged += CreateDamageText;

        textRoot = Find<DamageTextRoot>();

        currentCamera = Camera.main;
    }

    private void CreateDamageText(float value)
    {
        var screenPosition = currentCamera.WorldToScreenPoint(transform.position);
        var text = Instantiate(damageText, screenPosition, Quaternion.identity, textRoot.transform);
        var textMeshPro = text.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = $"{value: 0.#}";

        var randomEndPos = new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), Random.Range(-200, 200));
        text.transform.DOMove(screenPosition + randomEndPos, duration).SetEase(Ease.InOutSine).OnComplete(() => Destroy(text));
        textMeshPro.DOFade(0, duration -.3f).SetDelay(.3f);
    }
}