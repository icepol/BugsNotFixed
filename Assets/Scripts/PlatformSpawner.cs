using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private PlatformGroup[] platformGroups;
    [SerializeField] private float randomPosition = 0f;

    [SerializeField] private float firstSpawnPosition = 1f;
    [SerializeField] private float prewarmSpawningOffset = 14f;
    [SerializeField] private float spawningDistance = 4f;
    
    private Transform cameraTransform;
    private float nextSpawnY;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        nextSpawnY = firstSpawnPosition;
    }

    void Update()
    {
        Spawn();                    
    }

    void Spawn()
    {
        if (cameraTransform.position.y + prewarmSpawningOffset > nextSpawnY)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        PlatformGroup platform = Instantiate(platformGroups[Random.Range(0, platformGroups.Length)], transform);

        platform.transform.localPosition = new Vector2(Random.Range(-randomPosition, randomPosition), nextSpawnY);

        nextSpawnY += spawningDistance;
    }
}
