using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/* 
 * The GameManager class is the class of a GameManger object that is present in the main scene
 * The GameManager handles spawning enemies, building towers and updating the state of player or game
 * There should never be more than 1 GameManager object or script active at the same time
 */
public class GameManager : MonoBehaviour
{

    //editor fields
    [SerializeField] private float spawnZPosition;
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private List<GameObject> towerPrefabs;
    [SerializeField] private List<WaveTemplate> waveData;
    [SerializeField] private GameObject addPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject uiManager;

    //private fields
    private MainUIManager uiManagerScript;
    private int gold = 50;
    private int score = 0;
    private int wave = 0;
    private int deposit = 0;
    private bool isBuilding;
    private GameObject selectedTower;
    private float towerHeight = 0.5f;
    private int goldPerEnemy = 5;
    private int enemiesAlive = 0;
    private int spawnCoroutinesCount = 0;

    //public variables
    public float playerHealth = 10f;


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

        uiManagerScript = uiManager.GetComponent<MainUIManager>();
        UpdatePlayerHealthUI();
        UpdateGoldTextUI(gold);
    }

    // Update is called once per frame
    void Update()
    {

        if (isBuilding)
        {
            Vector3 towerPos = GetMousePositionRay();

            if(towerPos != Vector3.zero && towerPos.y >= towerHeight)
            {
                Vector3 adjustedPos = new Vector3(towerPos.x, towerPos.y + towerHeight, towerPos.z);

                selectedTower.transform.position = adjustedPos;
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
        }
    }


    //spawn each element of a list once at spawnPos in frequency based delay between element spawns
    IEnumerator SpawnRoutine(List<GameObject> enemyList, Vector3 spawnPos, float frequency)
    {
        spawnCoroutinesCount++;
        foreach (GameObject enemy in enemyList)
        {
            SpawnEnemy(enemy, spawnPos);
            yield return new WaitForSeconds(frequency);
        }
        spawnCoroutinesCount--;
    }

    //spawn random elements of a list at spawnPos for amount of times in frequency 
    IEnumerator SpawnRoutine(List<GameObject> randomEnemyList, Vector3 spawnPos, int amount, float frequency) 
    {
        spawnCoroutinesCount++;
        for (int i = 0; i < amount; i++)
        {
            SpawnRandomEnemy(randomEnemyList, spawnPos);
            yield return new WaitForSeconds(frequency);
        }
        spawnCoroutinesCount--;
    }

    //spawn specific gameObject at spawnPos for amount of times in frequency 
    IEnumerator SpawnRoutine(GameObject enemy, Vector3 spawnPos, int amount, float frequency)
    {
        spawnCoroutinesCount++;
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy(enemy, spawnPos);
            yield return new WaitForSeconds(frequency);
        }
        spawnCoroutinesCount--;
    }

    //Spawn adds when big enemies get defeated
    private void SpawnAdds(Vector3 spawnPos)
    {
        Debug.Log("Start Spawning Adds!");
        float addSpawnFrequency = 0.5f;
        int amountOfAdds = 2;
        StartCoroutine(SpawnRoutine(addPrefab, spawnPos, amountOfAdds, addSpawnFrequency));
    }

    //The final and only method that instantiates enemies
    private void SpawnEnemy(GameObject enemy, Vector3 spawnPos)
    {
        Instantiate(enemy, spawnPos, enemy.transform.rotation);
        EventManager.Instance.EnemySpawn();
        enemiesAlive++;
    }

    //Method for spawning a single enemy multiple times
    public void SpawnEnemies(GameObject enemy, Vector3 spawnPos, int amount, float frequency)
    {
        StartCoroutine(SpawnRoutine(enemy, spawnPos, amount, frequency));
    }

    //Method for spawning each enemy of a list for a single time
    public void SpawnEnemies(List<GameObject> enemyList, Vector3 spawnPos, float frequency)
    {
        StartCoroutine(SpawnRoutine(enemyList, spawnPos, frequency));   
    }

    //Method for spawning random enemies of a list multiple times
    public void SpawnEnemies(List<GameObject> enemyList, Vector3 spawnPos, int amount, float frequency)
    {
        StartCoroutine(SpawnRoutine(enemyList, spawnPos, amount, frequency));
    }

    //Method for spawning a single random enemy from a list
    private void SpawnRandomEnemy(List<GameObject> enemyList, Vector3 spawnPos)
    {
        int spawnIndex = Random.Range(0, enemyList.Count);
        GameObject randomEnemyPrefab = enemyList[spawnIndex];
        Vector3 spawnPosition = new Vector3(spawnPos.x, randomEnemyPrefab.transform.position.y, spawnPos.z);

        SpawnEnemy(randomEnemyPrefab, spawnPosition);
    }

    //Method that is bound to the OnEnemyDeath event that checks if defeated enemy spawns adds
    private void EnemyDeathReaction(GameObject enemy)
    {
        enemiesAlive--;
        if(enemiesAlive == 0 && spawnCoroutinesCount == 0)
        {
            EventManager.Instance.FinishWave();
        }
        if(enemy.GetComponent<EnemyBehavior>().baseSpeed == 3)
        {
            //Debug.Log("Found slow and big!");
            SpawnAdds(enemy.transform.position);
        }
        AddMoney(enemy);
    }

    //Update variables and UI after enemy defeat
    private void AddMoney(GameObject enemy)
    {
        score++;
        gold += goldPerEnemy;
        uiManagerScript.UpdateScoreText(score);
        UpdateGoldTextUI(gold);
    }

    private void UpdateGoldTextUI(int gold)
    {
        uiManagerScript.UpdateGoldText(gold);
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
        uiManagerScript.UpdatePlayerHealthSlider(playerHealth);
    }

    //timeScale to 0 is scene independent, needs to be set to 1 if changing scene
    private void GameOver()
    {
        Time.timeScale = 0;
        uiManagerScript.UpdateGameOverPanel();
    }

    public void RestartGame()
    {
        MainManager.Instance.StartGame();
    }

    public void LoadMainMenu()
    {
        MainManager.Instance.MainMenu();
    }

    //Check WaveTemplate for next wave and start Coroutine for each WaveIndividual
    public void SpawnNextWave()
    {
        if (wave >= waveData.Count)
        {
            Debug.LogError("Next wave index is greater than the amount of waves. Cancel spawn wave.");
            return;
        }
        uiManagerScript.UpdateWaveText(wave + 1);

        Vector3 spawnPosition = new Vector3(0, 0, spawnZPosition);

        WaveTemplate currentWaveData = waveData[wave]; 

        foreach(WaveIndividual enemyType in currentWaveData.enemies)
        {
            StartCoroutine(SpawnRoutine(enemyType.enemyPrefab, spawnPosition, enemyType.numberOfSpawns, enemyType.spawnFrequency));
        }

        wave++;
        EventManager.Instance.NextWave(wave);
    }

    //Method called when shop button is clicked, selects tower based on cost and starts building process
    public void BuyTower(int cost)
    {

        if (gold >= cost)
        {
            deposit = cost;
            gold -= cost;
            uiManagerScript.UpdateGoldText(gold);
            isBuilding = true;
            
            selectedTower = GetTowerByCost(cost);
            BuildTower(selectedTower);

        } else
        {
            Debug.Log("Not enough money! You have: " + gold + ", you need: " + cost);
        }
    }

    //Instantiate tower at mouse raycast position
    private void BuildTower(GameObject towerToBuild)
    {
        Vector3 spawnPos = GetMousePositionRay();
        if(spawnPos != Vector3.zero)
        {
            selectedTower = Instantiate(towerToBuild, spawnPos, selectedTower.transform.rotation);
            selectedTower.GetComponent<TowerBehavior>().DisableTower();
        }
    }

    //Finalize building process
    private void PlaceTower()
    {
        isBuilding = false;
        selectedTower.GetComponent<TowerBehavior>().EnableTower();
        selectedTower = null;
    }

    //Uses raycast to check if mouse position covers the ray layer
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

    //WIP method of getting tower prefab based on cost, might change index -> cost
    private GameObject GetTowerByCost(int cost)
    {
        if(cost == 10)
        {
            return towerPrefabs[0];
        }
        if(cost == 15)
        {
            return towerPrefabs[1];
        }

        return towerPrefabs[2];
    }
}
