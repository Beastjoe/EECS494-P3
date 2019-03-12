using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixRotation : MonoBehaviour {

  Quaternion rotation;

  private void Awake() {
    rotation = transform.rotation;
  }

  private void LateUpdate() {
    transform.localPosition = new Vector3(0, 0, 0);
    transform.rotation = rotation;
  }
}
