using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OnSelected will be called by SelectionManager
/// </summary>
public interface ISelectionObserver
{
    void OnSelect(Object obj);
    void OnDeselect(Object obj);
    void OnMouseDown(Object obj);
    void OnMouseUp(Object obj);
    void OnMouseEnter(Object obj);
    void OnMouseExit(Object obj);
}
