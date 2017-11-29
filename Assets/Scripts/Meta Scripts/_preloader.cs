using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _preloader : MonoBehaviour {

	void Awake () {
        //version 4 is KONGREGATE
        LoggingManager.instance.Initialize(094, 4, true);
        LoggingManager.instance.RecordPageLoad();
        GameController.instance.ABValue = LoggingManager.instance.assignABTestValue(Random.Range(1, 3));
        LoggingManager.instance.RecordABTestValue();
        SceneManager.LoadScene ("Menu");

        //resubmit total stars on game load
        Application.ExternalCall("kongregate.stats.submit", "Stars", SaveManager.instance.totalStars());
        //submit highest level completed
        Application.ExternalCall("kongregate.stats.submit", "HighestLevel", SaveManager.instance.GetLastLevelCompleted());

        /* GAME EVENT IDS
         * 0 Location (LoggingCalls)
         * 1 Submission Wrong (SubmissionController)
         * 2 Submission Correct (SubmissionController)
         * 3 Trashing (TrashingController)
         * 4 Dashing (MovementControllerIsometricNew)
         * 5 NOTHING
         * 6 Catching Ingredients (ObjectCatcher)
         * 7 Level end + timer (LevelLoader && SubmissionController && Timer)
         * 8 Level win + timer (SubmissionController)
         * 9 Powerup Time (PowerupAddTime)
         * 10 Powerup Invulnerable (PowerupMakeInvulnerable)
         * 11 Death to Chef(VulnerableToHazards)
         * 12 Death to DeadlyHazard (VulnerableToHazards) //USELESS
         * 13 Death to Rat (Rats)
         * 14 Rat steal object (Rats)
         */
    }
}
