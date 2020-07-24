using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager INSTANCE;

    public enum ObserverType { Tile, Unit }
    private Dictionary<ObserverType, List<ISelectionObserver>> observers = new Dictionary<ObserverType, List<ISelectionObserver>>();



    // Configurations
    int unitsLayerMask = 1 << 8;
    int tilesLayerMask = 1 << 9;



    private struct State
    {
        public Object mouseOver;
        public Object mouseDown;
        public Object selected;
    }

    State tilePrevState = new State();
    State unitPrevState = new State();
    State tileCurState = new State();
    State unitCurState = new State();


    // Alias
    KeyCode primaryMouseButton = KeyCode.Mouse0;


    void Awake()
    {
        INSTANCE = this;

        observers.Add(ObserverType.Tile, new List<ISelectionObserver>());
        observers.Add(ObserverType.Unit, new List<ISelectionObserver>());
    }

    void Update()
    {
        // Compute states
        tileCurState = GetTileCurState();
        unitCurState = GetUnitCurState();

        if (unitCurState.selected)
            print(((GameObject)unitCurState.selected).name);

        // Propagate events to observers, based on the computed states
        UpdateUnitSelection();

        UpdateTileSelection();
        // Update tile after Tile selection.
        // Tile selection could place a tower on the tile
        UpdateTileHover();


        // Save previous states
        tilePrevState = tileCurState;
        unitPrevState = unitCurState;
    }

    Tile GetTileAtMousePos() => GetCompenentAtMousePos<Tile>(tilesLayerMask);
    GameObject GetUnitAtMousePos() => GetGameObjectAtMousePos(unitsLayerMask);


    private State GetUnitCurState()
    {
        var result = new State();

        // If a tool is selected, set every state to null
        if (UI_SelectionManager.INSTANCE.selectedTool) return result;

        result.mouseOver = GetUnitAtMousePos();
        result.mouseDown = Input.GetKey(primaryMouseButton) ? result.mouseOver : null;

        // If the user released the mouse button while hovering over a unit,
        // it makes the unit stay selected
        if (result.mouseOver && result.mouseDown != unitPrevState.mouseDown && result.mouseDown == null)
            result.selected = unitPrevState.mouseDown;
        return result;
    }

    private State GetTileCurState()
    {
        var result = new State();
        result.mouseOver = GetTileAtMousePos();
        result.mouseDown = Input.GetKey(primaryMouseButton) ? result.mouseOver : null;

        // If no selected tool, or the selected tool is being deselected on the UI
        if (!UI_SelectionManager.INSTANCE.selectedTool)
            result.mouseOver = null;
        return result;
    }

    private void UpdateTileSelection()
    {
        if (tileCurState.mouseDown != tilePrevState.mouseDown)
        {
            // If clicked on the UI elements in front
            if (isPointerOverUIElement()) return;

            // On mouse button release
            if (tileCurState.mouseDown == null)
            {
                // Propagate the mouse down event to the tile
                Tile tile = (Tile)tilePrevState.mouseDown;
                tile.OnClick();
                observers[ObserverType.Tile].ForEach(e => e.OnMouseUp(tilePrevState.mouseDown));
            }
            else
            {
                Tile tile = (Tile)tileCurState.mouseDown;
                if (tileCurState.mouseDown)
                    observers[ObserverType.Tile].ForEach(e => e.OnMouseDown(tileCurState.mouseDown));
            }

        }
    }

    private void UpdateTileHover()
    {
        // If tile at mouse pos is changed
        if (tileCurState.mouseOver != tilePrevState.mouseOver)
        {
            if (tilePrevState.mouseOver)
                observers[ObserverType.Tile].ForEach(e => e.OnMouseExit(tilePrevState.mouseOver));
            if (tileCurState.mouseOver)
                observers[ObserverType.Tile].ForEach(e => e.OnMouseEnter(tileCurState.mouseOver));
        }

        // If the user clicked on a tile.
        // Check if the user placed a tower, which should call OnMouseEnter once more.
        // FIXME: Fix properly
        if (tileCurState.mouseDown != tilePrevState.mouseDown)
        {
            if (tilePrevState.mouseOver)
                observers[ObserverType.Tile].ForEach(e => e.OnMouseExit(tilePrevState.mouseOver));
            if (tileCurState.mouseOver)
                observers[ObserverType.Tile].ForEach(e => e.OnMouseEnter(tileCurState.mouseOver));
        }
    }

    void UpdateUnitSelection()
    {
        if (unitPrevState.selected != unitCurState.selected)
        {
            // If clicked on the UI elements in front
            if (isPointerOverUIElement()) return;

            if (unitPrevState.selected)
                observers[ObserverType.Unit].ForEach(e => e.OnDeselect(unitPrevState.selected));
            if (unitCurState.selected)
                observers[ObserverType.Unit].ForEach(e => e.OnSelect(unitCurState.selected));
        }

    }


    GameObject GetGameObjectAtMousePos(LayerMask layerMask)
    {
        int cameraDistance = 600;
        RaycastHit rh;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out rh, cameraDistance, layerMask))
            return rh.transform.gameObject;
        return default;
    }

    T GetCompenentAtMousePos<T>(LayerMask layerMask)
    {
        var go = GetGameObjectAtMousePos(layerMask);
        return go ? go.GetComponentInParent<T>() : default;
    }

    private bool isPointerOverUIElement()
    {
        var currentEventSystem = UnityEngine.EventSystems.EventSystem.current;
        bool hasCanvasUI = currentEventSystem != null;
        bool isPointerOverUIElement = currentEventSystem.IsPointerOverGameObject();
        return hasCanvasUI && isPointerOverUIElement;
    }


    public void AddListener(ObserverType type, ISelectionObserver observer)
    {
        observers[type].Add(observer);
    }

    public void RemoveListener(ObserverType type, ISelectionObserver observer)
    {
        observers[type].Add(observer);
    }
}
