using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    protected GameUnit gameUnit;
    protected GameUnit target;
    protected SphereCollider sphereCollider;
    protected HashSet<GameUnit> gameUnitsInRange = new HashSet<GameUnit>();
    
    bool triggered = false;
    Collider other;

    void Update()
    {
        if (triggered && !target) 
        {
            SetTarget(GetNewTarget());
        }
    }
    public void Init(GameUnit gu, float radius) 
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null) 
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = true;
        SetGameUnit(gu);
        SetRadius(radius);
    }

    public void SetGameUnit(GameUnit gameUnit)
    {
        this.gameUnit = gameUnit;
    }

    public float GetRadius() 
    {
        return sphereCollider.radius;
    }

    public void SetRadius(float r)
    {
        sphereCollider.radius = r;
    }

    public virtual bool IsPotentialTarget(GameUnit gu)
    {
        /*
        Note: Should be overridden by children
        
        Return:
            Whether the given GameUnit is a potential target for
            the GameUnit that this Range belongs to.
        */
        if (gu == null || gu.GetIsDummy()) return false;
        return true;
    }

    public virtual bool IsTarget(GameUnit gu)
    {
        /*
        Note: Should be overriden by children.

        Return:
            Whether the given GameUnit is the current target of
            the GameUnit that this Range belongs to.
        */
        return true;
    }

    public virtual GameUnit GetCurrentTarget()
    {
        /*
        Return:
            The current target of the GameUnit that this Range
            belongs to.
        */
        return target;
    }

    public virtual void SetTarget(GameUnit gu)
    {
        // Note: Should be overridden by children.
        target = gu;
        triggered = gu;  // bool
    }

    void OnTriggerEnter(Collider other)
    {
        GameUnit gameUnit = other.GetComponent<GameUnit>();
        if (IsPotentialTarget(gameUnit)) // the collided object is a Unit
        {
            // Debug.Log(enemy.GetTarget());
            if (GetCurrentTarget() == null) 
            {
                SetTarget(gameUnit);
            }
            else
            {
                gameUnitsInRange.Add(gameUnit); // backup enemies
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameUnit gu = other.GetComponent<GameUnit>();
        if (IsPotentialTarget(gu))
        {
            gameUnitsInRange.Remove(gu);
            if (gu == GetCurrentTarget()) 
            {
                SetTarget(GetNewTarget());
            }
        }
    }

    /// <summary>
    /// Chooses a new target by the a set of policies.
    /// </summary>
    /// <returns>The Enemy satisfying the set of policies. Null when none was found.</returns>
    public GameUnit GetNewTarget()
    {
        /*
        Chooses a new target by following policy:
        - The firstmost GameUnit that entered this Range
          and is still in range.
        */

        GameUnit target = null;
        // Store nulls in the list of Game Units
        // (a result of Game Units dying while in range)
        var toRemove = new List<GameUnit>();
        foreach (GameUnit e in gameUnitsInRange)
        {
            // mark for removal afterwards 
            // (can't remove during foreach loop of HashSet)
            toRemove.Add(e);
            if (e != null)
            {
                target = e; // found new target
                break;
            }
        }

        // Remove the null's in gameUnitsInRange
        foreach (var e in toRemove)
        {
            gameUnitsInRange.Remove(e);
        }
        return target;
    }
}
