using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class GrabHandPose1 : MonoBehaviour
{
    public HandData rightHandPose;
    public HandData leftHandPose;
    public XRGrabInteractableTwoAttached gun;
    public XRGrabInteractableTwoAttached slider;



    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetupPose);
        gun.selectEntered.AddListener(GunIsOn);
        gun.selectExited.AddListener(GunIsOff);
        rightHandPose.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        leftHandPose.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    private void GunIsOff(SelectExitEventArgs arg0)
    {
        slider.enabled = false;
    }

    private void GunIsOn(SelectEnterEventArgs arg0)
    {
        slider.enabled = true;
    }


    // Update is called once per frame
    public void SetupPose(BaseInteractionEventArgs args)

    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            var controller = controllerInteractor.xrController.transform.GetComponentInChildren<HandData>();
            controller.animator.enabled = false;

            if (controller.modelType == HandData.HandModelType.Right)
            {
                ShowHandPose(controller, rightHandPose);
            }
            else
            {
                ShowHandPose(controller, leftHandPose);
            }

        }

    }

    public void ShowHandPose(HandData h1, HandData h2)
    {
        h2.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        h1.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }


    public void UnSetupPose(BaseInteractionEventArgs args)

    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            var controller = controllerInteractor.xrController.transform.GetComponentInChildren<HandData>();
            controller.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            if (controller.modelType == HandData.HandModelType.Right)
            {
                rightHandPose.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                leftHandPose.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }

        }

    }
}
