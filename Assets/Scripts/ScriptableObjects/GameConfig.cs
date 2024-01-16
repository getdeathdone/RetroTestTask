using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
  [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig")]
  public class GameConfig : ScriptableObject
  {
    [Header("Projectile")]
    public float projectileChanceToBounce = 0.3f;
    public float projectileBounceStrengthGain = 10f;
    public float projectileBounceHealthGainPercentage = 0.5f;

    [Header("Ultimate Ability")]
    public int ultimateAbilityThreshold = 100;

    [Header("Player Movement")]
    public float teleportEdgeDistance = 2f;

    [Header("Enemy Stats")]
    public int initialBlueEnemyHealth = 100;
    public int initialRedEnemyHealth = 50;

    [Header("Enemy Spawn")]
    public float initialSpawnInterval = 10f;
    public float minSpawnInterval = 6f;
    public int maxEnemiesOnMap = 30;
    public float spawnIntervalDecreaseRate = 2f;
  }
}