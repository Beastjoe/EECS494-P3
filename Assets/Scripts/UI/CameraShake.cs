using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input.Haptics;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPos;
    private void Awake() {
        originalPos = transform.localPosition;
        Debug.Log(originalPos);
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public void ShakeCameraOnHurt(float duration, float magnitude) {
        StartCoroutine(ShakeOnHurt(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elasped = 0.0f;

        while (elasped < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y, originalPos.z + z);

            elasped += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    IEnumerator ShakeOnHurt(float duration, float magnitude) {
        float elasped = 0.0f;
        
        while (elasped < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;
            Vector3 originalPos = transform.position;
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y, originalPos.z + z);

            elasped += Time.deltaTime;

            yield return null;
        }

    }
}
