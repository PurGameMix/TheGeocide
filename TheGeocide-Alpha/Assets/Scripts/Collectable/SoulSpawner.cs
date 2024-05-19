using UnityEngine;

public class SoulSpawner : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Soul soulPrefab;

    [SerializeField]
    private Transform _playerPoint;

    private int _currentSoulAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(int soulAmount)
    {
        _animator.Play("SoulSpawn");
        _currentSoulAmount = soulAmount;
    }

    public void OnSpawnAnimationCompleted()
    {
        var instance = Instantiate(soulPrefab, transform.position, transform.rotation);
        instance.Init(_currentSoulAmount);
        _currentSoulAmount = 0;
    }
}
