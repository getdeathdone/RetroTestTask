using DefaultNamespace.Hero;
using DefaultNamespace.Interfaces;

namespace DefaultNamespace.Component
{
  public abstract class ComponentBase : object, IInitialize
  {
    public HeroBase ComponentOwner
    {
      get;
      private set;
    }

    public void InitializeComponent (HeroBase heroBase)
    {
      ComponentOwner = heroBase;
    }

    public abstract void Initialize();

    public bool IsInitialized
    {
      get;
      protected set;
    }
  }
}