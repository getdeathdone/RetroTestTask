using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Handler
{
  public class CoroutineHandler : MonoBehaviour
  {
    public Coroutine StartRoutine (IEnumerator enumerator)
    {
      return StartCoroutine(enumerator);
    }

    public void StopRoutine (IEnumerator routine)
    {
      if (routine != null)
      {
        StopCoroutine(routine);
      }
    }
  }
}