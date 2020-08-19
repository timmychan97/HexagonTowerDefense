using UnityEngine;
using System;

public class UI_MapEditor_TileContentButtons : MonoBehaviour
{
    public UI_Tool toolPf;

    void Start()
    {
        var tileContentsPf = UI_MapEditor_Utils.GetAllTileContentsPrefabs();
        Array.ForEach(tileContentsPf, go => CreateTools(go));
    }


    void CreateTools(GameObject tileContentPf)
    {
        UI_Tool tool = Instantiate(toolPf, transform);
        tool.SetButtonText(tileContentPf.name);
        tool.SetAction(() => TileManager.INSTANCE.SetTileContent(tileContentPf));
    }
}
