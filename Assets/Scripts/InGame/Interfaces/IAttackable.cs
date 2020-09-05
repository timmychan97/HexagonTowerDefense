using UnityEngine;

public interface IAttackable
{
    float GetAttackDamage();
    void SetAttackDamage(float value);

    float GetAttackRadius();
    void SetAttackRadius(float value);

    void Attack();
}
