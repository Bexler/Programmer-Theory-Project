using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private float spawnZPosition;
    [SerializeField] private List<GameObject> spawnPrefabs;
    private float spawnFrequency = 1f;

    private int gold = 0;
    public float playerHealth = 10f;

    [SerializeField] private GameObject UIManager;
    private MainUIManager UIManagerScript;

    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDeath += AddMoney;
        EventManager.Instance.OnEnemySurvival += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= AddMoney;
        EventManager.Instance.OnEnemySurvival -= UpdatePlayerHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
        UIManagerScript = UIManager.GetComponent<MainUIManager>();
        UpdatePlayerHealthUI();
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
            SpawnRandomEnemy();
        }
    }

    private void SpawnRandomEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPrefabs.Count);
        GameObject randomEnemyPrefab = spawnPrefabs[spawnIndex];
        Vector3 spawnPosition = new Vector3(0, randomEnemyPrefab.transform.position.y, spawnZPosition);
        Instantiate(randomEnemyPrefab, spawnPosition, randomEnemyPrefab.transform.rotation);
    }

    private void AddMoney(GameObject enemy)
    {
        gold++;
        UIManagerScript.UpdateGoldText(gold);
    }

    public void UpdatePlayerHealth(float damage)
    {
        playerHealth -= damage;
        UpdatePlayerHealthUI();
    }

    public void UpdatePlayerHealthUI()
    {
        UIManagerScript.UpdatePlayerHealthSlider(playerHealth);
    }

    public void WaveStart()
    {
        Debug.Log("Start Wave!");
    }
}
