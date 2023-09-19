using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Individual", menuName = "Wave Individual")]
public class WaveIndividual : ScriptableObject
{
    public GameObject enemyPrefab;
    public int numberOfSpawns;
    public float spawnFrequency;
}
