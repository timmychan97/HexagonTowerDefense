using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents all Units in game. The following objects are all Game Units
///  - Enemy
///  - Tower
///  - Base
/// </summary>
public class GameUnit : MonoBehaviour, IDamagable, IPropertiesDisplayable
{
    public Sprite iconSmall;
    public string _name;
    public string description;

    /* public stats */
    public float maxHp;
    public int cost;
    public int sellWorth;

    /* protected stats */
    protected float hp;
    protected bool isDummy = false;

    public virtual void OnBuy()
    {

    }

    public virtual void Die(AttackInfo attackInfo)
    {
        Debug.Log("Die() was called in GameUnit.cs");
    }

    public virtual void TakeDmg(AttackInfo attackInfo)
    {
        hp -= attackInfo.damage;
        if (hp <= 0) 
        {
            Die(attackInfo);
        }
        
        // if panel unit info is displaying this unit's info, update it
        UI_PanelUnitInfoManager.INSTANCE?.OnDisplayableTakeDmg(this);
    }

    public virtual void TakeEffect(Effect effect)
    {

    }

    ////////////////////////////////////
    //      Getters and Setters
    ////////////////////////////////////

    public UI_PanelUnitInfo GetPanelUnitInfo()
    {
        Debug.LogWarning("Error: GetPanelUnitInfo() was called in GameUnit");
        // To be overridden by children
        return null;
    }
    
    public string GetName() => _name;
    public string GetDescription() => description;
    public float GetMaxHp() => maxHp;
    public float GetHp() => hp;
    public void SetHp(float value) => hp = value;
    public int GetCost() => cost;
    public bool GetIsDummy() => isDummy;
    public void SetIsDummy(bool b) => isDummy = b;
}
