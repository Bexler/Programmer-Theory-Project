using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    [SerializeField] private LayerMask layerUI;
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

                bool isTowerBlocked = selectedTower.GetComponent<TowerBehavior>().IsTowerBlocked();
                if (Input.GetMouseButtonDown(0) && !isTowerBlocked && !IsMouseOverButton())
                {
                    PlaceTower();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ////Debug.Log("Clicked.");
                //if (IsMouseOverSomething())
                //{
                //    Debug.Log("Clicked something!");
                //}
                //if (IsMouseOverButton())
                //{
                //    Debug.Log("Clicked Button!");
                //}
            }
        }
    }

    private bool IsMouseOverSomething()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsMouseOverButton()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        Debug.Log("Raycast hit " + raycastResultList.Count + " targets!");

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            //Debug.Log("Layer check: " + raycastResultList[i].gameObject.layer + " = " + layerUI.value);
            //if (!IsInLayermask(raycastResultList[i].gameObject.layer, layerUI))
            //{
            //    Debug.Log("Not the same!");
            //    raycastResultList.RemoveAt(i);
            //    i--;
            //}
            if (!raycastResultList[i].gameObject.CompareTag("Button"))
            {
                Debug.Log("Removed non-button!");
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    private bool IsInLayermask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    //spawn each element of a list once at spawnPos in frequency based delay between element spawns
    IEnumerator SpawnRoutine(List<GameObject> enemyList, Vector3 spawnPos, float frequency)
    {
        spawnCoroutinesCount++;
        foreach (GameObject enemy in enemyList)
        {
            SpawnEnemy(enemy, spawnPos);
            yield return new WaitForSeconds(1/frequency);
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
            yield return new WaitForSeconds(1/frequency);
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
            yield return new WaitForSeconds(1/frequency);
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
        GameObject spawnedEnemy = Instantiate(enemy, spawnPos, enemy.transform.rotation);
        EventManager.Instance.EnemySpawn();
        enemiesAlive++;
        spawnedEnemy.GetComponent<EnemyBehavior>().SetSpawnedWave(wave); 
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

        if (currentWaveData.isRandom)
        {
            List<GameObject> enemyTypes = new List<GameObject>();
            foreach (WaveIndividual enemyType in currentWaveData.enemies)
            {
                enemyTypes.Add(enemyType.enemyPrefab);
            }
            WaveIndividual infoEnemy = currentWaveData.enemies[0];

            StartCoroutine(SpawnRoutine(enemyTypes, spawnPosition, infoEnemy.numberOfSpawns, infoEnemy.spawnFrequency));

        } else
        {
            foreach (WaveIndividual enemyType in currentWaveData.enemies)
            {
                StartCoroutine(SpawnRoutine(enemyType.enemyPrefab, spawnPosition, enemyType.numberOfSpawns, enemyType.spawnFrequency));
            }
        }

        wave++;
        EventManager.Instance.NextWave(wave);
    }

    //Method called when shop button is clicked, selects tower based on cost and starts building process
    public void BuyTower(int cost)
    {

        if (!isBuilding && gold >= cost)
        {
            deposit = cost;
            gold -= cost;
            UpdateGoldTextUI(gold);
            isBuilding = true;
            
            selectedTower = GetTowerByCost(cost);
            BuildTower(selectedTower);

        } else
        {
            Debug.Log("Not enough money or currently building");
        }
    }

    public void SellTower()
    {
        if (isBuilding)
        {
            isBuilding = false;
            gold += deposit;
            UpdateGoldTextUI(gold);
            deposit = 0;
            Destroy(selectedTower);
            selectedTower = null;
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
