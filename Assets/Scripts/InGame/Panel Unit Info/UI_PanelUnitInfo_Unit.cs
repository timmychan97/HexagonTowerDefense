using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_PanelUnitInfo_Unit : UI_PanelUnitInfo
{
    public Unit unit;
    public Text unitName;
    public Text level;
    public Text hp;
    public Text atk;
    public Text atkSpeed;
    public Text description;
    public Text cost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateInfo()
    {
        // updates info based on tower object
        // can be optimized by updating only part of the info
        SetInfo(unit.towerName, unit.level, unit.GetHp(), unit.maxHp, unit.atk, unit.atkSpeed, unit.description, unit.cost);
    }

    public void SetUnit(Unit _t) 
    {
        unit = _t;
    }
    void SetInfo(string _name, int _level, int _hp, int _maxHp, int _atk, float _atkSpeed, string _description, int _cost) 
    {
        SetTowerName(_name);
        SetLevel(_level);
        SetHp(_hp, _maxHp);
        SetAtk(_atk);
        SetAtkSpeed(_atkSpeed);
        SetDescription(_description);
        SetCost(_cost);
    }
    void SetTowerName(string s) => unitName.text = s;
    void SetLevel(int n) => level.text = n.ToString(); 
    void SetHp(int _hp, int _maxHp)
    {
        hp.text = $"{_hp}/{_maxHp}";
    }
    void SetAtk(int n) => atk.text = n.ToString();
    void SetAtkSpeed(float f) => atkSpeed.text = f.ToString("0.00");
    void SetDescription(string s) => description.text = s;
    void SetCost(int n) => cost.text = n.ToString();
}
