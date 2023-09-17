using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] protected float speed = 5f;
    public float health = 10f;
    public bool isDefeated = false;

    //for testing purposes check tower targeting functionality
    public int towerTargetCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        speed /= (1+(slowStrength/100));
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