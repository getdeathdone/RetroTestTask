using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Component.AI
{
  public class Navigation : Movement
  {
    private const int PATROL_POINT_COUNT = 3;
    private const float PATROL_POINT_RANGE = 10f;
    private const float DESTINATION_THRESHOLD = 1f;
    private Transform [] _patrolPoints;
    private int _currentPatrolIndex;

    public override void Initialize()
    { 
      base.Initialize();
      
      if (_patrolPoints == null || _patrolPoints.Length == 0)
      {
        _patrolPoints = GeneratePatrolPoints(PATROL_POINT_COUNT);
      }
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();

      Patrol();
    }

    private Transform [] GeneratePatrolPoints (int value)
    {
      Transform [] array = new Transform[value];

      for (int i = 0; i < array.Length; i++)
      {
        Vector3 randomPoint = GetRandomPointOnNavMesh();

        GameObject pointObject = new GameObject($"PatrolPoint{i + 1} {ComponentOwner.name}")
        {
          transform =
          {
            position = randomPoint
          }
        };

        array[i] = pointObject.transform;
      }

      return array;
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
      Vector3 randomPoint = NavMeshAgent.transform.position
        + new Vector3(Random.Range(-PATROL_POINT_RANGE, PATROL_POINT_RANGE), 0f, Random.Range(-PATROL_POINT_RANGE, PATROL_POINT_RANGE));

      NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, PATROL_POINT_RANGE, NavMesh.AllAreas);

      return hit.position;
    }
    
    private void Patrol()
    {
      if (NavMeshAgent.remainingDistance < DESTINATION_THRESHOLD && !NavMeshAgent.pathPending)
      {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
        SetDestinationToNextPatrolPoint();
      }
    }

    private void SetDestinationToNextPatrolPoint()
    {
      if (_patrolPoints != null && _patrolPoints.Length > 0)
      {
        Vector3 nextDestination = _patrolPoints[_currentPatrolIndex].position;
        
        if (Vector3.Distance(NavMeshAgent.destination, nextDestination) > DESTINATION_THRESHOLD)
        {
          NavMeshAgent.SetDestination(nextDestination);
        } else
        {
          _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
          SetDestinationToNextPatrolPoint();
        }
      }
    }
  }
}