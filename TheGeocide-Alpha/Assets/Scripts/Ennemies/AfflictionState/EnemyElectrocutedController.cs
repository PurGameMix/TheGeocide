using Assets.Scripts.PathBerserker2dExtentions;
using UnityEngine;

public class EnemyElectrocutedController : MonoBehaviour
{

    [SerializeField]
    internal EnemyStateChannel _aiStateChannel;

    [SerializeField]
    internal SpriteRenderer _spriteRenderer;

    [SerializeField]
    internal Color _color;

    [SerializeField]
    internal Transform _basePoint;

    [SerializeField]
    GameObject ElectrocutedFXPrefab;

    private ElectrocuteEffect _currentElectrocutedInstance;

    private void Awake()
    {
        _aiStateChannel.OnElectrocuteTickChanged += OnElectrocutedTickChanged;
    }

    private void OnDestroy()
    {
        _aiStateChannel.OnElectrocuteTickChanged -= OnElectrocutedTickChanged;
    }


    private void OnElectrocutedTickChanged(ElectrocuteStateEvent elecEvt)
    {
        if (name != elecEvt.EnemyId)
        {
            return;
        }

        HandleElectrocute(elecEvt.ElecTick, elecEvt.ElecMax);
    }

    private void HandleElectrocute(int tick, int max)
    {

        //HandleColor && FrostEffect
        if(tick == 0)
        {
            _spriteRenderer.color = Color.white;

            _currentElectrocutedInstance.Destroy();
            _currentElectrocutedInstance = null;
        }
        else
        {
            if(_currentElectrocutedInstance == null)
            {
                _currentElectrocutedInstance = Instantiate(ElectrocutedFXPrefab, _basePoint).GetComponent<ElectrocuteEffect>();
                _currentElectrocutedInstance.Init( (float) tick/max);
            }

            _spriteRenderer.color = _color;
        }

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
