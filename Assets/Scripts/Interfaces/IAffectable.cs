using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectable
{
    void TakeEffect(Effect effect);
    void RemoveEffect(Effect effect);
}
