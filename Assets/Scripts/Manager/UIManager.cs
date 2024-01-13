using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.Manager
{
  public class UIManager : MonoBehaviour, IInitialize
  {
    [SerializeField]
    private InputPanel _inputPanel;
    public InputPanel InputPanel => _inputPanel;
    
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