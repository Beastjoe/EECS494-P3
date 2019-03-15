using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialThrowController : MonoBehaviour {
  public GameObject BlueDummy;
  public GameObject RedDummy;

  public ArrowKeyMovementDummy Blue;
  public ArrowKeyMovementDummy Red;

  public bool passTutorial6 = false;
  // Start is called before the first frame update
  void Start() {
    Blue = BlueDummy.GetComponent<ArrowKeyMovementDummy>();
    Red = RedDummy.GetComponent<ArrowKeyMovementDummy>();
  }

  // Update is called once per frame
  void Update() {
    if(Blue.number_of_rupees + Red.number_of_rupees == 0) {
      passTutorial6 = true;
    }
  }
}
