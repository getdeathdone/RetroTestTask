using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BattleCard : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI _attackerName;
  [SerializeField]
  private TextMeshProUGUI _defenderName;
  [SerializeField]
  private TextMeshProUGUI _damageTMP;
  [SerializeField]
  private Image _image;
  [SerializeField]
  private Sprite _killSprite;
  [SerializeField]
  private Sprite _attackSprite;
  
  public void InitCard (string attackerName,string defender,int damage, bool isKill)
  {
    _attackerName.text = attackerName;
    _defenderName.text = defender;
    _damageTMP.text = damage.ToString();

    _image.sprite = isKill ? _killSprite : _attackSprite;
  }
}