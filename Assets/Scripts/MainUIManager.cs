using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MainUIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausedPanel;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button pausedIcon;
    

    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        playerName = MainManager.Instance.playerName;
        UpdateNameText();
        UpdateScoreText(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateNameText()
    {
        nameText.text = "Player: " + playerName;
    }

    public void UpdateGoldText(int gold)
    {
        goldText.text = "" + gold + 'g';
    }

    public void UpdatePlayerHealthSlider(float health)
    {
        playerHealthSlider.value = health;
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        UpdateGameOverScoreText();
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = "Wave: " + wave;
    }

    private void UpdateGameOverScoreText()
    {
        gameOverScoreText.text = "Your " + scoreText.text;
    }

    public void UpdatePausedUI(bool isPaused)
    {
        pausedIcon.gameObject.SetActive(!isPaused);
        pausedPanel.SetActive(isPaused);
    }

    public void DisableSellButton()
    {
        sellButton.gameObject.SetActive(false);
    }

    public void EnableSellButton()
    {
        sellButton.gameObject.SetActive(true);
    }
}
