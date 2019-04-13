using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMoney : MonoBehaviour {

    private Vector3 redFireWorkPos1 = new Vector3(-5.4f, 1.95f, 8.13f);
    private Vector3 redFireWorkPos2 = new Vector3(-10.63f, 0f, -1.38f);
    private Vector3 blueFireWorkPos1 = new Vector3(7.09f, 1.32f, 9.09f);
    private Vector3 blueFireWorkPos2 = new Vector3(11.96f, 0, 0.51f);
    
    GameObject redScoreBoard;
    GameObject blueScoreBoard;
    GameObject redLightBoard;
    GameObject blueLightBoard;
    GameObject blueBuilding;
    GameObject redBuilding;
    Rigidbody rb;

    public GameObject blueFireWork;
    public GameObject redFireWork;
    public GameObject punch;
    public AudioClip scoreClip;
    public bool redTeamScore;
    public float flyingSpeed = 10.0f;

    // Start is called before the first frame update
    void Start() {
        redScoreBoard = GameObject.Find("LightBoardRed/Background(Clone)");
        blueScoreBoard = GameObject.Find("LightBoardBlue/Background(Clone)");
        redLightBoard = GameObject.Find("LightBoardRed");
        blueLightBoard = GameObject.Find("LightBoardBlue");
        blueBuilding = GameObject.Find("Skyscapers/Skyscraper_01");
        redBuilding = GameObject.Find("Skyscapers/Skyscraper_08");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (redTeamScore)
        {
            Vector3 dir = (redScoreBoard.transform.position - transform.position).normalized;
            rb.velocity = dir * flyingSpeed;
        }
        else
        {
            Vector3 dir = (blueScoreBoard.transform.position - transform.position).normalized;
            //transform.position += dir * flyingSpeed * Time.deltaTime;
            rb.velocity = dir * flyingSpeed;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("ScoreBoard"))
        {
            int teamIndex = redTeamScore ? 0 : 1;
            Debug.Log("Here: " + other.gameObject.transform.position);
            if (teamIndex == 0)
            {
                Inventory.instance.numOfRedTeamResource += 1;
                Inventory.instance.panelRed.TextMotionStyle = PanelGenerator.MotionStyle.Blink;
                Inventory.instance.panelRed.RefreshTime = 0.3f;
                if (!GameControl.instance.tutorialState)
                {
                    Instantiate(redFireWork, redFireWorkPos1, Quaternion.Euler(-90, 0, 0));
                    Instantiate(redFireWork, redFireWorkPos2, Quaternion.Euler(-90, 0, 0));
                    Instantiate(punch, new Vector3(-9.14f, 0.18f, 5.18f), Quaternion.Euler(-0.889f, -64.498f, 89.83401f));
                    Camera.main.GetComponent<AudioSource>().PlayOneShot(scoreClip, 1.0f);
                    redLightBoard.GetComponent<ShakeObject>().shake(1.0f);
                    redBuilding.GetComponent<ShakeObject>().shake(1.0f);
                }
            }
            else
            {
                Inventory.instance.numOfBlueTeamResource += 1;
                Inventory.instance.panelBlue.TextMotionStyle = PanelGenerator.MotionStyle.Blink;
                Inventory.instance.panelBlue.RefreshTime = 0.3f;
                if (!GameControl.instance.tutorialState)
                {
                    Instantiate(blueFireWork, blueFireWorkPos1, Quaternion.Euler(-90, 0, 0));
                    Instantiate(blueFireWork, blueFireWorkPos2, Quaternion.Euler(-90, 0, 0));
                    Instantiate(punch, new Vector3(10.16988f, -0.08f, 5.15183f), Quaternion.Euler(0f, 68.54f, 90f));
                    Camera.main.GetComponent<AudioSource>().PlayOneShot(scoreClip, 1);
                    blueLightBoard.GetComponent<ShakeObject>().shake(1.0f);
                    blueBuilding.GetComponent<ShakeObject>().shake(1.0f);
                }
            }
            Inventory.instance.stopBlinkPanel(teamIndex);

            Destroy(gameObject);
        }
    }

}
