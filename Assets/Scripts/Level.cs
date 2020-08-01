using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level DEFAULT = new Level("Level 1", 
                                            "Default Level", 
                                            GlobalSettings.Difficulty.Easy,
                                            "level1", 
                                            "Scenes/Levels/Level1", 
                                            1);
    public string levelName = "Level 1: The S Road";
    public string description = "An S-shaped road that is well-suited for defense. The beautiful scenery is a great view to enjoy when you finish killing all your enemies.";
    public Sprite sprite;
    public GlobalSettings.Difficulty difficulty;
    string wavesFile;
    string scenePath;
    public int levelId = 1;

    void Start()
    {
        wavesFile = "level" + levelId.ToString();
        scenePath = "Scenes/Levels/Level" + levelId.ToString();
    }

    public Level(string _name, string _description, GlobalSettings.Difficulty _difficulty,
                 string _wavesFile, string _scenePath, int _id)
    {
        levelName = _name;
        description = _description;
        difficulty = _difficulty;
        wavesFile = _wavesFile;
        scenePath = _scenePath;
        levelId = _id;
    }
    
    public string GetWavesFile() { return wavesFile; }
    public string GetScenePath() { return scenePath; }
}
