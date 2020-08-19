using System.Collections;
using System.Collections.Generic;

public class Level
{
    public static Level DEFAULT = new Level("Level 1", 
                                            "Default Level", 
                                            1,
                                            GlobalSettings.Difficulty.Easy,
                                            "level1", 
                                            "Scenes/Levels/Level1");
    public string levelName = "Level 1: The S Road";
    public string description = "An S-shaped road that is well-suited for defense. The beautiful scenery is a great view to enjoy when you finish killing all your enemies.";
    public int levelId;
    public GlobalSettings.Difficulty difficulty;
    string wavesFile;
    string scenePath;

    public Level(string _name, 
                 string _description, 
                 int _id,
                 GlobalSettings.Difficulty _difficulty,
                 string _wavesFile, 
                 string _scenePath)
    {
        levelName = _name;
        description = _description;
        difficulty = _difficulty;
        wavesFile = _wavesFile;
        scenePath = _scenePath;
        levelId = _id;
    }

    public Level(string _name, 
                 string _description, 
                 int _id,
                 GlobalSettings.Difficulty _difficulty = GlobalSettings.Difficulty.Easy)
    {
        levelName = _name;
        description = _description;
        levelId = _id;
        difficulty = _difficulty;
        wavesFile = "level" + levelId.ToString();
        scenePath = "Scenes/Levels/Level" + levelId.ToString();
    }

    public string GetWavesFile() => wavesFile;
    public string GetScenePath() => scenePath;
}
