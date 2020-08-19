using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour, IDamagable, IPropertiesDisplayable
{
    public Sprite iconSmall;
    public string _name;
    public string description;
    /* public stats */
    public int maxHp;
    public int cost;
    public int sellWorth;
    /* protected stats */
    protected int hp;
    protected bool isDummy = false;

    public virtual void OnBuy()
    {

    }

    public virtual void Die()
    {
        Debug.Log("Die() was called in GameUnit.cs");
    }

    public virtual void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
        if (hp <= 0) 
        {
            Die();
        }
        
        // if panel unit info is displaying this unit's info, update it
        UI_PanelUnitInfoManager.INSTANCE.OnDisplayableTakeDmg(this);
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

    public int GetMaxHp() => maxHp;

    public int GetHp() => hp;

    public void SetHp(int a) => hp = a;

    public int GetCost() => cost;

    public bool GetIsDummy() => isDummy;

    public void SetIsDummy(bool b) => isDummy = b;
}
