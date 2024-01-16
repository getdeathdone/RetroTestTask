using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
  [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig")]
  public class GameConfig : ScriptableObject
  {
    [SerializeField]
    private HeroConfigDictionary _heroConfigDictionary = new HeroConfigDictionary();
    
    public HeroConfig GetHeroConfig (HeroType heroType)
    {
      if (_heroConfigDictionary.TryGetValue(heroType, out HeroConfig config))
      {
        return config;
      }

      Debug.LogError("HeroConfig not found for HeroType: " + heroType);
      return null;
    }
    
    public class HeroConfig
    {
      [Header("Common Parameters")]
      [SerializeField]
      private float _projectileSpeed;
      [SerializeField]
      private float _projectileLifetime;

      [Header("Rebound Parameters")]
      [SerializeField]
      private float _reboundChance;
      [SerializeField]
      private float _additionalStrengthOnRebound;
      [SerializeField]
      private float _additionalHealthOnRebound;

      [Header("Ultimate Ability Parameters")]
      [SerializeField]
      private float _ultimateAbilityRadius;

      public float ProjectileSpeed => _projectileSpeed;
      public float ProjectileLifetime => _projectileLifetime;
      public float ReboundChance => _reboundChance;
      public float AdditionalStrengthOnRebound => _additionalStrengthOnRebound;
      public float AdditionalHealthOnRebound => _additionalHealthOnRebound;
      public float UltimateAbilityRadius => _ultimateAbilityRadius;
    }
    
    [Serializable]
    public class HeroConfigDictionary : SerializableDictionaryBase<HeroType, HeroConfig>
    {}
  }
}