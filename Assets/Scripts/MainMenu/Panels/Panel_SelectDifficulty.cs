using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Panel_SelectDifficulty : MonoBehaviour
{
    public Button btnEasy;
    public Button btnNormal;
    public Button btnHard;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCancel()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnClickEasy()
    {
        MainMenuManager.INSTANCE.OnDifficultySelected(GlobalSettings.Difficulty.Easy);
    }
    public void OnClickNormal()
    {
        MainMenuManager.INSTANCE.OnDifficultySelected(GlobalSettings.Difficulty.Normal);
    }
    public void OnClickHard()
    {
        MainMenuManager.INSTANCE.OnDifficultySelected(GlobalSettings.Difficulty.Hard);
    }
}
