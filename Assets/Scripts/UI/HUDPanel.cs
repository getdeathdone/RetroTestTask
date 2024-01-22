using DefaultNamespace.Component;
using DefaultNamespace.Interfaces;
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