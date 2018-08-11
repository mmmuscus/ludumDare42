using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    private bool dragging = false;
    private float distance;

	public float StartXPosition = -5f;
	public float StartYPosition = 0f;

	private GameObject[] OFB;
	private Collider2D[] OFBC;
	private GameObject[] OFP;
	private Collider2D[] OFPC;
	private Collider2D ThisCollider;
	private Rigidbody2D ThisRigidbody;

	public GameObject Indicator;
	private GameObject IndicatorHandle;
	private bool InstantAlive = false;
	private bool InOFBC = false;
	private bool InOFPC = false;
	private float LastX;
	private float LastY;

	public void SetBackToSpawnPosition()
	{
		transform.position = new Vector3 (StartXPosition, StartYPosition, 0f);
		ThisRigidbody.isKinematic = true;
		SetBackToNormalStance();
	}

	public void SetBackToNormalStance()
	{
		ThisRigidbody.velocity = new Vector2 (0f, 0f);
		ThisRigidbody.freezeRotation = true;
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		ThisRigidbody.freezeRotation = false;
	}

	void Start()
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

		SetBackToSpawnPosition();
	}

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
		dragging = true;
		ThisCollider.isTrigger = true;

		SetBackToNormalStance();
    }
 
    void OnMouseUp()
    {
        dragging = false;

		if (InstantAlive)
		{
			Destroy(IndicatorHandle);
			InstantAlive = false;
		}

		foreach (Collider2D other in OFBC)
		{
			if (ThisCollider.IsTouching(other))
			{
				SetBackToSpawnPosition();
			}
		}

		if (transform.position != new Vector3 (StartXPosition, StartYPosition, 0f))
		{
			ThisRigidbody.isKinematic = false;
			ThisRigidbody.velocity = new Vector2 (0f, 0f);
		}

		ThisCollider.isTrigger = false;
    }
 
    void Update()
    {
        if (dragging)
        {
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
   	        Vector3 rayPoint = ray.GetPoint(distance);
	        transform.position = rayPoint;

			//Indicator stuff
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

			if (!InOFBC && !InOFPC && InstantAlive)
			{
				Destroy(IndicatorHandle);
				InstantAlive = false;
			}

			if (InOFBC && InstantAlive)
			{
				Destroy(IndicatorHandle);
				InstantAlive = false;
			}

			if (InOFPC && !InOFBC)
			{
				if (LastX != transform.position.x || LastY != transform.position.y)
				{
					if (InstantAlive)
					{
						Destroy(IndicatorHandle);
					}
					IndicatorHandle = Instantiate(Indicator, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
					InstantAlive = true;
				}
			}
        }

		LastX = transform.position.x;
		LastY = transform.position.y;
    }
}