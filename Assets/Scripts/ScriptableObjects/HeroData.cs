using System;
using DefaultNamespace.Interfaces;
using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
  [CreateAssetMenu(fileName = "HeroData", menuName = "ScriptableObject/HeroData")]
  public class HeroData : ScriptableObject, IInject
  {
    [SerializeField]
    private float _speed;
    [SerializeField] 
    private float _rotationSpeed;

    public float Speed => _speed;
    public float RotationSpeed => _rotationSpeed;
    public Type Type => GetType();
  }
}