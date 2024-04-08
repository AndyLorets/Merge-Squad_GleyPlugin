using D2D;
using D2D.Utilities;
using DG.Tweening;
using System;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class NeutralMember : Unit
{
    [SerializeField] private SexyOverlap neutralOverlap;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    public Action OnDespawn;

    private Color activeColor;
    private Color deactivatedColor = Color.white;

    private void Awake()
    {
        activeColor = meshRenderer.material.color;

        meshRenderer.material.DOColor(deactivatedColor, 0);
    }

    private void Update()
    {
        if (neutralOverlap.HasTouch && neutralOverlap.Touched.gameObject != this)
        {
            AddNeutralToSquad();

            enabled = false;
            neutralOverlap.gameObject.SetActive(false);

            Despawn();
        }

        if (Vector3.Distance(transform.position, _formation.transform.position) > _gameData.neutralDespawnDistance)
        {
            Despawn();
            
            Destroy(gameObject);
        }
    }

    private void AddNeutralToSquad()
    {
        _squad.AddMember(Get<SquadMember>());

        gameObject.SetLayerRecursively(playerLayer.ToLayer());

        meshRenderer.material.DOColor(activeColor, .5f);
    }
    private void Despawn()
    {
        OnDespawn?.Invoke();
    }
}