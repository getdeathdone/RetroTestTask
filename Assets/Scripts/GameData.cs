using UnityEngine;

namespace DefaultNamespace
{
  public static class GameData
  {
    public static string PlayerName
    {
      get => PlayerPrefs.GetString(nameof(PlayerName), null);
      set => PlayerPrefs.SetString(nameof(PlayerName), value);
    }
  }
}