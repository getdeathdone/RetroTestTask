using System;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Projectile;
using UnityEngine;

namespace DefaultNamespace.ScriptableObjects
{
  [CreateAssetMenu(fileName = "HeroData", menuName = "ScriptableObject/HeroData")]
  public class HeroData : ScriptableObject, IInject
  {
    [SerializeField]
    private float _speed;
    [SerializeField] 
    private int _health;
    [SerializeField]
    private int _strengthInit;
    [SerializeField]
    private int _strengthMax;
    [SerializeField]
    private ProjectileBase _bullet;

    public float Speed => _speed;
    public int Health => _health;
    public int StrengthInit => _strengthInit;
    public int StrengthMax => _strengthMax;
    public ProjectileBase Bullet => _bullet;
    public Type Type => GetType();
  }
}