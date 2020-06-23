using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUtils : MonoBehaviour
{
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
		Vector3 r = new Vector3(Mathf.Sqrt(3), 0f, -1f) / 2f;
		Vector3 g = new Vector3(-Mathf.Sqrt(3), 0f, -1f) / 2f;
		Vector3 b = new Vector3(0f, 0f, 1f);

		return coords.x * r + coords.y * g + coords.z * b;
	}
	
	public static Vector3Int WorldToRGBCoords(Vector3 coords)
	{

		Vector3 r = new Vector3(Mathf.Sqrt(3), 0f, -1f) / 2f;
		Vector3 g = new Vector3(-Mathf.Sqrt(3), 0f, -1f) / 2f;
		Vector3 b = new Vector3(0f, 0f, 1f);


		var newB = coords.z;
		var newR = (Mathf.Sqrt(3) * coords.x - coords.z) / 2f;
		var newG = 0 - newR - newB;

		Vector3 rgbfloat = new Vector3(newR, newG, newB);

		// Try to find the closest grid item.

		// Find closest point from coords to r vector line
		var x = (Mathf.Pow(r.y, 2) * (Mathf.Pow(r.y, 2)* coords.x - Mathf.Pow(r.x, 2) * coords.y)) / (Mathf.Pow(r.x, 2) + Mathf.Pow(r.y, 2));
		var y = (Mathf.Pow(r.x, 2) * (Mathf.Pow(r.x, 2) * coords.y - Mathf.Pow(r.y, 2) * coords.x)) / (Mathf.Pow(r.x, 2) + Mathf.Pow(r.y, 2));

		return Vector3Int.zero;
    }

}
