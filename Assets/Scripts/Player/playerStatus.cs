using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatus : MonoBehaviour {

  public enum status { DEFENSE, NORMAL, HOLDING, HELD, FLYING }
  public status currStatus;

  // Start is called before the first frame update
  void Start() {
    currStatus = status.NORMAL;
  }

  // Update is called once per frame
  void Update() {

  }
}
