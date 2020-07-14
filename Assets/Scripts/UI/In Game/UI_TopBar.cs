using UnityEngine;
using UnityEngine.UI;

public class UI_TopBar : MonoBehaviour
{
    public Text textGold;
    public Text textHp;
    public Text textRound;

    public void SetTextGold(int n) => textGold.text = n.ToString();

    public void SetTextHp(int n) => textHp.text = n.ToString();

    public void SetTextRound(int n) => textRound.text = n.ToString();
}
