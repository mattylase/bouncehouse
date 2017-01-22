using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    private Object groundUnit;
    public int terrainWidth;
    public int terrainLength;

	// Use this for initialization
	void Start () {
        groundUnit = Resources.Load("Prefabs/Cube");
	}
	
    public void GenerateGrid(int sizeX, int sizeZ)
    {
        GameObject parent = new GameObject("Terrain Parent");
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                GameObject go = Instantiate(groundUnit) as GameObject;
                float width = go.GetComponent<Renderer>().bounds.extents.x * 2;
                float height = go.GetComponent<Renderer>().bounds.extents.y * 2;
                go.GetComponent<Renderer>().material.SetColor("_Color", new Color(Mathf.Cos(i), .5f, Mathf.Sin(j)));
                go.transform.position = new Vector3(width * i,  0, height * j);
                go.transform.parent = parent.transform;
            }
        }
    }

}
