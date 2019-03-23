using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RupeeInitialization : MonoBehaviour
{
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        StartCoroutine(initialize());
    }

    IEnumerator initialize() {
        material.DisableKeyword("_EMISSION");
        gameObject.layer = 11;
        for (int i=0; i<3; i++)
        {
            Color c = material.color;
            c.a = 0.5f;
            material.color = c;
            yield return new WaitForSeconds(0.3f);
            c = material.color;
            c.a = 1.0f;
            material.color = c;
            yield return new WaitForSeconds(0.3f);
        }
        material.EnableKeyword("_EMISSION");
        gameObject.layer = 14;
    }

    
}
