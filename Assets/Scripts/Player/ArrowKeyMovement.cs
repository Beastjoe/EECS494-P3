using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;

public class ArrowKeyMovement : MonoBehaviour {

  public int playerIndex;
  public float movingSpeed = 1.0f;
  public float rotatingSpeed = 3.0f;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    // get GamePad
    Gamepad gp = Gamepad.all[playerIndex];
    float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < 0.11? 0: gp.leftStick.x.ReadValue();
    float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue())<0.11? 0: gp.leftStick.y.ReadValue();

    float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < 0.11 ? 0 : gp.rightStick.x.ReadValue();
    float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < 0.11 ? 0 : gp.rightStick.y.ReadValue();
    Vector3 NextDir = new Vector3(right_horizontal_val, 0, right_vertical_val);
    if (NextDir != Vector3.zero) {
      Vector3 currDir = transform.rotation.eulerAngles;
      NextDir = Quaternion.LookRotation(NextDir).eulerAngles;
      Vector3  currentAngle = new Vector3(
             Mathf.LerpAngle(currDir.x, NextDir.x, rotatingSpeed * Time.deltaTime),
             Mathf.LerpAngle(currDir.y, NextDir.y, rotatingSpeed * Time.deltaTime),
             Mathf.LerpAngle(currDir.z, NextDir.z, rotatingSpeed * Time.deltaTime));
      transform.eulerAngles = currentAngle;
    }

    Debug.Log(Vector3.forward * vertical_val + Vector3.right * horizontal_val);
    if (horizontal_val != 0 && vertical_val != 0) {
      transform.position += movingSpeed *
        (Vector3.forward * vertical_val + Vector3.right * horizontal_val) * Time.deltaTime;
    }
  }
}
