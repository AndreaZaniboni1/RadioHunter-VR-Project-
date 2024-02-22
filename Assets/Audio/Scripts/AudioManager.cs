using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public static float masterVolume = 1;
    [Range(0, 1)]
    public static float musicVolume = 1;
    [Range(0, 1)]
    public static float ambienceVolume = 1;
    [Range(0, 1)]
    public static float SFXVolume = 1;
    [Range(0, 1)]
    public static float SpiderSFXVolume = 1;

    private Bus masterBus;
    private Bus ambienceBus;
    private Bus sfxBus;
    private Bus SpiderSfxBus;


    public GameObject batterySpawns;
    public GameObject batteryPrefab;
    public GameObject magazineSpawns;
    public GameObject magazinePrefab;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;

    [SerializeField] private EventReference windAmbienceSound;
    [SerializeField] private EventReference earieeAmbienceSound;

    private EventInstance musicEventInstance;

    public Scrollbar masterScrollbar;
    public Scrollbar SFXScrollbar;
    public Scrollbar AmbienceScrollbar;

    public Material mimicMaterial;


    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        if (masterScrollbar != null)
        {
            masterScrollbar.onValueChanged.AddListener((float val) => MasterScrollbarCallback(val));
            SFXScrollbar.onValueChanged.AddListener((float val) => SFXScrollbarCallback(val));
            AmbienceScrollbar.onValueChanged.AddListener((float val) => AmbienceScrollbarCallback(val));

        }

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        SpiderSfxBus = RuntimeManager.GetBus("bus:/SFX/SpiderSFX");
    }

    void MasterScrollbarCallback(float value)
    {
        masterVolume=value;
    }
    void SFXScrollbarCallback(float value)
    {
        SFXVolume = value;
    }
    void AmbienceScrollbarCallback(float value)
    {
        ambienceVolume = value;
    }

    private void Start()
    {

        mimicMaterial.color = UnityEngine.Color.black;
        foreach (Transform child in batterySpawns.transform)
        {
            Instantiate(batteryPrefab, child.transform.position, Quaternion.identity);
        }
        foreach (Transform child in magazineSpawns.transform)
        {
            Instantiate(magazinePrefab, child.transform.position, Quaternion.identity);
        }
        InitializeAmbience(windAmbienceSound);
        InitializeAmbience(earieeAmbienceSound);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
        SpiderSfxBus.setVolume(SpiderSFXVolume);
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    public void ChangeVolume(float value, int busIndex) 
    {
        if (busIndex == 0)
        {
            masterVolume = value;
        }            
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}