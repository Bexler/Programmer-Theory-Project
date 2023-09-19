using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Template", menuName = "Wave Template")]
public class WaveTemplate : ScriptableObject
{

    public List<WaveIndividual> enemies;
    public bool isRandom;

}
    



