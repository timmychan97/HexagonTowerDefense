using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject panel_mainMenu;
    public GameObject panel_selectMode;
    public GameObject panel_selectLevel;
    public GameObject panel_options;
    public Btn_LoadLevel pf_btnLoadLevel;
    private string path_levelScenes = "Scenes/Levels/";
    private string scene_lvl0 = "Scenes/TestScenes/DonnyScene"; 

    // Start is called before the first frame update
    void Start()
    {
        GenLevelBtns();
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
        panel_selectMode.SetActive(false);
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

    public void OnClickedCampaign()
    {
        if (panel_selectLevel.activeInHierarchy)
        {
            panel_selectLevel.SetActive(false);
        }
        else
        {
            panel_selectLevel.SetActive(true);
        }
    }

    public void OnClickedStartGame()
    {
        if (panel_selectMode.activeInHierarchy)
        {
            HideMenus();
        }
        else
        {
            HideMenus();
            panel_selectMode.SetActive(true);
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
