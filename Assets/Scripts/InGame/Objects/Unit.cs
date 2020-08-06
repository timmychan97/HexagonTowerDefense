using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : GameUnit, IPlacable, IDamagable, IPropertiesDisplayable
{
    public Sprite iconSmall;
    public int level;
    private int id;
    public int atk;
    public Effect effect;
    public float atkSpeed; // in Hz
    private float atkPeriod; // in s
    public float lastAtkTime;
    private int hp;
    public int sellWorth;
    public float range;
    GameObject target = null;
    public Projectile projectile;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    SphereCollider sphereCollider;
    public GameObject platform;
    public GameObject unitContent;
    public Transform emitter;
    public UnitRange pf_unitRange;
    UnitRange unitRange;
    // Start is called before the first frame update
    void Start()
    {
        Init();

        // following are only applicable after instantiation
        atkPeriod = 1.0f / atkSpeed;
        lastAtkTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDummy) return;
        if (target == null) ChooseNewTarget();

        if (target)
        {
            // rotate unit content to look at target
            if (unitContent != null)
            {
                Vector3 lookAtPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                unitContent.transform.LookAt(lookAtPos);
            }
            HandleAtk();
        }
    }

    public void Init()
    {
        // initial member variables
        // we might need them before instantiation (i.e. before Start() is called)
        hp = maxHp;
        unitRange = Instantiate(pf_unitRange, transform);
        // unitRange = t.GetComponent<UnitRange>();
        if (unitRange == null)
        {
            Debug.LogWarning("Unit Range prefab has no UnitRange script attached to it");
        }
        unitRange.Init(this);
    }

    public void HandleAtk()
    {
        if (!GameController.INSTANCE.IsGamePlaying()) return;
        float timeSinceAtk = Time.time - lastAtkTime;
        if (timeSinceAtk > atkPeriod)
        {
            Atk(target);
            lastAtkTime = Time.time;
        }
    }

    void Atk(GameObject target)
    {
        Projectile p = Instantiate(projectile, emitter.position, transform.rotation);
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            p.Init(this, enemy);
        }
        else
        {
            p.Init(this, target);
        }
    }

    public void SetTarget(GameObject _target) { target = _target; }
    public GameObject GetTarget() { return target; }

    public void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
    }

    void ChooseNewTarget()
    {
        target = unitRange.GetNewTarget();
    }

    public UI_PanelUnitInfo GetPanelUnitInfo()
    {
        // get the prefab from Panel Unit Info Manager, 
        // link it with this object, then return
        UI_PanelUnitInfo_Unit panel = UI_PanelUnitInfoManager.INSTANCE.pf_unitPanel;
        panel.SetUnit(this);
        return panel;
    }

    public int GetHp()
    {
        return hp;
    }

    public float GetRange()
    {
        return range;
    }

    public Tile GetTile()
    {
        if (transform.parent == null) return null;
        return transform.parent.GetComponentInParent<Tile>();
    }
}
