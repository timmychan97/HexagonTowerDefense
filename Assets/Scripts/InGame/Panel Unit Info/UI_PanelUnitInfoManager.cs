using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PanelUnitInfoManager : MonoBehaviour
{
    int unitsLayerMask;
    public Canvas canvas;
    public UI_PanelUnitInfo_Enemy pf_enemyPanel;
    public UI_PanelUnitInfo_Tower pf_towerPanel;
    UI_PanelUnitInfo panelUnitInfo;
    public IPropertiesDisplayable displaying;
    public static UI_PanelUnitInfoManager INSTANCE;
    Camera cam;
    KeyCode primaryMouseButton = KeyCode.Mouse0; // left mouseButton

    void Start()
    {
        INSTANCE = this;

        // panelUnitInfo.gameObject.SetActive(false);
        cam = Camera.main;
        unitsLayerMask = LayerMask.NameToLayer("Units");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(primaryMouseButton)) {
            // Do nothing if clicked on the UI elements in front
            bool hasCanvasUI = UnityEngine.EventSystems.EventSystem.current != null;
            if (hasCanvasUI && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Raycast a unit and invoke click event
            int mask = (1 << unitsLayerMask);
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 600, mask))
            {
                Transform unitHit = hit.transform;
                OnClick(unitHit.gameObject);
            }
            else
            {
                CloseInfo();
            }
        }
    }

    public void CloseInfo()
    {
        if (panelUnitInfo != null) 
        {
            Destroy(panelUnitInfo.gameObject);
        }
        TowerRangeMarker.ShowTowerRangeMarkerOnTower(null);
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

        // Show tower range marker, if the selected unit is a Tower
        // TODO: Move to elsewhere, as it does not really fit in here
        TowerRangeMarker.ShowTowerRangeMarkerOnTower(unit.GetComponent<Tower>());
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
}
