using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField]
    private int _soulAmount;

    [SerializeField]
    private Collider2D _collider2D;

    [SerializeField]
    private GameStateChannel _gsc;
    [SerializeField]
    private AudioChannel _guiAudio;

    [SerializeField]
    private Orbit orbitController;
    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private AnimationCurve _magnetSpeedCurve;

    [SerializeField]
    private AnimationCurve _magnetSpreadCurve;

    [Header("Advanced settings")]
    [SerializeField]
    private float _startRotationSpeed = 0.2f;
    [SerializeField]
    private float _maxRotationSpeed = 10f;
    [SerializeField]
    private float _magnetDuration = 5f;

    //[SerializeField]
    //private Transform point1;
    //[SerializeField]
    //private Transform point2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var entity = other.GetComponent<Player>();
        if (entity != null)
        {
            AbsorbSoul(entity.GetCenterPoint());
        }
    }

    private void AbsorbSoul(Transform playerPoint)
    {
        InitOrbit(playerPoint);
        PlayMagnet();
        _audioController.Stop("Idle");
        _audioController.Play("Absorb");
    }

    public void Init(int amount)
    {
        _soulAmount = amount;
        HandleHitStart();
    }

    void HandleHitStart()
    {
        Collider2D[] hitPLayer = Physics2D.OverlapBoxAll(_collider2D.bounds.center, _collider2D.bounds.size, 0, LayerMask.NameToLayer("Player"));

        if(hitPLayer.Length > 0)
        {
            InitOrbit(hitPLayer[0].gameObject.GetComponent<Player>().GetCenterPoint());
            PlayMagnet();
        }
    }


    private void InitOrbit(Transform playerPoint)
    {
        var diff = playerPoint.position - transform.position;
        var distance = Mathf.Abs(diff.x);
        orbitController.CenterPoint = playerPoint;


        orbitController.StartRadAngle = AngleUtils.GetRadAngle(playerPoint.position, transform.position);
        orbitController.SpreadX = distance;
        orbitController.SpreadY = distance/2;
        orbitController.RotationSpeed = _startRotationSpeed;
        orbitController.Start(null);
    }

    private void PlayMagnet()
    {
        StartCoroutine(Lerp());
    }

    IEnumerator Lerp()
    {
        var startXValue = orbitController.SpreadX;
        var startYValue = orbitController.SpreadY;
        var startSpeed = _startRotationSpeed;
        float timeElapsed = 0;
        while (timeElapsed < _magnetDuration)
        {
            var time = timeElapsed / _magnetDuration;
            var spreadMagnitude = _magnetSpreadCurve.Evaluate(time);
            var speedMagnitude = _magnetSpeedCurve.Evaluate(time);
            var x = Mathf.Lerp(startXValue, 0, spreadMagnitude);
            var y = Mathf.Lerp(startYValue, 0, spreadMagnitude);
            var speed = Mathf.Lerp(startSpeed, _maxRotationSpeed, speedMagnitude);
            orbitController.UpdateOrbiting(x, y, speed);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
        orbitController.UpdateOrbiting(0, 0, _maxRotationSpeed);
        HandleSoulAbsorbtion();
    }


    private void HandleSoulAbsorbtion()
    {
        _guiAudio.RaiseAudioRequest(new AudioEvent("USCollected"));
        _gsc.RaiseUnfortunateSoulAddition(_soulAmount);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(point1.position, 0.05f);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(point2.position, 0.05f);


        //Vector2 refPoint = point1.position + Vector3.right;
        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(point1.position, refPoint);

        //#if UNITY_EDITOR
        //UnityEditor.Handles.color = Color.black;
        //UnityEditor.Handles.Label(point2.position, "Point A " + GetAngle(point1.position, point2.position).ToString());
        //#endif
    }
}
