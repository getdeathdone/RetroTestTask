using System;
using DefaultNamespace.Controller;
using DefaultNamespace.Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Manager
{
  public class AreaManager : MonoBehaviour, IInject
  {
    [SerializeField]
    private float _offset;
    [SerializeField]
    private SphereCollider _sphereCollider;

    public float Radius => _sphereCollider.radius - _offset;
    public Vector3 CenterPoint => _sphereCollider.center;
    public Type Type => GetType();
    
    private EnemyController _enemyController;

    [Inject]
    private void Construct(EnemyController enemyController)
    {
      _enemyController = enemyController;
    }
    
    public Vector3 GetNearestEnemy(Vector3 positionPlayer)
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      Handles.color = Color.yellow;
      Handles.DrawWireDisc(CenterPoint, Vector3.up, Radius);
    }
#endif
  }
}