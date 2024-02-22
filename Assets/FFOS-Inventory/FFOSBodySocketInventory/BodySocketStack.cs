using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BodySocketStack : MonoBehaviour
{
    public XRBaseInteractor socketInteractor;
    void Start()
    {
        socketInteractor.hoverEntered.AddListener(AddStack);
        socketInteractor.hoverExited.AddListener(ExitHover);
        socketInteractor.selectExited.AddListener(TakeStack);


    }

    private void ExitHover(HoverExitEventArgs arg0)
    {
        arg0.interactableObject.transform.localScale = Vector3.one;
    }

    private void AddStack(HoverEnterEventArgs arg0)
    {
        arg0.interactableObject.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
        Debug.Log(arg0.interactableObject.transform.localScale);
    }

    public void TakeStack(SelectExitEventArgs arg0)
    {

            arg0.interactableObject.transform.localScale = Vector3.one;

    }



}
