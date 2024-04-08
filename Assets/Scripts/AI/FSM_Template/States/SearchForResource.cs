using System.Linq;
using UnityEngine;

public class SearchForResource : AIState
{
    private readonly Gatherer _gatherer;

    public SearchForResource(Gatherer gatherer)
    {
        _gatherer = gatherer;
    }
    public override void Tick()
    {
        _gatherer.Target = ChooseOneOfTheNearestResources(5);
    }

    private GatherableResource ChooseOneOfTheNearestResources(int pickFromNearest)
    {
         return Object.FindObjectsOfType<GatherableResource>()
             .OrderBy(t=> Vector3.Distance(_gatherer.transform.position, t.transform.position))
             .Where(t=> t.IsDepleted == false)
             .Take(pickFromNearest)
             .OrderBy(t => Random.Range(0, int.MaxValue))
             .FirstOrDefault();
    }

    public override void OnEnter() { }
    public override void OnExit() { }
}