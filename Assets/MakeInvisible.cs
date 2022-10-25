using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakeInvisible : MonoBehaviour
{
    lineDraw linedraw;
    // Start is called before the first frame update
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        linedraw = FindObjectOfType<lineDraw>();
        linedraw.offScreenShot += OnActive;
        linedraw.onScreenShot += DisableButtons;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnActive()
    {
        Invoke("ActivateButtons", 0.5f);
    }

    public void ActivateButtons()
    {
        //Debug.Log("increased alpha to 1");
        canvasGroup.alpha = 1;
    }

    public void DisableButtons()
    {
       // Debug.Log("decreased alpha to 0");
        canvasGroup.alpha = 0;
    }
}
