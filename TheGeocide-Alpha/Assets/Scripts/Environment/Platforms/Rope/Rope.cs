using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rope : MonoBehaviour
{

	public Rigidbody2D hook;
	public GameObject LinksContainer;
	public bool IsBreak;
	//public HingeJoint2D weight;
	[SerializeField]
	private AudioController _audioController;

	private int _totalLinks;

	void Start()
	{
		//GenerateRope();
	}

	public Rigidbody2D GenerateRope(RopeLink linkPrefab, int links, Rigidbody2D baseHook = null)
	{
		var acId = _audioController.GetId();

		if(baseHook != null)
        {
			hook = baseHook;
		}
		Rigidbody2D previousRB = hook;

		_totalLinks = links;
		for (int i = 0; i < links; i++)
		{
			RopeLink link = Instantiate(linkPrefab, LinksContainer.transform);
			link.tag = "Breakable";
			link.SetAudioManagerId(acId);
			link.transform.position = new Vector2(link.transform.position.x, link.transform.position.y - 0.5f * i);
			HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
			joint.connectedBody = previousRB;
			if(i == 0)
            {
				joint.connectedAnchor = new Vector2(0, 0);
            }
			if (i <= links - 1)
			{
				previousRB = link.GetComponent<Rigidbody2D>();
			}

		}

		return previousRB;
	}

    private void Update()
    {
        if (IsBreak)
        {
			return;
        }

		var nbLinks = LinksContainer.transform.childCount;
		IsBreak = nbLinks < _totalLinks;
	}
}
