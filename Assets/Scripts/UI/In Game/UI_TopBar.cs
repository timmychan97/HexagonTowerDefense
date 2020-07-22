using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_TopBar : MonoBehaviour
{
    Color red = new Color(1.0f, 0.3f, 0.3f);
    Color white = Color.white;
    public Text textGold;
    public Text textHp;
    public Text textWave;
    public Text textCountdown;

    public void SetTextGold(int n) => textGold.text = n.ToString();

    public void SetTextHp(int n) => textHp.text = n.ToString();

    public void SetTextWave(int n) => textWave.text = n.ToString();

    public void SetTextCountdown(float f) => textCountdown.text = Mathf.Ceil(f).ToString();

    public void OnNotEnoughGold()
    {
        StartCoroutine(ShowNotEnoughGold());
    }

    IEnumerator ShowNotEnoughGold()
    {
        textGold.color = red;
        textGold.fontSize = 18;
        textGold.fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(0.3f);
        textGold.color = white;
        textGold.fontSize = 14;
        textGold.fontStyle = FontStyle.Normal;
    }

    public void OnBtnNextWaveClicked()
    {
        GameController.INSTANCE.NextWave();
    }

    public void OnBtnPauseClicked()
    {
        GameController.INSTANCE.PauseGame();
    }
}
