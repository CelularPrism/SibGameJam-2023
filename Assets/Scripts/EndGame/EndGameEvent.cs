using Assets.Scripts.Fire;
using Assets.Scripts.Tears;
using FMODUnity;
using TMPro;
using UnityEngine;

public class EndGameEvent : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CharacterMotionController character;
    [SerializeField] private TearsSource tears;
    [SerializeField] private GameObject BGMusic;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text totalTime;

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void Lose()
    {
        MuteAll();
        timer.enabled = false;
        character.enabled = false;
        tears.enabled = false;
        losePanel.SetActive(true);
    }

    public void Win()
    {
        MuteAll();
        timer.enabled = false;
        character.enabled = false;
        tears.enabled = false;
        totalTime.text = $"{timer.GetTime().Minutes}:{timer.GetTime().Seconds}";
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
    }
}
