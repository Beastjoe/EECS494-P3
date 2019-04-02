using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class rubeeInstantiation : MonoBehaviour {
  public GameObject rupee;
  public float instantiationDuration;
  private float instantiationDurationCd;
  // Start is called before the first frame update
  void Start() {
    instantiationDurationCd = instantiationDuration;
  }

  // Update is called once per frame
  void Update() {
    if (GameControl.instance.isPaused) {
      return;
    }
    instantiationDurationCd -= Time.deltaTime;
    if (instantiationDurationCd < 0) {
      Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
      Instantiate(rupee, generateRandomVector(), rotation);
      instantiationDurationCd = instantiationDuration;
    }
  }

  Vector3 generateRandomVector() {
    float ro = Random.Range(0, 6.5f);
    float theta = Random.Range(0, 2 * Mathf.PI);
    return new Vector3(ro * Mathf.Cos(theta), 0f, ro * Mathf.Sin(theta));
  }
}
