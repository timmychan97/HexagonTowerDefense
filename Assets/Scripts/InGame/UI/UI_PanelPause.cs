using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PanelPause : MonoBehaviour
{
    public void OnResumeClicked() => GameController.INSTANCE.ResumeGame();
    public void OnRestartClicked() => GameController.INSTANCE.RestartGame();
    public void OnBackToMainMenuClicked() => GameController.INSTANCE.BackToMainMenu();
}
