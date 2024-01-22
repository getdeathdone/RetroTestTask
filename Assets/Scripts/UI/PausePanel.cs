using DefaultNamespace.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
  public class PausePanel : MenuBase
  {
    private const string VICTORY = "Victory";
    private const string LOSS = "Loss";
    private const string PAUSE = "Pause";

    [SerializeField]
    private Button _startGame;
    [SerializeField]
    private TextMeshProUGUI _gameStatus;

    private BattleController _battleController;

    [Inject]
    private void Construct(
      BattleController battleController)
    {
      _battleController = battleController;
    }

    protected override void Awake()
    {
      base.Awake();

      _startGame.onClick.AddListener(StartGame);
      _battleController.OnFinishBattle += FinishBattle;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      _startGame.onClick.RemoveListener(StartGame);
      _battleController.OnFinishBattle -= FinishBattle;
    }

    private void StartGame()
    {
      _gameController.StartGame();
      OpenCloseMenu();
      
      _startGame.gameObject.SetActive(false);
      _restartGameButton.gameObject.SetActive(true);
      _closeButton.gameObject.SetActive(true);
    }

    private void FinishBattle (bool obj)
    {
      _gameStatus.text = obj ? VICTORY : LOSS;
      OpenCloseMenu();
    }

    public override void OpenCloseMenu()
    {
      if (!_gameController.IsStop)
      {
        _gameStatus.text = PAUSE;
      }
      
      base.OpenCloseMenu();
    }
  }
}