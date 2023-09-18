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


    /*

    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference Rain { get; private set; }

    [field: Header("Music")]

    [field: SerializeField] public EventReference Music { get; private set; }

    [field: Header("Enverenment")]
    [field: SerializeField] public EventReference RandomSFX { get; private set; }
    [field: SerializeField] public EventReference Thunder { get; private set; }
    [field: SerializeField] public EventReference Exit { get; private set; }


    [field: Header("Player")]

    [field: SerializeField] public EventReference PlayerStepSound { get; private set; }
    [field: SerializeField] public EventReference PlayerStepSoundOnWater { get; private set; }
    [field: SerializeField] public EventReference DieSound { get; private set; }

    [field: Header("Enemy")]


    [field: SerializeField] public EventReference DefIdle { get; private set; }
    [field: SerializeField] public EventReference DefJump { get; private set; }

    [field: SerializeField] public EventReference BlindIdle { get; private set; }
    [field: SerializeField] public EventReference BlindJump { get; private set; }

    [field: Header("UI")]

    [field: SerializeField] public EventReference ButtonPress { get; private set; }
        */

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();

    }

    void Start()
    {
        /*
        EventInstancesDict.Add("Music", CreateInstance(Music)); 
        EventInstancesDict.Add("RandomSFX", CreateInstance(RandomSFX));
        EventInstancesDict.Add("Ambience", CreateInstance(Ambience));
        EventInstancesDict.Add("Rain", CreateInstance(Rain));

        EventInstancesDict.Add("ButtonPress", CreateInstance(ButtonPress));
        EventInstancesDict.Add("Exit", CreateInstance(Exit));
        //EventInstancesDict.Add("PlaySound", CreateInstance(PlaySound));
        //EventInstancesDict.Add("GameOverSound", CreateInstance(GameOverSound));

        EventInstancesDict.Add("PlayerStepSound", CreateInstance(PlayerStepSound));
        EventInstancesDict.Add("DieSound", CreateInstance(DieSound));

        /*
        EventInstancesDict.Add("PlayerStepSoundOnWater", CreateInstance(PlayerStepSoundOnWater));
        */
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

    public void ToggleSound(bool val)
    {
        RuntimeManager.GetBus("bus:/").setVolume(val ? 1 : 0);
    }
}