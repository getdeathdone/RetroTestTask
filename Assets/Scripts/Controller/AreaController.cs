using System;
using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace DefaultNamespace.Controller
{
  public class AreaController : MonoBehaviour, IInitialize, IDeinitialize
  {
    private const int MAX_ATTEMPTS = 30;
    public static Action<Transform> OnTeleport;

    [SerializeField]
    private float _offsetTeleport;
    [SerializeField]
    private SphereCollider _sphereCollider;

    private float Radius => _sphereCollider.radius - _offsetTeleport;
    private float SpawnRadius => Radius - _offsetTeleport;
    private Vector3 CenterPoint => _sphereCollider.center;
    public Type Type => GetType();

    private EnemyController _enemyController;
    private PlayerController _playerController;
    private GameController _gameController;

    [Inject]
    private void Construct (EnemyController enemyController, PlayerController playerController, GameController gameController)
    {
      _enemyController = enemyController;
      _playerController = playerController;
      _gameController = gameController;
    }

    public void Initialize()
    {
      if (IsInitialized)
      {
        return;
      }

      _enemyController.OnSpawnEnemy += GeneratePosition;
      _playerController.OnSpawnPlayer += GeneratePosition;

      IsInitialized = true;
    }

    public void Deinitialize()
    {
      if (!IsInitialized)
      {
        return;
      }

      _enemyController.OnSpawnEnemy -= GeneratePosition;
      _playerController.OnSpawnPlayer -= GeneratePosition;

      IsInitialized = false;
    }

    private void GeneratePosition (HeroBase obj)
    {
      obj.transform.position = GenerateRandomPositionInCircle(SpawnRadius);
    }

    private void Update()
    {
      if (!IsInitialized || _gameController.IsPaused)
      {
        return;
      }

      CheckNearEdge(_playerController.Player.transform, MoveAwayFromEnemies);
    }

    private void CheckNearEdge (Transform heroTransform, Action<Transform> moveAction)
    {
      float distanceToCenter = Vector3.Distance(heroTransform.position, CenterPoint);

      if (distanceToCenter > Radius)
      {
        moveAction?.Invoke(heroTransform);
        heroTransform.rotation = Quaternion.LookRotation((_sphereCollider.transform.position - heroTransform.transform.position).normalized);
        OnTeleport?.Invoke(heroTransform);
      }
    }

    private void MoveAwayFromEnemies (Transform heroTransform)
    {
      Vector3 nearestEnemy = GetNearestEnemy(heroTransform.position);

      Vector3 direction = (heroTransform.position - nearestEnemy).normalized;
      Vector3 safePosition = CenterPoint + direction * SpawnRadius;

      heroTransform.position = safePosition;
    }

    private Vector3 GetNearestEnemy (Vector3 positionPlayer)
    {
      Vector3 nearestEnemy = default;
      float minDistance = float.MaxValue;

      foreach (var enemy in _enemyController.Enemies)
      {
        float distance = Vector3.Distance(positionPlayer, enemy.transform.position);

        if (distance < minDistance && distance < SpawnRadius)
        {
          minDistance = distance;
          nearestEnemy = enemy.transform.position;
        }
      }

      return nearestEnemy;
    }


    private Vector3 GenerateRandomPositionInCircle (float radius)
    {
      Vector3 center = _sphereCollider.transform.position;

      for (int i = 0; i < MAX_ATTEMPTS; i++)
      {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        float distance = UnityEngine.Random.Range(0f, radius);

        float spawnX = center.x + distance * Mathf.Cos(angle);
        float spawnZ = center.z + distance * Mathf.Sin(angle);

        Vector3 randomPosition = new Vector3(spawnX, center.y, spawnZ);

        bool validPosition = NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, NavMesh.AllAreas);

        if (validPosition)
        {
          return hit.position;
        }
      }

      return center;
    }

    public bool IsInitialized
    {
      get;
      private set;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      Handles.color = Color.yellow;
      Handles.DrawWireDisc(CenterPoint, Vector3.up, Radius);
    }
#endif
  }
}