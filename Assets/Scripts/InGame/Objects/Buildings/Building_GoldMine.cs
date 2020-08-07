using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_GoldMine : Building
{
    public int goldPerSec = 1;

    void Awake()
    {
        SetDescriptionText();
    }

    void SetDescriptionText()
    {
        // Set the description of this Building programmatically
        // because part of its specs do not fit into the
        // default layout of Panel Unit Info for Buildings
        description = $"Produces {goldPerSec} per second";
    }

    public override void OnBuy()
    {
        BuildingManager.INSTANCE.OnBuyGoldMine(this);
    }
}
