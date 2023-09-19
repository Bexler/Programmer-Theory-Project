using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class RotatingCube : MonoBehaviour
{

    [SerializeField] private float changeDelay = 3f;

    private MeshRenderer cubeMR;
    private Material texture;
    private Color colorDifference;

    private Vector3 rotationDifference;
    //[SerializeField] private Quaternion currentRotation;
    [SerializeField] private Vector3 eulerRotation;


    // Start is called before the first frame update
    void Start()
    {

        //Calculate();
        cubeMR = GetComponent<MeshRenderer>();
        texture = cubeMR.material;
        texture.color = GetRandomColor();

        StartCoroutine(ChangeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentColor();
        UpdateRotation();
    }

    IEnumerator ChangeRoutine()
    {
        while (true)
        {
            GetRandomColor();
            GetRandomRotation();
            yield return new WaitForSeconds(changeDelay);
        }
    }

    private Color GetRandomColor()
    {
        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        colorDifference = randomColor - texture.color;
        return randomColor;
    }

    private Vector3 GetRandomRotation()
    {
        Vector3 randomRotation = new Vector3 (Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

        rotationDifference = (randomRotation - eulerRotation);

        return randomRotation;
    }

    private void UpdateCurrentColor()
    {
        float colorTime = Time.deltaTime / changeDelay;
        texture.color = texture.color + colorDifference * colorTime;
    }

    private void UpdateRotation()
    {

        transform.Rotate(eulerRotation * Time.deltaTime / changeDelay);
        eulerRotation = (eulerRotation + (rotationDifference * Time.deltaTime / changeDelay ));
    }

    private void Calculate()
    {
        double result = 1;
        
        for(float i = 20; i > 0; i--)
        {
            result *= (i / (49 - i));
            Debug.Log("Current number: " + i + " :" + 1/result);
        }
        Debug.Log("Result: " + result);
        Debug.Log(1 / result);
    }
}
