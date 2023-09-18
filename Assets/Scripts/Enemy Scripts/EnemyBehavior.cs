using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] protected float speed = 5f;
    protected float baseSpeed;
    public float health = 10f;
    public bool isDefeated = false;

    //for testing purposes check tower targeting functionality
    public int towerTargetCount = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        if(health <= 0)
        {
            isDefeated = true;
            EventManager.Instance.EnemyDeath(gameObject);
        }
    }
}
