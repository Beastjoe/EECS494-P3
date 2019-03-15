using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageController : MonoBehaviour
{
    private MeshRenderer blueRenderer;
    private MeshRenderer redRenderer;

    public GameObject BlueStorage;
    public GameObject RedStorage;
    
    public float tmpCount = 10.0f;
    private float tmpCountDown = 0.0f;
    private bool isLit = true;
    
    // Start is called before the first frame update
    void Start()
    {
        blueRenderer = BlueStorage.GetComponent<MeshRenderer>();
        redRenderer = RedStorage.GetComponent<MeshRenderer>();
        blueRenderer.material.EnableKeyword("_EmissionColor");
        redRenderer.material.EnableKeyword("_EmissionColor");
        tmpCountDown = tmpCount;
    }

    // Update is called once per frame
    void Update()
    {
        tmpCountDown -= Time.deltaTime;
        
        if (tmpCountDown < 0)
        {
            if (isLit)
            {
                blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.05f, 0.05f, 1f));
                blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.05f, 0.05f, 1f));
                
                redRenderer.material.SetColor("_Color", new Vector4(0.07f, 0f, 0.05f, 1f));
                redRenderer.material.SetColor("_EmissionColor", new Vector4(0.07f, 0f, 0.05f, 1f));
                
                isLit = false;
            }
            else
            {
                blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.749f, 0.749f, 1f));
                blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.749f, 0.749f, 1f) * 1.5f);
                
                redRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
                redRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
                
                isLit = true;
            }
            tmpCountDown = tmpCount;
        }
    }
}
