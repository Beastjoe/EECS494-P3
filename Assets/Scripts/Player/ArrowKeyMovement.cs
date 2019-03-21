using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;
using XInputDotNetPure;
using UnityEngine.UI;

public class ArrowKeyMovement : MonoBehaviour {

  public int playerIndex;
  public float movingSpeed = 1.0f;
  public float rotatingSpeed = 3.0f;
  public float dashSpeed = 10.0f;
  public bool defenseMode = false;
  public bool onOuterGround = false;
  public GameObject teamMember;
  public GameObject rupee;
  public AnimationCurve dashSpeedCurve;
  public Image staminaBar;
  public GameObject dashTrail;
  public GameObject flyingFX;
  public float pickUpDistance = 5.0f;
  public float flyingSpeed = 20.0f;
  public float maximumFlyingSpeed = 20.0f;
  public bool inTutorialMode;
  public float knockBackSpeed = 10.0f;
  public float stunnedTime = 2.5f;
  public Vector3 flyingDir;
  public Vector3 NextDir;

  public AudioClip readyClip;
  public AudioClip stunningClip;

  [HideInInspector]
  public bool hitEnemy = false;
  public bool stopDash = false;

  public float total_radius = 7.5f;

  public GameObject stunnedEffect;

  public float defenseInitialConsumption = 0.2f;
  public float defenseContinuousConsumption = 0.1f; // per second
  public float staminaRechargeSpeed = 0.2f; // per second
  public float dashConsumption = 0.3f;

  Animator anim;
  playerStatus ps;
  Rigidbody rb;
  bool Ready = true;
  bool defenseReady = true;
  bool rightTriggerReady = true;
  bool startButtonReady = true;

  // Start is called before the first frame update
  void Start() {
    anim = GetComponent<Animator>();
    ps = GetComponent<playerStatus>();
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update() {
    if (ps.currStatus == playerStatus.status.STUNNED || ps.currStatus == playerStatus.status.DASH) {
      return;
    }
    if (GetComponent<StoreResource>().isStoring) {
      return;
    }
    if (GameControl.instance.isPaused || GameControl.instance.tutorialPaused) {
      if (anim.GetBool("moving")) {
        anim.SetBool("moving", false);
      }
      return;
    }

    // Refill Stamina Bar if not full
    if (staminaBar.fillAmount < 1.0f) {
      Color c = staminaBar.color;
      c.a = 1.0f;
      staminaBar.color = c;
      if (ps.currStatus != playerStatus.status.DEFENSE) {
        staminaBar.fillAmount += staminaRechargeSpeed * Time.deltaTime;
      }
    } else {
      if (staminaBar.color.a > 0) {
        Color c = staminaBar.color;
        c.a -= 3.33f * Time.deltaTime;
        staminaBar.color = c;
      }
    }

    // get GamePad
    float threshold = 0.21f;
    Gamepad gp = Gamepad.all[playerIndex];

    if (gp.startButton.isPressed && startButtonReady && GameControl.instance.pauseReady) {
      startButtonReady = false;
      StartCoroutine(startButtonCoolDown(0.5f));
      GameControl.instance.isPaused = true;
      return;
    }

    // defense status
    if (ps.currStatus == playerStatus.status.DEFENSE) {
      if (GetComponent<StoreResource>().onStorageArea) {
        ps.currStatus = playerStatus.status.NORMAL;
        anim.SetTrigger("IdelTrigger");
      } else {
        float fa = staminaBar.fillAmount - Time.deltaTime * defenseInitialConsumption;
        if (fa <= 0.0f) {
          ps.currStatus = playerStatus.status.NORMAL;
          anim.SetTrigger("IdelTrigger");
        } else {
          staminaBar.fillAmount = fa;
        }
      } 
    }

    // normal status
    if (ps.currStatus == playerStatus.status.NORMAL || ps.currStatus == playerStatus.status.HOLDING) {
      if (inTutorialMode && GameControl.instance.tutorialProgres>-2) {
        return;
      }
      bool isIdle = true;
      float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
      float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < threshold ? 0 : gp.leftStick.y.ReadValue();

      float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();
      float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < threshold ? 0 : gp.rightStick.y.ReadValue();
      
      // move
      if (horizontal_val != 0 || vertical_val != 0) {
        isIdle = false;
        rb.velocity = movingSpeed *
          ((Vector3.forward * vertical_val + Vector3.right * horizontal_val)).normalized;
      }
      else {
        rb.velocity = Vector3.zero;
      }

      // change direction
      if (right_horizontal_val == 0 && right_vertical_val == 0) {
        NextDir = new Vector3(horizontal_val, 0, vertical_val);
      }
      else {
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

      if (!isIdle) {
        anim.SetBool("moving", true);
      }
      else {
        anim.SetBool("moving", false);
      }

      if (inTutorialMode && GameControl.instance.tutorialProgres > -3) {
        return;
      }
      if (ps.currStatus == playerStatus.status.NORMAL) {
        if (gp.rightTrigger.isPressed && rightTriggerReady) {
          rightTriggerReady = false;
          if (staminaBar.fillAmount < dashConsumption) {
            //TODO:CALL WARNING OF STAMINBAR
            StartCoroutine(staminBarWarning());
            StartCoroutine(rightTriggerCoolDown(0.3f));
          }
          else {
            StartCoroutine(dashCoolDown(2.0f));
          }
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

    if (inTutorialMode && GameControl.instance.tutorialProgres > -6) {
      return;
    }
    if (gp.leftTrigger.isPressed && defenseReady && !GetComponent<StoreResource>().onStorageArea) {
      defenseReady = false;
      StartCoroutine(defenseCoolDown(0.5f));
      if (ps.currStatus == playerStatus.status.NORMAL) {
        if (staminaBar.fillAmount < defenseInitialConsumption) {
          // TODO: do something
          StartCoroutine(staminBarWarning());
        } else {
          Camera.main.GetComponent<AudioSource>().PlayOneShot(readyClip, 2.0f);
          ps.currStatus = playerStatus.status.DEFENSE;
          anim.SetTrigger("defenseTrigger");
          // consume stamina
          staminaBar.gameObject.SetActive(true);
          staminaBar.fillAmount -= defenseInitialConsumption;
        }
      }
      else if (ps.currStatus == playerStatus.status.DEFENSE) {
        ps.currStatus = playerStatus.status.NORMAL;
        anim.SetTrigger("IdelTrigger");
      }
    }

    if (ps.currStatus == playerStatus.status.HELD) {
      transform.Find("positionIndicator").gameObject.SetActive(false);
      transform.Find("directionIndicator").gameObject.SetActive(true);
      transform.position = teamMember.transform.position + new Vector3(0, 1.0f, 0);
      transform.Find("directionIndicator").gameObject.transform.localPosition = new Vector3(0, -0.9f, 2);
      float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
      float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < threshold ? 0 : gp.leftStick.y.ReadValue();
      float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();
      float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < threshold ? 0 : gp.rightStick.y.ReadValue();
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
    GameObject auroa = Instantiate(flyingFX, transform.position, Quaternion.identity);
    auroa.transform.parent = transform;
    transform.Find("directionIndicator").gameObject.SetActive(false);
    transform.position -= new Vector3(0, 0.7f, 0);
    GetComponent<BoxCollider>().size = new Vector3(1.5f, 1.5f, 1.5f);

    //for (float t = 0.0f; t <= 0.2f; t += Time.deltaTime) {
    //  transform.position += 5 * movingDir * Time.deltaTime;
    //  yield return new WaitForSeconds(Time.deltaTime);
    //}

    flyingDir = (transform.rotation * Vector3.forward).normalized;

    for (float t = 0.0f; t <= 1.5f; t += Time.deltaTime) {
      if (hitEnemy) {
        break;
      }
      rb.velocity = flyingSpeed * flyingDir;
      transform.Rotate(0, 30, 0);
      yield return new WaitForSeconds(Time.deltaTime);
    }

    hitEnemy = false;
    ps.currStatus = playerStatus.status.NORMAL;
    anim.SetTrigger("flyingEndTrigger");
    transform.Find("directionIndicator").gameObject.SetActive(false);
    transform.Find("directionIndicator").gameObject.transform.position = new Vector3(0, 0.1f, 2);
    transform.Find("positionIndicator").gameObject.SetActive(true);
    GetComponent<BoxCollider>().size = new Vector3(1f, 1.0f, 1f);
    Destroy(auroa);
  }

  public void hurt(Vector3 dir) {
    anim.SetTrigger("hurtTrigger");
    Camera.main.GetComponent<AudioSource>().PlayOneShot(stunningClip, 10.0f);
    gameObject.layer = 11;
    playerStatus.status prevStatus = ps.currStatus;
    ps.currStatus = playerStatus.status.FLYING;
    dropMoneyAfterhurt();
    StartCoroutine(knockBack(dir, prevStatus));
  }

  private void dropMoneyAfterhurt() {
    ArrowKeyMovement arr = GetComponent<ArrowKeyMovement>();
    int pre_num_of_res = Inventory.instance.numOfPlayerResource[arr.playerIndex];
    Inventory.instance.numOfPlayerResource[arr.playerIndex] /= 2;
    int num_of_money_dropped = pre_num_of_res - Inventory.instance.numOfPlayerResource[arr.playerIndex];
    for (int i = 0; i < num_of_money_dropped; ++i) {
      GameObject coin = Instantiate(rupee, transform.position, Quaternion.identity);
      coin.transform.GetComponentInChildren<Rigidbody>().velocity = new Vector3(Random.Range(0.0f, 3.0f), 10, Random.Range(0.0f, 3.0f)); ;
    }

  }

  private bool judgeWhetherOutOfBound(Vector3 pos) {
    if (pos.x * pos.x + pos.z * pos.z <= total_radius * total_radius)
      return true;
    return false;
  }

  public IEnumerator knockBack(Vector3 dir, playerStatus.status prevStatus) {
    GameObject stunningEffectObject = Instantiate(stunnedEffect);
    stunningEffectObject.transform.position = transform.position;
    stunningEffectObject.transform.parent = transform;
    stunningEffectObject.transform.localPosition += new Vector3(0, 1.2f, 0);
    if (!(prevStatus == playerStatus.status.HOLDING || prevStatus == playerStatus.status.HELD)) {
      Debug.Log(playerIndex + " " + ps.currStatus.ToString() + " " + dir);
      for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime) {
        float speed = knockBackSpeed - dashSpeedCurve.Evaluate(t / 0.5f) * (knockBackSpeed - 5);
        rb.velocity = speed * dir;
        yield return new WaitForSeconds(Time.deltaTime);
      }
    }
    StartCoroutine(getStunned(stunnedTime, stunningEffectObject, prevStatus));
  }

  IEnumerator defenseCoolDown(float t) {
    yield return new WaitForSeconds(t);
    defenseReady = true;
  }

  IEnumerator dashCoolDown(float cd) {
    // Consume energy for dash
    staminaBar.fillAmount -= dashConsumption;
    if(!staminaBar.isActiveAndEnabled)
      staminaBar.gameObject.SetActive(true);

    anim.SetTrigger("dashTrigger");
    ps.currStatus = playerStatus.status.DASH;

    Vector3 dashDir = (transform.rotation * Vector3.forward).normalized;

    // Dash along one direction; Speed gradually decreased
    stopDash = false;
    GameObject trail = Instantiate(dashTrail, transform.position, transform.rotation);
    trail.transform.Rotate(0, 90, 0);
    trail.transform.parent = transform;
    for (float t = 0.0f; t < 0.15f; t += Time.deltaTime) {
      if (stopDash) {
        break;
      }
      float speed = dashSpeed - dashSpeedCurve.Evaluate(t / 0.15f) * (dashSpeed - 10);
      rb.velocity = dashDir * speed;
      yield return new WaitForSeconds(Time.deltaTime);
    }


    // cast backswing time
    yield return new WaitForSeconds(0.3f);
    ps.currStatus = playerStatus.status.NORMAL;

    // Set dash ready
    rightTriggerReady = true;
  }

  IEnumerator rightTriggerCoolDown(float t) {
    yield return new WaitForSeconds(t);
    rightTriggerReady = true;
  }

  IEnumerator startButtonCoolDown(float t) {
    yield return new WaitForSeconds(t);
    startButtonReady = true;
  }

  IEnumerator getStunned(float t, GameObject stunningEffectObject, playerStatus.status prevStatus) {
    ps.currStatus = playerStatus.status.STUNNED;
    yield return new WaitForSeconds(t);
    if (prevStatus == playerStatus.status.DEFENSE) {
      ps.currStatus = playerStatus.status.NORMAL;
    } else if (prevStatus == playerStatus.status.DASH) {
      ps.currStatus = playerStatus.status.NORMAL;
    } else {
      ps.currStatus = prevStatus;
    }
    gameObject.layer = 12;
    Destroy(stunningEffectObject);
  }

  IEnumerator setBackLayer(float t) {
    yield return new WaitForSeconds(t);
    gameObject.layer = 12;
  }

  IEnumerator staminBarWarning() {
    for(int i= 0; i < 2; ++i) {
      staminaBar.color = Color.red;
      yield return new WaitForSeconds(0.1f);
      staminaBar.color = Color.white;
      yield return new WaitForSeconds(0.1f);
    }
  }

  void FixedUpdate() {
    if (GameControl.instance.isPaused) {
      return;
    }
    if (onOuterGround && ps.currStatus != playerStatus.status.FLYING) {
      transform.RotateAround(Vector3.zero, Vector3.up, -31.5f * Time.deltaTime);
    }
  }

}
