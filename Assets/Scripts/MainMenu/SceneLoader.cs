using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader INSTANCE;

    void Awake() => INSTANCE = this;

    public void LoadLevel(Level level)
    {
        GlobalSettings.level = level;
        GameController.level = level;
        SceneManager.LoadScene(level.GetScenePath());
    }
}
