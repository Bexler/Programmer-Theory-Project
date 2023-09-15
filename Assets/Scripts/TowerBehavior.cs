using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{

    private float range;
    private List<GameObject> enemiesInRange;
    [SerializeField] private GameObject rangeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        DisableRangeIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        enemiesInRange.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInRange.Remove(other.gameObject);
    }

    private void DisableRangeIndicator()
    {
        rangeIndicator.SetActive(false);
    }

    private void EnableRangeIndicator()
    {
        rangeIndicator.SetActive(true);
    }
}
