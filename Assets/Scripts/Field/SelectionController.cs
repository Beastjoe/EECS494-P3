using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Input;
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
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[0].xButton.isPressed)
        {
            redRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            redRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
        }
        
        if (Gamepad.all[1].xButton.isPressed)
        {
            purpleRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            purpleRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
        }
        
        if (Gamepad.all[2].xButton.isPressed)
        {
            greenRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            greenRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
        }
        
        
        if (Gamepad.all[3].xButton.isPressed)
        {
            blueRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            blueRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
        }
    }
}
