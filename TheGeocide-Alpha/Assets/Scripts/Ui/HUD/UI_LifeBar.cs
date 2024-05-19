using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_LifeBar : MonoBehaviour
{

    [SerializeField]
    private GameStateChannel _gameStateChannel;
    [SerializeField]
    private Image _hpBar;
    [SerializeField]
    private Image _dmgBar;
    [SerializeField]
    private Color _dmgColor;
    [SerializeField]
    private Color _healColor;

    private int _currentHealth;
    private int _maxHealth;
    private float _stabilizationTime = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        _gameStateChannel.OnHealthChanged += OnHealthChanged;
        _gameStateChannel.OnHealthPointAnswered += OnHealthPointAnswered;
    }

    private void OnDestroy()
    {
        _gameStateChannel.OnHealthChanged -= OnHealthChanged;
        _gameStateChannel.OnHealthPointAnswered -= OnHealthPointAnswered;
    }

    private void Start()
    {
        _gameStateChannel.RaisedHealthPointRequest();
    }
    private void OnHealthPointAnswered(int maxHp, int currenthp)
    {
        _currentHealth = currenthp;
        _maxHealth = maxHp;

        _hpBar.fillAmount = _currentHealth / maxHp;
        _dmgBar.fillAmount = _currentHealth / maxHp;
    }

    private void OnHealthChanged(HealthEvent hpEvt)
    {
        _currentHealth += hpEvt.Difference;
        var barRatio = (float) _currentHealth / _maxHealth;

        if (hpEvt.IsHpGain())
        {
            _HandleHealUp(barRatio);
            return;
        }

        if (hpEvt.IsHpLoss())
        {
            _HandleDamage(barRatio);
            return;
        }
    }

    private void _HandleDamage(float barRatio)
    {
        _dmgBar.color = _dmgColor;
        _hpBar.fillAmount = barRatio;
        StartCoroutine(StabilizeDmgBar(barRatio));
    }

    private void _HandleHealUp(float barRatio)
    {
        _dmgBar.color = _healColor;
        _dmgBar.fillAmount = barRatio;
        StartCoroutine(StabilizeHpBar(barRatio));
    }

    private IEnumerator StabilizeDmgBar(float barRatio)
    {
        yield return new WaitForSeconds(_stabilizationTime);
        _dmgBar.fillAmount = barRatio;
    }


    private IEnumerator StabilizeHpBar(float barRatio)
    {
        yield return new WaitForSeconds(_stabilizationTime);
        _hpBar.fillAmount = barRatio;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
