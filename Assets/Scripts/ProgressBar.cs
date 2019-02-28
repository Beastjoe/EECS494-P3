using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float time = 1.0f;
    private Slider slider;
    private bool isCounting;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
        isCounting = false;
    }

    void Update()
    {
        if (!isCounting)
        {
            isCounting = true;
            StartCoroutine(StartCounting());
        }
        
    }

    IEnumerator StartCounting()
    {
        for (int i = 0; i < 100; ++i)
        {
            slider.value += 0.01f;
            yield return new WaitForSeconds(time / 100f);
        }
        isCounting = false;
        gameObject.SetActive(false);
    }
}
