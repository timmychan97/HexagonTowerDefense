using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected GameObject emitter;
    protected Vector3 origin; // original position
    public bool follow; 
    public float speed; // move distance per second
    public Effect effect;
    public float damage;

    public virtual void Init(Unit _emitter, GameUnit _target) {}
    public virtual void Init(Enemy enemy, GameUnit _target) {}
    public virtual void OnHit() {}
}
