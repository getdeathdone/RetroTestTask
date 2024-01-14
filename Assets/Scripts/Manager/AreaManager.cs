using System;
using DefaultNamespace.Interfaces;
using UnityEditor;
using UnityEngine;

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      Handles.color = Color.yellow;
      Handles.DrawWireDisc(CenterPoint, Vector3.up, Radius);
    }
#endif
  }
}