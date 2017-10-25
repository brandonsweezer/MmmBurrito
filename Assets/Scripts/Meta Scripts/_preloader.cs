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
         * 1 Ingredient Spawn (ObjectSpawn)
         * 2 Submission (SubmissionController Twice)
         * 3 Trashing (TrashController & MovementControllerIsometricNew)
         * 4 Dashing (MovementControllerIsometricNew)
         * 5 Death (VulnerableToHazards)
         * 6 Catching Ingredients (ObjectCatcher)
         * 7 Level end + timer (LevelLoader)
         * 8 Level win + timer (SubmissionController)
         */
    }
}
