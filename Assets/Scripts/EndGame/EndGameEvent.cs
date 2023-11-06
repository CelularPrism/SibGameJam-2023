using Assets.Scripts.Fire;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameEvent : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CharacterMotionController character;
    [SerializeField] private GameObject BGMusic;

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void Lose()
    {
        MuteAll();
        character.Stop();
        losePanel.SetActive(true);
    }

    public void Win()
    {
        MuteAll();
        character.Stop();
        winPanel.SetActive(true);
    }

    private void MuteAll()
    {
        BGMusic.SetActive(false);
        var fires = FindObjectsOfType<FireInstance>();
        if (fires != null)
        {
            foreach (var fire in fires)
            {
                var events = fire.GetComponents<StudioEventEmitter>();
                foreach (var ev in events)
                {
                    ev.Stop();
                }

                Destroy(fire.gameObject);
            }
        }
        RuntimeManager.GetBus("bus:/SFX").setMute(true);
    }
}
