using Assets.Scripts.Fire;
using Assets.Scripts.Tears;
using FMODUnity;
using TMPro;
using UnityEngine;

public class EndGameEvent : MonoBehaviour
{
    [SerializeField] private GameObject winPanel, losePanel, pausePannel;
    [SerializeField] private CharacterMotionController character;
    [SerializeField] private TearsSource tears;
    [SerializeField] private GameObject BGMusic;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text totalTime;
    private bool isPaused;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        if (isPaused)
            return;

        isPaused = true;
        //MuteAll();
        timer.enabled = false;
        character.enabled = false;
        tears.enabled = false;
        pausePannel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        if (isPaused == false)
            return;

        isPaused = false;
        //UnMuteAll();
        timer.enabled = true;
        character.enabled = true;
        tears.enabled = true;
        pausePannel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Win()
    {
        MuteAll();
        timer.enabled = false;
        character.enabled = false;
        tears.enabled = false;
        totalTime.text = $"{timer.GetPlaytime().Minutes}:{timer.GetPlaytime().Seconds}";
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

                fire.gameObject.SetActive(false);
            }
        }
    }

    private void UnMuteAll()
    {
        BGMusic.SetActive(true);
        var fires = FindObjectsOfType<FireInstance>();
        if (fires != null)
        {
            foreach (var fire in fires)
            {
                var events = fire.GetComponents<StudioEventEmitter>();
                foreach (var ev in events)
                {
                    ev.Play();
                }

                fire.gameObject.SetActive(true);
            }
        }
    }
}
