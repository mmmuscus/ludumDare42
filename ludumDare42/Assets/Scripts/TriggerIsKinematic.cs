using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerIsKinematic : MonoBehaviour {

	private GameObject[] OFB;
	private Collider2D[] OFBC;
	private GameObject[] OFP;
	private Collider2D[] OFPC;
	private Collider2D ThisCollider;
	private Rigidbody2D ThisRigidbody;

	private bool InOFBC = false;
	private bool InOFPC = false;

	void Awake () 
	{
		ThisCollider = this.GetComponent<Collider2D>();
		ThisRigidbody = this.GetComponent<Rigidbody2D>();

		OFB = GameObject.FindGameObjectsWithTag("OFB");
		OFP = GameObject.FindGameObjectsWithTag("OFP");

		OFBC = new Collider2D[OFB.Length];
		OFPC = new Collider2D[OFP.Length];

		for (int i = 0; i < OFB.Length; i++)
		{
			OFBC[i] = OFB[i].GetComponent<Collider2D>();
		}

		for (int i = 0; i < OFP.Length; i++)
		{
			OFPC[i] = OFP[i].GetComponent<Collider2D>();
		}	
	}
	
	void Update () 
	{
		if (!ThisRigidbody.isKinematic)
		{
			InOFBC = false;
			InOFPC = false;

			foreach (Collider2D other in OFBC)
			{
				if (ThisCollider.IsTouching(other))
				{
					InOFBC = true;
				}
			}

			foreach (Collider2D other in OFPC)
			{
				if (ThisCollider.IsTouching(other))
				{
					InOFPC = true;
				}
			}

			if (!InOFBC && !InOFPC)
			{
				ThisRigidbody.isKinematic = true;
				ThisRigidbody.velocity = new Vector2 (0f, 0f);
				ThisRigidbody.freezeRotation = true;
			}
		}
	}
}
