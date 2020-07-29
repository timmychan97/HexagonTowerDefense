using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_SelectMode : MainMenuPanel
{
    public MainMenuPanel panel_selectLevel;

    new void Start()
    {
        base.Start();
        // panel_selectLevel.Hide();
    }

    public void OnClickedCampaign()
    {
        panel_selectLevel.Show();
    }
}
