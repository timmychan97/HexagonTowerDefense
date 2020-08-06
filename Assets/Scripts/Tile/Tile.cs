using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour {
	public static Tile active;
	public Transform tileContentContainer;
	public Transform tileMeshContainer;
	public Vector3Int coords;
	public enum TileType {Basic, Grass, Stone, Road, Water}
	public bool isWalkable = false;
	// Use this for initialization
	public TileType tileType = TileType.Basic;

	public void SetTileMesh(GameObject tileMeshPf)
    {
		foreach (Transform c in tileMeshContainer)
			Destroy(c.gameObject);
		var tileMesh = Instantiate(tileMeshPf, tileMeshContainer);

		// Set tile type
		if (tileMesh.tag == "Grass") 
			tileType = TileType.Grass;
		else if (tileMesh.tag == "Stone")
			tileType = TileType.Stone; 
		else if (tileMesh.tag == "Road")
			tileType = TileType.Road;
		else if (tileMesh.tag == "Water")
			tileType = TileType.Water;
		else
			tileType = TileType.Basic;

		this.name = "Tile - " + tileMesh.name;
	}

	public bool CanPlaceUnit() 
	{
		if (HasUnit()) return false;
		if (tileType == TileType.Basic || tileType == TileType.Grass || tileType == TileType.Stone)
		{
			return true;
		}
		return false;
	}

	public bool HasUnit() 
	{
		IPlacable t = tileContentContainer.GetComponentInChildren<IPlacable>();
		if (t == null) return false;
		return true;
	}

	public GameUnit GetGameUnit()
	{
		return tileContentContainer.GetComponentInChildren<GameUnit>();
	}

	public void SetTileContent(GameObject obj)
	{
		foreach (Transform c in tileContentContainer)
			Destroy(c.gameObject);
		obj.transform.SetParent(tileContentContainer);
		obj.transform.localPosition = Vector3.zero;
	}

	public void OnClick()
    {
		TileManager.INSTANCE.OnClick(this);
    }


	#region highlighter
	public void Activate()
	{
		SetHighlight (transform, true);
	}
	public void Deactivate()
	{
		SetHighlight (transform, false);
	}

	void SetHighlight(Transform t, bool isEnabled)
	{
		// for each children, if it can be outlined, outline it.
		return;
	}
	#endregion

    public void Highlight()
    {
		Highlight(null);
	}

	public void Highlight(Color? color)
	{
		var _propBlock = new MaterialPropertyBlock();
		var _renderer = tileMeshContainer.GetComponentInChildren<Renderer>();
		_renderer.GetPropertyBlock(_propBlock);

		// Create two colors to loop between
		Color color1 = _renderer.material.color;
		color1 = color1 * 1.12f;
		if (color != null)
        {
			float gs = color1.grayscale;
			//color1 = new Color(gs, gs, gs, 1) + (Color)color * 0.7f;
			Color gsColor = new Color(gs, gs, gs, 1);

			// Average the colors
			color1 = Color.Lerp(gsColor, (Color)color, .6f);
		}
		Color color2 = color1 * 1.14f;

		var speed = 5f;
		var offset = 1f;
		// Assign our new value.
		// URP uses "_BaseColor" instead of the old name "_Color"
		_propBlock.SetColor("_BaseColor", Color.Lerp(color1, color2, (Mathf.Sin(Time.time * speed + offset) + 1) / 2f));
		// Apply the edited values to the renderer.
		_renderer.SetPropertyBlock(_propBlock);
	}

    public void DeHighlight()
	{
		var _propBlock = new MaterialPropertyBlock();
		var _renderer = tileMeshContainer.GetComponentInChildren<Renderer>();
		_renderer.GetPropertyBlock(_propBlock);

		Color color1 = _renderer.material.color;
		_propBlock.SetColor("_BaseColor", color1);
		_renderer.SetPropertyBlock(_propBlock);

	}

	public float GetY()
	{
		return transform.position.y;
	}
}
