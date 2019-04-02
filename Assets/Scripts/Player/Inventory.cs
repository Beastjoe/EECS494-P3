using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory instance;

  public int numOfBlueTeamResource = 0;
  public int numOfRedTeamResource = 0;
  public int[] numOfPlayerResource;
  public bool inTutorialMode = false;

  public PanelGenerator panelRed, panelBlue;

  private float panelRedBlinkTime = 0.0f;
  private float panelBlueBlinkTime = 0.0f;
  private bool panelRedBlinkFirstCall = true;
  private bool panelBlueBlinkFirstCall = true;


  void Awake() {
    if (!instance)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  private void Update() {
    //Debug.Log(numOfBlueTeamResource);
    if (numOfBlueTeamResource < 10) {
      panelBlue.StringToDraw = "0" + numOfBlueTeamResource.ToString();
    }
    else {
      panelBlue.StringToDraw = numOfBlueTeamResource.ToString();
    }
    if (numOfRedTeamResource < 10) {
      panelRed.StringToDraw = "0" + numOfRedTeamResource.ToString();
    }
    else {
      panelRed.StringToDraw = numOfRedTeamResource.ToString();
    }

  }

  public void addRupee(int playerIndex) {
    numOfPlayerResource[playerIndex]++;
  }

  public void stopBlinkPanel(int teamIdx) {
    if (teamIdx==0) {
      panelRedBlinkTime = 1.2f;
      if (panelRedBlinkFirstCall) {
        StartCoroutine(stopBlink(teamIdx));
        panelRedBlinkFirstCall = false;
      }
    } else {
      panelBlueBlinkTime = 1.2f;
      if (panelBlueBlinkFirstCall) {
        StartCoroutine(stopBlink(teamIdx));
        panelBlueBlinkFirstCall = false;
      }
    }
  }

  IEnumerator stopBlink(int teamIdx) {
    if (teamIdx==0) {
      if (panelRedBlinkTime<=0.0f) {
        panelRedBlinkTime = 1.2f;
      }
      while (panelRedBlinkTime>0.0f) {
        panelRedBlinkTime -= Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);
      }
      panelRed.TextMotionStyle = PanelGenerator.MotionStyle.Pages;
      panelRed.RefreshTime = 0;
      panelRedBlinkFirstCall = true;
    } else {
      if (panelBlueBlinkTime <= 0.0f) {
        panelBlueBlinkTime = 1.2f;
      }
      while (panelBlueBlinkTime > 0.0f) {
        panelBlueBlinkTime -= Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);
      }
      panelBlue.TextMotionStyle = PanelGenerator.MotionStyle.Pages;
      panelBlue.RefreshTime = 0;
      panelBlueBlinkFirstCall = true;
    }
  }
}
