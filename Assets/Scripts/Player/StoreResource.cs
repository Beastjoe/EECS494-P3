using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

//RequireComponent(typeof(ArrowKeyMovement))]
public class StoreResource : MonoBehaviour {

    public GameObject slider;
    public GameObject flyMoney;

    // 0 for red and 1 for blue
    public int effective_radius;

    ArrowKeyMovement arr;

    public bool isStoring = false;
    public bool onStorageArea = false;
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
        // fix isStoring
        if (isStoring && !onStorageArea)
        {
            isStoring = false;
        }
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

        if (((teamIndex == 0 && other.gameObject.CompareTag("outground_red") && StorageController.instance.GetRedStatus())
                    || (teamIndex == 1 && other.gameObject.CompareTag("outground_blue") && StorageController.instance.GetBlueStatus()))
                && (x * x + z * z >= effective_radius * effective_radius))
        {
            onStorageArea = true;
        }
        else
        {
            onStorageArea = false;
        }

        if (gp.leftTrigger.ReadValue() > lowerboundForLeftTrigger
                && ((teamIndex == 0 && other.gameObject.CompareTag("outground_red") && StorageController.instance.GetRedStatus())
                    || (teamIndex == 1 && other.gameObject.CompareTag("outground_blue") && StorageController.instance.GetBlueStatus()))
                && (x * x + z * z >= effective_radius * effective_radius)
                && Inventory.instance.numOfPlayerResource[playerIndex] > 0 && GetComponent<playerStatus>().currStatus != playerStatus.status.STUNNED)
        {
            if (GetComponent<playerStatus>().currStatus == playerStatus.status.DEFENSE)
            {
                return;
            }
            if (!isStoring)
            {
                slider.SetActive(true);
                GetComponent<Animator>().SetBool("moving", false);
                SoundPlayer2D.clip = storingClip;
                SoundPlayer2D.Play();
            }

            timer += Time.deltaTime;
            if (timer >= storeTime)
            {
                // Store Resource here
                slider.GetComponent<Slider>().value = 0;

                // instantiate
                GameObject flyMoneyObject = Instantiate(flyMoney, transform.position, Quaternion.identity);
                flyMoneyObject.GetComponent<FlyMoney>().redTeamScore = teamIndex == 0;
                Inventory.instance.numOfPlayerResource[playerIndex] -= 1;

                timer = 0.0f;
                SoundPlayer2D.clip = storingClip;
                SoundPlayer2D.Play();
            }
            else
            {
                slider.GetComponent<Slider>().value += Time.deltaTime / storeTime;
                if (timer / storeTime > 0.75f)
                {
                    GetComponent<ArrowKeyMovement>().gp.SetMotorSpeeds(1.0f, 1.0f);
                }
                else if (timer / storeTime > 0.5f)
                {
                    GetComponent<ArrowKeyMovement>().gp.SetMotorSpeeds(timer/storeTime, timer/storeTime);
                }
            }
            isStoring = true;
        }
        else
        {
            SoundPlayer2D.Stop();
            timer = 0.0f;
            slider.GetComponent<Slider>().value = 0.0f;
            slider.SetActive(false);
            isStoring = false;
            GetComponent<ArrowKeyMovement>().gp.SetMotorSpeeds(0, 0);
        }

    }

    private void OnTriggerExit(Collider other) {
        int playerIndex = arr.playerIndex;
        int teamIndex = GetComponent<playerStatus>().teamIdx;
        float x = transform.position.x;
        float z = transform.position.z;
        if (((teamIndex == 0 && other.gameObject.CompareTag("outground_red") && StorageController.instance.GetRedStatus())
                   || (teamIndex == 1 && other.gameObject.CompareTag("outground_blue") && StorageController.instance.GetBlueStatus()))
               && (x * x + z * z >= effective_radius * effective_radius))
        {
            onStorageArea = false;
        }
    }
}
