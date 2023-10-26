using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void ChangeSound() =>
        soundBus.setVolume(sound.value);

    public void ChangeMusic() =>
        musicBus.setVolume(music.value);

    public void ChangeVFX() =>
        vfxBus.setVolume(vfx.value);
}
