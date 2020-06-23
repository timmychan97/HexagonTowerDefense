
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapEditor_Utils : MonoBehaviour
{
    // The path within Assets/Resources/ that contains the TileMesh prefabs
    private static string RESOURCES_PATH_TILE_MESH = "Tile/TileMeshes";
    private static string RESOURCES_PATH_TILE_CONTENT = "Tile/TileContents";


    //public static void FillWithButtonsOfType()
    //{
    //    // Generate the buttons as children
    //    foreach (var tileMeshPrefabObject in tileMeshPrefabObjects)
    //    {
    //        GameObject tileMeshGameObject = (GameObject)tileMeshPrefabObject;
    //        CreateTileMeshButton(tileMeshPrefabObject.name, tileMeshGameObject);
    //    }
    //    Debug.Log("[UI] - Loaded " + sortedList.Count + " TileMesh buttons");

    //}


    public static GameObject[] GetAllTileMeshesPrefabs()
    {
        return Resources_GetPrefabsSorted(RESOURCES_PATH_TILE_MESH);
    }

    public static GameObject[] GetAllTileContentsPrefabs()
    {
        return Resources_GetPrefabsSorted(RESOURCES_PATH_TILE_CONTENT);
    }

    public static GameObject[] Resources_GetPrefabsSorted(string resourcesPath)
    {
        // The path within Assets/Resources/ which contains the prefabs

        // Get the objects from Resources folder
        //GameObject[] tileMeshPrefabObjects = (GameObject[])Resources.LoadAll(resourcesPath, typeof(GameObject));


        GameObject[] tileMeshPrefabs = Resources.LoadAll(resourcesPath, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Sort the objects by name
        return tileMeshPrefabs.OrderBy(go => go.name).ToArray();
    }
}
