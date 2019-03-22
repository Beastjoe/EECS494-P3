using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl instance;
    public bool isPaused = false;
    public bool tutorialPaused = false;
    public GameObject pausePanel;
    public GameObject BlackShader;
    public bool panelReady = false;
    public bool pauseReady = true;
    public bool selectState = false;
    public bool isStarted = false;
    public bool playState = false;
    public bool tutorialState = false;
    public int tutorialProgres;

    public GameObject startTimer;
    public GameObject go;

    void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        isPaused = true;
        StartCoroutine(startCountDown());
    }

    // Update is called once per frame
    void Update() {
        if (isPaused && isStarted)
        {
            pausePanel.SetActive(true);
            if (panelReady)
            {
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    Gamepad gp = Gamepad.all[i];
                    if (gp.startButton.isPressed)
                    {
                        isPaused = false;
                        panelReady = false;
                        pauseReady = false;
                        StartCoroutine(pauseReadyCoolDown());
                        pausePanel.SetActive(false);
                        break;
                    }
                    else if (gp.aButton.isPressed && playState)
                    {
                        StartCoroutine(Fading());
                    }
                    else if (gp.aButton.isPressed && tutorialState)
                    {
                        SceneManager.LoadScene("TutorialIndividualLab");
                    }
                    else if (gp.bButton.isPressed)
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                }
            }
            else
            {
                StartCoroutine(panelReadyCoolDown());
            }
        }
    }

    IEnumerator panelReadyCoolDown() {
        yield return new WaitForSeconds(0.5f);
        panelReady = true;
    }

    IEnumerator pauseReadyCoolDown() {
        yield return new WaitForSeconds(1.0f);
        pauseReady = true;
    }

    IEnumerator startCountDown() {
        
        if (SceneManager.GetActiveScene().name == "playLab")
        {
            Color c = BlackShader.GetComponent<Image>().color;
            for (int i = 0; i < 100; ++i)
            {
                c = BlackShader.GetComponent<Image>().color;
                c.a -= 0.01f;
                BlackShader.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.005f);
            }
            c.a = 0f;
            BlackShader.GetComponent<Image>().color = c;
        }


        if (startTimer != null)
        {
            startTimer.SetActive(true);
            Text timerText = startTimer.GetComponent<Text>();
            float timer = float.Parse(timerText.text);
            int originalFontSize = timerText.fontSize;
            while (timer >= 0.0f)
            {
                timer -= 1.5f * Time.deltaTime;
                timerText.fontSize += (int)(500 * Time.deltaTime);
                if (Mathf.Ceil(timer) != float.Parse(timerText.text))
                {
                    if (Mathf.Ceil(timer) == 0.0)
                    {
                        break;
                    }
                    timerText.text = Mathf.Ceil(timer).ToString();
                    timerText.fontSize = originalFontSize;
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
            startTimer.SetActive(false);
            go.SetActive(true);
            for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime)
            {
                Text goText = go.GetComponent<Text>();
                goText.fontSize += (int)(500 * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            go.SetActive(false);
        }
        isPaused = false;
        isStarted = true;
    }

    IEnumerator Fading() {
        for(int i = 0; i < 100; ++i)
        {
            Color c = BlackShader.GetComponent<Image>().color;
            c.a += 0.01f;
            BlackShader.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.005f);
        }
        SceneManager.LoadScene("playLab");
    }
}
