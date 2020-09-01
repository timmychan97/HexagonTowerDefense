using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : GameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    /* stats */
    public int level;
    public float attackDamage;
    public Effect effect;
    public float attackSpeed = 1; // in Hz
    private float attackPeriod; // in s
    private float lastAtkTime;
    public float attackRange;

    /* other members */
    GameUnit target = null;
    public Projectile projectile;
    public GameObject platform;
    public GameObject unitContent;
    public Transform emitter;

    public UnitRange pf_unitRange;
    UnitRange unitRange;

    void Start()
    {
        Init();

        // Following are only applicable after instantiation
        attackPeriod = 1.0f / attackSpeed;
        lastAtkTime = Time.time;
    }

    void Update()
    {
        if (GameController.INSTANCE)
            if (!GameController.INSTANCE.IsGamePlaying()) return;
        if (isDummy) return;
        HandleAtk();
    }

    public void Init()
    {
        // Initialize member variables
        // We might need them before instantiation (i.e. before Start() is called)
        hp = maxHp;
        unitRange = Instantiate(pf_unitRange, transform);
        // unitRange = t.GetComponent<UnitRange>();
        if (unitRange == null)
        {
            Debug.LogWarning("Unit Range prefab has no UnitRange script attached to it");
        }
        unitRange.Init(this);
    }

    // public void OnBuy()
    // {
        
    // }

    public void HandleAtk()
    {
        if (target) 
        {
            // Rotate unit content to look at target
            // if this Unit has unit content (model that aims)
            if (unitContent != null)
            {
                Vector3 lookAtPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                unitContent.transform.LookAt(lookAtPos);
            }

            float timeSinceAtk = Time.time - lastAtkTime;
            if (timeSinceAtk > attackPeriod)
            {
                Attack(target);
                lastAtkTime = Time.time;
            }
        }
    }

    void Attack(GameUnit target)
    {
        Projectile p = Instantiate(projectile, emitter.position, transform.rotation);
        p.Init(this, target);
    }

    public void SetTarget(GameUnit _target) => target = _target;
    public GameUnit GetTarget() => target;

    public override void Die(AttackInfo attackInfo)
    {
        GameController.INSTANCE.OnUnitDie(this);
        Destroy(gameObject);
    }

    void ChooseNewTarget() => target = unitRange.GetNewTarget();

    public new UI_PanelUnitInfo GetPanelUnitInfo()
    {
        // get the prefab from Panel Unit Info Manager, 
        // link it with this object, then return
        UI_PanelUnitInfo_Unit panel = UI_PanelUnitInfoManager.INSTANCE.pf_unitPanel;
        panel.SetUnit(this);
        return panel;
    }

    public float GetRange() => attackRange;

    public Tile GetTile()
    {
        if (transform.parent == null) return null;
        return transform.parent.GetComponentInParent<Tile>();
    }

    public float GetAttackDamage() => attackDamage;
    public int GetAttackDamageInt() => (int)Mathf.Round(attackDamage);
    public void SetAttackDamage(float value) => attackDamage = value;
    public float GetAttackRange() => attackRange;
    public void SetAttackRange(float a) => attackRange = a;
}
