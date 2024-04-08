using SRF;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class SurfaceMaterialRandom : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRenderers;

    private Material currentMaterial;

    private void Awake()
    {
        currentMaterial = _gameData.groundMaterials.Random();

        foreach (var item in meshRenderers)
        {
            item.material = currentMaterial;
        }
    }
}