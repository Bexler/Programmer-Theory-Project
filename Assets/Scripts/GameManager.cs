using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private float spawnZPosition;
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private List<GameObject> towerPrefabs;
    [SerializeField] private GameObject addPrefab;
    [SerializeField] private LayerMask groundLayer;
    private float spawnFrequency = 1f;

    private int gold = 0;
    public float playerHealth = 10f;
    private int score = 0;
    private int wave = 0;
    private int deposit = 0;
    private bool isBuilding;
    private GameObject selectedTower;

    [SerializeField] private GameObject UIManager;
    private MainUIManager UIManagerScript;

    private void OnEnable()
    {
        EventManager.Instance.OnEnemyDeath += EnemyDeathReaction;
        EventManager.Instance.OnEnemySurvival += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnEnemyDeath -= EnemyDeathReaction;
        EventManager.Instance.OnEnemySurvival -= UpdatePlayerHealth;
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
        if (isBuilding)
        {
            selectedTower.transform.position = GetMousePositionRay();
            //Update position of tower being built accordingly here
        }
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
        EventManager.Instance.EnemySpawn();
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

        SpawnEnemy(randomEnemyPrefab, spawnPosition);
    }

    private void SpawnRandomEnemy(List<GameObject> enemyList, Vector3 spawnPos)
    {
        int spawnIndex = Random.Range(0, enemyList.Count);
        GameObject randomEnemyPrefab = enemyList[spawnIndex];
        Vector3 spawnPosition = new Vector3(spawnPos.x, randomEnemyPrefab.transform.position.y, spawnPos.z);

        SpawnEnemy(randomEnemyPrefab, spawnPosition);
    }

    private void EnemyDeathReaction(GameObject enemy)
    {
        if(enemy.GetComponent<EnemyBehavior>().baseSpeed == 3)
        {
            Debug.Log("Found slow and big!");
            SpawnAdds(enemy.transform.position);
        }
        AddMoney(enemy);
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
        if(playerHealth <= 0)
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

        StartCoroutine(SpawnRoutine(spawnPrefabs[0], spawnPosition, wave, 1));
        //StartCoroutine(SpawnRoutine(spawnPrefabs, spawnPosition, 10, spawnFrequency));
        
    }

    public void BuyTower(int cost)
    {
        if (gold > cost)
        {
            deposit = cost;
            gold -= cost;
            isBuilding = true;
            
            selectedTower = GetTowerByCost(cost);
            BuildTower(selectedTower);

        }
    }

    private void BuildTower(GameObject towerToBuild)
    {
        Vector3 spawnPos = GetMousePositionRay();
        if(spawnPos != Vector3.zero)
        {
            selectedTower = Instantiate(towerToBuild, spawnPos, selectedTower.transform.rotation);

        }

        //Initiate tower but set it to inactive -> access towerBehavior and implement logic
        //Update position of tower every frame ScreenToWorldPoint or Raycast
        //On second click place tower or error if collision with tower or floor
    }

    private Vector3 GetMousePositionRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 hitPoint = Vector3.zero;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Get the hit point.
            hitPoint = hit.point;

        }

        return hitPoint;
    }

    private GameObject GetTowerByCost(int cost)
    {
        if(cost == 10)
        {
            return spawnPrefabs[0];
        }
        if(cost == 15)
        {
            return spawnPrefabs[1];
        }

        return spawnPrefabs[2];
    }
}
