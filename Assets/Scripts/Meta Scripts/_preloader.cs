using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _preloader : MonoBehaviour {

	void Awake () {
		SceneManager.LoadScene ("LevelSelection");
        LoggingManager.instance.Initialize(094, 2, true);
        LoggingManager.instance.RecordPageLoad();

        /* GAME EVENT IDS
         * 0 Location (LoggingCalls)
         * 1 Submission Wrong (SubmissionController)
         * 2 Submission Correct (SubmissionController)
         * 3 Trashing (TrashingController)
         * 4 Dashing (MovementControllerIsometricNew)
         * 5 NOTHING
         * 6 Catching Ingredients (ObjectCatcher)
         * 7 Level end + timer (LevelLoader)
         * 8 Level win + timer (SubmissionController)
         * 9 Powerup Time (PowerupAddTime)
         * 10 Powerup Invulnerable (PowerupMakeInvulnerable)
         * 11 Death to Chef(VulnerableToHazards)
         * 12 Death to DeadlyHazard (VulnerableToHazards)
         * 13 Death to Rat (Rats)
         * 14 Rat steal object (Rats)
         */
    }
}
