using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    [SerializeField] private int _gameplayScene;
    [SerializeField] private EventReference clickEvent;
    [SerializeField] private Camera _camera;

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void QuitGame() => Application.Quit();

    public void StartGame() => SceneManager.LoadScene(_gameplayScene);

    public void ClickBtn() => RuntimeManager.PlayOneShot(clickEvent, _camera.GetComponent<Transform>().position);
}
