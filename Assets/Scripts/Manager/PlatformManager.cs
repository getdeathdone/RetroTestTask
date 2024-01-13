namespace DefaultNamespace.Manager
{
  public static class PlatformManager
  {
#if UNITY_ANDROID
    public const bool IS_MOBILE = true;
#elif UNITY_IOS
    public const bool IS_MOBILE = true;
#elif UNITY_STANDALONE
      public const bool IS_MOBILE = false;
#else
    public const bool IS_MOBILE = false;
#endif
  }
}