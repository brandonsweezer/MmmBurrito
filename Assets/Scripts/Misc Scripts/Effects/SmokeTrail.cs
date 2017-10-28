using UnityEngine;
using System.Collections;

public class SmokeTrail : MonoBehaviour {

	public float minSpeedForParticles;
	public float speedForMaxEmissionRate;
	public float maxEmissionRateMultiplier;
	// Multiplies negative velocity by this value to determine distance behind parent.
	public float speedDistanceFromParentMultiplier;
	public float yOffsetFromParent;
	private float defaultEmissionRate;

	private Rigidbody rb;
	private Vector3 yOffset;
	public ParticleSystem smokeTrailPrefab;
	private ParticleSystem smokeTrail;

	void Awake() {
		rb = GetComponent<Rigidbody> ();
		yOffset = new Vector3 (0, yOffsetFromParent, 0);
	}

	void Start () {
		smokeTrail = Instantiate (smokeTrailPrefab, transform) as ParticleSystem;
		defaultEmissionRate = smokeTrail.emission.rateOverTime.constant;
	}


	void Update () {
		smokeTrail.transform.position = gameObject.transform.position + yOffset + (-rb.velocity * speedDistanceFromParentMultiplier);
		float currentSpeed = rb.velocity.magnitude;
		if (currentSpeed > minSpeedForParticles) {
			float lerp = Mathf.Min (1, (currentSpeed - minSpeedForParticles) / (currentSpeed - maxEmissionRateMultiplier));
			smokeTrail.emissionRate = Mathf.Lerp(defaultEmissionRate, defaultEmissionRate*maxEmissionRateMultiplier, lerp);
		} else {
			smokeTrail.emissionRate = 0;
		}
	}

}