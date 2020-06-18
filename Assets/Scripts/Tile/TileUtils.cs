using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUtils : MonoBehaviour
{
	public static Tile CreateTileAtCoord(Tile tilePf, Vector3Int coords)
	{
		var tile = Instantiate(tilePf);
		tile.transform.position = RGBCoordToXY(coords);
		tile.coords = coords;
		return tile;
	}

	public static Vector3 RGBCoordToXY(Vector3Int coords)
	{
		Vector3 r = new Vector3(Mathf.Sqrt(3), 0f, -1f) / 2f;
		Vector3 g = new Vector3(-Mathf.Sqrt(3), 0f, -1f) / 2f;
		Vector3 b = new Vector3(0f, 0f, 1f);

		return coords.x * r + coords.y * g + coords.z * b;
	}
}
