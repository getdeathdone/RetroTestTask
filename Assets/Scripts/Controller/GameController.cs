using System;
using UnityEngine;

namespace DefaultNamespace.Controller
{
  public class GameController
  {
    public event Action OnRestart;
    public event Action<bool> OnPause;

    private bool _isStop = true;
    private bool _isPaused;

    public bool IsStop => _isStop;
    public bool IsPaused => _isPaused || _isStop;

    public void TogglePause()
    {
      _isPaused = !_isPaused;
      OnPause?.Invoke(IsPaused);
      Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
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