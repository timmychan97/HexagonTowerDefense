using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectionManager : MonoBehaviour
{
    public static UI_SelectionManager INSTANCE;
    public UI_Tool selectedTool;

    void Start() => INSTANCE = this;

    public void SetSelection(UI_Tool tool)
    {
        if (selectedTool == tool)
        {
            DummyUnitManager.INSTANCE.OnToolDeselected(selectedTool);
            selectedTool.Deselect();
            selectedTool = null;
        }
        else
        {
            if (selectedTool)
            {
                selectedTool.Deselect();
            }

            selectedTool = tool;
            // Debug.Log("Changed tool");
            selectedTool.Select();
            DummyUnitManager.INSTANCE.OnToolSelected(selectedTool);
        }
    }

    // returns true when successfully used selected tool
    public bool UseSelectedTool()
    {
        if (selectedTool)
        {
            selectedTool.Action();
        }
        else
        {
            Debug.Log("No tool is selected");
            return false;
        }
        return true;
    }
}
