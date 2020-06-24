using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject panel_mainMenu;

    private string scene_lvl0 = "Scenes/TestScenes/Level0Test"; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() 
    {
        Debug.Log("Go to scene: 'Level 0 Test'");
        SceneManager.LoadScene(scene_lvl0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
