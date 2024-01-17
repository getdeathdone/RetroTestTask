using DefaultNamespace.Controller;
using TMPro;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
  public class BattleStatsPanel : MenuBase
  {
    public const string VICTORY = "Victory";
    public const string LOSS = "Loss";
    
    [SerializeField]
    private TextMeshProUGUI _playerKill;
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
      
      _battleController.OnFinishBattle += FinishBattle;
      _battleController.OnUpdateVisual += UpdatePlayerKill;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      _battleController.OnFinishBattle -= FinishBattle;
      _battleController.OnUpdateVisual -= UpdatePlayerKill;
    }

    private void UpdatePlayerKill (float obj)
    {
      _playerKill.text = ((int)obj).ToString();
    }

    private void FinishBattle (bool obj)
    {
      _gameStatus.text = obj ? VICTORY : LOSS;
      OpenCloseMenu();
    }
  }
}