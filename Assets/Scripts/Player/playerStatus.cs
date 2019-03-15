using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatus : MonoBehaviour {

  public enum status { DEFENSE, NORMAL, HOLDING, HELD, FLYING, STUNNED, DASH }
  public status currStatus;
  public int teamIdx = 0; // 0 is red and 1 is blue

  // Start is called before the first frame update
  void Start() {
    currStatus = status.NORMAL;
  }

  // Update is called once per frame
  void Update() {

  }
}
