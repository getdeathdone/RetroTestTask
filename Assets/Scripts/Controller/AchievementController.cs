using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Controller
{
  public class AchievementController: object, IInitialize, IDeinitialize
  {
    private readonly EnemyController _enemyController;
    private readonly PlayerController _playerController;

    [Inject]
    public AchievementController(EnemyController enemyController, PlayerController playerController)
    {
      _playerController = playerController;
      _enemyController = enemyController;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }
      
      Subscribe(true);
      
      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }

      Subscribe(false);
      
      IsInitialized = false;
    }

    private void OnDied (DamageInfo damageInfo)
    {
      var hero = ((Health)damageInfo.Receiver).ComponentOwner;

      if (hero.Side == HeroSide.None)
      {
        Debug.LogWarning("HeroSide.None");
        return;
      }

      if (hero.Side == HeroSide.Player)
      {
        Deinitialize();
        return;
      }
      
      var attack = damageInfo.DamageDealer.GetAttachedComponent<Attack>();
      attack.AddStrength(GameConstants.Achievement.CalculateStrengthAchievement(hero.Type));

      if (damageInfo.AttackType == AttackType.Ricochet)
      {
        RicochetKillBonus();
      }

      void RicochetKillBonus()
      {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= GameConstants.Achievement.RicochetStrengthGainChance)
        {
          var attack = damageInfo.DamageDealer.GetAttachedComponent<Attack>();
          attack.AddStrength(GameConstants.Achievement.RicochetStrengthGain);
        } else
        {
          var health = damageInfo.DamageDealer.GetAttachedComponent<Health>();
          float healthToRestore = health.MaxHealth * GameConstants.Achievement.RicochetHealthGainPercentage;

          health.RestoreHealth((int)healthToRestore);
        }
      }
    }

    private void Subscribe (bool value)
    {
      if (value)
      {
        _playerController.Player.OnDeath += OnDied;
        
        foreach (var VARIABLE in _enemyController.Enemies)
        {
          VARIABLE.OnDeath += OnDied;
        }
      } else
      {
        _playerController.Player.OnDeath -= OnDied;
        
        foreach (var VARIABLE in _enemyController.Enemies)
        {
          VARIABLE.OnDeath -= OnDied;
        }
      }
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}