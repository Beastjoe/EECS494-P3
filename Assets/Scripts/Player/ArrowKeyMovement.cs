﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;

public class ArrowKeyMovement : MonoBehaviour {

  public int playerIndex;
  public float movingSpeed = 1.0f;
  public float rotatingSpeed = 3.0f;
  public float dashSpeed = 10.0f;
  public bool defenseMode = false;
  public bool onOuterGround = false;
  public GameObject teamMember;
  public GameObject rupee;
  public float pickUpDistance = 5.0f;
  public float flyingSpeed = 20.0f;
  public float maximumFlyingSpeed = 20.0f;
  public float knockBackSpeed = 10.0f;
  public float stunnedTime = 2.5f;
  public Vector3 flyingDir;

  [HideInInspector]
  public bool hitEnemy = false;

  public float total_radius = 7.5f;

  public GameObject stunnedEffect;

  Animator anim;
  playerStatus ps;
  Rigidbody rb;
  bool Ready = true;
  bool defenseReady = true;
  bool dashReday = true;
  bool rightTriggerReady = true;

  // Start is called before the first frame update
  void Start() {
    anim = GetComponent<Animator>();
    ps = GetComponent<playerStatus>();
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update() {
    if (ps.currStatus == playerStatus.status.STUNNED) {
      return;
    }
    if (GetComponent<StoreResource>().isStoring) {
      return;
    }

    // get GamePad
    Gamepad gp = Gamepad.all[playerIndex];

    if (gp.leftShoulder.isPressed && defenseReady) {
      defenseReady = false;
      StartCoroutine(defenseCoolDown(0.5f)); 
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
      Vector3 NextDir;
      if (right_horizontal_val == 0 && right_vertical_val == 0) {
        NextDir = new Vector3(horizontal_val, 0, vertical_val);
      } else {
        NextDir = new Vector3(right_horizontal_val, 0, right_vertical_val);
      }
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
        if (ps.currStatus == playerStatus.status.HOLDING && transform.position.x * transform.position.x + transform.position.z * transform.position.z >= 55f) {
          Vector3 p1 = transform.position - movingSpeed * (Vector3.forward * vertical_val + Vector3.right * horizontal_val) * Time.deltaTime;
          Vector3 p2 = transform.position + movingSpeed * (Vector3.forward * vertical_val + Vector3.right * horizontal_val) * Time.deltaTime;
          transform.position = p1.magnitude > p2.magnitude ? p2 : p1;
        }
        else {
          transform.position += movingSpeed *
            (Vector3.forward * vertical_val + Vector3.right * horizontal_val) * Time.deltaTime;
        }
      }

      if (!isIdle) {
        anim.SetBool("moving", true);
      }
      else {
        anim.SetBool("moving", false);
      }
      
      if (ps.currStatus == playerStatus.status.NORMAL) {
        if(gp.rightShoulder.isPressed && dashReday) {
          dashReday = false;
          StartCoroutine(dashCoolDown(2.0f));
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
          gameObject.layer = 11;
          teamMember.GetComponent<ArrowKeyMovement>().fly();
          StartCoroutine(setBackLayer(0.5f));
        }
      }
    }

    if (ps.currStatus == playerStatus.status.HELD) {
      transform.Find("positionIndicator").gameObject.SetActive(false);
      transform.Find("directionIndicator").gameObject.SetActive(true);
      transform.position = teamMember.transform.position + new Vector3(0, 0.8f, 0);
      transform.Find("directionIndicator").gameObject.transform.localPosition = new Vector3(0, -0.7f, 2);
      float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < 0.1 ? 0 : gp.leftStick.x.ReadValue();
      float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < 0.1 ? 0 : gp.leftStick.y.ReadValue();
      float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < 0.1 ? 0 : gp.rightStick.x.ReadValue();
      float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < 0.1 ? 0 : gp.rightStick.y.ReadValue();
      Vector3 NextDir;
      if (right_horizontal_val == 0 && right_vertical_val == 0) {
        NextDir = new Vector3(horizontal_val, 0, vertical_val);
      }
      else {
        NextDir = new Vector3(right_horizontal_val, 0, right_vertical_val);
      }
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
    flyingSpeed = maximumFlyingSpeed;
    Vector3 movingDir = (teamMember.transform.rotation * Vector3.forward).normalized;
    transform.Find("directionIndicator").gameObject.SetActive(false);
    transform.position -= new Vector3(0, 0.7f, 0);
    GetComponent<BoxCollider>().size = new Vector3(1.5f, 1.0f, 1.5f);

    //for (float t = 0.0f; t <= 0.2f; t += Time.deltaTime) {
    //  transform.position += 5 * movingDir * Time.deltaTime;
    //  yield return new WaitForSeconds(Time.deltaTime);
    //}

    flyingDir = (transform.rotation * Vector3.forward).normalized;
    
    for (float t=0.0f;t<=1.5f;t+=Time.deltaTime) {
      if (hitEnemy) {
        break;
      }
      transform.position += flyingSpeed * flyingDir * Time.deltaTime;
      yield return new WaitForSeconds(Time.deltaTime);
    }

    hitEnemy = false;
    ps.currStatus = playerStatus.status.NORMAL;
    anim.SetTrigger("flyingEndTrigger");
    transform.Find("directionIndicator").gameObject.SetActive(false);
    transform.Find("directionIndicator").gameObject.transform.position = new Vector3(0, 0.1f, 2);
    transform.Find("positionIndicator").gameObject.SetActive(true);
    GetComponent<BoxCollider>().size = new Vector3(1f, 1.0f, 1f);
  }

  public void hurt(Vector3 dir) {
    anim.SetTrigger("hurtTrigger");
    gameObject.layer = 11;
    ps.currStatus = playerStatus.status.FLYING;
    dropMoneyAfterhurt();
    StartCoroutine(knockBack(dir));
  }

  private void dropMoneyAfterhurt() {
    ArrowKeyMovement arr = GetComponent<ArrowKeyMovement>();
    int pre_num_of_res = Inventory.instance.numOfPlayerResource[arr.playerIndex];
    Inventory.instance.numOfPlayerResource[arr.playerIndex] /= 2;
    int num_of_money_dropped = pre_num_of_res - Inventory.instance.numOfPlayerResource[arr.playerIndex];
    for(int i = 0; i < num_of_money_dropped; ++i) {
      GameObject coin = Instantiate(rupee, transform.position, Quaternion.identity);
      coin.transform.GetComponentInChildren<Rigidbody>().velocity = new Vector3(Random.Range(0.0f, 3.0f), 10, Random.Range(0.0f, 3.0f)); ;
    }

  }

  private bool judgeWhetherOutOfBound(Vector3 pos) {
    if (pos.x * pos.x + pos.z * pos.z <= total_radius * total_radius)
      return true;
    return false;
  }

  IEnumerator knockBack(Vector3 dir) {
    GameObject stunningEffectObject = Instantiate(stunnedEffect);
    stunningEffectObject.transform.position = transform.position;
    stunningEffectObject.transform.parent = transform;
    stunningEffectObject.transform.localPosition += new Vector3(0, 1.2f, 0);
    for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime) {
      transform.position += knockBackSpeed * dir * Time.deltaTime;
      yield return new WaitForSeconds(Time.deltaTime);
    }

    StartCoroutine(getStunned(stunnedTime, stunningEffectObject));
  }

  IEnumerator defenseCoolDown(float t) {
    yield return new WaitForSeconds(t);
    defenseReady = true;
  }

  IEnumerator dashCoolDown(float cd) {
    ps.currStatus = playerStatus.status.STUNNED;
    gameObject.layer = 11;
    Vector3 dashDir = (transform.rotation * Vector3.forward).normalized;
    for (float t=0.0f;t<0.075f;t+=Time.deltaTime) {
      float speed = Mathf.Lerp(dashSpeed, movingSpeed, t / 0.075f);
      if (transform.position.x * transform.position.x + transform.position.z * transform.position.z >= 55f) {
        Vector3 p1 = transform.position - dashDir * dashSpeed * Time.deltaTime;
        Vector3 p2 = transform.position + dashDir * dashSpeed * Time.deltaTime;
        transform.position = p1.magnitude > p2.magnitude ? p2 : p1;
      } else {
        transform.position += dashDir * dashSpeed * Time.deltaTime;
      }
      yield return new WaitForSeconds(Time.deltaTime);
    }
    gameObject.layer = 12;
    ps.currStatus = playerStatus.status.NORMAL;
    yield return new WaitForSeconds(cd);
    dashReday = true;
  }

  IEnumerator rightTriggerCoolDown(float t) {
    yield return new WaitForSeconds(t);
    rightTriggerReady = true;
  }

  IEnumerator getStunned(float t, GameObject stunningEffectObject) {
    ps.currStatus = playerStatus.status.STUNNED;
    yield return new WaitForSeconds(t);
    ps.currStatus = playerStatus.status.NORMAL;
    gameObject.layer = 12;
    Destroy(stunningEffectObject);
  }

  IEnumerator setBackLayer(float t) {
    yield return new WaitForSeconds(t);
    gameObject.layer = 12;
  }

  void FixedUpdate() {
    if (onOuterGround && ps.currStatus!=playerStatus.status.FLYING) {
      transform.RotateAround(Vector3.zero, Vector3.up, -31.5f * Time.deltaTime);
    }
  }

}
