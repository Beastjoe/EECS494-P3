using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowAndPickController : MonoBehaviour {
  public GameObject player0;
  public GameObject player1;
  public GameObject player2;
  public GameObject player3;

  public bool passTutorialAimAndThrow = false;

  // Update is called once per frame
  void Update() {
    int held = 0;
    if (player0.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
      held += 1;
    if (player1.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
      held += 1;
    if (player2.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
      held += 1;
    if (player3.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
      held += 1;
    if(held ==2)
      passTutorialAimAndThrow = true;
  }
}
