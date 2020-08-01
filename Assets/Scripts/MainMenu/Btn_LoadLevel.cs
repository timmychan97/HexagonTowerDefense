using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Btn_LoadLevel : Btn_MainMenu
{
    Button btn;
    public int levelId;
    public string levelName;
    public string levelDescription;
    public Sprite levelSprite;

    public GameObject lockedFilter;
    public bool unlocked;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        text.text = levelName;
        HandleLock();
    }

    public void OnClick()
    {
        if (unlocked) 
        {
            Level level = new Level(levelName, levelDescription, levelId);
            MainMenuManager.INSTANCE.OnLevelSelected(level);
        }
    }

    void HandleLock()
    {
        int maxLevelCompleted = PlayerPrefs.GetInt("MaxLevelCompleted", 0); // level ID starts from 1
        unlocked = maxLevelCompleted + 1 >= levelId;
        lockedFilter.SetActive(!unlocked);
    }

    public void SetLevel(Level level) 
    {
        text.text = level.levelName;
    }
}
