using System.Collections;
using UnityEngine;

public class GroundedFire : MonoBehaviour
{

    public int Damage = 20;

    public float CountDown = 1;
    [SerializeField]
    private AudioController _audioController;

    private ICanBeDamaged _onFireObject = null;

    private float _LastBurn=0;
    void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.isTrigger)
        {
            return;
        }

        _onFireObject = Hit.GetComponent<ICanBeDamaged>();
    }

    void OnTriggerExit2D(Collider2D Hit)
    {
        if (Hit.isTrigger)
        {
            return;
        }
        var player = Hit.GetComponent<ICanBeDamaged>();
        if (player!= null && _onFireObject != null)
        {
            _onFireObject = null;
        }
    }

    private void Start()
    {
        StartCoroutine(PlayAmbient());
    }

    private void Update()
    {
        _LastBurn -= Time.deltaTime;
        StartCoroutine(Burn());
    }


    private IEnumerator PlayAmbient()
    {
        var random = Random.Range(0, 20) / 10;

        yield return new WaitForSeconds(random);

        _audioController.Play("Ambient");
    }

    private IEnumerator Burn()
    {
        if (_onFireObject == null || _LastBurn > 0)
        {
            yield break;
        }
        _LastBurn = CountDown;
        _audioController.Play("Burn");     
        _onFireObject.TakeDamage(Damage, HealthEffectorType.trap);
        yield return new WaitForSeconds(CountDown);
    }

}
