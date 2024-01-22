using DefaultNamespace.Controller;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
  public class MenuBase : MonoBehaviour
  {
    [SerializeField]
    private GameObject _panelParent;
    
    [SerializeField]
    private Button _openButton;
    [SerializeField]
    protected Button _closeButton;
    [SerializeField]
    private Button _exitGameButton;
    [SerializeField]
    protected Button _restartGameButton;

    protected GameController _gameController;
    private bool _isOpen;
    public bool IsOpen => _isOpen;

    [Inject]
    private void Construct(
      GameController gameController)
    {
      _gameController = gameController;
    }

    protected virtual void Awake()
    {
      _openButton.onClick.AddListener(OpenCloseMenu);
      _closeButton.onClick.AddListener(OpenCloseMenu);
      _exitGameButton.onClick.AddListener(ExitGame);
      _restartGameButton.onClick.AddListener(RestartGame);
    }

    protected virtual  void OnDestroy()
    {
      _openButton.onClick.RemoveListener(OpenCloseMenu);
      _closeButton.onClick.RemoveListener(OpenCloseMenu);
      _exitGameButton.onClick.RemoveListener(ExitGame);
      _restartGameButton.onClick.RemoveListener(RestartGame);
    }

    public virtual void OpenCloseMenu()
    {
      _isOpen = !IsOpen;
      _gameController.TogglePause();
      _panelParent.SetActive(_isOpen);
    }

    private void RestartGame()
    {
      _gameController.RestartGame();
      OpenCloseMenu();
    }

    private void ExitGame()
    {
      Application.Quit();
    }
  }
}