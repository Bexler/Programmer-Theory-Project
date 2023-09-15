using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;

    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        playerName = MainManager.Instance.playerName;
        UpdateNameText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateNameText()
    {
        nameText.text = playerName;
    }
}
