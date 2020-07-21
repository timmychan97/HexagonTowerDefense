using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;
    public enum GameState {Playing, Paused, Lost, Won};
    public GameState gameState;
    public UI_TopBar topBar;
    public Base myBase;
    public GameObject panel_gameLost;
    public GameObject panel_gameWon;
    public GameObject panel_pause;
    public int numRounds;
    public int gold;
    public int round;
    public int hp;
    public RoundParser roundParser;
    public EnemySpawner enemySpawner;
    private List<Round> rounds;
    public string roundsFilename = "level1.txt";
    string pathFileRounds;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;

        // parse txt file containing info about rounds
        pathFileRounds = Application.dataPath + "/Rounds/" + roundsFilename;
        Debug.Log($"Start parsing file: {pathFileRounds}");
        roundParser.ParseFileRounds(pathFileRounds);
        Debug.Log("Done parsing");
        
        // set initial game data
        gold = roundParser.GetGold();
        hp = roundParser.GetHp();
        numRounds = roundParser.GetNumRounds();
        rounds = roundParser.GetRounds();
        round = 0;
        
        Debug.Log($"numRounds = {numRounds}");
        Debug.Log($"gold = {gold}");
        Debug.Log($"hp = {hp}");

        // init UI elements
        panel_gameLost.SetActive(false);
        panel_gameWon.SetActive(false);
        panel_pause.SetActive(false);
        gameState = GameState.Playing;
        UpdateUiStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // UpdateUiStats();
        HandleGameOver();
    }

    public void UpdateUiStats()
    {
        topBar.SetTextGold(gold);
        topBar.SetTextHp(myBase.getHp());
        topBar.SetTextRound(round);
    }

    // start next round
    public void NextRound()
    {
        if (round < rounds.Count)
        {
            enemySpawner.StartRound(rounds[round]);
            round++;
            UpdateUiStats();
        }
    }

    // returns false when fails to buy tower (no money)
    public bool BuyTower(Tower unit) 
    {
        if (gold < unit.cost) return false;

        gold -= unit.cost;
        UpdateUiStats();
        return true;
    }

    // returns false if fails to damage castle

    public void OnGameLost()
    {
        gameState = GameState.Lost;
        panel_gameLost.SetActive(true);
    }

    public void OnGameWon()
    {
        gameState = GameState.Won;
        panel_gameWon.SetActive(true);
    }

    public void HandleGameOver() 
    {
        if (IsGameLost())
        {
            OnGameLost();
        } 
        else if (IsGameWon()) 
        {
            OnGameWon();
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

    public bool IsGameLost()
    {
        if (myBase.getHp() <= 0) return true;
        return false;
    }

    public bool IsGameWon()
    {
        if (round == rounds.Count && enemySpawner.GetEnemies().Count == 0) 
        {
            return true;
        }
        return false;
    }
    public void BackToMainMenu() 
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GainReward(int _money) 
    {
        gold += _money;
        UpdateUiStats();
    }

    public void OnRoundStart()
    {
        ++round;
        UpdateUiStats();
    }
}
