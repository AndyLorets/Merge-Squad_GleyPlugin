using D2D;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugAddMember : Unit
{
    [SerializeField] private SquadMember squadMember;

    private SquadComponent squadComponent;
    private void Awake()
    {
        squadComponent = Get<SquadComponent>();
    }

    [Button]
    public void AddMember()
    {
        var newMember = Instantiate(squadMember.gameObject, Vector3.forward, Quaternion.identity, transform).GetComponent<SquadMember>();

        squadComponent.AddMember(newMember);
    }
}