using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    public static Level level;
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
    public HashSet<Unit> units = new HashSet<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        if (level == null) 
        {
            level = Level.DEFAULT;
        }
        Init();
        ParseFileWaves(level);
        InitUI();
        gameState = GameState.Playing;
    }

    void Init()
    {
        enemySpawner.ClearAll();
        Time.timeScale = 1.0f;
    }

    void InitUI()
    {
        // init UI elements
        panel_gameLost.SetActive(false);
        panel_gameWon.SetActive(false);
        panel_pause.SetActive(false);
        UpdateUiStats();
    }

    // Parse the txt file containing info about waves
    // Then set values of variables
    // The path of the txt file is given in level as its member variable
    void ParseFileWaves(Level level)
    {
        wavesFilename = "level1_easy.txt"; // Default txt file name
        if (level != null) 
        {
            if (level.difficulty == GlobalSettings.Difficulty.Easy) 
            {
                wavesFilename = level.GetWavesFile() + "_easy.txt";
            }
            else if (level.difficulty == GlobalSettings.Difficulty.Normal)
            {
                wavesFilename = level.GetWavesFile() + "_normal.txt";
            }
            else
            {
                wavesFilename = level.GetWavesFile() + "_hard.txt";
            }
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
    public bool CanBuyUnit(Unit unit) 
    {
        if (!IsGamePlaying())
        {
            return false;
        }
        if (gold < unit.cost) // not enough gold
        {
            topBar.OnNotEnoughGold();
            return false;
        }
        return true;
    }
    
    public void OnBuyUnit(Unit unit)
    {
        gold -= unit.cost;
        topBar.onSpendGold();
        UpdateUiStats();
        units.Add(unit);
        // units.Add(Tile.active.GetUnit());
        // Debug.Log(units.Count);
    }

    public void OnSellUnit(Unit t)
    {
        gold += t.sellWorth;
        UpdateUiStats();
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

    public void OnGameLost()
    {
        gameState = GameState.Lost;
        panel_gameLost.SetActive(true);
    }

    public void OnGameWon()
    {
        gameState = GameState.Won;
        panel_gameWon.SetActive(true);
        PlayerPrefs.SetInt("MaxLevelCompleted", level.levelId);
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

    ///////////////////////////////////////////////////
    //              Pause Game
    ///////////////////////////////////////////////////

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
    
    public void BackToMainMenu() 
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartGame()
    {
        foreach (Unit u in units) {
            if (u != null) 
            {
                Debug.Log(u.transform.position);
                Destroy(u.gameObject);
            }
        }
        units.Clear();
        Init();
        ParseFileWaves(level);
        InitUI();
        ResumeGame();
    }

    //////////////////////////////////////////
    //         Game State Checks
    //////////////////////////////////////////

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

    /////////////////////////////////////////////
    //       End Game State Checks
    ////////////////////////////////////////////
}
