using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _preloader : MonoBehaviour {

	void Awake () {
		SceneManager.LoadScene ("LevelSelection");
        Debug.Log("hi");
        LoggingManager.instance.Initialize(094, 1, false);
        LoggingManager.instance.RecordPageLoad();

        /* GAME EVENT IDS
         * 0 Location (MovementControllerIsometric)
         * 1 Ingredient Spawn (ObjectSpawn)
         * 2 Submission (SubmissionController Twice)
         * 3 Trashing (TrashController)
         * 4 Dashing (MovementControllerIsometric)
         * 5 Death (VulnerableToHazards)
         * 6 Catching Ingredients (ObjectCatcher)
         */
    }
}
