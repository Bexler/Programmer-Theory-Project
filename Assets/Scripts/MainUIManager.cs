using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private GameObject gameOverPanel;

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
        nameText.text = "Player name: " + playerName;
    }

    public void UpdateGoldText(int gold)
    {
        goldText.text = "" + gold + 'g';
    }

    public void UpdatePlayerHealthSlider(float health)
    {
        playerHealthSlider.value = health;
    }

    public void UpdateGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
