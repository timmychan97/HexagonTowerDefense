using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PanelPause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnResumeClicked()
    {
        GameController.INSTANCE.ResumeGame();
    }

    public void OnRestartClicked()
    {
        GameController.INSTANCE.RestartGame();
    }

    public void OnBackToMainMenuClicked()
    {
        GameController.INSTANCE.BackToMainMenu();
    }
}
