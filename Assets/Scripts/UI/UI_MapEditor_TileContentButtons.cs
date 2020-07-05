using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;

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
