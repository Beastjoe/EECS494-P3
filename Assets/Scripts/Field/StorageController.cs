using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageController : MonoBehaviour
{
    public static StorageController instance;
    private MeshRenderer blueRenderer;
    private MeshRenderer redRenderer;

    public GameObject BlueStorage;
    public GameObject RedStorage;

    public float FlashCount = 0.5f;
    private float flashCountdown;
    
   
    private bool isBlueLit;
    private bool isRedLit;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        blueRenderer = BlueStorage.GetComponent<MeshRenderer>();
        redRenderer = RedStorage.GetComponent<MeshRenderer>();
        blueRenderer.material.EnableKeyword("_EmissionColor");
        redRenderer.material.EnableKeyword("_EmissionColor");

        isRedLit = true;
        isBlueLit = true;

        flashCountdown = FlashCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlueLit)
        {
            blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.749f, 0.749f, 1f));
            blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.749f, 0.749f, 1f) * 1.5f);
        }
        else
        {
            blueRenderer.material.SetColor("_Color", new Vector4(0f, 0.05f, 0.05f, 1f));
            blueRenderer.material.SetColor("_EmissionColor", new Vector4(0f, 0.05f, 0.05f, 1f));
        }

        if (isRedLit)
        {
            redRenderer.material.SetColor("_Color", new Vector4(0.749f, 0f, 0.529f, 1f));
            redRenderer.material.SetColor("_EmissionColor", new Vector4(0.749f, 0f, 0.529f, 1f) * 1.5f);
        }
        else
        {
            redRenderer.material.SetColor("_Color", new Vector4(0.07f, 0f, 0.05f, 1f));
            redRenderer.material.SetColor("_EmissionColor", new Vector4(0.07f, 0f, 0.05f, 1f));
        }
    }

    IEnumerator RedFlashOn()
    {
        int sparklingCount = 10;
        for (int i = 0; i < sparklingCount; i++)
        {
            float WaitTime = 0.05f + Random.Range(0f, 0.05f);
            yield return new WaitForSeconds(WaitTime);
            isRedLit = !isRedLit;
        }
        isRedLit = true;
    }
    
    IEnumerator BlueFlashOn()
    {
        int sparklingCount = 10;
        for (int i = 0; i < sparklingCount; i++)
        {
            float WaitTime = 0.05f + Random.Range(0f, 0.05f);
            yield return new WaitForSeconds(WaitTime);
            isBlueLit = !isBlueLit;
        }
        isBlueLit = true;
    }
    
    IEnumerator BlueFlashOff()
    {
        int sparklingCount = 10;
        for (int i = 0; i < sparklingCount; i++)
        {
            float WaitTime = 0.05f + Random.Range(0f, 0.05f);
            yield return new WaitForSeconds(WaitTime);
            isBlueLit = !isBlueLit;
        }
        isBlueLit = false;
    }
    
    IEnumerator RedFlashOff()
    {
        int sparklingCount = 10;
        for (int i = 0; i < sparklingCount; i++)
        {
            float WaitTime = 0.05f + Random.Range(0f, 0.05f);
            yield return new WaitForSeconds(WaitTime);
            isRedLit = !isRedLit;
        }
        isRedLit = false;
    }


    public void LightBlue()
    {
        StartCoroutine(BlueFlashOn());
    }

    public void LightRed()
    {
        StartCoroutine(RedFlashOn());
    }

    public void UnlightBlue()
    {
        StartCoroutine(BlueFlashOff());
    }

    public void UnlightRed()
    {
        StartCoroutine(RedFlashOff());
    }

    public bool GetBlueStatus()
    {
        return isBlueLit;
    }

    public bool GetRedStatus()
    {
        return isRedLit;
    }
}
