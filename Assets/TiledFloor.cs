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
		float tileOffsetX = -(transform.position.x + rend.bounds.size.x) % tileWidth;
		float tileOffsetY = -(transform.position.z + rend.bounds.size.z) % tileHeight;
		rend.material.mainTextureOffset = new Vector2 (tileOffsetX/2, tileOffsetY/2);
	}

}
