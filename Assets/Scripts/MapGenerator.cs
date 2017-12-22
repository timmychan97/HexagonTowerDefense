using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject tile;
    // 10 = x distance between hexagons
    // Use this for initialization
    private float x = 0f;
    private float z = 0f;
    private int xTiles = 30;
    private int zTiles = 18;
    void Start () {
        for (int i = 0; i < xTiles; i++) {
            for(int j = 0; j < zTiles; j++) {
                GameObject t = Instantiate(tile, this.transform);
                t.transform.position = new Vector3(x, t.transform.position.y, z);
                z += 8.66025403784f; //2 * sqrt(pow(5,2)+pow(2.5,2)) = y distance between hexagons
            }
            x += 7.5f;
            z = (i % 2 == 0) ? 4.33012701892f : 0f;
        }
	}
	// Update is called once per frame
	void Update () {
		
	}
}
