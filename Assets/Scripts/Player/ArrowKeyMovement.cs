﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;
//using XInputDotNetPure;
using UnityEngine.UI;

public class ArrowKeyMovement : MonoBehaviour {
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
    public GameObject hitYellow;
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
    public AudioClip errorResource;

    [HideInInspector]
    public bool hitEnemy = false;
    public bool stopDash = false;
    //public bool inHurtPhase = false;

    public float total_radius = 7.5f;
    public float maximumChargingTime = 2f;

    public GameObject stunnedEffect;
    GameObject barrier;
    GameObject positionIndicator;

    private float defenseInitialConsumption = 0.35f;
    private float defenseContinuousConsumption = 0.1f; // per second
    private float staminaRechargeSpeed = 0.2f; // per second
    private float dashConsumption = 0.6f;
    public float flyingTime = 1.5f;
    private playerStatus.status prevStatus;

    Animator anim;
    playerStatus ps;
    Rigidbody rb;
    bool Ready = true;
    public bool defenseReady = true;
    bool rightTriggerReady = true;
    bool startButtonReady = true;
    // chargingAmount is from 0 
    public float chargingAmount = 0.0f;

    float threshold = 0.21f;
    public Gamepad gp;


    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        ps = GetComponent<playerStatus>();
        rb = GetComponent<Rigidbody>();
        barrier = transform.Find("Barrier").gameObject;
        positionIndicator = transform.Find("positionIndicator").gameObject;
    }

    // Update is called once per frame
    void Update() {

        if (ps.currStatus != playerStatus.status.HELD && ps.currStatus != playerStatus.status.STUNNED)
        {
            rb.useGravity = true;
        } else if (ps.currStatus == playerStatus.status.HELD)
        {
            transform.position = teamMember.transform.position + new Vector3(0, 0.75f, 0);
 
        }

        playerIndex = PlayerIndexAssignment.instance.indices[initialIndex];

        if (ps.currStatus == playerStatus.status.STUNNED || ps.currStatus == playerStatus.status.DASH)
        {
            if (ps.currStatus == playerStatus.status.STUNNED && prevStatus == playerStatus.status.HELD)
            {
                transform.position = teamMember.transform.position + new Vector3(0, 0.75f, 0);
            }
            return;
        }
        if (GetComponent<StoreResource>().isStoring)
        {
            return;
        }
        if (GameControl.instance.isPaused || GameControl.instance.tutorialPaused || !GameControl.instance.isStarted)
        {
            chargingAmount = 0.0f;
            if (anim.GetBool("moving"))
            {
                anim.SetBool("moving", false);
            }

            bool isCountingDown = GameControl.instance.isCountingDown;
            if (isCountingDown)
            {
                gp = Gamepad.all[playerIndex];

                bool isIdle = true;
                float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
                float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < threshold ? 0 : gp.leftStick.y.ReadValue();

                float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();
                float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < threshold ? 0 : gp.rightStick.y.ReadValue();

                // change direction
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
            }

            return;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Defend 0") && ps.currStatus == playerStatus.status.NORMAL)
        {
            anim.SetTrigger("IdelTrigger");
        }

        // Refill Stamina Bar if not full
        if (staminaBar.fillAmount < 1.0f)
        {
            Color c = staminaBar.color;
            c.a = 1.0f;
            staminaBar.color = c;
            if (ps.currStatus != playerStatus.status.DEFENSE)
            {
                staminaBar.fillAmount += staminaRechargeSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (staminaBar.color.a > 0)
            {
                Color c = staminaBar.color;
                c.a -= 3.33f * Time.deltaTime;
                staminaBar.color = c;
            }
        }

        // get GamePad
        gp = Gamepad.all[playerIndex];
        if (gp.startButton.isPressed && startButtonReady && GameControl.instance.pauseReady)
        {
            startButtonReady = false;
            StartCoroutine(startButtonCoolDown(0.5f));
            GameControl.instance.isPaused = true;
            return;
        }

        // defense status
        if (ps.currStatus == playerStatus.status.DEFENSE)
        {
            barrier.transform.localScale += new Vector3(4 * Time.deltaTime, 4 * Time.deltaTime, 4 * Time.deltaTime);
            positionIndicator.transform.localScale -= new Vector3(0.2f * Time.deltaTime, 0.2f * Time.deltaTime, 0);
            if (barrier.transform.localScale.x>2)
            {
                barrier.transform.localScale = new Vector3(2, 2, 2);
            }
            if (positionIndicator.transform.localScale.y < 0)
            {
                positionIndicator.transform.localScale = new Vector3(0, 0, 1);
            }
            rb.isKinematic = true;
            if (GetComponent<StoreResource>().onStorageArea)
            {
                defenseToNormal();
                
            }
            else
            {
                float fa = staminaBar.fillAmount - Time.deltaTime * defenseInitialConsumption;
                if (fa <= 0.0f)
                {
                    defenseToNormal();
                }
                else
                {
                    staminaBar.fillAmount = fa;
                }
            }
        }
        else
        {
            rb.isKinematic = false;
            barrier.transform.localScale -= new Vector3(8 * Time.deltaTime, 8 * Time.deltaTime, 8 * Time.deltaTime);
            positionIndicator.transform.localScale += new Vector3(0.2f * Time.deltaTime, 0.2f * Time.deltaTime, 0);
            if (barrier.transform.localScale.x < 0)
            {
                barrier.transform.localScale = new Vector3(0, 0, 0);
            }
            if (positionIndicator.transform.localScale.y > 0.1)
            {
                positionIndicator.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            }
        }

        // normal status
        if (ps.currStatus == playerStatus.status.NORMAL || ps.currStatus == playerStatus.status.HOLDING)
        {
            if (inTutorialMode && GameControl.instance.tutorialProgres > -2)
            {
                return;
            }

            bool isIdle = true;
            float horizontal_val = Mathf.Abs(gp.leftStick.x.ReadValue()) < threshold ? 0 : gp.leftStick.x.ReadValue();
            float vertical_val = Mathf.Abs(gp.leftStick.y.ReadValue()) < threshold ? 0 : gp.leftStick.y.ReadValue();

            float right_horizontal_val = Mathf.Abs(gp.rightStick.x.ReadValue()) < threshold ? 0 : gp.rightStick.x.ReadValue();
            float right_vertical_val = Mathf.Abs(gp.rightStick.y.ReadValue()) < threshold ? 0 : gp.rightStick.y.ReadValue();
            // move

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


            // change direction
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

            if (!isIdle)
            {
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
            }

            if (inTutorialMode && GameControl.instance.tutorialProgres > -3)
            {
                return;
            }
            if (ps.currStatus == playerStatus.status.NORMAL)
            {
                if (gp.rightTrigger.isPressed && rightTriggerReady)
                {
                    rightTriggerReady = false;
                    if (staminaBar.fillAmount < dashConsumption)
                    {
                        //TODO:CALL WARNING OF STAMINBAR
                        if (defenseReady)
                        {
                            defenseReady = false;
                            StartCoroutine(defenseCoolDown(1.0f));
                            StartCoroutine(staminBarWarning());
                        }
                        StartCoroutine(rightTriggerCoolDown(0.3f));
                    }
                    else
                    {
                        StartCoroutine(dashCoolDown(2.0f));
                    }
                }
            }

            if (ps.currStatus == playerStatus.status.HOLDING)
            {
                if (gp.rightTrigger.isPressed && chargingAmount < 1.0f)
                {
                    chargingAmount += Time.deltaTime / maximumChargingTime;
                    teamMember.GetComponent<ArrowKeyMovement>().anim.SetBool("charging", true);
                    if (chargingAmount > 0.5f)
                    {
                        gp.SetMotorSpeeds(1.0f, 1.0f);
                    }
                    else if (chargingAmount > 0.25f)
                    {
                        gp.SetMotorSpeeds(chargingAmount, chargingAmount);
                    }
                } else if (chargingAmount != 0)
                {
                    // finish charging, throw
                    GetComponent<ArrowKeyMovement>().gp.SetMotorSpeeds(0, 0);
                    teamMember.GetComponent<ArrowKeyMovement>().anim.SetBool("charging", false);
                    teamMember.GetComponent<ArrowKeyMovement>().flyingTime = 1.5f * chargingAmount + 0.5f;
                    teamMember.GetComponent<ArrowKeyMovement>().maximumFlyingSpeed = 15f + 20f * chargingAmount;
                    chargingAmount = 0.0f;

                    rightTriggerReady = false;
                    StartCoroutine(rightTriggerCoolDown(1.0f));
                    ps.currStatus = playerStatus.status.NORMAL;
                    teamMember.GetComponent<playerStatus>().currStatus = playerStatus.status.FLYING;
                    teamMember.GetComponent<Animator>().SetBool("moving", false);
                    teamMember.GetComponent<Animator>().SetTrigger("shootTrigger");
                    anim.SetTrigger("throwTrigger");

                    //StartCoroutine(tmp());
                    gameObject.layer = 11;
                    teamMember.GetComponent<ArrowKeyMovement>().fly();
                    StartCoroutine(setBackLayer(0.5f));
                }
            }
        }

        if (inTutorialMode && GameControl.instance.tutorialProgres > -6)
        {
            return;
        }
        if (gp.leftTrigger.isPressed && !GetComponent<StoreResource>().onStorageArea)
        {
            if (ps.currStatus == playerStatus.status.NORMAL)
            {
                if (staminaBar.fillAmount < defenseInitialConsumption)
                {
                    // TODO: do something
                    if (defenseReady)
                    {
                        defenseReady = false;
                        StartCoroutine(defenseCoolDown(1.0f));
                        StartCoroutine(staminBarWarning());
                    }
                }
                else
                {
                    normalToDefense();
                }
            }
            else if (ps.currStatus == playerStatus.status.DEFENSE)
            {
                /*
                ps.currStatus = playerStatus.status.NORMAL;
                anim.SetTrigger("IdelTrigger");
                */
            }
        } else if (ps.currStatus == playerStatus.status.DEFENSE && !GetComponent<StoreResource>().onStorageArea && !gp.leftTrigger.isPressed)
        {
            defenseToNormal();
        }

        if (ps.currStatus == playerStatus.status.HELD)
        {
            gameObject.layer = 11;
            teamMember.GetComponent<playerStatus>().currStatus = playerStatus.status.HOLDING; 
            transform.Find("positionIndicator").gameObject.SetActive(false);
            transform.Find("directionIndicator").gameObject.SetActive(true);
            transform.Find("directionIndicator").gameObject.transform.localPosition = new Vector3(0, -0.7f, 2);
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
        gameObject.layer = 12;
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
        rb.velocity = Vector3.zero;
        for (float t = 0.0f; t <= flyingTime; t += Time.deltaTime)
        {
            if (!GameControl.instance.isStarted)
            {
                rb.velocity = Vector3.zero;
                break;
            }
            if (hitEnemy)
            {
                break;
            }
            flyingSpeed = Mathf.Lerp(maximumFlyingSpeed, 7.0f, t/flyingTime);
            rb.velocity = flyingSpeed * flyingDir + rb.velocity.y * Vector3.up;
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


    public void hurt(Collision collision) {
        chargingAmount = 0;
        anim.SetTrigger("hurtTrigger");
        anim.SetBool("moving", false);
        Camera.main.GetComponent<AudioSource>().PlayOneShot(stunningClip, 2.0f);
        //StartCoroutine(controllerVibration(1.0f));

        if (ps.currStatus == playerStatus.status.HOLDING || ps.currStatus == playerStatus.status.HELD)
        {
            if (teamMember.layer!=11)
            {
                teamMember.layer = 11;
                teamMember.GetComponent<ArrowKeyMovement>().hurt(collision);
            }
        }
        prevStatus = ps.currStatus;
        ps.currStatus = playerStatus.status.FLYING;
        dropMoneyAfterhurt();
        StartCoroutine(knockBack(collision, prevStatus));
    }

    private void dropMoneyAfterhurt() {
        ArrowKeyMovement arr = GetComponent<ArrowKeyMovement>();
        int pre_num_of_res = Inventory.instance.numOfPlayerResource[arr.playerIndex];
        Inventory.instance.numOfPlayerResource[arr.playerIndex] = 0;
        int num_of_money_dropped = pre_num_of_res - Inventory.instance.numOfPlayerResource[arr.playerIndex];
        for (int i = 0; i < num_of_money_dropped; ++i)
        {
            GameObject coin = Instantiate(rupee, transform.position, Quaternion.identity);
            coin.transform.GetChild(0).GetComponent<RupeeInitialization>().enabled = false;
            coin.transform.GetComponentInChildren<Rigidbody>().velocity = new Vector3(Random.Range(-5.0f, 5.0f), 15, Random.Range(-5.0f, 5.0f)); ;
        }

    }

    private bool judgeWhetherOutOfBound(Vector3 pos) {
        if (pos.x * pos.x + pos.z * pos.z <= total_radius * total_radius)
            return true;
        return false;
    }
    
    public IEnumerator knockBack(Collision collision, playerStatus.status prevStatus, float stunTime = 2.5f) {

        if (ps.currStatus == playerStatus.status.HOLDING || ps.currStatus == playerStatus.status.HELD)
        {
            if (teamMember.layer != 11)
            {
                teamMember.layer = 11;
                StartCoroutine(teamMember.GetComponent<ArrowKeyMovement>().knockBack(collision, teamMember.GetComponent<playerStatus>().currStatus, 1.5f));
            }
        }

        Instantiate(hitYellow, collision.contacts[0].point, Quaternion.identity);
        this.prevStatus = prevStatus; 
        Vector3 dir = -collision.contacts[0].normal;
        ps.currStatus = playerStatus.status.STUNNED;
        GameObject stunningEffectObject = Instantiate(stunnedEffect);
        stunningEffectObject.transform.position = transform.position;
        stunningEffectObject.transform.parent = transform;
        stunningEffectObject.transform.localPosition += new Vector3(0, 1.2f, 0);
        if (!(prevStatus == playerStatus.status.HOLDING || prevStatus == playerStatus.status.HELD))
        {
            for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime)
            {
                if (!GameControl.instance.isStarted)
                {
                    rb.velocity = Vector3.zero;
                    yield break;
                }
                float speed = knockBackSpeed - dashSpeedCurve.Evaluate(t / 0.5f) * (knockBackSpeed - 5);
                rb.velocity = speed * dir;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        StartCoroutine(getStunned(stunTime, stunningEffectObject, prevStatus));
    }

    IEnumerator defenseCoolDown(float t) {
        yield return new WaitForSeconds(t);
        defenseReady = true;
    }

    IEnumerator dashCoolDown(float cd) {
        // Consume energy for dash
        staminaBar.fillAmount -= dashConsumption;
        if (!staminaBar.isActiveAndEnabled)
            staminaBar.gameObject.SetActive(true);

        anim.SetTrigger("dashTrigger");
        ps.currStatus = playerStatus.status.DASH;

        Vector3 dashDir = (transform.rotation * Vector3.forward).normalized;

        // Dash along one direction; Speed gradually decreased
        stopDash = false;
        GameObject trail = Instantiate(dashTrail, transform.position, transform.rotation);
        trail.transform.Rotate(0, 90, 0);
        trail.transform.parent = transform;
        for (float t = 0.0f; t < 0.15f; t += Time.deltaTime)
        {
            if (stopDash)
            {
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
        yield return new WaitForSeconds(t);
        if (prevStatus == playerStatus.status.DEFENSE)
        {
            ps.currStatus = playerStatus.status.NORMAL;
        }
        else if (prevStatus == playerStatus.status.DASH)
        {
            ps.currStatus = playerStatus.status.NORMAL;
        }
        else
        {
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
        Camera.main.GetComponent<AudioSource>().PlayOneShot(errorResource, 1.5f);

        for (int i = 0; i < 2; ++i)
        {
            staminaBar.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            staminaBar.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FixedUpdate() {
        if (GameControl.instance.isPaused)
        {
            return;
        }
        if (onOuterGround && ps.currStatus != playerStatus.status.FLYING && GameControl.instance.isStarted)
        {
            if (anim.GetBool("moving"))
            {
                transform.RotateAround(Vector3.zero, Vector3.up, -25f * Time.deltaTime);
            }
            else
            {
                transform.RotateAround(Vector3.zero, Vector3.up, -31.5f * Time.deltaTime);
            }
        }
        if (!GameControl.instance.tutorialState)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y , - 1.0f, 0.3f), transform.position.z);
        }
    }

    public void Vibration(float t, float amount = 1.0f) {
        StartCoroutine(controllerVibration(t, amount));
    }

    IEnumerator controllerVibration(float t, float amount) {
        gp.SetMotorSpeeds(amount, amount);
        yield return new WaitForSeconds(t);
        gp.SetMotorSpeeds(0.0f, 0.0f);
    }

    private void defenseToNormal() {
        //barrier.SetActive(false);
        //positionIndicator.SetActive(true);
        ps.currStatus = playerStatus.status.NORMAL;
        anim.SetTrigger("IdelTrigger");
    }

    private void normalToDefense() {
        //barrier.SetActive(true);
        barrier.transform.localScale = Vector3.zero;
        //positionIndicator.SetActive(false);

        Camera.main.GetComponent<AudioSource>().PlayOneShot(readyClip, 0.35f);
        ps.currStatus = playerStatus.status.DEFENSE;
        anim.SetTrigger("defenseTrigger");
        // consume stamina
        staminaBar.gameObject.SetActive(true);
        staminaBar.fillAmount -= defenseInitialConsumption;
    }

    public void defenseToHeld() {
        //barrier.SetActive(false);

        ps.currStatus = playerStatus.status.HELD;
        rb.useGravity = false;
        anim.SetBool("moving", false);
        anim.SetTrigger("IdelTrigger");
        transform.position = teamMember.transform.position + new Vector3(0, 0.75f, 0);
    }

    IEnumerator tmp() {
        yield return new WaitForSeconds(0.25f);
        gameObject.layer = 11;
        teamMember.GetComponent<ArrowKeyMovement>().fly();
        StartCoroutine(setBackLayer(0.5f));
    }

}
