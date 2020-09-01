public interface IAttackable
{
    float GetAttackDamage();
    void SetAttackDamage(float value);

    float GetAttackRange();
    void SetAttackRange(float value);

    void Attack(IDamagable target);
}
