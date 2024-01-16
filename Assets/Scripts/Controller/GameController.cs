using System;
using UnityEngine;

namespace DefaultNamespace.Controller
{
  public class GameController
  {
    public event Action OnRestart;

    private bool _isStop;
    private bool _isPaused;

    public bool IsPaused => _isPaused || _isStop;

    public void TogglePause()
    {
      _isPaused = !_isPaused;
      Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
      OnRestart?.Invoke();

      StartGame();
    }

    public void StartGame()
    {
      _isStop = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void EndGame()
    {
      _isStop = true;
      Cursor.lockState = CursorLockMode.None;
    }
  }
}