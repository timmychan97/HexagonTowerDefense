using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager INSTANCE;
    public GameObject panel_mainMenu;
    public Panel_SelectMode panel_selectMode;
    public Panel_SelectDifficulty panel_selectDifficulty;
    public Panel_Options panel_options;
    public Btn_LoadLevel pf_btnLoadLevel;
    private string path_levelScenes = "Scenes/Levels/";
    Level curLevel;
    GlobalSettings.Difficulty curDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        ClearData();    // TURN THIS OFF WHEN BUILDING PROJECT!!!

        HideMenus();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Clear all player data, when developing, this is to make sure completed levels
    // are not registered as completed.
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    // hide all menus except main menu
    void HideMenus()
    {
        panel_options.Hide();
        panel_selectMode.Hide();
        panel_selectDifficulty.Hide();
    }

    public void PromptDifficulty()
    {
        Debug.Log("PromptDiff");
        panel_selectDifficulty.Show();
    }

    public void OnDifficultySelected(GlobalSettings.Difficulty d)
    {
        curLevel.difficulty = d;
        SceneLoader.INSTANCE.LoadLevel(curLevel);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    ///////////////////////////////////////////
    //           Event Listeners
    ///////////////////////////////////////////
    public void OnStartGameClicked()
    {
        if (panel_selectMode.gameObject.activeInHierarchy) 
        {
            // HideMenus();
            panel_selectMode.HideWithAnimation();
        } 
        else 
        {
            HideMenus();
            panel_selectMode.Show();
        }
    }

    public void OnOptionsClicked()
    {
        if (panel_options.gameObject.activeInHierarchy)
        {
            panel_options.HideWithAnimation();
        }
        else
        {
            HideMenus();
            panel_options.Show();
        }
    }

    public void OnLevelSelected(Level level)
    {
        curLevel = level;
        PromptDifficulty();
    }

    public void OnQuitGameClicked()
    {
        QuitGame();
    }
}
