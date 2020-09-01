using UnityEngine;

public class FixClothBounds : MonoBehaviour
{

    private SkinnedMeshRenderer[] renderers;

    void Awake()
    {
        renderers = new SkinnedMeshRenderer[] { GetComponent<SkinnedMeshRenderer>() };
    }

    void OnRenderObject()
    {
        foreach (SkinnedMeshRenderer smr in this.renderers)
        {
            smr.localBounds = new Bounds(Vector3.zero, new Vector3(10f, 0.2f, 10f)); // Paste your meshes real bounds here
        }
    }

}