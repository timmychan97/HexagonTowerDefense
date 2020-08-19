using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelUnitInfo_Building : UI_PanelUnitInfo
{
    public Building building;
    public Text buildingName;
    public Text level;
    public Text hp;
    public Text description;
    public Text cost;

    public override void UpdateInfo()
    {
        // Updates info based on building object
        // Can be optimized by updating only part of the info
        SetInfo(building.GetName(), building.level, building.GetHp(), building.maxHp, building.description, building.cost);
    }

    public void SetUnit(Building b) => building = b;
    void SetInfo(string _name, int _level, int _hp, int _maxHp, string _description, int _cost) 
    {
        SetBuildingName(_name);
        SetLevel(_level);
        SetHp(_hp, _maxHp);
        SetDescription(_description);
        SetCost(_cost);
    }
    void SetBuildingName(string s) => buildingName.text = s;
    void SetLevel(int n) => level.text = n.ToString(); 
    void SetHp(int _hp, int _maxHp)
    {
        hp.text = $"{_hp}/{_maxHp}";
    }
    public void SetDescription(string s) => description.text = s;
    void SetCost(int n) => cost.text = n.ToString();
}
