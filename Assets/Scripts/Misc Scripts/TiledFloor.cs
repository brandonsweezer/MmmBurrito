using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TiledFloor : MonoBehaviour {

	public static float tileWidth = 2f;
	public static float tileHeight = tileWidth;

	private Vector2 tileScale = new Vector2 ();
	private Vector2 tileOffset = new Vector2 ();

	private Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
		var tempMaterial = new Material(rend.sharedMaterial);
		rend.sharedMaterial = tempMaterial;
	}

	void Update() {

		tileScale.x = (rend.bounds.size.x / tileWidth);
		tileScale.y = (rend.bounds.size.z / tileHeight);

		float xEdge = transform.position.x + rend.bounds.size.x/2;
		float zEdge = transform.position.z + rend.bounds.size.z/2;
		tileOffset.x = (- ((xEdge - (tileWidth / 2)) % tileWidth)) / tileWidth;
		tileOffset.y = (- ((zEdge - (tileHeight / 2)) % tileHeight)) / tileHeight;

		// Adjust these two vectors based on the object's rotation
		Vector3 scale3D = new Vector3 (tileScale.x, 0, tileScale.y);
		scale3D = Quaternion.Inverse(transform.rotation) * scale3D;
		tileScale.x = scale3D.x;
		tileScale.y = scale3D.z;
		float yRot = (transform.rotation.eulerAngles.y+360f) % 360f;
		if (yRot == 0) {
			//tileOffset.y *= -1;
		} else if (yRot == 90) {
			tileOffset = SwapContents (tileOffset);
			tileOffset.y *= -1;
		} else if (yRot == 180) {
			tileOffset.y *= -1;
		} else if (yRot == 270) {
			tileOffset = SwapContents (tileOffset);
			tileOffset.y *= -1;
		}

		rend.sharedMaterial.mainTextureScale = tileScale;
		rend.sharedMaterial.mainTextureOffset = tileOffset;

	}

	Vector3 SwapContents(Vector2 v) {
		float tmp = v.x;
		v.x = v.y;
		v.y = tmp;
		return v;
	}

}
