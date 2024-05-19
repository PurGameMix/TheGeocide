using UnityEngine;

public class SwingingBlade : MonoBehaviour
{
	public Quaternion RotateFrom = Quaternion.identity;
	public Quaternion RotateTo = Quaternion.identity;
	public float Speed = 0.0f;

	[SerializeField]
	private AudioController _audioController;

	private float AngleOffset = 3f;
	private bool _hasSwingIn;
	private bool _hasSwingOut;

	// Update is called once per frame
	void Update()
	{
		//Moving blade
		var currentTime = Mathf.SmoothStep(0.0f, 1.0f, Mathf.PingPong(Time.time * Speed, 1.0f));
		transform.rotation = Quaternion.Slerp(RotateFrom, RotateTo, currentTime);



		//Playing sound
		var currentAngle = transform.rotation.eulerAngles.z;
		if (RotateFrom.eulerAngles.z <= currentAngle  && currentAngle <= RotateFrom.eulerAngles.z + AngleOffset && !_hasSwingIn)
        {
			_hasSwingIn = true;
			_hasSwingOut = false;
			_audioController.Stop("Swing_Out");
			_audioController.Play("Swing_In");
			
		}

		if (RotateTo.eulerAngles.z - AngleOffset <= currentAngle && currentAngle <= RotateTo.eulerAngles.z && !_hasSwingOut)
		{
			_hasSwingOut = true;
			_hasSwingIn = false;
			_audioController.Stop("Swing_In");
			_audioController.Play("Swing_Out");
		}
	}
}
