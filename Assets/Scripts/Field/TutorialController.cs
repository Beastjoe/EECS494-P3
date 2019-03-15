using System.Collections;
using System.Collections.Generic;

using UnityEngine.Experimental.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {

  public GameObject nextTutorial;
  public int tutorialIdx = 0;

  public bool[] playerFlag = new bool[4];
  Gamepad[] playerPads = new Gamepad[4];


  private void passToNextTutorial() {
    Debug.Log("pass");
    gameObject.SetActive(false);
    nextTutorial.SetActive(true);
    if (tutorialIdx == 2) {

    }
  }

  private void Start() {
    for (int i = 0; i < playerFlag.Length; i++) {
      playerFlag[i] = false;
      playerPads[i] = Gamepad.all[i];
    }
  }

  // Update is called once per frame
  void Update() {
    bool pass = true;
    if (tutorialIdx == 0) {
      for (int i = 0; i < 4; i++) {
        if (Mathf.Abs(playerPads[i].leftStick.ReadValue().x) > 0.1 || Mathf.Abs(playerPads[i].leftStick.ReadValue().y) > 0.1) {
          playerFlag[i] = true;
        }
      }
      for (int i = 0; i < 4; i++) {
        pass = pass & playerFlag[i];
      }
    }
    else if (tutorialIdx == 1) {
      for (int i = 0; i < 4; i++) {
        if (Mathf.Abs(playerPads[i].rightStick.ReadValue().x) > 0.1 || Mathf.Abs(playerPads[i].rightStick.ReadValue().y) > 0.1) {
          playerFlag[i] = true;
        }
      }
      for (int i = 0; i < 4; i++) {
        pass = pass & playerFlag[i];
      }
    }
    else if (tutorialIdx == 2) {
      for (int i = 0; i < 4; i++) {
        if (playerPads[i].rightShoulder.isPressed) {
          playerFlag[i] = true;
        }
      }
      for (int i = 0; i < 4; i++) {
        pass = pass & playerFlag[i];
      }
    }
    else if (tutorialIdx == 3) {
      int total_collected_rupees = 0;
      for (int i = 0; i < 4; ++i) {
        total_collected_rupees += Inventory.instance.numOfPlayerResource[i];
      }
      if (total_collected_rupees != 8) {
        pass = false;
      }
    }
    else if (tutorialIdx == 4) {
      if (!GetComponent<BurrowAndPickController>().passTutorialAimAndThrow) {
        pass = false;
      }
    }
    else if (tutorialIdx == 5) {
      if (!(GetComponent<TutorialThrowController>().Blue.number_of_rupees < 4
&& GetComponent<TutorialThrowController>().Red.number_of_rupees < 4)) {
        pass = false;
      }
    }
    else if (tutorialIdx == 6) {
      if (!GetComponent<TutorialThrowController>().passTutorial6) {
        pass = false;
      }
    }
    else if (tutorialIdx == 7) {
      pass = false;
      if ((Inventory.instance.numOfRedTeamResource >= 1 && Inventory.instance.numOfBlueTeamResource >= 1))
        SceneManager.LoadScene("playLab");
    }
    if (pass) {
      passToNextTutorial();
    }

  }
}
