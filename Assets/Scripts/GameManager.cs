using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private float spawnZPosition;
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private GameObject addPrefab;
    private float spawnFrequency = 1f;

    private int gold = 0;
    public float playerHealth = 10f;
    private int score = 0;
    private int wave = 0;

    [SerializeField] private GameObject UIManager;
    private MainUIManager UIManagerScript;

    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDeath += AddMoney;
        EventManager.Instance.OnEnemySurvival += UpdatePlayerHealth;
        EventManager.Instance.OnSpawnAdds += SpawnAdds;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= AddMoney;
        EventManager.Instance.OnEnemySurvival -= UpdatePlayerHealth;
        EventManager.Instance.OnSpawnAdds -= SpawnAdds;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnRoutine());
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
            StartCoroutine(SpawnRoutine(spawnPrefabs, Vector3.zero, 5, spawnFrequency));
        }
    }

    //spawn each element of a list once at spawnPos in frequency based delay between element spawns
    IEnumerator SpawnRoutine(List<GameObject> enemyList, Vector3 spawnPos, float frequency)
    {
        foreach (GameObject enemy in enemyList)
        {
            SpawnEnemy(enemy, spawnPos);
            yield return new WaitForSeconds(frequency);
        }
    }

    //spawn random elements of a list at spawnPos for amount of times in frequency 
    IEnumerator SpawnRoutine(List<GameObject> randomEnemyList, Vector3 spawnPos, int amount, float frequency) 
    {
        for(int i = 0; i < amount; i++)
        {
            SpawnRandomEnemy(randomEnemyList, spawnPos);
            yield return new WaitForSeconds(frequency);
        }
    }

    //spawn specific gameObject at spawnPos for amount of times in frequency 
    IEnumerator SpawnRoutine(GameObject enemy, Vector3 spawnPos, int amount, float frequency)
    {
        for(int i = 0; i < amount; i++)
        {
            SpawnEnemy(enemy, spawnPos);
            yield return new WaitForSeconds(frequency);
        }
    }

    private void SpawnAdds(Vector3 spawnPos)
    {
        Debug.Log("Start Spawning Adds!");
        StartCoroutine(SpawnRoutine(addPrefab, spawnPos, 2, 0.5f));
    }

    private void SpawnEnemy(GameObject enemy, Vector3 spawnPos)
    {
        Instantiate(enemy, spawnPos, enemy.transform.rotation);
    }

    public void SpawnEnemies(GameObject enemy, Vector3 spawnPos, int amount, float frequency)
    {
        StartCoroutine(SpawnRoutine(enemy, spawnPos, amount, frequency));
    }

    public void SpawnEnemies(List<GameObject> enemyList, Vector3 spawnPos, float frequency)
    {
        StartCoroutine(SpawnRoutine(enemyList, spawnPos, frequency));   
    }

    private void SpawnRandomEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPrefabs.Count);
        GameObject randomEnemyPrefab = spawnPrefabs[spawnIndex];
        Vector3 spawnPosition = new Vector3(0, randomEnemyPrefab.transform.position.y, spawnZPosition);

        Instantiate(randomEnemyPrefab, spawnPosition, randomEnemyPrefab.transform.rotation);
    }

    private void SpawnRandomEnemy(List<GameObject> enemyList, Vector3 spawnPos)
    {
        int spawnIndex = Random.Range(0, enemyList.Count);
        GameObject randomEnemyPrefab = enemyList[spawnIndex];
        Vector3 spawnPosition = new Vector3(spawnPos.x, randomEnemyPrefab.transform.position.y, spawnPos.z);

        Instantiate(randomEnemyPrefab, spawnPosition, randomEnemyPrefab.transform.rotation);
    }

    private void AddMoney(GameObject enemy)
    {
        score++;
        gold++;
        UIManagerScript.UpdateScoreText(score);
        UIManagerScript.UpdateGoldText(gold);
    }

    public void UpdatePlayerHealth(float damage)
    {
        playerHealth -= damage;
        UpdatePlayerHealthUI();
        if(playerHealth < 0)
        {
            GameOver();
        }
    }

    public void UpdatePlayerHealthUI()
    {
        UIManagerScript.UpdatePlayerHealthSlider(playerHealth);
    }

    public void WaveStart()
    {
        Debug.Log("Start Wave!");
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        UIManagerScript.UpdateGameOverPanel();
    }

    public void RestartGame()
    {
        MainManager.Instance.StartGame();
    }

    public void LoadMainMenu()
    {
        MainManager.Instance.MainMenu();
    }

    public void SpawnNextWave()
    {
        wave++;
        UIManagerScript.UpdateWaveText(wave);

        Vector3 spawnPosition = new Vector3(0, 0, spawnZPosition);
        StartCoroutine(SpawnRoutine(spawnPrefabs, spawnPosition, 10, spawnFrequency));
        
    }
}
