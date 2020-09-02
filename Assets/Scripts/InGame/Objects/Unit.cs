using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : AttackableGameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    /* stats */
    public int level;
    public Effect effect;

    /* other members */
    public Projectile projectile;
    public GameObject platform;
    public GameObject unitContent;
    public Transform emitter;

    protected override void Start()
    {
        base.Start();
        hp = maxHp;
    }

    protected override void Update()
    {
        if (GameController.INSTANCE)
            if (!GameController.INSTANCE.IsGamePlaying()) return;

        if (isDummy) return;

        base.Update(); // Handles attack
    }

    // public void OnBuy()
    // {
        
    // }

    public override void HandleAttack()
    {
        if (attackTarget != null) 
        {
            // Rotate unit content to look at target
            // if this Unit has unit content (model that aims)
            if (unitContent != null)
            {
                Vector3 lookAtPos = new Vector3(attackTarget.gameObject.transform.position.x, 
                    transform.position.y, attackTarget.gameObject.transform.position.z);
                unitContent.transform.LookAt(lookAtPos);
            }

            if (isReadyToAttack())
            {
                Attack();
                ResetAttackCountdown();
            }
        }
    }

    public override void Attack()
    {
        Projectile p = Instantiate(projectile, emitter.position, transform.rotation);
        p.Init(this, attackTarget.gameObject.GetComponent<GameUnit>());
    }

    public override void Die(AttackInfo attackInfo)
    {
        GameController.INSTANCE.OnUnitDie(this);
        Destroy(gameObject);
    }

    public new UI_PanelUnitInfo GetPanelUnitInfo()
    {
        // get the prefab from Panel Unit Info Manager, 
        // link it with this object, then return
        UI_PanelUnitInfo_Unit panel = UI_PanelUnitInfoManager.INSTANCE.pf_unitPanel;
        panel.SetUnit(this);
        return panel;
    }

    public Tile GetTile()
    {
        if (transform.parent == null) return null;
        return transform.parent.GetComponentInParent<Tile>();
    }
}
