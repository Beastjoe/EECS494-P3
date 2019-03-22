using System.Collections;
using System.Collections.Generic;

using UnityEngine.Experimental.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

  public GameObject nextTutorial;
  public int tutorialIdx = 0;
  public AnimationCurve curve;
  public float originalPos = 0;
  public GameObject platformRed;
  public GameObject platformBlue;
  public GameObject BlackShader;

  public bool[] playerFlag = new bool[4];
  Gamepad[] playerPads = new Gamepad[4];
  bool passToNextTutorialCalled = false;
  bool dialogueReadyToSkip = false;
  bool dialogueReadyToEnd = false;

  private void passToNextTutorial() {
    // Disappear Effect
    StartCoroutine(disappearEffect());
    if (tutorialIdx==3) {
      platformRed.GetComponent<platformMove>().triggerMove();
      platformBlue.GetComponent<platformMove>().triggerMove();
    }

    if (tutorialIdx < 0) {
      GameControl.instance.tutorialProgres = tutorialIdx;
    }
  }

  private void Start() {
    for (int i = 0; i < playerFlag.Length; i++) {
      playerFlag[i] = false;
      playerPads[i] = Gamepad.all[i];
    }
    StartCoroutine(dialogueReadyToSkipCoolDown());
  }
 

  // Update is called once per frame
  void Update() {
    bool pass = true;
    // Dialogues have negative index
    if (tutorialIdx < 0) {
      pass = false;
      for (int i=0; i<4; i++) {
        if (playerPads[i].bButton.isPressed && dialogueReadyToSkip && !dialogueReadyToEnd) {
          StartCoroutine(dialogueReadyToEndCoolDown());
          transform.GetChild(1).GetComponent<typerEffect>().jumpText();
          break;
        }
        if (playerPads[i].bButton.isPressed && dialogueReadyToEnd) {
          pass = true;
          if (tutorialIdx == -9)
          {
            StartCoroutine(Fading());
          }
          break;
        }
      }
    }

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
        if (playerPads[i].rightTrigger.isPressed) {
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
        pass = true;
    }
    if (pass && !passToNextTutorialCalled) {
      passToNextTutorialCalled = true;
      passToNextTutorial();
    }

  }

  IEnumerator disappearEffect() {
    for (float i=0.0f;i<=0.5f;i+=Time.deltaTime) {
      if (curve.Evaluate(i) * 2 * originalPos - originalPos > GetComponent<RectTransform>().anchoredPosition.y) {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, curve.Evaluate(i) * 2 * originalPos - originalPos, 0);
      }
      yield return new WaitForSeconds(Time.deltaTime);
    }
    // if Dialogue, continue to let player control
    if (tutorialIdx<0) {
      GameControl.instance.tutorialPaused = false;
    }
    nextTutorial.SetActive(true);
    if (tutorialIdx == -7) {
      Debug.Log(nextTutorial.GetComponent<RectTransform>().anchoredPosition);
    }
    nextTutorial.GetComponent<TutorialController>().originalPos = nextTutorial.GetComponent<RectTransform>().anchoredPosition.y;
    for (float i = 0.0f; i <= 0.5f; i += Time.deltaTime) {
      nextTutorial.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, nextTutorial.GetComponent<TutorialController>().originalPos - curve.Evaluate(i) * 2 * nextTutorial.GetComponent<TutorialController>().originalPos, 0);
      yield return new WaitForSeconds(Time.deltaTime);
    }
    // if Dialogue, block player control
  //  if (nextTutorial.GetComponent<TutorialController>().tutorialIdx < 0) {
  //    GameControl.instance.tutorialPaused = true;
  //  }
    gameObject.SetActive(false);
  }

  IEnumerator dialogueReadyToSkipCoolDown() {
    yield return new WaitForSeconds(0.5f);
    dialogueReadyToSkip = true;
  }

  IEnumerator dialogueReadyToEndCoolDown() {
    yield return new WaitForSeconds(0.1f);
    dialogueReadyToEnd = true;
  }
  
  IEnumerator Fading()
  {
    BlackShader.SetActive(true);
    for (int i = 0; i < 100; ++i)
    {
      Color c = BlackShader.GetComponent<Image>().color;
      c.a += 0.01f;
      BlackShader.GetComponent<Image>().color = c;
      yield return new WaitForSeconds(0.015f);
    }
    SceneManager.LoadScene("playLab");
  }
}
