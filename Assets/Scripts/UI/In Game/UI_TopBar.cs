using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TopBar : MonoBehaviour
{
    public Text textGold;
    public Text textHp;
    public Text textRound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextGold(int n){
        textGold.text = n.ToString();
    }

    public void SetTextHp(int n){
        textHp.text = n.ToString();
    }

    public void SetTextRound(int n){
        textRound.text = n.ToString();
    }
}
