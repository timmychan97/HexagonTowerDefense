using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Btn_LoadLevel : Btn_MainMenu
{
    Button btn;
    public Level level;
    public GameObject lockedFilter;
    public bool unlocked;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        text.text = level.levelName;
        HandleLock();
    }

    public void OnClick()
    {
        if (unlocked) 
        {
            MainMenuManager.INSTANCE.OnLevelSelected(level);
        }
    }

    void HandleLock()
    {
        int maxLevelCompleted = PlayerPrefs.GetInt("MaxLevelCompleted", 0); // level ID starts from 1
        unlocked = maxLevelCompleted+1 >= level.levelId;
        lockedFilter.SetActive(!unlocked);
    }

    public void SetLevel(Level level) 
    {
        text.text = level.levelName;
    }
}
