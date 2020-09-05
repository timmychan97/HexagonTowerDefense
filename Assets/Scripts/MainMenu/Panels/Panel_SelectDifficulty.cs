using UnityEngine;
using UnityEngine.UI;
public class Panel_SelectDifficulty : MonoBehaviour
{
    public Button btnEasy;
    public Button btnNormal;
    public Button btnHard;

    public void OnCancel() => Hide();
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void OnClickEasy() => MainMenuManager.INSTANCE.OnDifficultySelected(GlobalSettings.Difficulty.Easy);
    public void OnClickNormal() => MainMenuManager.INSTANCE.OnDifficultySelected(GlobalSettings.Difficulty.Normal);
    public void OnClickHard() => MainMenuManager.INSTANCE.OnDifficultySelected(GlobalSettings.Difficulty.Hard);
}
