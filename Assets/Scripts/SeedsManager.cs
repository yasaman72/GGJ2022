using System.Collections.Generic;
using UnityEngine;

public class SeedsManager : MonoBehaviour
{
    public static SeedsManager instance;
    [SerializeField] private GameObject seed;
    [SerializeField] private Collider2D topSpawnSpace, bottomSpawnSpace;
    [SerializeField] private Collider2D topPlantSpace, bottomPlantSpace;
    [SerializeField]
    private float spawnheight = 1
        ;
    [SerializeField] private GameObject plantedSeed;
    [SerializeField] private float plantedSeedDepth = 1;

    private float spawnheightTop, spawnheightBottom;
    private List<GameObject> topSeeds = new List<GameObject>();
    private List<GameObject> bottomSeeds = new List<GameObject>();

    private void Awake()
    {
        instance = this;
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 edgeVectorBottom = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        spawnheightTop = edgeVector.y - spawnheight;
        spawnheightBottom = edgeVectorBottom.y + spawnheight;

        DestroyAllSeedSources();
    }

    private void Start()
    {
        GameManager.OnGravitySwitched += OnGravitySwitched;
    }

    private void DestroyAllSeedSources()
    {
        GameObject[] allSeedsInScene = GameObject.FindGameObjectsWithTag("SeedCollectable");
        foreach (var seed in allSeedsInScene)
        {
            Destroy(seed);
        }
    }

    public void SpawnSeedSourceOnStart()
    {
        SpawnSeed(true);
        SpawnSeed(false);
    }

    public void SpawnSeed(bool OnBottom)
    {
        Vector2 spawnPos;
        if (OnBottom)
        {
            spawnPos = new Vector2(Random.Range(topSpawnSpace.bounds.min.x, topSpawnSpace.bounds.max.x), spawnheightTop);
        }
        else
        {
            spawnPos = new Vector2(Random.Range(bottomSpawnSpace.bounds.min.x, bottomSpawnSpace.bounds.max.x), spawnheightBottom);
        }
        if (Physics2D.OverlapCircle(spawnPos, 2, 6) != null)
        {
            Debug.Log("overlapping with player");
            SpawnSeed(OnBottom);
            return;
        }
        EnemySeedCollectable seedObj = Instantiate(seed, spawnPos,
            OnBottom ? Quaternion.Euler(0, 0, 180) : Quaternion.identity,
            null)
            .GetComponent<Collider2D>().GetComponent<EnemySeedCollectable>();
        seedObj.isOnBottom = OnBottom;

    }

    public void SeedPlant(Vector2 playerPosition, bool OnBottom)
    {
        Vector2 spawnPos;
        if (OnBottom)
        {
            spawnPos = new Vector2(playerPosition.x, bottomPlantSpace.bounds.center.y);
        }
        else
        {
            spawnPos = new Vector2(playerPosition.x, topPlantSpace.bounds.center.y);
        }
        GameObject newPlantedSeeObj = Instantiate(plantedSeed,
            spawnPos,
            OnBottom ? Quaternion.identity : Quaternion.Euler(0, 0, 180),
            null);

        if (OnBottom)
        {
            bottomSeeds.Add(newPlantedSeeObj);
        }
        else
        {
            topSeeds.Add(newPlantedSeeObj);
        }
    }

    public void OnGravitySwitched(bool isOnTheirLand)
    {
        if (isOnTheirLand)
        {
            SpawnSeed(true);
            SpawnSeed(false);
            foreach (var seed in topSeeds)
            {
                seed.GetComponent<EnemySeed>().ReturnEnemyToSeed();
            }

            foreach (var seed in bottomSeeds)
            {
                seed.GetComponent<EnemySeed>().ReturnEnemyToSeed();
            }
        }
        else
        {
            DestroyAllSeedSources();
            foreach (var seed in topSeeds)
            {
                seed.GetComponent<EnemySeed>().SpawnEnemy(true);
            }

            foreach (var seed in bottomSeeds)
            {
                seed.GetComponent<EnemySeed>().SpawnEnemy(false);
            }
        }
    }

    public void OnEnemyDied(GameObject diedSeed)
    {
        if (bottomSeeds.Contains(diedSeed))
        {
            bottomSeeds.Remove(diedSeed);
        }
        else if (topSeeds.Contains(diedSeed))
        {
            topSeeds.Remove(diedSeed);
        }
    }
}