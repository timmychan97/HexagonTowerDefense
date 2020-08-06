using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_GoldMine : Building
{
    public int goldPerSec = 1;

    public override void OnBuy()
    {
        BuildingManager.INSTANCE.OnBuyGoldMine(this);
    }
}
