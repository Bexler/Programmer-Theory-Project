using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{

    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;

    private bool isActive = true;
    private float speed = 10.0f;
    private Vector3 startPos;

    private void OnEnable()
    {
        EventManager.Instance.OnNextWave += SetInactive;
        EventManager.Instance.OnFinishWave += SetActive;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNextWave -= SetInactive;
        EventManager.Instance.OnFinishWave -= SetActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
            if(transform.position.z < -40)
            {
                transform.position = startPos;
            }
        }
    }

    private void SetActive()
    {
        gameObject.SetActive(true);
    }

    private void SetInactive(int waveIndex)
    {
        gameObject.SetActive(false);
    }
}
