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
    
    [SerializeField] 
    private int _health;

    public float Speed => _speed;
    public float RotationSpeed => _rotationSpeed;
    public int Health => _health;

    public Type Type => GetType();
  }
}