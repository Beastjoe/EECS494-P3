using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input.Haptics;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPos;
    private void Start() {
        originalPos = transform.localPosition;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
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


}
