using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    [SerializeField] private Material material;
    private Renderer outlineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // float scaleFactor = material.GetFloat("_scaleFactor");
        float scaleFactor = 1.5f;
        Color color = material.GetColor("_color");
        outlineRenderer = CreateOutlineRenderer(material, scaleFactor, color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        outlineRenderer.enabled = true;
        Debug.Log("renderer = true");
    }

    private void OnMouseExit()
    {
        outlineRenderer.enabled = false;
    }

    private Renderer CreateOutlineRenderer(Material outlineMat, float scaleFactor, Color color) 
	{
		GameObject outlineObj = Instantiate(gameObject, transform.position, transform.rotation, transform);
		Renderer rend = outlineObj.GetComponent<Renderer>();
		rend.material = outlineMat;
		rend.material.SetColor("Color", color);
		rend.material.SetFloat("Scale Factor", scaleFactor);
		rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		outlineObj.GetComponent<Outliner>().enabled = false;
		// outlineObj.GetComponent<Collider>().enabled = false;
        rend.enabled = false;

		return rend;
	}
}
