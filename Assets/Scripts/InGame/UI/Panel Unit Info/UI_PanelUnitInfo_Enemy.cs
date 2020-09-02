using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelUnitInfo_Enemy : UI_PanelUnitInfo
{
    public Enemy enemy;
    public Text enemyName;
    public Text level;
    public Text hp;
    public Text attack;
    public Text speed;
    public Text attackSpeed;
    public Text worth;

    public override void UpdateInfo()
    {
        SetInfo(enemy.GetName(), enemy.level, enemy.GetHp(), enemy.GetMaxHp(), enemy.GetAttackDamage(), 
                enemy.moveSpeed, enemy.attackFrequency, enemy.worth);
    }

    public void SetInfo(string _name, int _level, float _hp, float _maxHp, 
                        float _attackDamage, float _speed, float _attackSpeed, int _worth) 
    {
        SetEnemyName(_name);
        SetLevel(_level);
        SetHp(_hp, _maxHp);
        SetAttackDamage(_attackDamage);
        SetSpeed(_speed);
        SetAttackSpeed(_attackSpeed);
        SetWorth(_worth);
    }
    public void SetEnemy(Enemy _e) => enemy = _e;
    void SetEnemyName(string s) => enemyName.text = s;
    void SetLevel(int n) => level.text = n.ToString(); 
    void SetHp(float _hp, float _maxHp)
    {
        hp.text = $"{Utils.ConvertToString(_hp)}/{Utils.ConvertToString(_maxHp)}";
    }
    void SetAttackDamage(float value)
    {
        attack.text = Utils.ConvertToString(value);
    }

    void SetSpeed(float f) => speed.text = f.ToString("0.00");
    void SetAttackSpeed(float f) => attackSpeed.text = f.ToString("0.00");
    void SetWorth(int n) => worth.text = n.ToString();
}
