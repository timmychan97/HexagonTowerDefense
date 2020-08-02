using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelUnitInfoManager : MonoBehaviour, ISelectionObserver
{
    int unitsLayerMask;
    public Canvas canvas;
    public UI_PanelUnitInfo_Enemy pf_enemyPanel;
    public UI_PanelUnitInfo_Tower pf_towerPanel;
    UI_PanelUnitInfo panelUnitInfo;
    public IPropertiesDisplayable displaying;
    public static UI_PanelUnitInfoManager INSTANCE;

    void Start()
    {
        INSTANCE = this;

        // panelUnitInfo.gameObject.SetActive(false);
        unitsLayerMask = LayerMask.NameToLayer("Units");
        SelectionManager.INSTANCE.AddListener(SelectionManager.ObserverType.Unit, this);
    }


    public void CloseInfo()
    {
        if (panelUnitInfo != null) 
        {
            Destroy(panelUnitInfo.gameObject);
        }
    }

    public void OnClick(GameObject unit)
    {
        // executes when user clicks left mouse button
        // NOTE: only pass Game Objects under layer "Units" as parameter

        if (unit == null) return; // if no Game Objects under layer "Units" are clicked

        // Show info of game object in unit info panel if the object's properties are displayable
        IPropertiesDisplayable displayable = unit.GetComponent<IPropertiesDisplayable>();
        CloseInfo();
        if (displayable != null)
        {
            ShowInfo(displayable);
        }
    }


    public void ShowInfo(IPropertiesDisplayable displayable)
    {
        // display info of selected unit
        panelUnitInfo = Instantiate(displayable.GetPanelUnitInfo(), canvas.transform);
        UpdateInfo();
        displaying = displayable;
    }

    public void UpdateInfo()
    {
        // update info of selected unit
        panelUnitInfo.UpdateInfo();
    }

    void ISelectionObserver.OnSelect(Object obj) {
        OnClick((GameObject)obj);
        TowerRangeMarker.Show();
        TowerRangeMarker.MoveToTower(((GameObject)obj).GetComponent<Tower>());
    }

    void ISelectionObserver.OnDeselect(Object obj)
    {
        CloseInfo();
        TowerRangeMarker.Hide();
    }

    void ISelectionObserver.OnMouseDown(Object obj) { }

    void ISelectionObserver.OnMouseUp(Object obj) { }

    void ISelectionObserver.OnMouseEnter(Object obj) { }

    void ISelectionObserver.OnMouseExit(Object obj) { }
}
