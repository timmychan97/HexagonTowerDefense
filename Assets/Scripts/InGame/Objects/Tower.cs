using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : TileContent, IDamagable, IPropertiesDisplayable
{
    public Sprite iconSmall;
    public string towerName;
    public int level;
    public string description;
    private int id;
    public int atk;
    public Effect effect;
    public float atkSpeed; // in Hz
    private float atkPeriod; // in s
    public float lastAtkTime;
    public int maxHp;
    private int hp;
    public int cost;
    public int sellWorth;
    public float range;
    GameObject target = null;
    public Projectile projectile;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    SphereCollider sphereCollider;
    public GameObject platform;
    public GameObject towerContent;
    public Transform emitter;
    public Transform pf_towerRange;
    TowerRange towerRange;
    // Start is called before the first frame update
    void Start()
    {
        Transform t = Instantiate(pf_towerRange, transform);
        towerRange = t.GetComponent<TowerRange>();
        if (towerRange == null) {
            Debug.LogWarning("Tower Range prefab has no TowerRange script applied");
        }
        towerRange.Init(this);

        hp = maxHp;
        atkPeriod = 1.0f / atkSpeed;
        lastAtkTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) ChooseNewTarget();

        if (target) 
        {
            // rotate tower content to look at target
            if (towerContent != null) 
            {
                Vector3 lookAtPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                towerContent.transform.LookAt(lookAtPos);
            }
            HandleAtk();
        }
    }

    public void HandleAtk()
    {
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

    public void SetTarget(GameObject _target) {target = _target; }
    public GameObject GetTarget() { return target; }

    public void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
    }

    void ChooseNewTarget()
    {
        target = towerRange.GetNewTarget();
    }

    public UI_PanelUnitInfo GetPanelUnitInfo() 
    { 
        // get the prefab from Panel Unit Info Manager, 
        // link it with this object, then return
        UI_PanelUnitInfo_Tower panel = UI_PanelUnitInfoManager.INSTANCE.pf_towerPanel;
        panel.SetTower(this);
        return panel;
    }

    public int GetHp() 
    {
        return hp;
    }


    public float GetRange()
    {
        return towerRange.GetRadius();
    }
}
