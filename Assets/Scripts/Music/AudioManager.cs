using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    [HideInInspector] public Dictionary<string, EventInstance> EventInstancesDict;

    bool calmMusicIsCurrentlyPlaying;

    Coroutine fadeOutCoroutine;
    Coroutine randomSFXCor;

    [field: Header("Music")]
    [field: SerializeField] public EventReference MusicMainMenu { get; private set; }
    [field: SerializeField] public EventReference MusicHub { get; private set; }
    [field: SerializeField] public EventReference MusicFight { get; private set; }
    [field: SerializeField] public EventReference MusicNoFight { get; private set; }

    [field: Header("Enverenment")]
    [field: SerializeField] public EventReference RoomCleared { get; private set; }

    [field: Header("Player")]
    [field: SerializeField] public EventReference PlayerStepSound { get; private set; }
    [field: SerializeField] public EventReference ShootSound { get; private set; }

    [field: Header("UI")]
    [field: SerializeField] public EventReference ButtonPress { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();
    }

    void Start()
    {
        ToggleSound(Settings.isMusicOn);

        EventInstancesDict.Add("MusicMainMenu", CreateInstance(MusicMainMenu)); 
        EventInstancesDict.Add("MusicHub", CreateInstance(MusicHub));
        EventInstancesDict.Add("MusicFight", CreateInstance(MusicFight));
        EventInstancesDict.Add("MusicNoFight", CreateInstance(MusicNoFight));

        EventInstancesDict.Add("PlayerStepSound", CreateInstance(PlayerStepSound));
        EventInstancesDict.Add("ShootSound", CreateInstance(ShootSound));

        EventInstancesDict.Add("RoomCleared", CreateInstance(RoomCleared));
        EventInstancesDict.Add("ButtonPress", CreateInstance(ButtonPress));
    }

    public void SetParameter(string instanceName, string parameterName, float value)
    {
        EventInstancesDict[instanceName].setParameterByName(parameterName, value);
    }
    public void SetParameterWithCheck(string instanceName, string parameterName, float newValue)
    {
        float currentParameterValue;
        EventInstancesDict[parameterName].getParameterByName(parameterName, out currentParameterValue);

        if (currentParameterValue != newValue)
        {
            EventInstancesDict[parameterName].setParameterByName(parameterName, newValue);
        }
    }


    public void PlayOneShot(string sound)
    {
        EventInstancesDict[sound].start();
    }
    public void PlayOneShot(EventReference sound, Vector2 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
    public void PlayOneShot(EventReference sound, Vector2 position, float volume)
    {
        FMOD.Studio.EventInstance soundEvent = FMODUnity.RuntimeManager.CreateInstance(sound);
        soundEvent.setVolume(volume);
        soundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));

        soundEvent.start();
        soundEvent.release();
    }


    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public StudioEventEmitter initializeEventEmitter(EventReference eventReference, GameObject emitterGameO)
    {
        StudioEventEmitter emitter = emitterGameO.GetComponent<StudioEventEmitter>();

        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);

        return emitter;
    }

    public void StopAllEmitters()
    {
        foreach (var emitter in eventEmitters)
        {
            if (emitter != null && emitter.IsPlaying())
            {
                emitter.Stop();
            }
        }
    }

    public void StopMusic()
    {
        EventInstancesDict["MusicMainMenu"].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        EventInstancesDict["MusicHub"].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        EventInstancesDict["MusicFight"].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        EventInstancesDict["MusicNoFight"].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void ToggleSound(bool val)
    {
        RuntimeManager.GetBus("bus:/").setVolume(val ? 1 : 0);
    }
}