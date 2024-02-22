using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnOnCamera : MonoBehaviour
{
    public Camera camera;
    private bool isOn;
    private float minDurationBattery;
    public float maxDurationBattery;
    public float currentBattery;
    public float drainRate;
    public GameObject CoverCamera1;
    public GameObject CoverCamera2;
    public Transform batteryForegroundPivot;

    [SerializeField] private EventReference clickCameraSound;

    void Start()
    {
        XRGrabInteractableTwoAttached grabbable = GetComponent<XRGrabInteractableTwoAttached>();
        isOn = false;
        camera.enabled = false;
        minDurationBattery = 0.05f;
        maxDurationBattery = 1f;
        CoverCamera1.SetActive(true);
        CoverCamera2.SetActive(true);
        grabbable.activated.AddListener(TurnOn);

        currentBattery = maxDurationBattery;


    }

    private void TurnOn(ActivateEventArgs arg)
    {
        if (isOn == false && currentBattery > minDurationBattery)
        {
            camera.enabled = true;
            isOn = true;
            CoverCamera1.SetActive(false);
            CoverCamera2.SetActive(false);
        }
        else
        {
            TurnOff();
        }
        AudioManager.instance.PlayOneShot(clickCameraSound, this.transform.position);

    }
    private void TurnOff()
    {
        camera.enabled = false;
        isOn = false;
        CoverCamera1.SetActive(true);
        CoverCamera2.SetActive(true);

    }
    void Update()
    {
        if (isOn && currentBattery <= maxDurationBattery && currentBattery > minDurationBattery)
        { currentBattery -= Time.deltaTime * (drainRate / 1000);
            batteryForegroundPivot.localScale = new Vector3(1.0f, 1.0f, currentBattery); ;
        }

        if (isOn && currentBattery <= minDurationBattery)
        {
            TurnOff();
        }
        
    }
}
