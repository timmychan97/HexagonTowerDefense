using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public Tile tilePf;

    // 10 = x distance between hexagons
    // Use this for initialization
    public int xTiles = 30;
    public int zTiles = 20;

    void Start() => CreateEmptyBoard(xTiles, zTiles);

    public void CreateEmptyBoard(int nx, int nz)
    {
        for (int b = 0; b < nz; b++)
        {
            for (int i = 0; i < nx; i++)
            {
                int r = - b / 2 + i;
                int g = -(r + b);
                Tile tile = TileUtils.CreateTileAtCoord(tilePf, new Vector3Int(r, g, b));
                tile.transform.parent = transform;
            }
        }
    }
}
