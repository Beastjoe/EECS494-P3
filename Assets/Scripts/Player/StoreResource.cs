using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

[RequireComponent(typeof(ArrowKeyMovement))]
public class StoreResource : MonoBehaviour {

  public GameObject slider;

  // 0 for red and 1 for blue
  public int effective_radius;

  ArrowKeyMovement arr;

  private bool isStoring = false;

  private float timer = 0.0f;

  public float storeTime = 2.0f;

  float lowerboundForLeftTrigger = 0.5f;
  // Start is called before the first frame update
  void Start() {
    arr = GetComponent<ArrowKeyMovement>();
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
    if (gp.leftTrigger.ReadValue() != 0) {
      print(gp.leftTrigger.ReadValue());

    }

    if (gp.leftTrigger.ReadValue() > lowerboundForLeftTrigger
            && ((teamIndex == 0 && other.gameObject.CompareTag("outground_red"))
                || (teamIndex == 1 && other.gameObject.CompareTag("outground_blue")))
            && (x * x + z * z >= effective_radius * effective_radius)
            && Inventory.instance.numOfPlayerResource[playerIndex] > 0) {
      if (!isStoring) {
        slider.SetActive(true);
      }

      timer += Time.deltaTime;
      if (timer >= storeTime) {
        slider.GetComponent<Slider>().value = 0;
        Inventory.instance.numOfPlayerResource[playerIndex] -= 1;
        if (teamIndex == 0) {
          Inventory.instance.numOfRedTeamResource += 1;
        }
        else {
          Inventory.instance.numOfBlueTeamResource += 1;
        }
        timer = 0.0f;
      }
      else {
        slider.GetComponent<Slider>().value += Time.deltaTime / storeTime;
      }
      isStoring = true;
    }
    else {
      timer = 0.0f;
      slider.GetComponent<Slider>().value = 0.0f;
      slider.SetActive(false);
      isStoring = false;
    }

  }
}
