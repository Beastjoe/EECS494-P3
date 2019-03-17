using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;
using UnityEngine.UI;

public class ArrowKeyMovementDummy : MonoBehaviour {

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

  public AudioClip readyClip;
  public AudioClip stunningClip;

  [HideInInspector]
  public bool hitEnemy = false;

  public float total_radius = 7.5f;

  public GameObject stunnedEffect;

  Animator anim;
  playerStatus ps;
  Rigidbody rb;

  public int number_of_rupees = 4;

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
 
    
  public void hurt(Vector3 dir) {
    anim.SetTrigger("hurtTrigger");
    Camera.main.GetComponent<AudioSource>().PlayOneShot(stunningClip, 10.0f);
    gameObject.layer = 11;
    ps.currStatus = playerStatus.status.FLYING;
    dropMoneyAfterhurt();
    knockBack(dir);
  }

  void knockBack(Vector3 dir) {
    GameObject stunningEffectObject = Instantiate(stunnedEffect);
    stunningEffectObject.transform.position = transform.position;
    stunningEffectObject.transform.parent = transform;
    stunningEffectObject.transform.localPosition += new Vector3(0, 1.2f, 0);
    StartCoroutine(getStunned(stunnedTime, stunningEffectObject));
  }

  private void dropMoneyAfterhurt() {
    int pre_num_of_res = number_of_rupees;
    number_of_rupees /= 2;
    int num_of_money_dropped = pre_num_of_res - number_of_rupees;
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



  IEnumerator defenseCoolDown(float t) {
    yield return new WaitForSeconds(t);
    defenseReady = true;
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
    if (onOuterGround && ps.currStatus != playerStatus.status.FLYING) {
      transform.RotateAround(Vector3.zero, Vector3.up, -31.5f * Time.deltaTime);
    }
  }


  public void triggerForward() {
    StartCoroutine(forward());
  }

  IEnumerator forward() {
    Vector3 originalPos = transform.position;
    anim.SetBool("moving", true);
    for (float i=0;i<=1.0f;i+=Time.deltaTime) {
      transform.position = originalPos + new Vector3(0, 0, -Mathf.Lerp(0.0f, 3.0f, i));
      yield return new WaitForSeconds(Time.deltaTime);
    }
    anim.SetBool("moving", false);
    GameControl.instance.tutorialPaused = false;
  }
}
