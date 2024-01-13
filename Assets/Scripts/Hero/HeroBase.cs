using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Handler;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using UnityEngine;

namespace DefaultNamespace.Hero
{
  public class HeroBase : MonoBehaviour, IInitialize
  {
    private readonly List<ComponentBase> _componentsMap = new List<ComponentBase>();
    private readonly List<IUpdate> _updates = new List<IUpdate>();
    private readonly List<IFixedUpdate> _fixedUpdates = new List<IFixedUpdate>();
    
    [SerializeField]
    private float _speed;
    [SerializeField] 
    private float _rotationSpeed;
    
    [SerializeField]
    private Rigidbody _rigidbody;
    
    private AreaManager _areaManager;
    private InputHandler _inputHandler;

    public AreaManager AreaManager => _areaManager;
    public InputHandler InputHandler => _inputHandler;
    public Rigidbody Rigidbody => _rigidbody;
    
    public float Speed => _speed;
    public float RotationSpeed => _rotationSpeed;
    
    public void SetInject(
      AreaManager areaManager, 
      InputHandler inputHandler)
    {
      _areaManager = areaManager;
      _inputHandler = inputHandler;
    }

    public void Initialize()
    {
      foreach (var VARIABLE in _componentsMap)
      {
        VARIABLE.InitializeComponent(this);
        VARIABLE.Initialize();

        if (VARIABLE is IUpdate update)
        {
          _updates.Add(update);
        }
        
        if (VARIABLE is IFixedUpdate fixedUpdate)
        {
          _fixedUpdates.Add(fixedUpdate);
        }
      }
      
      IsInitialized = true;
    }

    private void Update()
    {
      if (!IsInitialized)
      {
        return;
      }
      
      foreach (var VARIABLE in _updates)
      {
        VARIABLE.Update();
      }
    }

    private void FixedUpdate()
    {
      if (!IsInitialized)
      {
        return;
      }

      foreach (var VARIABLE in _fixedUpdates)
      {
        VARIABLE.FixedUpdate();
      }
    }

    public T GetAttachedComponent<T>() where T : ComponentBase
    {
      Type type = typeof(T);

      var find = _componentsMap.Find(c => c.GetType() == type);
      
      if (find == null)
      {
        Debug.LogWarning($"The {type} component isn't attached to the character!");

        return default;
      }
      
      return (T)find;
    }

    public HeroBase AddComponent<T> () where T : ComponentBase, new()
    {
      T component = new T();
      _componentsMap.Add(component);
      return this;
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}