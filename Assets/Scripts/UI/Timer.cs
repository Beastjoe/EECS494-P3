using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Input;

public class Timer : MonoBehaviour {
    public GameObject blackPanel;
    public GameObject instruction;
    public GameObject restartTimer;
    public GameObject AutoPlayAgainText;
    public Camera redTeamCamera1, redTeamCamera2;
    public Camera blueTeamCamera1, blueTeamCamera2;
    bool gameEndAnimation = false;
    public AnimationCurve cutsceneCameraCurve;
    public AudioClip winningClip;
    public GameObject winningRainbow;

    float timer;
    Text text;
    Text winText;
    bool triggered = false;
    bool isCountingDown = false;
    void Start() {
        text = GetComponent<Text>();
        winText = transform.GetChild(0).GetComponent<Text>();
        winText.text = "";
        winText.gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
        timer = float.Parse(text.text);
    }

    void Update() {
        if (GameControl.instance.isPaused)
        {
            return;
        }
        timer -= 0.66f * Time.deltaTime;
        if (timer >= 0.0f)
        {
            text.text = Mathf.Ceil(timer).ToString("F0");
        }
        else
        {
            text.text = "00";
            GameControl.instance.isStarted = false;

            StartCoroutine(gameEnd());

            if (gameEndAnimation)
            {
                blackPanel.SetActive(true);
                instruction.SetActive(true);
                int redScore = Inventory.instance.numOfRedTeamResource;
                int blueScore = Inventory.instance.numOfBlueTeamResource;
                if (redScore > blueScore)
                {
                    winText.text = "Red Team Wins!";
                    if (!triggered)
                    {
                        winText.GetComponent<Animator>().SetTrigger("gameOver");
                        triggered = true;
                    }
                    blackPanel.GetComponent<Image>().color = new Vector4(1, 0.6f, 0.77f, 0.36f);
                }
                else if (redScore == blueScore)
                {
                    if (!triggered)
                    {
                        winText.GetComponent<Animator>().SetTrigger("gameOver");
                        triggered = true;
                    }
                    winText.text = "Draw";
                }
                else
                {
                    winText.text = "Blue Team Wins!";
                    if (!triggered)
                    {
                        winText.GetComponent<Animator>().SetTrigger("gameOver");
                        triggered = true;
                    }
                    blackPanel.GetComponent<Image>().color = new Vector4(0.33f, 0.68f, 1, 0.36f);
                }

                if (!isCountingDown)
                {
                    isCountingDown = true;
                    StartCoroutine(startCountDown());
                }

                if (Gamepad.all[0].aButton.isPressed || Gamepad.all[1].aButton.isPressed ||
                Gamepad.all[2].aButton.isPressed || Gamepad.all[3].aButton.isPressed)
                {
                    SceneManager.LoadScene("playLab");
                }

                if (Gamepad.all[0].bButton.isPressed || Gamepad.all[1].bButton.isPressed ||
                Gamepad.all[2].bButton.isPressed || Gamepad.all[3].bButton.isPressed)
                {
                    if (GameObject.Find("PlayerIndexAssignment") != null)
                    {
                        Destroy(GameObject.Find("PlayerIndexAssignment"));
                    }
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    IEnumerator startCountDown() {
        yield return new WaitForSeconds(0.5f);
        Text timerText = restartTimer.GetComponent<Text>();
        float timer = float.Parse(timerText.text);
        int originalFontSize = timerText.fontSize;
        restartTimer.SetActive(true);
        AutoPlayAgainText.SetActive(true);
        while (timer >= 0.0f)
        {
            timer -= 1.5f * Time.deltaTime;
            timerText.fontSize += (int)(80 * Time.deltaTime);
            if (Mathf.Ceil(timer) != float.Parse(timerText.text))
            {
                if (Mathf.Ceil(timer) == 0.0)
                {
                    SceneManager.LoadScene("playLab");
                }
                timerText.text = Mathf.Ceil(timer).ToString();
                timerText.fontSize = originalFontSize;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator gameEnd() {
        Vector3 originalPos = Camera.main.transform.position;
        int redScore = Inventory.instance.numOfRedTeamResource;
        int blueScore = Inventory.instance.numOfBlueTeamResource;
        redTeamCamera1.transform.parent.Find("Canvas").gameObject.SetActive(false);
        redTeamCamera2.transform.parent.Find("Canvas").gameObject.SetActive(false);
        blueTeamCamera1.transform.parent.Find("Canvas").gameObject.SetActive(false);
        blueTeamCamera2.transform.parent.Find("Canvas").gameObject.SetActive(false);

        if (redScore == blueScore)
        {
            gameEndAnimation = true;
            yield break;
        }
        bool redTeamWin = redScore > blueScore;
        if (redTeamWin)
        {
            Camera.main.gameObject.SetActive(false);
            
            redTeamCamera1.transform.position = originalPos;
            redTeamCamera2.transform.position = originalPos;
            disableStunned(redTeamCamera1.transform.parent.gameObject);
            disableStunned(redTeamCamera2.transform.parent.gameObject);
            redTeamCamera1.gameObject.SetActive(true);
            redTeamCamera2.gameObject.SetActive(true);
            redTeamCamera1.gameObject.GetComponent<AudioSource>().PlayOneShot(winningClip, 1.0f);

            bool createWinningRainbow = false;
            for (float t = 0.0f; t<=2f; t+=Time.deltaTime)
            {

                Vector3 targetPos1 = redTeamCamera1.transform.parent.transform.position + getOffset(redTeamCamera1);
                Vector3 targetPos2 = redTeamCamera2.transform.parent.transform.position + getOffset(redTeamCamera2);

                cutsceneCameraCurve.Evaluate(t / 2f);
                redTeamCamera1.transform.position = Vector3.Lerp(originalPos, targetPos1, cutsceneCameraCurve.Evaluate(t / 2f));
                redTeamCamera2.transform.position = Vector3.Lerp(originalPos, targetPos2, cutsceneCameraCurve.Evaluate(t / 2f));


                if(t >= 1f && !createWinningRainbow)
                {
                    createWinningRainbow = true;
                    GameObject rainbow1 = Instantiate(winningRainbow, redTeamCamera1.transform.parent.gameObject.transform.position + new Vector3(0, 3f, 1), Quaternion.identity);
                    GameObject rainbow2 = Instantiate(winningRainbow, redTeamCamera2.transform.parent.gameObject.transform.position + new Vector3(0, 3f, 1), Quaternion.identity);
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }

            redTeamCamera1.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("winTrigger1");
            redTeamCamera2.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("winTrigger2");

            blueTeamCamera1.gameObject.transform.parent.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(3.0f);
        }
        else
        {
            Camera.main.gameObject.SetActive(false);
            blueTeamCamera1.transform.position = originalPos;
            blueTeamCamera2.transform.position = originalPos;
            disableStunned(blueTeamCamera1.transform.parent.gameObject);
            disableStunned(blueTeamCamera2.transform.parent.gameObject);
            blueTeamCamera1.gameObject.SetActive(true);
            blueTeamCamera2.gameObject.SetActive(true);
            blueTeamCamera1.gameObject.GetComponent<AudioSource>().PlayOneShot(winningClip, 1.0f);

            bool createWinningRainbow = false;
            for (float t = 0.0f; t <= 2f; t += Time.deltaTime)
            {

                if (t >= 1f && !createWinningRainbow)
                {
                    createWinningRainbow = true;
                    GameObject rainbow1 = Instantiate(winningRainbow, blueTeamCamera1.transform.parent.gameObject.transform.position + new Vector3(0, 3f, 1), Quaternion.identity);
                    GameObject rainbow2 = Instantiate(winningRainbow, blueTeamCamera2.transform.parent.gameObject.transform.position + new Vector3(0, 3f, 1), Quaternion.identity);
                }

                Vector3 targetPos1 = blueTeamCamera1.transform.parent.transform.position + getOffset(blueTeamCamera1);
                Vector3 targetPos2 = blueTeamCamera2.transform.parent.transform.position + getOffset(blueTeamCamera2);

                cutsceneCameraCurve.Evaluate(t / 2f);
                blueTeamCamera1.transform.position = Vector3.Lerp(originalPos, targetPos1, cutsceneCameraCurve.Evaluate(t / 2f));
                blueTeamCamera2.transform.position = Vector3.Lerp(originalPos, targetPos2, cutsceneCameraCurve.Evaluate(t / 2f));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            blueTeamCamera1.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("winTrigger1");
            blueTeamCamera2.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("winTrigger2");

            redTeamCamera2.gameObject.transform.parent.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(3.0f);
        }

        gameEndAnimation = true;
    }


    private Vector3 getOffset(Camera obj) {
        return 1.3f * obj.gameObject.transform.parent.transform.up + 2.8f * obj.gameObject.transform.parent.transform.forward;
    }

    private void disableStunned(GameObject gameObject) {
        foreach (Transform tr in gameObject.transform)
        {
            if (tr.gameObject.CompareTag("FX"))
            {
                tr.gameObject.SetActive(false);
            }
        }
    }
}
