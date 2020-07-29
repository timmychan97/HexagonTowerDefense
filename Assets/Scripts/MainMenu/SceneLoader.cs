using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader INSTANCE;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string scenePath, GlobalSettings.Difficulty difficulty) 
    {
        GameController.difficulty = difficulty;
        SceneManager.LoadScene(scenePath);
    }
}
