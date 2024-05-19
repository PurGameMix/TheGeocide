using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingRope : MonoBehaviour
{

	public Rigidbody2D hook;
	public GameObject LinksContainer;

	public HingeJoint2D weight;


	public GameObject linkPrefab;

	public int links;

	void Start()
	{
		GenerateRope();
	}

	void GenerateRope()
	{
		Rigidbody2D previousRB = hook;

		var orderIndex = links;
		for (int i = 0; i < links; i++)
		{
			GameObject link = Instantiate(linkPrefab, LinksContainer.transform);
			link.transform.position = new Vector2(link.transform.position.x, link.transform.position.y - 0.5f * i);
			HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
			joint.connectedBody = previousRB;
			link.GetComponent<SpriteRenderer>().sortingOrder = orderIndex;
			orderIndex--;
			if (i == 0)
            {
				joint.connectedAnchor = new Vector2(0, 0);
            }
			if (i < links - 1)
			{
				previousRB = link.GetComponent<Rigidbody2D>();
			}

			if(i == links - 1)
            {
				weight.transform.position = new Vector2(link.transform.position.x, link.transform.position.y - 0.5f * (i +1));
				weight.connectedBody = link.GetComponent<Rigidbody2D>();
			}
		}
	}

	void OnDrawGizmos()
	{

		#if UNITY_EDITOR
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(hook.transform.position, new Vector2(hook.transform.position.x, hook.transform.position.y - (links * linkPrefab.GetComponent<SpriteRenderer>().bounds.size.y)));
		#endif
	}

}
