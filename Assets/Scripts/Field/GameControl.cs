using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

  public static GameControl instance;
  public bool isPaused = false;
  public bool tutorialPaused = false;
  public GameObject pausePanel;
  public bool panelReady = false;
  public bool pauseReady = true;
  public bool selectState = false;
  public bool playState = false;
  public bool tutorialState = false;

  void Awake() {
    if (!instance)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  // Update is called once per frame
  void Update() {
    if (isPaused) {
      pausePanel.SetActive(true);
      if (panelReady) {
        for (int i = 0; i < Gamepad.all.Count; i++) {
          Gamepad gp = Gamepad.all[i];
          if (gp.startButton.isPressed) {
            isPaused = false;
            panelReady = false;
            pauseReady = false;
            StartCoroutine(pauseReadyCoolDown());
            pausePanel.SetActive(false);
            break;
          }
          else if (gp.aButton.isPressed && playState) {
            SceneManager.LoadScene("playLab");
          }
          else if (gp.aButton.isPressed && tutorialState) {
            SceneManager.LoadScene("TutorialIndividualLab");
          }
          else if (gp.bButton.isPressed) {
            SceneManager.LoadScene("MainMenu");
          }
        }
      } else {
        StartCoroutine(panelReadyCoolDown());
      }
    }
  }

  IEnumerator panelReadyCoolDown() {
    yield return new WaitForSeconds(0.5f);
    panelReady = true;
  }

  IEnumerator pauseReadyCoolDown() {
    yield return new WaitForSeconds(1.0f);
    pauseReady = true;
  }
}
