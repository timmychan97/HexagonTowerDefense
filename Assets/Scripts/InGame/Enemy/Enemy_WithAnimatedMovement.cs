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
}
