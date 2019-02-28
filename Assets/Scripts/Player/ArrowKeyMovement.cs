using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;

public class ArrowKeyMovement : MonoBehaviour {

  public int playerIndex;
  public float movingSpeed = 1.0f;
  public float rotatingSpeed = 3.0f;
  public bool defenseMode = false;
  public GameObject teamMember;
  public float pickUpDistance = 5.0f;
  public float flyingSpeed = 20.0f;
  public Vector3 flyingDir;

  Animator anim;
  playerStatus ps;
  Rigidbody rb;
  bool Ready = true;
  bool defenseReady = true;
  bool pickUpReady = true;
  bool rightTriggerReady = true;

  // Start is called before the first frame update
  void Start() {
    anim = GetComponent<Animator>();
    ps = GetComponent<playerStatus>();
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update() {
    // get GamePad
    Gamepad gp = Gamepad.all[playerIndex];

    if (gp.leftShoulder.isPressed && defenseReady) {
      defenseReady = false;
      StartCoroutine(defenseCoolDown(0.5f)); 
      Debug.Log(defenseMode);
      if (ps.currStatus == playerStatus.status.NORMAL) {
        ps.currStatus = playerStatus.status.DEFENSE;
        anim.SetTrigger("defenseTrigger");
      } else if (ps.currStatus == playerStatus.status.DEFENSE) {
        ps.currStatus = playerStatus.status.NORMAL;
        anim.SetTrigger("IdelTrigger");
      }
    }

    // normal status
    if (ps.currStatus == playerStatus.status.NORMAL || ps.currStatus == playerStatus.status.HOLDING) {
      bool isIdle = true;
      float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < 0.1 ? 0 : gp.leftStick.x.ReadValue();
      float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < 0.1 ? 0 : gp.leftStick.y.ReadValue();

      float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < 0.1 ? 0 : gp.rightStick.x.ReadValue();
      float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < 0.1 ? 0 : gp.rightStick.y.ReadValue();
      Vector3 NextDir = new Vector3(right_horizontal_val, 0, right_vertical_val);
      if (NextDir != Vector3.zero) {
        isIdle = false;
        Vector3 currDir = transform.rotation.eulerAngles;
        NextDir = Quaternion.LookRotation(NextDir).eulerAngles;
        Vector3 currentAngle = new Vector3(
               Mathf.LerpAngle(currDir.x, NextDir.x, rotatingSpeed * Time.deltaTime),
               Mathf.LerpAngle(currDir.y, NextDir.y, rotatingSpeed * Time.deltaTime),
               Mathf.LerpAngle(currDir.z, NextDir.z, rotatingSpeed * Time.deltaTime));
        transform.eulerAngles = currentAngle;
      }

      if (horizontal_val != 0 || vertical_val != 0) {
        isIdle = false;
        transform.position += movingSpeed *
          (Vector3.forward * vertical_val + Vector3.right * horizontal_val) * Time.deltaTime;
      }

      if (!isIdle) {
        anim.SetBool("moving", true);
      }
      else {
        anim.SetBool("moving", false);
      }

      if (ps.currStatus == playerStatus.status.NORMAL) {
        if (teamMember.GetComponent<playerStatus>().currStatus == playerStatus.status.DEFENSE
          && (teamMember.transform.position - transform.position).sqrMagnitude <= pickUpDistance
          && gp.rightShoulder.isPressed
          && pickUpReady) {
          pickUpReady = false;
          StartCoroutine(pickUPCoolDown(0.5f));
          ps.currStatus = playerStatus.status.HOLDING;
          teamMember.GetComponent<playerStatus>().currStatus = playerStatus.status.HELD;
          teamMember.GetComponent<Animator>().SetBool("moving", false);
          teamMember.GetComponent<Animator>().SetTrigger("IdelTrigger");
          teamMember.transform.position = transform.position + new Vector3(0, 0.8f, 0);
        }
      }

      if (ps.currStatus == playerStatus.status.HOLDING) {
        if (gp.rightTrigger.isPressed
          && rightTriggerReady) {
          rightTriggerReady = false;
          StartCoroutine(rightTriggerCoolDown(0.5f));
          ps.currStatus = playerStatus.status.NORMAL;
          teamMember.GetComponent<playerStatus>().currStatus = playerStatus.status.FLYING;
          teamMember.GetComponent<Animator>().SetBool("moving", false);
          teamMember.GetComponent<Animator>().SetTrigger("shootTrigger");
          anim.SetTrigger("throwTrigger");
          teamMember.GetComponent<ArrowKeyMovement>().fly();
        }
      }
    }

    if (ps.currStatus == playerStatus.status.HELD) {
      transform.Find("positionIndicator").gameObject.SetActive(false);
      transform.Find("directionIndicator").gameObject.SetActive(true);
      transform.position = teamMember.transform.position + new Vector3(0, 0.8f, 0);
      transform.Find("directionIndicator").gameObject.transform.localPosition = new Vector3(0, -0.7f, 2);
      float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < 0.1 ? 0 : gp.rightStick.x.ReadValue();
      float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < 0.1 ? 0 : gp.rightStick.y.ReadValue();
      Vector3 NextDir = new Vector3(right_horizontal_val, 0, right_vertical_val);
      if (NextDir != Vector3.zero) {
        Vector3 currDir = transform.rotation.eulerAngles;
        NextDir = Quaternion.LookRotation(NextDir).eulerAngles;
        Vector3 currentAngle = new Vector3(
               Mathf.LerpAngle(currDir.x, NextDir.x, rotatingSpeed * Time.deltaTime),
               Mathf.LerpAngle(currDir.y, NextDir.y, rotatingSpeed * Time.deltaTime),
               Mathf.LerpAngle(currDir.z, NextDir.z, rotatingSpeed * Time.deltaTime));
        transform.eulerAngles = currentAngle;
      }
    }
  }

  public void fly() {
    StartCoroutine(flying());
  }

  IEnumerator flying() {
    Vector3 movingDir = (teamMember.transform.rotation * Vector3.forward).normalized;
    for (float t = 0.0f; t <= 0.2f; t += Time.deltaTime) {
      transform.position += 5 * movingDir * Time.deltaTime;
      yield return new WaitForSeconds(Time.deltaTime);
    }

    flyingDir = (transform.rotation * Vector3.forward).normalized;
   // rb.velocity = velocityDir * flyingSpeed;
    
    for (float t=0.0f;t<=1.5f;t+=Time.deltaTime) {
      transform.position += flyingSpeed * flyingDir * Time.deltaTime;
      yield return new WaitForSeconds(Time.deltaTime);
    }
    
    ps.currStatus = playerStatus.status.NORMAL;
    anim.SetTrigger("flyingEndTrigger");
  }

  IEnumerator defenseCoolDown(float t) {
    yield return new WaitForSeconds(t);
    defenseReady = true;
  }

  IEnumerator pickUPCoolDown(float t) {
    yield return new WaitForSeconds(t);
    pickUpReady = true;
  }

  IEnumerator rightTriggerCoolDown(float t) {
    yield return new WaitForSeconds(t);
    rightTriggerReady = true;
  }

}
