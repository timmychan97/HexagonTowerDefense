using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class UI_MapEditor_TileContentButtons : MonoBehaviour
{
    public Button buttonPf;

    void Start()
    {
        var tileContentsPf = UI_MapEditor_Utils.GetAllTileContentsPrefabs();
        Array.ForEach(tileContentsPf, go => CreateButtons(go.name, go));

    }

    void CreateButtons(string text, GameObject tileMeshPf)
    {
        Button button = Instantiate(buttonPf, transform);
        button.GetComponentInChildren<Text>().text = text;
        button.onClick.AddListener(() => TileManager.INSTANCE.SetTileContent(tileMeshPf));
    }
}
