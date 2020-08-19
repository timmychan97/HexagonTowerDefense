using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : GameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    /* stats */
    public int level;
    public int atk;
    public Effect effect;
    public float atkSpeed; // in Hz
    private float atkPeriod; // in s
    public float lastAtkTime;
    public float atkRange;

    /* other members */
    GameUnit target = null;
    public Projectile projectile;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    SphereCollider sphereCollider;
    public GameObject platform;
    public GameObject unitContent;
    public Transform emitter;
    UnitRange unitRange;

    void Start()
    {
        Init();

        // Following are only applicable after instantiation
        atkPeriod = 1.0f / atkSpeed;
        lastAtkTime = Time.time;
    }

    void Update()
    {
        if (!GameController.INSTANCE.IsGamePlaying()) return;
        if (isDummy) return;
        HandleAtk();
    }

    public void Init()
    {
        // Initialize member variables
        // We might need them before instantiation (i.e. before Start() is called)
        hp = maxHp;
        unitRange = Instantiate(GameController.INSTANCE.pf_unitRange, transform);
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
        if (target != null) 
        {
            // Rotate unit content to look at target
            // if this Unit has unit content (model that aims)
            if (unitContent != null)
            {
                Vector3 lookAtPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                unitContent.transform.LookAt(lookAtPos);
            }

            float timeSinceAtk = Time.time - lastAtkTime;
            if (timeSinceAtk > atkPeriod)
            {
                Atk(target);
                lastAtkTime = Time.time;
            }
        }
    }

    void Atk(GameUnit target)
    {
        Projectile p = Instantiate(projectile, emitter.position, transform.rotation);
        p.Init(this, target);
    }

    public void SetTarget(GameUnit _target) => target = _target;
    public GameUnit GetTarget() => target;

    public override void Die()
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

    public float GetRange() => atkRange;

    public Tile GetTile()
    {
        if (transform.parent == null) return null;
        return transform.parent.GetComponentInParent<Tile>();
    }

    public int GetAtk() => atk;

    public void SetAtk(int a) => atk = a;

    public float GetAtkRange() => atkRange;

    public void SetAtkRange(float a) => atkRange = a;
}
