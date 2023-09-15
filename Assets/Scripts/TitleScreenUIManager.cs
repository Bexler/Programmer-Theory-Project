using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleScreenUIManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField nameInputField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputPlayerName()
    {
        MainManager.Instance.SetPlayerName(nameInputField.text);
    }



}
