using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Btn_LoadLevel : Btn_MainMenu
{
    // Text text;
    Button btn;
    public string scenePath;

    // Start is called before the first frame update
    void Start()
    {
        // text = GetComponentInChildren<Text>();
        btn = GetComponent<Button>();

        // if (text == null) 
        // {
        //     Debug.LogWarning("No Text component in children of Btn_LoadLevel");
        // }

        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        // SceneManager.LoadScene(scenePath);
        MainMenuManager.INSTANCE.OnSelectLevel(this);
    }

    // public void SetText(string s)
    // {
    //     this.text.text = s;
    // }
}
