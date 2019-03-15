using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory instance;

  public int numOfBlueTeamResource = 0;
  public int numOfRedTeamResource = 0;
  public int[] numOfPlayerResource;

  public PanelGenerator panelRed, panelBlue;


  void Awake() {
    if (!instance)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  private void Update() {
    //Debug.Log(numOfBlueTeamResource);
    if (numOfBlueTeamResource<10) {
      panelBlue.StringToDraw = "0" + numOfBlueTeamResource.ToString();
    } else {
      panelBlue.StringToDraw = numOfBlueTeamResource.ToString();
    }
    if (numOfRedTeamResource < 10) {
      panelRed.StringToDraw = "0" + numOfRedTeamResource.ToString();
    }
    else {
      panelRed.StringToDraw = numOfRedTeamResource.ToString();
    }
    //numOfBlueTeamResource = numOfPlayerResource[2] + numOfPlayerResource[3];
    //numOfRedTeamResource = numOfPlayerResource[0] + numOfPlayerResource[1];
  }

  public void addRupee(int playerIndex) {
    numOfPlayerResource[playerIndex]++;
  }

}
