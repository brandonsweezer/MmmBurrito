using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledFloor : MonoBehaviour {

	public static float tileWidth = 2f;
	public static float tileHeight = tileWidth;

	public Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}

	void Update() {
		float tileScaleX = rend.bounds.size.x / tileWidth;
		float tileScaleY = rend.bounds.size.z / tileHeight;
		rend.material.mainTextureScale = new Vector2(tileScaleX, tileScaleY);

		float xEdge = transform.position.x + rend.bounds.size.x/2;
		float zEdge = transform.position.z + rend.bounds.size.z/2;
		float tileOffsetX = (- ((xEdge - (tileWidth / 2)) % tileWidth)) / tileWidth;
		float tileOffsetY = (- ((zEdge - (tileHeight / 2)) % tileHeight)) / tileHeight;
		rend.material.mainTextureOffset = new Vector2 (tileOffsetX, tileOffsetY);
	}

}
