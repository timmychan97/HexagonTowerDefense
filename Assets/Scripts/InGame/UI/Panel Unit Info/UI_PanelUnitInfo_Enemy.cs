using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelUnitInfo_Enemy : UI_PanelUnitInfo
{
    public Enemy enemy;
    public Text enemyName;
    public Text level;
    public Text hp;
    public Text atk;
    public Text speed;
    public Text atkSpeed;
    public Text worth;

    public override void UpdateInfo()
    {
        SetInfo(enemy.enemyName, enemy.level, enemy.GetHp(), enemy.maxHp, enemy.atk, 
                enemy.moveSpeed, enemy.atkSpeed, enemy.worth);
    }

    public void SetInfo(string _name, int _level, int _hp, int _maxHp, 
                        int _atk, float _speed, float _atkSpeed, int _worth) 
    {
        SetEnemyName(_name);
        SetLevel(_level);
        SetHp(_hp, _maxHp);
        SetAtk(_atk);
        SetSpeed(_speed);
        SetAtkSpeed(_atkSpeed);
        SetWorth(_worth);
    }
    public void SetEnemy(Enemy _e) => enemy = _e;
    void SetEnemyName(string s) => enemyName.text = s;
    void SetLevel(int n) => level.text = n.ToString(); 
    void SetHp(int _hp, int _maxHp)
    {
        hp.text = $"{_hp}/{_maxHp}";
    }
    void SetAtk(int n) => atk.text = n.ToString();
    void SetSpeed(float f) => speed.text = f.ToString("0.00");
    void SetAtkSpeed(float f) => atkSpeed.text = f.ToString("0.00");
    void SetWorth(int n) => worth.text = n.ToString();
}
