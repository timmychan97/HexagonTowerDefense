using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : GameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    public int level;

    void Start()
    {
        hp = maxHp;
    }

    public override void OnBuy() {}

    public new void Die()
    {
        GameController.INSTANCE.OnBuildingDie(this);
        Destroy(gameObject);
    }

    public new UI_PanelUnitInfo GetPanelUnitInfo() 
    { 
        // Get the prefab from UI_PanelUnitInfoManager, 
        // link it with this object, then return The Panel
        UI_PanelUnitInfo_Building panel = UI_PanelUnitInfoManager.INSTANCE.pf_buildingPanel;
        panel.SetUnit(this);
        return panel;
    }
}
