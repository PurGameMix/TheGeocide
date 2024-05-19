using Assets.Data.Enemy;
using System;
using UnityEngine;

public class DamagePopupManager : MonoBehaviour {


    [SerializeField]
    private EnemyStateChannel _enemyChannel;

    [SerializeField]
    private DamagePopup _damagePopup;

    private void Awake()
    {
        _enemyChannel.OnDamageTaken += OnDamageTaken;
    }

    private void OnDestroy()
    {
        _enemyChannel.OnDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(EnemyDamageTakenEvent dmgTakenEvt)
    {
        Create(dmgTakenEvt.position, dmgTakenEvt.Damage, dmgTakenEvt.IsCriticalStrike);
    }

    // Create a Damage Popup
    public DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit) {
        var popup = Instantiate(_damagePopup, position, Quaternion.identity);
        Transform damagePopupTransform = popup.transform;

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }
}
