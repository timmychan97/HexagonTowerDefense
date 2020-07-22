using UnityEngine;
using UnityEngine.UI;

public class UI_TopBar : MonoBehaviour
{
    public Text textGold;
    public Text textHp;
    public Text textRound;
    public Text textCountdown;

    public void SetTextGold(int n) => textGold.text = n.ToString();

    public void SetTextHp(int n) => textHp.text = n.ToString();

    public void SetTextRound(int n) => textRound.text = n.ToString();

    public void SetTextCountdown(float f) => textCountdown.text = Mathf.Ceil(f).ToString();

    public void OnBtnNextWaveClicked()
    {
        GameController.INSTANCE.NextRound();
    }

    public void OnBtnPauseClicked()
    {
        GameController.INSTANCE.PauseGame();
    }
}
