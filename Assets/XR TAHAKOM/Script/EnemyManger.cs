using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class EnemyManger
{
    public int spawnTime;
    public int spawner; 
    public bool randomSpawn; 
    public bool isSpawned;
    public EnemyControllerVR enemyController; 
    public GameObject enemyGameObject;
}

public enum EnemyType
{
    None,
    Enemy_Elemental,
    Enemy_Arcane_Fiend,
    Enemy_Shadow_Caster
}
