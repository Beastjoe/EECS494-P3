using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDummyBurrowController : MonoBehaviour {

    public GameObject redSideDummy;
    public GameObject blueSideDummy;
    public bool blueHit = false;
    public bool redHit = false;

    // Start is called before the first frame update
    void Start() {
        redSideDummy.GetComponent<playerStatus>().currStatus = playerStatus.status.DEFENSE;
        redSideDummy.GetComponent<Animator>().SetTrigger("defenseTrigger");
        blueSideDummy.GetComponent<playerStatus>().currStatus = playerStatus.status.DEFENSE;
        blueSideDummy.GetComponent<Animator>().SetTrigger("defenseTrigger");
    }

    // Update is called once per frame
    void Update() {

    }
}
