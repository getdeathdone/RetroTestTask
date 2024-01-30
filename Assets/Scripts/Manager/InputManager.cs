using UnityEngine;

namespace DefaultNamespace.Manager
{
  public class InputManager
  {
    public bool Pause => Input.GetKeyDown(KeyCode.Escape);
  }
}