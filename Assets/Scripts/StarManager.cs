using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    [SerializeField] private GameObject star;
    [SerializeField] private Transform starSpawnPoint;
    [SerializeField] private float spawnInterval = 4;
    [SerializeField] private float maxStar = 2;

    private void Start()
    {
        GameManager.startGame += StartSpawning;
    }

    private void OnDisable()
    {
        GameManager.startGame -= StartSpawning;
    }

    private void StartSpawning()
    {

        StartCoroutine(SpawnStar());
    }

    private IEnumerator SpawnStar()
    {
        while (GameManager.InGameplay)
        {
            if (GameObject.FindGameObjectsWithTag("Star").Length < maxStar)
            {
                Instantiate(star, starSpawnPoint.transform.position, Quaternion.identity, null);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
