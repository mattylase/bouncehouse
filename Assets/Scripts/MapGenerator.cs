using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public Object groundUnit;
	public Vector2 Size = new Vector2 (20, 20);
	public float Height = 10.0f;
	public float NoiseSize = 10.0f;

	private GameObject root;

	void Start ()
	{
		//Generate ();

	}

	public float PerlinNoise (float x, float y)
	{
		float noise = Mathf.PerlinNoise (x / NoiseSize, y / NoiseSize);

		return noise * Height;

	}

	public void Generate ()
	{
		Vector2 offset = new Vector2(Random.Range(0,500),Random.Range(0,500));
		Destroy (GameObject.Find ("Terrain"));
		root = new GameObject ("Terrain");
		root.transform.position = new Vector3 (Size.x / 2, 0, Size.y / 2);

		for (int i = 0; i <= Size.x; i++) {
			for (int p = 0; p <= Size.y; p++) {
				int spotHeight = (int)PerlinNoise (i + offset.x, p + offset.y);
				for (int j = spotHeight; j >= 0; j--) {
						GameObject box = Instantiate (groundUnit) as GameObject;
						float col = (1/Height) * j;	
						box.GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color (col, col, col));
						float dimensions = box.GetComponent<Renderer> ().bounds.extents.x * 2;
						box.transform.position = new Vector3 (i * dimensions, j * dimensions, p * dimensions);
						box.transform.parent = root.transform;
						box.GetComponent<Rigidbody> ().drag = 1f + (Height - j); 
				}
			}
		}
		root.transform.position = Vector3.zero;
	}
}