using UnityEngine;

public interface IDamagable : IGameObject
{
    void TakeEffect(Effect effect);
    void TakeDmg(AttackInfo attackInfo);
    void Die(AttackInfo attackInfo);
    bool IsDead();
}
