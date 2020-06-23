using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_BtnBackToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() 
    {
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene("Main Menu");
    }
}
