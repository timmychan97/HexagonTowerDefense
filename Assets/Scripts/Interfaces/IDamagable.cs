using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeEffect(Effect effect);
    void TakeDmg(float dmg);
    void Die();
}
