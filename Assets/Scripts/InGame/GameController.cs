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

    public GameObject panel_gameLost;

    public int money;
    public int hp;
    public int maxHp;
    public int round;

    public Text textMoney;
    public Text textHp;

    // Start is called before the first frame update
    void Start()
    {
        panel_gameLost.SetActive(false);
        gameState = GameState.Playing;
        INSTANCE = this;

        // init stats
        round = 1;
        // money = 500;
        maxHp = 50;
        hp = maxHp;
        UpdateUiStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUiStats()
    {
        textMoney.text = money.ToString();
        textHp.text = hp.ToString();
    }

    // returns false when fails to buy tower (no money)
    public bool BuyTower(Tower tower) 
    {
        if (money < tower.cost) return false;

        money -= tower.cost;
        UpdateUiStats();
        HandleGameOver();
        return true;
    }

    // returns false if fails to damage castle
    public bool DamageCastle(int damage) 
    {
        hp -= damage;
        if (hp < 0) hp = 0;
        UpdateUiStats();
        HandleGameOver();
        return true;
    }

    public void HandleGameOver() 
    {
        if (IsGameOver())
        {
            panel_gameLost.SetActive(true);
        }
    }

    public bool IsGameOver()
    {
        if (hp <= 0) return true;
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
