using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

  public static GameControl instance;
  public bool isPaused = false; 

  void Awake() {
    if (!instance)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  // Update is called once per frame
  void Update() {

  }
}
