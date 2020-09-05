using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelGameWon : MonoBehaviour
{
    public void OnReplayClicked() => GameController.INSTANCE.RestartGame();

    public void OnQuitClicked() => GameController.INSTANCE.BackToMainMenu();
}
