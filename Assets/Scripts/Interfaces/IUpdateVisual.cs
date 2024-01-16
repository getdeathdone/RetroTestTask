using System;

namespace DefaultNamespace.Interfaces
{
  public interface IUpdateVisual
  {
    public event Action<float> OnUpdateVisual;
  }
}