using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPose;
    public HandData leftHandPose;
    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;



    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetupPose);
        rightHandPose.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        leftHandPose.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
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
                SetHandDataValues(controller, rightHandPose);
            }
            else
            {
                SetHandDataValues(controller, leftHandPose);
            }

            SetHandDataValues(controller, rightHandPose);
            SetHandData(controller, finalHandPosition, finalHandRotation, finalFingerRotations);
        }

    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = new Vector3(h1.root.localPosition.x /h1.root.localScale.x, h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x, h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations= new Quaternion[h1.fingerBones.Length];
        finalFingerRotations=new Quaternion[h2.fingerBones.Length]; 

        for (int i= 0; i<h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation; 
        }

    }
    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation) 
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;
        for (int i= 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }

    }

    public void UnSetupPose(BaseInteractionEventArgs args)

    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            var controller = controllerInteractor.xrController.transform.GetComponentInChildren<HandData>();
            controller.animator.enabled = true;

            SetHandData(controller, startingHandPosition, startingHandRotation, startingFingerRotations);
        }

    }
}
