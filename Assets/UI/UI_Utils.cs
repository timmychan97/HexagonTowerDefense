
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Utils : MonoBehaviour
{
    public static GameObject[] GetPrefabsSorted(string resourcesPath)
    {
        // The path within Assets/Resources/ which contains the prefabs
        GameObject[] pf = Resources.LoadAll(resourcesPath, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Sort the objects by name
        return pf.OrderBy(x => x.name).ToArray();
    }
}
