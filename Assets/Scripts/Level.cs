using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelName = "Level 1: The S Road";
    public string description = "An S-shaped road that is well-suited for defense. The beautiful scenery is a great view to enjoy when you finish killing all your enemies.";
    public Sprite sprite;
    public GlobalSettings.Difficulty difficulty;
    string wavesFile;
    string scenePath;
    public int levelId = 1;
    public bool unlocked = false;
    public int numTimesPlayed = 0;
    public int bestScore = 0;
    public float bestTime = 0.0f;

    void Start()
    {
        wavesFile = "level" + levelId.ToString();
        scenePath = "Scenes/Levels/Level" + levelId.ToString();
    }
    
    public string GetWavesFile() { return wavesFile; }
    public string GetScenePath() { return scenePath; }
}
