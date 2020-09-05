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
    public Text attack;
    public Text attackSpeed;
    public Text description;
    public Text cost;

    public override void UpdateInfo()
    {
        // updates info based on unit object
        // can be optimized by updating only part of the info
        SetInfo(unit.GetName(), unit.level, unit.GetHp(), unit.GetMaxHp(),
            unit.GetAttackDamage(), unit.attackFrequency, unit.GetDescription(), unit.cost);
    }

    public void SetUnit(Unit u) => unit = u;
    void SetInfo(string _name, int _level, float _hp, float _maxHp, float _attackDamage, float _attackSpeed, string _description, int _cost) 
    {
        SetUnitName(_name);
        SetLevel(_level);
        SetHp(_hp, _maxHp);
        SetAttackDamage(_attackDamage);
        SetAttackSpeed(_attackSpeed);
        SetDescription(_description);
        SetCost(_cost);
    }
    void SetUnitName(string s) => unitName.text = s;
    void SetLevel(int n) => level.text = n.ToString();
    void SetHp(float _hp, float _maxHp)
    {
        hp.text = $"{Utils.ConvertToString(_hp)}/{Utils.ConvertToString(_maxHp)}";
    }
    void SetAttackDamage(float value)
    {
        attack.text = Utils.ConvertToString(value);
    }
    void SetAttackSpeed(float f) => attackSpeed.text = f.ToString("0.00");
    void SetDescription(string s) => description.text = s;
    void SetCost(int n) => cost.text = n.ToString();
}
