using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameController : MonoBehaviour
{
    /* static variables */ 
    public static string WAVE_FILE_EXT = ".txt";
    public static string EASY_SUFFIX = "_easy";
    public static string NOMRAL_SUFFIX = "_norma";
    public static string HARD_SUFFIX = "_hard";
    public static Level level = Level.DEFAULT;   // NOTE: This should be set before loading the scene
    public static GameController INSTANCE;
    public enum GameState {Playing, Paused, Lost, Won};


    /* UI elements */
    public UI_TopBar topBar;
    public GameObject panel_gameLost;
    public GameObject panel_gameWon;
    public GameObject panel_pause;

    /* public stats */
    public int numWaves;
    public int gold;
    public int wave;
    public int maxHp;
    public float waveCd;
    /* private stats */
    float goldGainMultiplier = 1.0f; // Gold gains should be multiplied by this
    string wavesFilename = "level1";
    int hp;
    float waveCountdown;
    string wavesFilePath;

    public GameState gameState;
    public Base myBase;
    public WaveParser waveParser;
    public EnemySpawner enemySpawner;
    private List<Wave> waves;
    public HashSet<GameUnit> gameUnits = new HashSet<GameUnit>();
    public UnitRange pf_unitRange;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        if (level == null) 
        {
            level = Level.DEFAULT;
        }
        Init();
        gameState = GameState.Playing;
    }
    
    // Update is called once per frame
    void Update()
    {
        HandleWave();
        HandleGameOver();
    }

    void Init()
    {
        // NOTE: Remove the follwing line after finishing developing
        // It is only for dealing with clearing PlayerPrefs during development
        GlobalSettings.oneTime = false;

        enemySpawner.ClearAll();
        level = Level.DEFAULT;
        ParseFileWaves(level);
        gameState = GameState.Playing;

        InitUI();
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

    void ParseFileWaves(Level level)
    {
        // Parse the txt file containing info about waves
        // Then set values of variables
        // The path of the txt file is given in level as its member variable
        wavesFilename = level.GetWavesFile() + DifficultyToFileSuffix(level.difficulty);
        wavesFilePath = Application.dataPath + "/Waves/" + wavesFilename;

        Debug.Log($"Start parsing file: {wavesFilePath}");
        waveParser.ParseFileWaves(wavesFilePath);
        Debug.Log("Done parsing");
        
        // get initial game data from WaveParser
        gold = waveParser.GetGold();
        maxHp = waveParser.GetHp();
        waveCd = waveParser.GetWaveCd();
        waves = waveParser.GetWaves();
        // set remaining initial game data
        numWaves = waves.Count;
        hp = maxHp;   // full health by default
        wave = 0;
        waveCountdown = waveCd;

        myBase.SetHp(hp);

        Debug.Log($"numWaves = {numWaves}\ngold = {gold}\nhp = {hp}\nwaveCd = {waveCd}");
    }

    string DifficultyToFileSuffix(GlobalSettings.Difficulty difficulty)
    {
        // Returns:
        //      File suffix corresponding to the difficulty.
        //      If not corresponding suffix is found, returns EASY_SUFFIX

        string ret;
        switch (difficulty) 
        {
            case GlobalSettings.Difficulty.Easy:
                ret = EASY_SUFFIX;
                break;
            case GlobalSettings.Difficulty.Normal:
                ret = NOMRAL_SUFFIX;
                break;
            case GlobalSettings.Difficulty.Hard:
                ret = HARD_SUFFIX;
                break;
            default:
                ret = EASY_SUFFIX;
                break;
        }
        return ret + WAVE_FILE_EXT;
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
        topBar.SetTextHp(myBase.GetHp(), maxHp);
        topBar.SetTextWave(wave);
        topBar.SetTextCountdown(waveCountdown);
    }

    // start next wave
    public void NextWave()
    {
        if (!IsGamePlaying()) return;
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
    public bool CanBuyGameUnit(GameUnit unit) 
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

    public void GainGold(int amount, bool absVal = false) 
    {
        // Parameter:
        //      amount:
        //          The amount to be gained
        //
        //      absVal:
        //          If absVal is true, the amount will not be multiplied by multiplier
        float gainAmount = amount;
        if (!absVal)
        {

            gainAmount *= goldGainMultiplier;
        }
        int gainInt = Mathf.RoundToInt(gainAmount);
        gold += gainInt;
        if (gainInt != 0) {
            Debug.Log($"Gains {gainAmount} gold");
            UpdateUiStats();
            topBar.OnGainGold(gainInt);
        }
    }

    public void AddGainGoldMultiplier(float amount)
    {
        // Increase Gain Gold Multiplier by amount
        goldGainMultiplier += amount;
    }

    public void HandleGameOver() 
    {
        if (!IsGamePlaying()) return;
        if (IsGameLost())
        {
            OnGameLost();
        } 
        else if (IsGameWon()) 
        {
            OnGameWon();
        }
    }

    ////////////////////////////////////////////
    //           Change Game State
    ////////////////////////////////////////////

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
        hp = 100;
        foreach (GameUnit u in gameUnits) {
            if (u != null) 
            {
                Destroy(u.gameObject);
            }
        }
        gameUnits.Clear();
        BuildingManager.INSTANCE.Init();
        Init();
    }

    //////////////////////////////////////////
    //        End Change Game State
    //////////////////////////////////////////

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
        if (gameState == GameState.Lost) 
        {
            return true;
        }
        if (myBase.GetHp() <= 0)
        {
            return true;
        }
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
    /////////////////////////////////////////////

    /////////////////////////////////////////////
    //         Event Listeners
    /////////////////////////////////////////////

    public void OnWaveStart()
    {
        ++wave;
        UpdateUiStats();
    }

    public void OnGameLost()
    {
        gameState = GameState.Lost;
        panel_gameLost.SetActive(true);
        BuildingManager.INSTANCE.StopProduction();
    }

    public void OnGameWon()
    {
        gameState = GameState.Won;
        panel_gameWon.SetActive(true);
        PlayerPrefs.SetInt("MaxLevelCompleted", level.levelId);
        BuildingManager.INSTANCE.StopProduction();
    }

    public void OnBuyGameUnit(GameUnit gameUnit)
    {
        gameUnit.OnBuy();
        int spendAmount = gameUnit.cost;
        gameUnits.Add(gameUnit);
        if (spendAmount != 0)
        {
            gold -= gameUnit.cost;
            topBar.onSpendGold(spendAmount);
            UpdateUiStats();
        }
    }

    public void OnSellGameUnit(GameUnit t)
    {
        GainGold(t.sellWorth);
        UpdateUiStats();
    }
    
    /////////////////////////////////////////////
    //       End Event Listeners
    /////////////////////////////////////////////
}
