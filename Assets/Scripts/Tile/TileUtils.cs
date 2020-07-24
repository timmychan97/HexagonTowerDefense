using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUtils : MonoBehaviour
{
	public const int MAP_LAYER_MASK = 1 << 9;
	static float sqrt3 = Mathf.Sqrt(3);

	/* The coordinate system look like this:
	 *					   .
	 *					  /|\
	 *					   | 
	 *					   | B
	 *					   |
	 *					   |
	 *					   |
	 *					 ,-`-,
	 *			  G  _-'`     ´'-_  R
	 *		     _,-`             ´-._
	 *	      |<,                     ,>|
	 * 
	 * 
	 * World: World space coords, (x,0,z) values;
	 * 
	 */
	public static Tile CreateTileAtCoord(Tile tilePf, Vector3Int coords)
	{
		var tile = Instantiate(tilePf);
		tile.transform.position = RGBCoordsToWorld(coords);
		tile.coords = coords;
		return tile;
	}

	public static Vector3 RGBCoordsToWorld(Vector3Int coords)
	{
		Vector3 r = new Vector3(sqrt3, 0f, -1f) / 2f;
		Vector3 g = new Vector3(-sqrt3, 0f, -1f) / 2f;
		Vector3 b = new Vector3(0f, 0f, 1f);

		return coords.x * r + coords.y * g + coords.z * b;
	}
	
	public static Vector3Int WorldToRGBCoords(Vector3 coords)
	{

		Vector3 r = new Vector3(sqrt3, 0f, -1f) / 2f;
		Vector3 g = new Vector3(-sqrt3, 0f, -1f) / 2f;
		Vector3 b = new Vector3(0f, 0f, 1f);


		var newB = coords.z;
		var newR = (sqrt3 * coords.x - coords.z) / 2f;
		var newG = 0 - newR - newB;

		Vector3 rgbfloat = new Vector3(newR, newG, newB);

		// Try to find the closest grid item.

		// Find closest point from coords to r vector line
		var x = (Mathf.Pow(r.y, 2) * (Mathf.Pow(r.y, 2)* coords.x - Mathf.Pow(r.x, 2) * coords.y)) / (Mathf.Pow(r.x, 2) + Mathf.Pow(r.y, 2));
		var y = (Mathf.Pow(r.x, 2) * (Mathf.Pow(r.x, 2) * coords.y - Mathf.Pow(r.y, 2) * coords.x)) / (Mathf.Pow(r.x, 2) + Mathf.Pow(r.y, 2));

		return Vector3Int.zero;
    }


	public static Tile GetTileUnderMouse(){
		// Raycast through mouse position, and get the Tile
		// NOTE: returns null when no intersection
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 600, MAP_LAYER_MASK))
		{
			Transform objectHit = hit.transform;
			Tile tile = objectHit.gameObject.GetComponentInParent<Tile>();
			return tile;
        }
		return null;
	}
}
