using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject spawnPrefab;
    private float spawnFrequency = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(spawnPrefab, spawnPosition, spawnPrefab.transform.rotation);
    }
}
