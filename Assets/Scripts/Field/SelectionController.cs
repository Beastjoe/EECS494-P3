using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    private MeshRenderer blueRenderer;
    private MeshRenderer greenRenderer;
    private MeshRenderer redRenderer;
    private MeshRenderer purpleRenderer;

    public GameObject BlueStorage;
    public GameObject RedStorage;
    public GameObject GreenStorage;
    public GameObject PurpleStorage;

    public GameObject RedRobot;
    public GameObject PurpleRobot;
    public GameObject GreenRobot;
    public GameObject BlueRobot;

    public GameObject RedIndicator;
    public GameObject PurpleIndicator;
    public GameObject GreenIndicator;
    public GameObject BlueIndicator;

    public GameObject Select;
    public GameObject Continue;
    public GameObject count;
    Text countText;
    int cnt;

    public GameObject Timer;
    Text timerText;
    float timer;

    private bool Player0IsReady = false;
    private bool Player1IsReady = false;
    private bool Player2IsReady = false;
    private bool Player3IsReady = false;

    private bool Player0Confirmed;
    private bool Player1Confirmed;
    private bool Player2Confirmed;
    private bool Player3Confirmed;

    void Start()
    {
        blueRenderer = BlueStorage.GetComponent<MeshRenderer>();
        greenRenderer = GreenStorage.GetComponent<MeshRenderer>();
        redRenderer = RedStorage.GetComponent<MeshRenderer>();
        purpleRenderer = PurpleStorage.GetComponent<MeshRenderer>();
        blueRenderer.material.EnableKeyword("_EmissionColor");
        redRenderer.material.EnableKeyword("_EmissionColor");
        purpleRenderer.material.EnableKeyword("_EmissionColor");
        greenRenderer.material.EnableKeyword("_EmissionColor");
        
        redRenderer.material.SetColor("_Color", new Vector4(0.07f, 0f, 0.05f, 1f));
        redRenderer.material.SetColor("_EmissionColor", new Vector4(0.07f, 0f, 0.05f, 1f));
        
        purpleRenderer.material.SetColor("_Color", new Vector4(0.07f, 0f, 0.05f, 1f));
        purpleRenderer.material.SetColor("_EmissionColor", new Vector4(0.07f, 0f, 0.05f, 1f));
        
        blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.05f, 0.05f, 1f));
        blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.05f, 0.05f, 1f));
        
        greenRenderer.material.SetColor("_Color", new Vector4(0f, 0.05f, 0.05f, 1f));
        greenRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.05f, 0.05f, 1f));


        RedRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Vector4(0.5f, 0.5f, 0.5f, 1f));
        PurpleRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Vector4(0.5f, 0.5f, 0.5f, 1f));
        GreenRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Vector4(0.5f, 0.5f, 0.5f, 1f));
        BlueRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Vector4(0.5f, 0.5f, 0.5f, 1f));

        RedIndicator.GetComponent<SpriteRenderer>().material.DisableKeyword("_EMISSION");
        PurpleIndicator.GetComponent<SpriteRenderer>().material.DisableKeyword("_EMISSION");
        GreenIndicator.GetComponent<SpriteRenderer>().material.DisableKeyword("_EMISSION");
        BlueIndicator.GetComponent<SpriteRenderer>().material.DisableKeyword("_EMISSION");

        countText = count.GetComponent<Text>();
        timerText = Timer.GetComponent<Text>();
        timer = float.Parse(timerText.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[0].aButton.isPressed && !Player0IsReady)
        {
            redRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            redRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
            RedRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            RedIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Player0IsReady = true;
        }

        if (Gamepad.all[1].aButton.isPressed && !Player1IsReady)
        {
            purpleRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            purpleRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
            PurpleRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            PurpleIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Player1IsReady = true;
        }

        if (Gamepad.all[2].aButton.isPressed && !Player2IsReady)
        {
            greenRenderer.material.SetColor("_Color", new Vector4(0f, 0.749f, 0.749f, 1f));
            greenRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.749f, 0.749f, 1f) * 1.5f);
            GreenRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            GreenIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Player2IsReady = true;
        }
        
        
        if (Gamepad.all[3].aButton.isPressed && !Player3IsReady)
        {
            blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.749f, 0.749f, 1f));
            blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.749f, 0.749f, 1f) * 1.5f);
            BlueRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            BlueIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Player3IsReady = true;
        }

        bool selectionFinished = Player0IsReady && Player1IsReady && Player2IsReady && Player3IsReady;

        if (selectionFinished)
        {
            Select.SetActive(false);
            Continue.SetActive(true);

            if (Gamepad.all[0].xButton.isPressed && !Player0Confirmed)
            {
                cnt++;
                Player0Confirmed = true;
            }

            if (Gamepad.all[1].xButton.isPressed && !Player1Confirmed)
            {
                cnt++;
                Player1Confirmed = true;
            }

            if (Gamepad.all[2].xButton.isPressed && !Player2Confirmed)
            {
                cnt++;
                Player2Confirmed = true;
            }

            if (Gamepad.all[3].xButton.isPressed && !Player3Confirmed)
            {
                cnt++;
                Player3Confirmed = true;
            }

            countText.text = cnt.ToString() + "/4";

            if (cnt == 4)
                SceneManager.LoadScene("PlayLab");

            timer -= Time.deltaTime;
            if (Mathf.Ceil(timer) != float.Parse(timerText.text))
            {
                timerText.text = Mathf.Ceil(timer).ToString();
            }
            if (timer <= 0)
            {
                SceneManager.LoadScene("TutorialIndividualLab");
            }
        }

    }
    
    
    
}
