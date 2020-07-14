using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Btn_LoadLevel : MonoBehaviour
{
    public Text text;
    Button btn;
    public string scenePath;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        btn = GetComponent<Button>();

        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        SceneManager.LoadScene(scenePath);
    }

    public void SetText(string s)
    {
        text = GetComponentInChildren<Text>();
        if (text == null) 
        {
            Debug.LogWarning("No Text component in children of Btn_LoadLevel");
        }
        text.text = s;
    }
}
