using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WithAnimatedMovement : Enemy
{
    public EnemyCharacter character;
    public Animator animator;

    protected override void ValidateAttachedObjects()
    {
        base.ValidateAttachedObjects();
        if (!animator) Debug.LogWarning($"No Animator is attached to \"{gameObject.name}\" " +
            $"Enemy in inspector. Animations will not work properly without it");
    }

    public override void Attack()
    {
        // TODO: Add delay before actually attacking
        base.Attack();
        animator.SetTrigger("Attack");
    }

    protected override void Move(Vector3 destination)
    {
        character.Move(destination, false, false);
    }

    protected override void PlayDieAnimation(AttackInfo attackInfo)
    {
        character.DoRagdoll(attackInfo);
    }

    public override void Die(AttackInfo attackInfo)
    {
        GameController.INSTANCE?.OnEnemyDie(this);

        PlayDieAnimation(attackInfo);


        navMeshAgent.enabled = false;
        // Destroy gameObject after seconds
        Destroy(gameObject, 5f);
        Destroy(healthBarPivot, 1);

        // Destroy all components, except transform and this script
        foreach(var comp in GetComponents<Component>())
        {
            if (!(comp is Transform) && comp != this) Destroy(comp);
        }

        if (range && range.gameObject)
            Destroy(range.gameObject);

        // Destroy this script to invoke OnTriggerExit() event in the UnitRange object, so that the towers can pick another target
        Destroy(this);
    }
}
