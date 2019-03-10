using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory instance;

  public int numOfBlueTeamResource = 0;
  public int numOfRedTeamResource = 0;
  public int[] numOfPlayerResource;

  void Awake() {
    if (!instance)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  private void Update() {
    numOfBlueTeamResource = numOfPlayerResource[2] + numOfPlayerResource[3];
    numOfRedTeamResource = numOfPlayerResource[0] + numOfPlayerResource[1];
  }

  public void addRupee(int playerIndex) {
    numOfPlayerResource[playerIndex]++;
  }

  public void addTeamRupee(int teamIndex) {
    if (teamIndex == 0) numOfRedTeamResource++;
    
  }
}
