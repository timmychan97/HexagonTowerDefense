using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : GameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnBuy() {}

    public new void Die()
    {
        GameController.INSTANCE.OnBuildingDie(this);
        Destroy(gameObject);
    }

    public new UI_PanelUnitInfo GetPanelUnitInfo() 
    { 
        // get the prefab from UI_PanelUnitInfoManager, 
        // link it with this object, then return The Panel
        UI_PanelUnitInfo_Building panel = UI_PanelUnitInfoManager.INSTANCE.pf_buildingPanel;
        panel.SetUnit(this);
        return panel;
    }
}
