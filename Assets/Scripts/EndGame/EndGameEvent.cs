using Assets.Scripts.Fire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEvent : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CharacterMotionController character;

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void Lose()
    {
        var fires = FindObjectsOfType<FireInstance>();
        if (fires != null)
        {
            foreach (var fire in fires)
            {
                Destroy(fire.gameObject);
            }
        }
        character.Stop();
        losePanel.SetActive(true);
    }

    public void Win()
    {
        var fires = FindObjectsOfType<FireInstance>();
        if (fires != null)
        {
            foreach (var fire in fires)
            {
                Destroy(fire.gameObject);
            }
        }
        character.Stop();
        winPanel.SetActive(true);
    }
}
