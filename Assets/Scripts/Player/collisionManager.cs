using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionManager : MonoBehaviour {
    // Start is called before the first frame update
    playerStatus ps;
    ArrowKeyMovement am;
    ArrowKeyMovementDummy amd;
    Inventory inventory;
    Rigidbody rb;

    public AudioClip collectClip;

    void Start() {
        ps = GetComponent<playerStatus>();
        am = GetComponent<ArrowKeyMovement>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("cuboid"))
        {
            Destroy(collision.gameObject);
            inventory.addRupee(am.playerIndex);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(collectClip, 0.5f);
        }
        if (collision.gameObject.CompareTag("airwall"))
        {
            if (ps.currStatus == playerStatus.status.DASH)
            {
                am.stopDash = true;
            }
            if (ps.currStatus == playerStatus.status.FLYING)
            {
                ReflectProjectile(collision.contacts[0].normal);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            playerStatus player_ps = player.GetComponent<playerStatus>();
            ArrowKeyMovement player_am = player.GetComponent<ArrowKeyMovement>();

            // I am a bullet
            if (ps.currStatus == playerStatus.status.FLYING)
            {
                // same team
                if (player_ps.teamIdx == ps.teamIdx)
                {
                    ReflectProjectile(collision.contacts[0].normal);
                }
                else
                {
                    // different team
                    if (player_ps.currStatus == playerStatus.status.FLYING || player_ps.currStatus == playerStatus.status.DEFENSE)
                    {
                        // hit another bullet
                        //player_ps.GetComponent<Rigidbody>().isKinematic = true;
                        ReflectProjectile(collision.contacts[0].normal);
                        //player_ps.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    else
                    {
                        am.hitEnemy = true;
                        if (player_am == null)
                        {
                            player.GetComponent<ArrowKeyMovementDummy>().hurt(am.flyingDir);
                        }
                        else
                        {
                            if (Inventory.instance.numOfPlayerResource[player_am.playerIndex]>=3
                                || ((player_ps.currStatus==playerStatus.status.HELD || player_ps.currStatus==playerStatus.status.HOLDING) 
                                && Inventory.instance.numOfPlayerResource[player_am.teamMember.GetComponent<ArrowKeyMovement>().playerIndex]>=3)) {
                                Camera.main.GetComponent<TimeManager>().DoSlowMotion(collision.contacts[0].point);
                                Camera.main.GetComponent<CameraShake>().ShakeCameraOnHurt(0.5f, 0.15f);
                                //ScreenShakeManager.Bump(0.15f);
                            } else {
                                Camera.main.GetComponent<CameraShake>().ShakeCamera(0.5f, 0.5f);
                                //ScreenShakeManager.Bump(0.5f);
                            }
                            if (player.layer == 11) return;
                            player.layer = 11;
                            player_am.hurt(collision);
                            player_am.Vibration(0.5f);
                            am.Vibration(0.5f);
                        }
                    }
                }
            }
            else if (ps.currStatus == playerStatus.status.DASH)
            {
                // Dash will knock back enemy
                if (player_ps.teamIdx != ps.teamIdx && player.GetComponent<ArrowKeyMovement>() != null)
                {
                    if (player_ps.currStatus == playerStatus.status.DEFENSE
                        || player_ps.currStatus == playerStatus.status.FLYING
                        || player_ps.currStatus == playerStatus.status.DASH)
                    {
                        // do nothing
                        am.stopDash = true;
                    }
                    else
                    {
                        if (player.layer == 11) return;
                        player.layer = 11;
                        am.stopDash = true;
                        player_am.Vibration(0.2f);
                        am.Vibration(0.2f);
                        StartCoroutine(player.GetComponent<ArrowKeyMovement>().knockBack(collision, player_ps.currStatus, 1.5f));
                    }
                }
            }
            else if (player_ps.teamIdx == ps.teamIdx && player_ps.currStatus == playerStatus.status.DEFENSE && ps.currStatus == playerStatus.status.NORMAL)
            {
                Debug.Log("Holding");
                ps.currStatus = playerStatus.status.HOLDING;
                player_am.defenseToHeld();
            }
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            playerStatus player_ps = player.GetComponent<playerStatus>();
            ArrowKeyMovement player_am = player.GetComponent<ArrowKeyMovement>();
            if (player_ps.teamIdx == ps.teamIdx && player_ps.currStatus == playerStatus.status.DEFENSE && ps.currStatus == playerStatus.status.NORMAL)
            {
                ps.currStatus = playerStatus.status.HOLDING;
                player_am.defenseToHeld();
            }
        }
        if (collision.gameObject.CompareTag("airwall"))
        {
            if (ps.currStatus == playerStatus.status.DASH)
            {
                am.stopDash = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // get cuboid
        if (other.gameObject.CompareTag("cuboid"))
        {
            Destroy(other.gameObject);
            inventory.addRupee(am.playerIndex);
        }
    }

    private void ReflectProjectile(Vector3 reflectVector) {
        am.flyingDir = Vector3.Reflect(am.flyingDir, reflectVector);
        //am.flyingSpeed *= 0.75f;
    }

}
