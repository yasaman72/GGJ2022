using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsManager : MonoBehaviour
{
    public static SeedsManager instance;
    [SerializeField] private GameObject seed;
    [SerializeField] private Collider2D topSpawnSpace, bottomSpawnSpace;
    [SerializeField] private float spawnheight = 2;
    
    private float spawnheightTop, spawnheightBottom;

    private void Awake()
    {
        instance = this;
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 edgeVectorBottom = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        spawnheightTop = edgeVector.y - spawnheight;
        spawnheightBottom = edgeVectorBottom.y + spawnheight;
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
        Instantiate(seed, spawnPos, Quaternion.identity, null);
    }
}
