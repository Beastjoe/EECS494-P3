using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;
using UnityEngine.UI;

public class ArrowKeyMovementSelection : MonoBehaviour
{
    public int initialIndex;
    [HideInInspector]
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
    public AudioClip confirmationClip;

    Animator anim;
    playerStatus ps;
    Rigidbody rb;
    bool Ready = true;
    bool defenseReady = true;
    bool dashReday = true;
    bool rightTriggerReady = true;
    bool startButtonReady = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ps = GetComponent<playerStatus>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerIndexAssignment.instance.robotSelected[initialIndex])
            return;

        playerIndex = PlayerIndexAssignment.instance.indices[initialIndex];

        if (GameControl.instance.isPaused || GameControl.instance.tutorialPaused)
        {
            if (anim.GetBool("moving"))
            {
                anim.SetBool("moving", false);
            }
            return;
        }

        if (Gamepad.all[playerIndex].aButton.isPressed && ps.currStatus == playerStatus.status.DEFENSE)
        {
            GetComponent<AudioSource>().PlayOneShot(confirmationClip);
            ps.currStatus = playerStatus.status.NORMAL;
            anim.SetTrigger("IdelTrigger");
        }

        // get GamePad
        float threshold = 0.21f;
        Gamepad gp = Gamepad.all[playerIndex];
        if (gp.startButton.isPressed && startButtonReady && GameControl.instance.pauseReady)
        {
            startButtonReady = false;
            StartCoroutine(startButtonCoolDown(0.5f));
            GameControl.instance.isPaused = true;
            return;
        }

        // normal status
        if (ps.currStatus == playerStatus.status.NORMAL || ps.currStatus == playerStatus.status.HOLDING)
        {
            bool isIdle = true;
            float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
            float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < threshold ? 0 : gp.leftStick.y.ReadValue();

            float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();
            float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < threshold ? 0 : gp.rightStick.y.ReadValue();
            if (right_horizontal_val == 0 && right_vertical_val == 0)
            {
                NextDir = new Vector3(horizontal_val, 0, vertical_val);
            }
            else
            {
                NextDir = new Vector3(right_horizontal_val, 0, right_vertical_val);
            }
            if (NextDir != Vector3.zero)
            {
                isIdle = false;
                Vector3 currDir = transform.rotation.eulerAngles;
                NextDir = Quaternion.LookRotation(NextDir).eulerAngles;
                Vector3 currentAngle = new Vector3(
                       Mathf.LerpAngle(currDir.x, NextDir.x, rotatingSpeed * Time.deltaTime),
                       Mathf.LerpAngle(currDir.y, NextDir.y, rotatingSpeed * Time.deltaTime),
                       Mathf.LerpAngle(currDir.z, NextDir.z, rotatingSpeed * Time.deltaTime));
                transform.eulerAngles = currentAngle;
            }

            if (horizontal_val != 0 || vertical_val != 0)
            {
                isIdle = false;
                rb.velocity = movingSpeed *
                  ((Vector3.forward * vertical_val + Vector3.right * horizontal_val)).normalized;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            if (!isIdle)
            {
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
            }
        }
    }


    IEnumerator startButtonCoolDown(float t)
    {
        yield return new WaitForSeconds(t);
        startButtonReady = true;
    }


    IEnumerator setBackLayer(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.layer = 12;
    }

}
