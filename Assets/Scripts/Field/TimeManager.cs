using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 3f;
    public AnimationCurve curve;
    bool inSlowMotion = false;
    Vector3 originalCameraPos;
    Vector3 originalCameraAngle;
    Vector3 cameraFocus;
    Vector3 cameraFocusAngle;
    public Text slowMotionText;
    public string[] textExample;

    private void Start() {
        originalCameraPos = transform.position;
        originalCameraAngle = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale>=1)
        {
            inSlowMotion = false;
        }
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (inSlowMotion)
        {
            if (Time.timeScale > 0.6f)
            {
                transform.position = Vector3.Lerp(cameraFocus, originalCameraPos, Time.timeScale);
            }
            transform.eulerAngles = Vector3.Lerp(cameraFocusAngle, originalCameraAngle, Time.timeScale);
        }
    }

    public void DoSlowMotion(Vector3 focus) {
        inSlowMotion = true;
        cameraFocus = focus;
        transform.position = Vector3.Lerp(focus, originalCameraPos, 0.6f);
        transform.LookAt(focus, Vector3.up);
        cameraFocusAngle = transform.eulerAngles;
        StartCoroutine(showupText());
        if (cameraFocusAngle.y>180)
        {
            cameraFocusAngle.y -= 360;
        }
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    IEnumerator showupText() {
        slowMotionText.gameObject.SetActive(true);
        slowMotionText.text = textExample[Random.Range(0, textExample.Length)];

        for (float i=0.0f;i<0.3f;i+=Time.deltaTime)
        {
            slowMotionText.gameObject.GetComponent<RectTransform>().localScale =  new Vector3(curve.Evaluate(i/0.3f), curve.Evaluate(i / 0.3f), 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(1.2f);
        for (float i = 0.0f; i < 0.25f; i += Time.deltaTime)
        {
            slowMotionText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, Mathf.Lerp(-20.0f, 50.0f, curve.Evaluate(i / 0.25f)), 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        slowMotionText.gameObject.SetActive(false);
        slowMotionText.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
        slowMotionText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -20.0f, 0);
    }
}
