using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager INSTANCE;
    public GameObject panel_mainMenu;
    public MainMenuPanel panel_selectMode;
    public Panel_SelectDifficulty panel_selectDifficulty;
    public GameObject panel_selectLevel;
    public GameObject panel_options;
    public Btn_LoadLevel pf_btnLoadLevel;
    private string path_levelScenes = "Scenes/Levels/";
    Level curLevel;
    GlobalSettings.Difficulty curDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        // GenLevelBtns();

        HideMenus();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // hide all menus except main menu
    void HideMenus()
    {
        panel_options.SetActive(false);
        panel_selectLevel.SetActive(false);
        panel_selectMode.Hide();
        panel_selectDifficulty.Hide();
    }

    public void GenLevelBtns()
    {
        List<Level> levels = LevelManager.INSTANCE.GetLevels();
        foreach (Level level in levels)
        {
            Btn_LoadLevel btn = Instantiate(pf_btnLoadLevel, panel_selectLevel.transform);
            btn.SetLevel(level);
        }
    }

    public void OnStartGameClicked()
    {
        if (panel_selectMode.gameObject.activeInHierarchy)
        {
            HideMenus();
        }
        panel_selectMode.Show();
    }

    public void OnClickedOptions()
    {
        if (panel_options.activeInHierarchy)
        {
            HideMenus();
        }
        panel_options.SetActive(true);
    }

    public void OnLevelSelected(Level level)
    {
        curLevel = level;
        PromptDifficulty();
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
}
