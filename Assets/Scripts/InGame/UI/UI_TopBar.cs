using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_TopBar : MonoBehaviour
{
    Color red = Color.red;
    Color green = Color.green;
    Color yellow = Color.yellow;
    Color white = Color.white;
    public Text textGold;
    public Text textHp;
    public Text textWave;
    public Text textCountdown;

    public void SetTextGold(int n) => textGold.text = n.ToString();

    public void SetTextHp(int hp, int maxHp) 
    {
        textHp.text = $"{hp}/{maxHp}";
    }

    public void SetTextWave(int n) => textWave.text = n.ToString();

    public void SetTextCountdown(float f) => textCountdown.text = Mathf.Ceil(f).ToString();

    public void OnNotEnoughGold()
    {
        StartCoroutine(ShowNotEnoughGold());
    }

    public void OnGainGold(int amount)
    {
        StartCoroutine(ShowGainGold(amount));
    }

    public void onSpendGold(int amount)
    {
        StartCoroutine(ShowSpendGold(amount));
    }

    IEnumerator ShowSpendGold(int amount)
    {
        textGold.color = yellow;
        textGold.fontSize = 18;
        textGold.fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(0.3f);
        textGold.color = white;
        textGold.fontSize = 14;
        textGold.fontStyle = FontStyle.Normal;
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

    IEnumerator ShowGainGold(int amount)
    {
        textGold.color = green;
        textGold.fontSize = 18;
        textGold.fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(0.3f);
        textGold.color = white;
        textGold.fontSize = 14;
        textGold.fontStyle = FontStyle.Normal;
    }

    public void OnBtnNextWaveClicked()
    {
        if (GameController.INSTANCE.IsGamePlaying()) 
        {
            GameController.INSTANCE.NextWave();
        }
    }

    public void OnBtnPauseClicked()
    {
        if (GameController.INSTANCE.IsGamePlaying()) 
        {
            
            GameController.INSTANCE.PauseGame();
        }
        else if (GameController.INSTANCE.IsGamePaused())
        {
            GameController.INSTANCE.ResumeGame();
        }
    }
}
