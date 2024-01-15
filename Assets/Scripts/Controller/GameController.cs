using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Controller
{
  public class GameController
  {
    private bool _isPaused;

    public bool IsPaused => _isPaused;

    void TogglePause()
    {
      _isPaused = !_isPaused;
    }

    void RestartGame()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
      Cursor.lockState = CursorLockMode.Locked;
    }
  }
}