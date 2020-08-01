using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_BtnBackToMainMenu : MonoBehaviour
{
    public void OnClick() 
    {
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene("Main Menu");
    }
}
