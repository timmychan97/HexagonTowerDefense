using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;

public class UI_InGame : MonoBehaviour
{
    public UI_Tool_Tower toolPf;

    private static string PATH_TOWER_MESH = "TowerMeshes";

    // Start is called before the first frame update
    void Start()
    {
        Tower[] towerMeshPf = GetPfSortedTower(PATH_TOWER_MESH);
        Array.ForEach(towerMeshPf, x => CreateBtnTower(x));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBtnTower(Tower tower)
    {
        UI_Tool_Tower toolTower = Instantiate(toolPf, transform);
        toolTower.SetButtonText(tower.name);
        toolTower.tower = tower;
        toolTower.SetAction(() =>
        {
            if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
            // Debug.Log("Try setting tile content");
            if (Tile.active.CanPlaceTower() && GameController.INSTANCE.BuyTower(tower))
            {
                TileManager.INSTANCE.SetTileContent(tower.gameObject);
            }
        });
    }

    // return array of prefabs sorted by name
    public static Tower[] GetPfSortedTower(string path) 
    {
        Tower[] towerMeshPrefabs = Resources.LoadAll(path, typeof(Tower)).Cast<Tower>().ToArray();
        return towerMeshPrefabs.OrderBy(go => go.name).ToArray();
    }
}
