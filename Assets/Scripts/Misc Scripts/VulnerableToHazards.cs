using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableToHazards : MonoBehaviour {

	private float invulnerableTimeLeft;
	ColorFlash flashScript;
    AudioSource audSrc;

    void Start () {

        audSrc = SoundController.instance.audSrc;

        invulnerableTimeLeft = 0;
		flashScript = GetComponent<ColorFlash> ();
	}

	void Update() {
		invulnerableTimeLeft -= Time.deltaTime;

		// Change visual if invulnerable
		if (!IsInvulnerable()) {
			flashScript.SetActive (false);
		}
	}

	/** Handle collisions with deadly hazards */
	void OnCollisionEnter (Collision collision) {
		if (IsInvulnerable()) {
			return;
		}

		GameObject gameObj = collision.gameObject;
		if (gameObj.tag == "DeadlyHazard" || gameObj.tag == "Chef" || gameObj.tag == "Rat") {
            //TODO: DEATH SOUND
			audSrc.PlayOneShot(SoundController.instance.death, SoundController.instance.SoundEffectVolume);
            GameController.instance.dead = true;
            /*OrderUI.instance.ResetAfterDeath();
            OrderUI.instance.CollectionUIUpdate();*/
			OrderUI.instance.setGeneralMessage ("You have died!");
            if (gameObj.tag == "DeadlyHazard")
            {
                LoggingManager.instance.RecordEvent(12, "Died to a " + gameObj.tag);
            }
            else if(gameObj.tag == "Chef")
            {
                LoggingManager.instance.RecordEvent(11, "Died to a " + gameObj.tag);
            }
            else
            {
                LoggingManager.instance.RecordEvent(13, "Died to a " + gameObj.tag);
            }
            
        }
    }

	/** Makes this object invulnerable for the specified number of seconds */
	public void SetInvulnerableDuration(float duration) {
		invulnerableTimeLeft = duration;
		if (duration > 0) {
			flashScript.SetActive (true);
		}
	}

	public bool IsInvulnerable() {
		return invulnerableTimeLeft > 0;
	}
}
