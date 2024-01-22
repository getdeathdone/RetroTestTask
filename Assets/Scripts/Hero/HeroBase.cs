using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using DefaultNamespace.ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace.Hero
{
  public abstract class HeroBase : MonoBehaviour, IInitialize
  {
    public event Action<DamageInfo> OnDeath;
    public event Action<bool, Collision> OnCollisionEvent;

    private readonly List<ComponentBase> _componentsMap = new List<ComponentBase>();
    private readonly Dictionary<Type, IUpdate> _updates = new Dictionary<Type, IUpdate>();
    private readonly Dictionary<Type, IFixedUpdate> _fixedUpdates = new Dictionary<Type, IFixedUpdate>();

    [SerializeField]
    private HeroType _type;
    
    private bool _isAlive;
    private bool _isActive;
    private HeroData _heroData;
    
    public bool IsAlive => _isAlive;
    public bool IsActive => _isActive;
    public HeroData HeroData => _heroData;
    public HeroType Type => _type;
    public virtual HeroSide Side => HeroSide.None;

    public virtual void SetInject(List<IInject> injects)
    {
      _heroData = SetInject<HeroData>(injects);
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

    private void OnCollisionEnter (Collision other)
    {
      OnCollisionEvent?.Invoke(true, other);
    }

    private void OnCollisionExit (Collision other)
    {
      OnCollisionEvent?.Invoke(false, other);
    }
    
    public Transform FindTarget(float DETECTION_RANGE)
    {
      Collider [] hitColliders = Physics.SphereCastAll(transform.position, DETECTION_RANGE, transform.forward).Select(hit => hit.collider).ToArray();

      foreach (Collider collider in hitColliders)
      {
        if (collider.TryGetComponent(out HeroBase heroBase))
        {
          if (Side != heroBase.Side)
          {
            return collider.transform;
          }
        }
      }

      return null;
    }

    public void Death(DamageInfo damageInfo)
    {
      _isAlive = false;
      OnDeath?.Invoke(damageInfo);
      gameObject.SetActive(false);
    }
    
    public void Death()
    {
      _isAlive = false;
      gameObject.SetActive(false);
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

    public List<T> GetInterfaceImplementations<T>()
    {
      List<T> implementations = _componentsMap
        .Where(componentBase => componentBase is T && typeof(T).IsInterface)
        .Cast<T>()
        .ToList();

      return implementations.Count > 0 ? implementations : null;
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