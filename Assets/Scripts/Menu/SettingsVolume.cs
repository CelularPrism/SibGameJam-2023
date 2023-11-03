using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsVolume : MonoBehaviour
{
    [SerializeField] private Slider sound;
    [SerializeField] private Slider music;
    [SerializeField] private Slider vfx;

    private Bus soundBus;
    private Bus musicBus;
    private Bus vfxBus;

    private void Start()
    {
        soundBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        vfxBus   = FMODUnity.RuntimeManager.GetBus("bus:/UI");

        soundBus.setVolume(sound.value);
        musicBus.setVolume(music.value);
        vfxBus.setVolume(vfx.value);
    }

    public void ChangeSound()
    {
        soundBus.setVolume(sound.value);
        soundBus.getVolume(out float volume);

        Debug.Log(volume);
    }

    public void ChangeMusic()
    {
        musicBus.setVolume(music.value);
        musicBus.getVolume(out float volume);

        Debug.Log(volume);
    }

    public void ChangeVFX()
    {
        vfxBus.setVolume(vfx.value);
        vfxBus.getVolume(out float volume);

        Debug.Log(volume);
    }
}
