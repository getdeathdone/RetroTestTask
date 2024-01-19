using System;
using DefaultNamespace.Controller;
using DefaultNamespace.Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class AreaController : MonoBehaviour, IInitialize
  {
    [SerializeField]
    private float _offset;
    [SerializeField]
    private SphereCollider _sphereCollider;

    private float Radius => _sphereCollider.radius - _offset;
    private Vector3 CenterPoint => _sphereCollider.center;
    public Type Type => GetType();
    
    private EnemyController _enemyController;
    private PlayerController _playerController;
    private GameController _gameController;

    [Inject]
    private void Construct(EnemyController enemyController, PlayerController playerController, GameController gameController)
    {
      _enemyController = enemyController;
      _playerController = playerController;
      _gameController = gameController;
    }

    public void Initialize()
    {
      _playerController.Player.transform.position = GenerateRandomPositionInCircle(Radius);

      foreach (var enemy in _enemyController.Enemies)
      {
        enemy.transform.position = GenerateRandomPositionInCircle(Radius);
      }
      
      IsInitialized = true;
    }

    private void Update()
    {
      if (!IsInitialized || _gameController.IsPaused)
      {
        return;
      }
      
      CheckNearEdge(_playerController.Player.transform, MoveAwayFromEnemies);

      foreach (var enemy in _enemyController.Enemies)
      {
        CheckNearEdge(enemy.transform, ClampMovementToBounds);
      }
    }

    private void CheckNearEdge(Transform heroTransform, Action<Transform> moveAction)
    {
      float distanceToCenter = Vector3.Distance(heroTransform.position, CenterPoint);

      if (distanceToCenter > Radius)
      {
        moveAction?.Invoke(heroTransform);
      }
    }

    private void ClampMovementToBounds(Transform heroTransform)
    {
      Vector3 normalizedDirection = (heroTransform.position - CenterPoint).normalized;
      Vector3 newPosition = CenterPoint + normalizedDirection * Radius;
        
      newPosition.x = Mathf.Clamp(newPosition.x, CenterPoint.x - Radius, CenterPoint.x + Radius);
      newPosition.z = Mathf.Clamp(newPosition.z, CenterPoint.z - Radius, CenterPoint.z + Radius);
        
      heroTransform.position = newPosition;
    }

    private void MoveAwayFromEnemies(Transform heroTransform)
    {
      Vector3 nearestEnemy = GetNearestEnemy(heroTransform.position);
      
      heroTransform.position = GetSafePosition(nearestEnemy);
      
      Vector3 GetSafePosition(Vector3 enemyPosition)
      {
        Vector3 direction = (heroTransform.position - enemyPosition).normalized;
        Vector3 safePosition = CenterPoint + direction * Radius;

        return safePosition;
      }
    }

    private Vector3 GetNearestEnemy(Vector3 positionPlayer)
    {
      Vector3 nearestEnemy = default;
      float minDistance = float.MaxValue;

      foreach (var enemy in _enemyController.Enemies)
      {
        float distance = Vector3.Distance(positionPlayer, enemy.transform.position);

        if (distance < minDistance)
        {
          minDistance = distance;
          nearestEnemy = enemy.transform.position;
        }
      }

      return nearestEnemy;
    }

    private Vector3 GenerateRandomPositionInCircle(float radius)
    {
      float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
      float distance = UnityEngine.Random.Range(0f, radius);
      
      float spawnX = distance * Mathf.Cos(angle);
      float spawnZ = distance * Mathf.Sin(angle);

      return new Vector3(spawnX, 0f, spawnZ);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
      Handles.color = Color.yellow;
      Handles.DrawWireDisc(CenterPoint, Vector3.up, Radius);
    }
#endif

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}