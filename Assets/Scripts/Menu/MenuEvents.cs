using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    [SerializeField] private int _gameplayScene;

    public void OpenPanel(GameObject panel) => panel.SetActive(true);

    public void ClosePanel(GameObject panel) => panel.SetActive(false);

    public void QuitGame() => Application.Quit();

    public void StartGame() => SceneManager.LoadScene(_gameplayScene);

    public void ClickBtn() => Debug.Log("Click");
}
