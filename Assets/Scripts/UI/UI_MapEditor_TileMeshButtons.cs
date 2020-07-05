using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class UI_MapEditor_TileMeshButtons : MonoBehaviour
{
    public UI_Tool toolPf;

    void Start()
    {
        var tileMeshesPf = UI_MapEditor_Utils.GetAllTileMeshesPrefabs();
        Array.ForEach(tileMeshesPf, go => CreateTools(go));
    }

    void CreateTools(GameObject tileContentPf)
    {
        UI_Tool tool = Instantiate(toolPf, transform);
        tool.SetButtonText(tileContentPf.name);
        tool.SetAction(() => TileManager.INSTANCE.SetTileMesh(tileContentPf));
    }
}
