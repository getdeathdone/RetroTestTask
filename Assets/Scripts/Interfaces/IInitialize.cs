namespace DefaultNamespace.Interfaces
{
  public interface IInitialize
  {
    public void Initialize ();
    
    public bool IsInitialized { get; }
  }
}