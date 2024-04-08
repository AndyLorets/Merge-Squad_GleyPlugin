using D2D;
using D2D.Utilities;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class ObstacleSpawner : Unit
{
    [SerializeField] private int maxObstacleOnScreen = 3;
    [SerializeField] private int maxObstacleOnLabyrinth = 3;
    [SerializeField] private int labyrinthLevel = 3;
    [SerializeField] private float spawnCooldown = 3f;
    [SerializeField] private Obstacle[] obstaclePrefabs;
    [SerializeField] private Vector3 minSize;
    [SerializeField] private Vector3 maxSize;
    [SerializeField] private float maxXRotation = 45;

    private Camera currentCamera;

    private int currentAmount;
    private int currentMax;

    private float timer;

    private void Awake()
    {
        currentCamera = Camera.main;

        currentMax = _db.PassedLevels.Value % labyrinthLevel == 0 && _db.PassedLevels.Value != 0 ? maxObstacleOnLabyrinth : maxObstacleOnScreen;
    }

    private void Update()
    {
        if (currentMax > currentAmount)
        {
            if (Time.time <= timer)
            {
                return;
            }

            SpawnObstacle();

            timer = Time.time + spawnCooldown;
        }
    }
    private void DespawnObstacle()
    {
        currentAmount--;
    }
    private void SpawnObstacle()
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
            Obstacle obstacleComp = Instantiate(obstaclePrefabs.GetRandomElement().gameObject, hit.point, Quaternion.identity).Get<Obstacle>();

            Vector3 randomScale = new Vector3(
                Random.Range(minSize.x, maxSize.x),
                Random.Range(minSize.y, maxSize.y),
                Random.Range(minSize.z, maxSize.z));

            obstacleComp.gameObject.transform.localScale = randomScale;

            Vector3 newRotation = new Vector3(
                Random.Range(-maxXRotation, maxXRotation),
                Random.Range(0, 359f),
                Random.Range(-maxXRotation, maxXRotation));

            obstacleComp.transform.rotation = Quaternion.Euler(newRotation);

            obstacleComp.OnDespawn += DespawnObstacle;

            currentAmount++;
        }
    }
}