using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Manager
{
  public class UIManager : MonoBehaviour, IInitialize
  {
    [SerializeField]
    private InputPanel _inputPanel;
    [SerializeField]
    private PausePanel _pausePanel;
    [SerializeField]
    private HUDPanel _hudPanel;
    public InputPanel InputPanel => _inputPanel;
    public PausePanel PausePanel => _pausePanel;
    public HUDPanel HUDPanel => _hudPanel;

    public void Initialize()
    {
      InputPanel.gameObject.SetActive(PlatformManager.IS_MOBILE);
      PausePanel.KeyMap.SetActive(!PlatformManager.IS_MOBILE);

      IsInitialized = true;
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}