using UnityEngine;

namespace DefaultNamespace.Component
{
  public class EnemyMovement : Movement
  {
    protected override void OutOfBounds()
    {
      Vector3 normalizedDirection = (_transform.position - CenterPoint).normalized;
      Vector3 newPosition = CenterPoint + normalizedDirection * Radius;
        
      newPosition.x = Mathf.Clamp(newPosition.x, CenterPoint.x - Radius, CenterPoint.x + Radius);
      newPosition.z = Mathf.Clamp(newPosition.z, CenterPoint.z - Radius, CenterPoint.z + Radius);
        
      _transform.position = newPosition;
    }
  }
}