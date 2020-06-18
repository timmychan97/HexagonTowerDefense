using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class UI_MapEditor_TileMeshButtons : MonoBehaviour
{
    public Button buttonPf;

    void Start()
    {
        var tileMeshesPf = UI_MapEditor_Utils.GetAllTileMeshesPrefabs();
        Array.ForEach(tileMeshesPf, go => CreateButtons(go.name, go));

    }

    void CreateButtons(string text, GameObject tileMeshPf)
    {
        Button button = Instantiate(buttonPf, transform);
        button.GetComponentInChildren<Text>().text = text;
        button.onClick.AddListener(() => TileManager.INSTANCE.SetTileMesh(tileMeshPf));
    }
}
