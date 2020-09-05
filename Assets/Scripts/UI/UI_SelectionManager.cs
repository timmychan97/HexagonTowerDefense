using UnityEngine;

public class UI_SelectionManager : MonoBehaviour
{
    public static UI_SelectionManager INSTANCE;
    public UI_Tool selectedTool;

    void Awake() => INSTANCE = this;

    private void Update()
    {
        // Right click deselect selected tool
        if (Input.GetMouseButtonDown(1) && selectedTool)
        {
            if (!UI_Utils.IsPointerOverUIElement())
            {
                DeselectTool();
            }
        } 
    }
    public void SetSelection(UI_Tool tool)
    {
        if (selectedTool == tool)
        {
            DeselectTool();
        }
        else
        {
            if (selectedTool)
            {
                DeselectTool();
            }
            SelectTool(tool);
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

    public void DeselectTool()
    {
        DummyUnitManager.INSTANCE.OnToolDeselected(selectedTool);
        selectedTool.Deselect();
        selectedTool = null;
    }

    public void SelectTool(UI_Tool tool)
    {
        selectedTool = tool;
        Debug.Log("Changed tool");
        selectedTool.Select();
        DummyUnitManager.INSTANCE.OnToolSelected(selectedTool);
    }
}
