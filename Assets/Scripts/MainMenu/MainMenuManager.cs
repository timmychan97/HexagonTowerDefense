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
    string curScenePath;
    GlobalSettings.Difficulty curDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        // GenLevelBtns();
        HideMenus();
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
        var dirInfo = new DirectoryInfo("Assets/" + path_levelScenes);
        var allFileInfos = dirInfo.GetFiles("*.unity", SearchOption.AllDirectories);
        int n = 1;
        foreach (var fileInfo in allFileInfos)
        {
            Btn_LoadLevel btn = Instantiate(pf_btnLoadLevel, panel_selectLevel.transform);
            btn.scenePath = path_levelScenes + fileInfo.Name.Substring(0, fileInfo.Name.Length - 6);
            btn.SetText(n.ToString());
            ++n;
            Debug.Log(fileInfo.Name);
        }
    }

    public void OnClickedStartGame()
    {
        if (panel_selectMode.gameObject.activeInHierarchy)
        {
            HideMenus();
        }
        else
        {
            HideMenus();
            panel_selectMode.Show();
        }
    }

    public void OnClickedOptions()
    {
        if (panel_options.activeInHierarchy)
        {
            HideMenus();
        }
        else
        {
            HideMenus();
            panel_options.SetActive(true);
        }
    }

    public void OnSelectLevel(Btn_LoadLevel btn)
    {
        curScenePath = btn.scenePath;
        PromptDifficulty();
    }

    public void PromptDifficulty()
    {
        panel_selectDifficulty.Show();
    }

    public void OnSelectDifficulty(GlobalSettings.Difficulty d)
    {
        SceneLoader.INSTANCE.LoadScene(curScenePath, d);
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
