public interface IDamagable
{
    void TakeEffect(Effect effect);
    void TakeDmg(AttackInfo attackInfo);
    void Die(AttackInfo attackInfo);
}
