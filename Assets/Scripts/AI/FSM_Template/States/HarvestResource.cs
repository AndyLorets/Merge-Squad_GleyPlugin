using UnityEngine;

internal class HarvestResource : AIState
{
    private readonly Gatherer _gatherer;
    private readonly Animator _animator;
    private float _resourcesPerSecond = 3;

    private float _nextTakeResourceTime;
    private static readonly int Harvest = Animator.StringToHash("Harvest");

    public HarvestResource(Gatherer gatherer, Animator animator)
    {
        _gatherer = gatherer;
        _animator = animator;
    }

    public override void Tick()
    {
        if (_gatherer.Target != null)
        {
            if (_nextTakeResourceTime <= Time.time)
            {
                _nextTakeResourceTime = Time.time + (1f / _resourcesPerSecond);
                _gatherer.TakeFromTarget();
                _animator.SetTrigger(Harvest);
            }
        }
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
}