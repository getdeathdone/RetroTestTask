using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Manager
{
  public class UIManager : MonoBehaviour, IInitialize
  {
    [SerializeField]
    private InputPanel _inputPanel;
    [SerializeField]
    private HUDPanel _hudPanel;
    public InputPanel InputPanel => _inputPanel;
    public HUDPanel HUDPanel => _hudPanel;

    public void Initialize()
    {
      InputPanel.gameObject.SetActive(PlatformManager.IS_MOBILE);

      IsInitialized = true;
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}