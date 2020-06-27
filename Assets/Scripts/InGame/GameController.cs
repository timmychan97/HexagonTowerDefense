using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;
    public enum GameState {Playing, Paused, Lost, Won};
    public GameState gameState;
    public Base myBase;
    public GameObject panel_gameLost;
    public GameObject panel_pause;

    public int money;
    public int round;

    public Text textMoney;
    public Text textHp;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        panel_gameLost.SetActive(false);
        panel_pause.SetActive(false);
        gameState = GameState.Playing;

        // init stats
        round = 1;
        // money = 500;
        UpdateUiStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        UpdateUiStats();
        HandleGameOver();
    }

    void UpdateUiStats()
    {
        textMoney.text = money.ToString();
        textHp.text = myBase.getHp().ToString();
    }

    // returns false when fails to buy tower (no money)
    public bool BuyTower(Tower tower) 
    {
        if (money < tower.cost) return false;

        money -= tower.cost;
        UpdateUiStats();
        return true;
    }

    // returns false if fails to damage castle

    public void GameOver()
    {
        gameState = GameState.Lost;
        panel_gameLost.SetActive(true);
    }

    public void HandleGameOver() 
    {
        if (IsGameOver())
        {
            GameOver();
        }
    }

    public void TogglePause()
    {
        if (gameState == GameState.Paused) 
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameState = GameState.Paused;
        panel_pause.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameState = GameState.Playing;
        panel_pause.SetActive(false);
    }

    public bool IsGameOver()
    {
        if (myBase.getHp() <= 0) return true;
        return false;
    }

    public void BackToMainMenu() 
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GainReward(int _money) 
    {
        money += _money;
        UpdateUiStats();
    }
}
