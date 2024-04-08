using Animancer;
using D2D;
using D2D.Gameplay;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

using static D2D.Utilities.CommonGameplayFacade;

[RequireComponent(typeof(Health))]
public class SquadMember : Unit
{
    public NavMeshAgent navMesh;
    public SexyOverlap overLapObstacle;
    public SexyOverlap overLapEnemy;
    public MemberClass memberClass;
    public Animations animations;
    public Health health;
    public AnimancerComponent animancer;
    public Transform shootPoint;
    public int EvolveLevel;
    public SquadMember NextEvolution;

    [Header("Movement Settings")]
    public Vector3 targetVector;
    public float rotationLerp = 10f;
    public AnimationClip runForward;

    [HideInInspector]
    public EnemyComponent currentTarget;
    public SquadMember evolveTarget;

    [SerializeField] internal Color memberColor;

    private CharacterCanvas canvas;
    private Camera currentCamera;

    private Tween punchTween;

    internal float reloadTime = 0;
    internal float delayBeforeEvolve = 0;
    internal float lastShotSoundTime = 0;

    public bool IsEvolving;


    [Button("Debug Kill")]
    private void DebugKill()
    {
        health.ApplyDamage(null, health.CurrentPoints);
    }
    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        overLapObstacle = GetComponent<SexyOverlap>();
        health = GetComponent<Health>();

        if (canvas == null)
        {
            canvas = GetComponentInChildren<CharacterCanvas>();
        }

        targetVector = transform.forward * 10f + transform.up;

        canvas.HealthBar.SetHealth(health);
    }
    private void Update()
    {
        if (IsEvolving)
        {
            if (evolveTarget == null)
            {
                StopEvolving();

                return;
            }

            delayBeforeEvolve += Time.deltaTime;

            if (delayBeforeEvolve < _gameData.evolveDelay)
            {
                animancer.Play(animations.Idle);

                return;
            }

            var targetDistance = Vector3.Distance(evolveTarget.transform.position, transform.position);

            animancer.Play(runForward);
            navMesh.Move((evolveTarget.transform.position - transform.position) * Time.deltaTime * navMesh.speed * (1.7f / targetDistance));

            if (targetDistance < 0.5f)
            {
                _squad.EvolveMembers(this, evolveTarget);

                IsEvolving = false;
            }

            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetVector - transform.position), Time.deltaTime * rotationLerp);
    }
    public virtual void Init()
    {
        runForward = animations.RunForward;

        if (canvas == null)
        {
            canvas = GetComponentInChildren<CharacterCanvas>();
        }

        animancer.Play(animations.Idle);

        canvas.EvolutionText.text = $"{Mathf.Pow(2, EvolveLevel + 1)}";
    }
    public virtual void Shoot(Transform target)
    {

    }
    public virtual void PunchScaleWithDelay(float delay)
    {
        punchTween.KillTo0();

        punchTween = transform.DOPunchScale(Vector3.one * _gameData.punchScale, _gameData.punchDuration, 0, 0).SetDelay(delay);
    }
    public virtual void SetIdleAnimation() 
    {
        animancer.Layers[0].Play(animations.Idle);
    }
    public void SetDanceAnimation(AnimationClip animation)
    {
        animancer.Play(animation, 0.5f);
    }
    public void Evolve(SquadMember evolveTarget)
    {
        if (NextEvolution == null)
        {
            return;
        }

        animancer.Play(animations.RunForward);
        navMesh.ResetPath();
        IsEvolving = true;
        transform.LookAt(evolveTarget.transform.position);
        this.evolveTarget = evolveTarget;
    }
    private void StopEvolving()
    {
        animancer.Play(animations.Idle);
        IsEvolving = false;
    }
}