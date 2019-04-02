using System.Collections;
using System.Collections.Generic;
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

    public GameObject RedSprite;
    public GameObject PurpleSprite;
    public GameObject GreenSprite;
    public GameObject BlueSprite;

    public GameObject Select;
    public GameObject Continue;
    public GameObject count;
    public GameObject BlackShader;
    public GameObject TransitionShader;

    Text countText;
    int cnt;

    public GameObject Timer;
    Text timerText;
    float timer;

    private bool Player0Confirmed;
    private bool Player1Confirmed;
    private bool Player2Confirmed;
    private bool Player3Confirmed;

    private bool isFading = false;

    private Vector3[] velocity;
    private float smoothTime = 0.1f;

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

        velocity = new Vector3[4];
        RedSprite.transform.localPosition += new Vector3(0, 30, 0);
        PurpleSprite.transform.localPosition += new Vector3(0, 30, 0);
        GreenSprite.transform.localPosition += new Vector3(0, 30, 0);
        BlueSprite.transform.localPosition += new Vector3(0, 30, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerIndexAssignment.instance.robotSelected[0])
        {
            redRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            redRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
            RedRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            RedIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Move(RedSprite, 0);
        }

        if (PlayerIndexAssignment.instance.robotSelected[1])
        {
            purpleRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            purpleRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
            PurpleRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            PurpleIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Move(PurpleSprite, 1);
        }

        if (PlayerIndexAssignment.instance.robotSelected[2])
        {
            greenRenderer.material.SetColor("_Color", new Vector4(0f, 0.749f, 0.749f, 1f));
            greenRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.749f, 0.749f, 1f) * 1.5f);
            GreenRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            GreenIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Move(GreenSprite, 2);
        }


        if (PlayerIndexAssignment.instance.robotSelected[3])
        {
            blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.749f, 0.749f, 1f));
            blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.749f, 0.749f, 1f) * 1.5f);
            BlueRobot.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.white);
            BlueIndicator.GetComponent<SpriteRenderer>().material.EnableKeyword("_EMISSION");
            Move(BlueSprite, 3);
        }

        bool selectionFinished = PlayerIndexAssignment.instance.robotSelected[0] &&
                PlayerIndexAssignment.instance.robotSelected[1] &&
                PlayerIndexAssignment.instance.robotSelected[2] &&
                PlayerIndexAssignment.instance.robotSelected[3];

        if (selectionFinished)
        {
            Select.SetActive(false);
            Continue.SetActive(true);
            Timer.SetActive(true);

            if (Gamepad.all[0].aButton.isPressed && !Player0Confirmed)
            {
                cnt++;
                Player0Confirmed = true;
            }

            if (Gamepad.all[1].aButton.isPressed && !Player1Confirmed)
            {
                cnt++;
                Player1Confirmed = true;
            }

            if (Gamepad.all[2].aButton.isPressed && !Player2Confirmed)
            {
                cnt++;
                Player2Confirmed = true;
            }

            if (Gamepad.all[3].aButton.isPressed && !Player3Confirmed)
            {
                cnt++;
                Player3Confirmed = true;
            }

            countText.text = cnt.ToString() + "/4";

            if (cnt == 4 && !isFading)
            {
                StartCoroutine(Fading());
            }

            if (GameControl.instance.isPaused)
                return;

            if(!isFading)
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

    IEnumerator Fading() {
        TransitionShader.SetActive(true);
        BlackShader.SetActive(true);
        isFading = true;
        for (int i = 560; i > 8; i -= 10)
        {
            if (i < 280)
            {
                Color c = BlackShader.GetComponent<Image>().color;
                c.a += 0.045f;
                BlackShader.GetComponent<Image>().color = c;
            }

            TransitionShader.gameObject.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.001f);
        }
        SceneManager.LoadScene("playLab");

    }

    void Move(GameObject obj, int index)
    {
        Vector3 targetPosition = new Vector3(obj.transform.localPosition.x, 0f, 0f);

        obj.transform.localPosition = Vector3.SmoothDamp(obj.transform.localPosition, targetPosition, ref velocity[index], smoothTime);
        if (Vector3.Magnitude(obj.transform.localPosition - targetPosition) < 0.0001f)
        {
            obj.transform.localPosition = targetPosition;
        }
    }
}
