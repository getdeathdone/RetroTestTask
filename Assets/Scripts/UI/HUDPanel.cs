using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    [SerializeField]
    private Image _healthSlider;
    [SerializeField]
    private BattleCard _battleCard;
    [SerializeField]
    private Transform _battleCardParent;
    [SerializeField]
    private TextMeshProUGUI _playerKill;

    public void UpdatePlayerKill (int obj)
    {
        _playerKill.text = $"Player Kill {obj}";
    }

    public void SetHealth (float value)
    {
        _healthSlider.fillAmount = value;
    }

    public void SpawnBattleInfoCard (DamageInfo damageInfo, bool isDead = false)
    {
        var card = Instantiate(_battleCard, _battleCardParent);
        card.InitCard(((ComponentBase)damageInfo.DamageDealer).ComponentOwner.name, ((ComponentBase)damageInfo.Receiver).ComponentOwner.name, damageInfo.Damage, isDead);
        card.transform.SetAsFirstSibling();
    }
}