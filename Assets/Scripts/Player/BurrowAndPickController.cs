using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowAndPickController : MonoBehaviour {
    public GameObject player0;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    public bool passTutorialAimAndThrow = false;
    public bool redTeamReady = false, blueTeamReady = false;

    // Update is called once per frame
    void Update() {
        if (player0.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
            redTeamReady = true;
      if (player1.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
            redTeamReady = true;
        if (player2.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
            blueTeamReady = true;
        if (player3.GetComponent<playerStatus>().currStatus == playerStatus.status.HELD)
            blueTeamReady = true;
        passTutorialAimAndThrow = blueTeamReady && redTeamReady;
    }
}
