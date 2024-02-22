using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour

{
    private XRBaseInteractable interactable;
    public Vector3 localAxis;
    private Vector3 initialPose;
    private bool isFallowing = false;
    public float followAngleTreshould = 45;
    private bool freeze =false;
    public Transform visualTarget;
    private Vector3 offset;
    private Transform pokeAttachTransform;
    public float resetSpeed = 5;
    [SerializeField] private EventReference clickSound;
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        initialPose = visualTarget.localPosition;
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);

    }

    private void Reset(HoverExitEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFallowing = false;
        }
    }

    private void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;

            pokeAttachTransform = interactor.transform;
            offset = visualTarget.position - pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle (offset, visualTarget.TransformDirection(localAxis));

            if (pokeAngle < followAngleTreshould)
            {
                isFallowing = true;

            }
        }
    }
    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            AudioManager.instance.PlayOneShot(clickSound, this.transform.position);
            freeze = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            if (isFallowing)
            {
                Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
                Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

                visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
            }
            else
            {
                visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialPose, Time.deltaTime * resetSpeed);
            }
        }
    }
}
