using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

//RequireComponent(typeof(ArrowKeyMovement))]
public class StoreResource : MonoBehaviour {

  public GameObject slider;

  // 0 for red and 1 for blue
  public int effective_radius;

  ArrowKeyMovement arr;

  public bool isStoring = false;

  private float timer = 0.0f;

  public float storeTime = 2.0f;
  public AudioClip storingClip;
  private AudioSource SoundPlayer2D;
  private float blinkTime = 0.0f;

  float lowerboundForLeftTrigger = 0.5f;
  // Start is called before the first frame update
  void Start() {
    arr = GetComponent<ArrowKeyMovement>();
    SoundPlayer2D = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnTriggerStay(Collider other) {
    int playerIndex = arr.playerIndex;
    int teamIndex = GetComponent<playerStatus>().teamIdx;

    Gamepad gp = Gamepad.all[playerIndex];

    float x = transform.position.x;
    float z = transform.position.z;
    //if (gp.leftTrigger.ReadValue() != 0) {
    //  print(gp.leftTrigger.ReadValue());

    //}

    if (gp.leftTrigger.ReadValue() > lowerboundForLeftTrigger
            && ((teamIndex == 0 && other.gameObject.CompareTag("outground_red") && StorageController.instance.GetRedStatus())
                || (teamIndex == 1 && other.gameObject.CompareTag("outground_blue") && StorageController.instance.GetBlueStatus()))
            && (x * x + z * z >= effective_radius * effective_radius)
            && Inventory.instance.numOfPlayerResource[playerIndex] > 0) {
      if (!isStoring) {
        slider.SetActive(true);
        GetComponent<Animator>().SetBool("moving", false);
        SoundPlayer2D.clip = storingClip;
        SoundPlayer2D.Play();
      }

      timer += Time.deltaTime;
      if (timer >= storeTime) {
        // Store Resource here
        slider.GetComponent<Slider>().value = 0;
        Inventory.instance.numOfPlayerResource[playerIndex] -= 1;
        if (teamIndex == 0) {
          Inventory.instance.numOfRedTeamResource += 1;
          Inventory.instance.panelRed.TextMotionStyle = PanelGenerator.MotionStyle.Blink;
          Inventory.instance.panelRed.RefreshTime = 0.3f;
        }
        else {
          Inventory.instance.numOfBlueTeamResource += 1;
          Inventory.instance.panelBlue.TextMotionStyle = PanelGenerator.MotionStyle.Blink;
          Inventory.instance.panelBlue.RefreshTime = 0.3f;
        }
        Inventory.instance.stopBlinkPanel(teamIndex);
        timer = 0.0f;
        SoundPlayer2D.clip = storingClip;
        SoundPlayer2D.Play();
      }
      else {
        slider.GetComponent<Slider>().value += Time.deltaTime / storeTime;
      }
      isStoring = true;
    }
    else {
      SoundPlayer2D.Stop();
      timer = 0.0f;
      slider.GetComponent<Slider>().value = 0.0f;
      slider.SetActive(false);
      isStoring = false;
    }

  }
 
}
