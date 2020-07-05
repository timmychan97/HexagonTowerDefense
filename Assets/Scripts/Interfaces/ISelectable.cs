using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    // When hovered, a selectable tile should be able to show highlight
    void Highlight();
    void Highlight(Color? color);
    void DeHighlight();

    void Select();
    void DeSelect();


}
