using Assets.Scripts.PathBerserker2dExtentions;
using UnityEngine;

public class EnemyFrozenController : MonoBehaviour
{

    [SerializeField]
    internal EnemyStateChannel _aiStateChannel;

    [SerializeField]
    internal EnemyIAMouvement _mouvementController;

    [SerializeField]
    internal AnimationCurve _frozenCurve;

    [SerializeField]
    internal Animator _animator;

    [SerializeField]
    internal SpriteRenderer _spriteRenderer;

    [SerializeField]
    internal Color _color;

    [SerializeField]
    internal Transform _basePoint;

    [SerializeField]
    GameObject frozenFXPrefab;

    private FrozenEffect _currentFrozenInstance;

    private void Awake()
    {
        _aiStateChannel.OnFrozenTickChanged += OnFrozenTickChanged;
    }

    private void OnDestroy()
    {
        _aiStateChannel.OnFrozenTickChanged -= OnFrozenTickChanged;
    }


    private void OnFrozenTickChanged(FrozenStateEvent frozenEvt)
    {
        if (name != frozenEvt.EnemyId)
        {
            return;
        }

        var frozenCoef = 0f;

        if (frozenEvt.FrozenTick != 0)
        {
            frozenCoef = (float)frozenEvt.FrozenTick / frozenEvt.FrozenMax;
       
        }

        HandleFrost(frozenCoef);
    }

    private void HandleFrost(float frozenCoef)
    {

        //HandleColor && FrostEffect
        if(frozenCoef == 0)
        {
            _spriteRenderer.color = Color.white;

            _currentFrozenInstance.Destroy();
            _currentFrozenInstance = null;
        }
        else
        {
            if(_currentFrozenInstance == null)
            {
                _currentFrozenInstance = Instantiate(frozenFXPrefab, _basePoint).GetComponent<FrozenEffect>();
                _currentFrozenInstance.Init(frozenCoef);
            }

            _spriteRenderer.color = _color;
        }

        //Handle frost effect
        if(_currentFrozenInstance != null)
        {
            _currentFrozenInstance.SetFrostRatio(frozenCoef);
        }

        // slowCoef => Closest to 1 lesser you are frost
        var slowRatio = _frozenCurve.Evaluate(frozenCoef);

        //Handle animation speed
        _animator.SetFloat("AnimSpeed", slowRatio);

        //Handle move speed
        _mouvementController.SetSlowRatio(slowRatio);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
