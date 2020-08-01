using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_SelectMode : MainMenuPanel
{
    public MainMenuPanel panel_selectLevel;

    new void Start()
    {
        base.Start();
        panel_selectLevel.Hide();
    }

    public void OnCampaignClicked()
    {
        if (panel_selectLevel.gameObject.activeInHierarchy) 
        {
            panel_selectLevel.HideWithAnimation();
        }
        else
        {
            panel_selectLevel.Show();
        }
    }

    new public void Hide()
    {
        panel_selectLevel.Hide();
        base.Hide();
    }

    new public void HideWithAnimation()
    {
        panel_selectLevel.Hide();
        base.HideWithAnimation();
    }
}
