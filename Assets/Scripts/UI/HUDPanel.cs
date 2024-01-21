using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    [SerializeField]
    private Image _healthSlider;

    public void SetHealth (float value)
    {
        _healthSlider.fillAmount = value;
    }
}
