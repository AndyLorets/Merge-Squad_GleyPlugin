using D2D;
using D2D.Utilities;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class NeutralsSpawn : Unit
{
    [SerializeField] private int maxNeutralsOnScreen = 8;
    [SerializeField] private float spawnCooldown = .8f;
    [SerializeField] private NeutralMember neutralMember;

    private Camera currentCamera;

    private int currentAmount;

    private float timer;

    private void Awake()
    {
        currentCamera = Camera.main;
    }

    private void Update()
    {
        if (maxNeutralsOnScreen > currentAmount)
        {
            if (Time.time <= timer)
            {
                return;
            }

            SpawnNeutral();

            timer = Time.time + spawnCooldown;
        }
    }
    private void DespawnNeutral()
    {
        currentAmount--;
    }
    private void SpawnNeutral()
    {
        float xPos;
        float yPos;
        float sign = Mathf.Sign(Random.Range(-1, 2));

        if (Random.Range(0, 100) > 50)
        {
            xPos = 0.5f + 0.6f * sign;
            yPos = Random.Range(0, 100) / 100f;
        }
        else
        {
            xPos = Random.Range(0, 100) / 100f;
            yPos = 0.5f + 0.6f * sign;
        }

        Vector3 direction = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, -10));

        Ray ray = new Ray(currentCamera.transform.position, currentCamera.transform.position - direction);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _gameData.GroundLayer))
        {
            NeutralMember neutralMemberComp = Instantiate(neutralMember.gameObject, hit.point, Quaternion.identity).Get<NeutralMember>();
            neutralMemberComp.OnDespawn += DespawnNeutral;

            var memberComp = neutralMemberComp.Get<SquadMember>();
            memberComp.Init();

            currentAmount++;
        }
    }
}