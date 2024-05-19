using System.Collections;
using UnityEngine;

/*Script spawning portal and displaying player*/
public class SpawningPortal : MonoBehaviour, IPortal
{
    [SerializeField]
    private Rigidbody2D _playerrb;

    [Header("Debug")]
    [SerializeField]
    //uncheck if you want test level on an other place
    private bool forcePlayerState = true;

    private AudioController _audioController;
    public GameObject PortalGfx;
    private Animator _PortalOpeningAnimator;
    public GameObject PortalOpeningGfx;

    private float _currentGravityScale; 
    private void Start()
    {
        _audioController = GetComponent<AudioController>();
        _PortalOpeningAnimator = PortalOpeningGfx.GetComponent<Animator>();

        if (forcePlayerState)
        {
            _playerrb.transform.position = transform.position;
            _playerrb.transform.localScale = new Vector2(0, 0);

            _currentGravityScale = _playerrb.gravityScale;
            _playerrb.gravityScale = 0;
        }

        OpenPortal();
    }

    public void OpenPortal()
    {
        PortalOpeningGfx.SetActive(true);
        _PortalOpeningAnimator.Play("Portal_Open");
        _audioController.Play("portal_open");
    }

    public void OnPortalOpeningCompleted()
    {
        PortalOpeningGfx.SetActive(false);
        PortalGfx.SetActive(true);
        _audioController.Play("portal_idle");

        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        _audioController.Play("portal_enter");
        yield return new WaitForSeconds(0.5f);

        //Destroy(_portalAudioListener);

        if (forcePlayerState)
        {
            _playerrb.transform.localScale = new Vector2(1, 1);
            _playerrb.gravityScale = _currentGravityScale;
        }

        StartCoroutine(Dispawn());
    }

    private IEnumerator Dispawn()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
