using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
  public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
  {
    private bool _isButtonHeld;
    public bool IsButtonHeld => _isButtonHeld;

    public void OnPointerDown (PointerEventData eventData)
    {
      _isButtonHeld = true;
    }

    public void OnPointerUp (PointerEventData eventData)
    {
      _isButtonHeld = false;
    }
  }
}