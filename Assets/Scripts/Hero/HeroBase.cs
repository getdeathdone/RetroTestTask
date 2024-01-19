using System;
using System.Collections.Generic;
using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using DefaultNamespace.Manager;
using DefaultNamespace.ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace.Hero
{
  public abstract class HeroBase : MonoBehaviour, IInitialize
  {
    public event Action<DamageInfo> OnDeath;
    
    private readonly List<ComponentBase> _componentsMap = new List<ComponentBase>();
    private readonly Dictionary<Type, IUpdate> _updates = new Dictionary<Type, IUpdate>();
    private readonly Dictionary<Type, IFixedUpdate> _fixedUpdates = new Dictionary<Type, IFixedUpdate>();

    [SerializeField]
    private HeroType _type;
    
    private HeroData _heroData;
    private AreaManager _areaManager;

    private bool _isAlive;
    private bool _isActive;

    public bool IsAlive => _isAlive;
    public bool IsActive => _isActive;

    public HeroData HeroData => _heroData;
    public AreaManager AreaManager => _areaManager;
    public HeroType Type => _type;
    public virtual HeroSide Side => HeroSide.None;

    public virtual void SetInject(List<IInject> injects)
    {
      _heroData = SetInject<HeroData>(injects);
      _areaManager = SetInject<AreaManager>(injects);
    }

    public void Initialize()
    {
      foreach (var component in _componentsMap)
      {
        component.InitializeComponent(this);
        component.Initialize();

        if (component is IUpdate update)
        {
          _updates.TryAdd(update.GetType(), update);
        }

        if (component is IFixedUpdate fixedUpdate)
        {
          _fixedUpdates.Add(fixedUpdate.GetType(), fixedUpdate);
        }
      }

      _isAlive = true;
      IsInitialized = true;
    }

    private void Update()
    {
      if (!IsInitialized || !IsActive)
      {
        return;
      }
      
      foreach (var VARIABLE in _updates)
      {
        VARIABLE.Value.Update();
      }
    }

    private void FixedUpdate()
    {
      if (!IsInitialized || !IsActive)
      {
        return;
      }

      foreach (var VARIABLE in _fixedUpdates)
      {
        VARIABLE.Value.FixedUpdate();
      }
    }

    public void Death(DamageInfo damageInfo)
    {
      _isAlive = false;
      OnDeath?.Invoke(damageInfo);
    }
    
    public HeroBase SetActive (bool status)
    {
      _isActive = status;

      return this;
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

    protected T SetInject<T>(List<IInject> injects) where T: IInject
    {
      return (T)injects.Find(c => c.Type == typeof(T));
    }

    public bool IsInitialized
    {
      get;
      private set;
    }
  }
}