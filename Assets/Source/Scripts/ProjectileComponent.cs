using UnityEngine;

[RequireComponent(typeof(OnTriggerEnterComponent))]
public class ProjectileComponent : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    public OnTriggerEnterComponent enterComponent;
    public Rigidbody rb;
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        enterComponent = GetComponent<OnTriggerEnterComponent>();
        rb = GetComponent<Rigidbody>();
    }
    public void EnableVFX(bool enable)
    {
        if (enable)
        {
            particle.Play(true);
        }
        else
        {
            particle.Stop(true);
        }
    }
    public void SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }
}