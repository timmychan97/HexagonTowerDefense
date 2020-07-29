using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_SelectMode : MainMenuPanel
{
    public MainMenuPanel panel_selectLevel;

    new void Start()
    {
        base.Start();
        // NOTE: Have to set RectTransfrom from parent!
        // base.rectTransform = this.GetComponent<RectTransform>();

        panel_selectLevel.Hide();
    }

    public void OnClickedCampaign()
    {
        panel_selectLevel.Show();
    }
}
