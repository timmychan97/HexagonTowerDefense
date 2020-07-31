using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static GlobalSettings.Difficulty difficulty;
    public static GameController INSTANCE;
    public enum GameState {Playing, Paused, Lost, Won};
    public GameState gameState;
    public UI_TopBar topBar;
    public Base myBase;
    public GameObject panel_gameLost;
    public GameObject panel_gameWon;
    public GameObject panel_pause;
    public int numWaves;
    public int gold;
    public int wave;
    public int hp;
    public WaveParser waveParser;
    public EnemySpawner enemySpawner;
    private List<Wave> waves;
    public string wavesFilename = "level1";
    string pathFileWaves;
    public float waveCd;
    float waveCountdown;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;

        // parse txt file containing info about waves
        if (difficulty == GlobalSettings.Difficulty.Easy) 
        {
            wavesFilename += "_easy.txt";
        }
        else if (difficulty == GlobalSettings.Difficulty.Normal)
        {
            wavesFilename += "_normal.txt";
        }
        else
        {
            wavesFilename += "_hard.txt";
        }
        pathFileWaves = Application.dataPath + "/Waves/" + wavesFilename;
        Debug.Log($"Start parsing file: {pathFileWaves}");
        waveParser.ParseFileWaves(pathFileWaves);
        Debug.Log("Done parsing");
        
        // set initial game data
        gold = waveParser.GetGold();
        hp = waveParser.GetHp();
        numWaves = waveParser.GetNumWaves();
        waveCd = waveParser.GetWaveCd();
        waves = waveParser.GetWaves();
        wave = 0;
        waveCountdown = waveCd;
        
        // Debug.Log($"numWaves = {numWaves}");
        // Debug.Log($"gold = {gold}");
        // Debug.Log($"hp = {hp}");
        // Debug.Log($"waveCd = {waveCd}");

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
        HandleWave();
    }

    void LateUpdate()
    {
        // UpdateUiStats();
        HandleGameOver();
    }

    void HandleWave()
    {
        // Starts next wave automatically when countdown reaches zero
        if (wave == waves.Count) return; // has reached last wave => don't countdown
        waveCountdown -= Time.deltaTime;
        UpdateUiStats();
        if (waveCountdown < 0) 
        {
            NextWave();
        }
    }

    public void UpdateUiStats()
    {
        topBar.SetTextGold(gold);
        topBar.SetTextHp(myBase.getHp());
        topBar.SetTextWave(wave);
        topBar.SetTextCountdown(waveCountdown);
    }

    // start next wave
    public void NextWave()
    {
        if (wave < waves.Count)
        {
            enemySpawner.StartWave(waves[wave]);
            wave++;
            if (wave < waves.Count) 
            {
                waveCountdown = waveCd;
            }
            else
            {
                waveCountdown = 0;
            }
            UpdateUiStats();
        }
    }

    // returns false when fails to buy tower (no money)
    public bool BuyTower(Tower unit) 
    {
        if (gold < unit.cost) 
        {
            topBar.OnNotEnoughGold();
            return false;
        }

        gold -= unit.cost;
        topBar.onSpendGold();
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

    public bool IsGamePlaying()
    {
        return gameState == GameState.Playing;
    }

    public bool IsGamePaused()
    {
        return gameState == GameState.Paused;
    }

    public bool IsGameLost()
    {
        if (myBase.getHp() <= 0) return true;
        return false;
    }

    public bool IsGameWon()
    {
        if (wave == waves.Count && enemySpawner.GetEnemies().Count == 0 
            && enemySpawner.GetOngoingWaves().Count == 0) 
        {
            return true;
        }
        return false;
    }
    public void BackToMainMenu() 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");
    }

    public void GainReward(int _gold) 
    {
        gold += _gold;
        UpdateUiStats();
        topBar.OnGainGold();
    }

    public void OnWaveStart()
    {
        ++wave;
        UpdateUiStats();
    }

    public void OnSellTower(Tower t)
    {
        gold += t.sellWorth;
        UpdateUiStats();
    }
}
