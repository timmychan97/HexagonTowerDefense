using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : GameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    public int level;
    int hp;
    public int sellWorth;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetHp() { return hp; }

    public void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
    }

    public UI_PanelUnitInfo GetPanelUnitInfo() 
    { 
        // get the prefab from Panel Unit Info Manager, 
        // link it with this object, then return
        UI_PanelUnitInfo_Building panel = UI_PanelUnitInfoManager.INSTANCE.pf_buildingPanel;
        panel.SetUnit(this);
        return panel;
    }
}
