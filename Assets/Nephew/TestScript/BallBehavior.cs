using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{

    private Rigidbody ballRb;
    [SerializeField] private float Geschwindigkeit = 5000;
    [SerializeField] TMPro.TextMeshProUGUI winText;
    [SerializeField] GameObject flatWall;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput != 0)
        {
            ballRb.AddForce(Vector3.right * Time.deltaTime * horizontalInput * Geschwindigkeit);
        }

        if(verticalInput != 0)
        {
            ballRb.AddForce(Vector3.forward * Time.deltaTime * verticalInput * Geschwindigkeit);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = startPos;
            winText.text = "";
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            flatWall.SetActive(!flatWall.activeSelf);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            winText.text = "GEWONNEN!";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            winText.text = "VERLOREN!";
        }
    }
}
