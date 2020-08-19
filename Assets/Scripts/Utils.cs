using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils : MonoBehaviour
{
    /// <summary>
    /// Get all GameObjects from a specific path within the resources folder.
    /// The array is sorted alphabetically by the gameObjects name, ascending.
    /// </summary>
    /// <param name="resourcesPath">Relative path inside the Resources folder</param>
    /// <returns></returns>
    public static GameObject[] GetResourcePrefabsSorted(string resourcesPath)
    {

        GameObject[] pfs = Resources.LoadAll(resourcesPath, typeof(GameObject)).Cast<GameObject>().ToArray();

        // Sort the objects by name, ascending
        return pfs.OrderBy(x => x.name).ToArray();
    }

    /// <summary>
    /// Get all Compenents of the given type from a specific path within the resources folder.
    /// The array is sorted alphabetically by the gameObjects name, ascending.
    /// </summary>
    /// <typeparam name="T">The Component type</typeparam>
    /// <param name="resourcesPath">Relative path inside the Resources folder</param>
    /// <returns></returns>
    public static T[] GetResourcePrefabsComponentsSorted<T>(string resourcesPath)
    {
        T[] pfs = Resources.LoadAll(resourcesPath, typeof(T)).Cast<T>().ToArray();

        // Sort the components by the gameObject's name, ascending
        return pfs.OrderBy(x => ((Component)(object)x).gameObject.name).ToArray();
    }

    public static GameObject GetGameObjectAtMousePos(LayerMask layerMask)
    {
        int cameraDistance = 600;
        RaycastHit rh;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out rh, cameraDistance, layerMask))
            return rh.transform.gameObject;
        return default;
    }

    public static T GetCompenentAtMousePos<T>(LayerMask layerMask)
    {
        var go = GetGameObjectAtMousePos(layerMask);
        return go ? go.GetComponentInParent<T>() : default;
    }
}
