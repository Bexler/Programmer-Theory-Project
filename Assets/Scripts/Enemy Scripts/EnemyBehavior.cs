using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{

    protected float speed;
    public float baseSpeed;
    public float health;
    protected float baseHealth;
    public bool isDefeated = false;
    protected int waveWhenSpawned;

    private Material enemyMaterial;

    //for testing purposes check tower targeting functionality
    public int towerTargetCount = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyMaterial = GetComponent<MeshRenderer>().material;
        baseHealth = health;
        baseSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
    }

    private void LateUpdate()
    {
        if (isDefeated)
        {
            Destroy(gameObject);
        }
    }

    public virtual void SlowSpeed(float slowStrength)
    {
        if(speed > baseSpeed / 2)
        {
            speed /= (1 + (slowStrength / 10));
        }
        if(speed < baseSpeed / 2)
        {
            speed = baseSpeed / 2;
        }
        
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public virtual void DamageEnemy(float damage)
    {
        health -= damage;
        UpdateMaterial();
        if(health <= 0)
        {
            isDefeated = true;
            EventManager.Instance.EnemyDeath(gameObject);
        }
    }

    public void SetSpawnedWave(int wave)
    {
        waveWhenSpawned = wave;
    }

    private void UpdateMaterial()
    {
        float healthPercentage = health / baseHealth;
        enemyMaterial.color = Color.white * (1f - healthPercentage);
    }
}
