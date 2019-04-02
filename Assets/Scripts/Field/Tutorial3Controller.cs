using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3Controller : MonoBehaviour {

  bool generated = false;
  public GameObject[] rupees;
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (isActiveAndEnabled && !generated) {
      generated = true;
      for (int i=0;i<rupees.Length;i++) {
        rupees[i].SetActive(true);
      }
    }
  }
}
