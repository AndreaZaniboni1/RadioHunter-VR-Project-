using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Battery : MonoBehaviour
{
    public XRBaseInteractor socketInteractor;
    public float batteryAmount;
    private TurnOnCamera camera;

    [SerializeField] private EventReference batterySound;
    void Start()
    {
        socketInteractor.selectEntered.AddListener(AddBattery);
        camera=this.GetComponent<TurnOnCamera>();

    }

    private void AddBattery(SelectEnterEventArgs arg0)
    {
        if(camera.currentBattery < camera.maxDurationBattery-batteryAmount)
        {
            camera.currentBattery += batteryAmount;
        }
        else { camera.currentBattery =camera.maxDurationBattery;}

        AudioManager.instance.PlayOneShot(batterySound, this.transform.position);
        Destroy(arg0.interactableObject.transform.gameObject);
    }
}
