using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float health = 10f;
    private bool isDefeated = false;
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

    public void SlowSpeed(float slowStrength)
    {
        speed /= (1+(slowStrength/100));
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void DamageEnemy(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            isDefeated = true;
            EventManager.Instance.EnemyDeath(gameObject);
        }
    }
}
