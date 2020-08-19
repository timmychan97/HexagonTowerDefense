using UnityEngine;
using UnityEngine.UI;

public class UI_Utils : MonoBehaviour
{
    // Checks if a UI element is visible at all on screen. Return true if at least one pixel of UI is visible.
    public static bool IsVisibleOnScreen(RectTransform rectTransform)
    {
        Vector3[] v = new Vector3[4];
        rectTransform.GetWorldCorners(v);

        float maxX = Mathf.Max(v[0].x, v[1].x, v[2].x, v[3].x);
        if (maxX < 0) return false;

        float minX = Mathf.Min(v[0].x, v[1].x, v[2].x, v[3].x);
        if (minX > Screen.width) return false;

        float maxY = Mathf.Max(v[0].y, v[1].y, v[2].y, v[3].y);
        if (maxY < 0) return false;

        float minY = Mathf.Min(v[0].y, v[1].y, v[2].y, v[3].y);
        if (minY > Screen.height) return false;

        return true;
    }

    public static bool IsPointerOverUIElement()
    {
        var currentEventSystem = UnityEngine.EventSystems.EventSystem.current;
        bool hasCanvasUI = currentEventSystem != null;
        bool isPointerOverUIElement = currentEventSystem.IsPointerOverGameObject();
        return hasCanvasUI && isPointerOverUIElement;
    }
}
