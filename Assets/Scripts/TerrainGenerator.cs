using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

	public Object groundUnit;
    public int terrainWidth;
    public int terrainLength;
	GameObject parent;

	// Use this for initialization
	void Start () {
       // groundUnit = Resources.Load("Prefabs/Cube");
		parent = new GameObject("Terrain Parent");
        GenerateGrid(terrainWidth, terrainLength);
	}
	
    void GenerateGrid(int sizeX, int sizeZ)
    {
        
		for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
				if(i+j %5 != 0)
				MakeCube (i, j);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

	void MakeCube(int x, int y) {
		GameObject go = Instantiate(groundUnit) as GameObject;
		float width = go.GetComponent<Renderer>().bounds.extents.x * 2;
		float height = go.GetComponent<Renderer>().bounds.extents.y * 2;
		go.transform.position = new Vector3(width * x,  0, height * y);
		go.transform.parent = parent.transform;
	}
}
