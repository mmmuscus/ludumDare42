using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    private bool dragging = false;
    private float distance;

	public float StartXPosition = -0f;
	public float StartYPosition = 0f;
	public int SubjectType;

	private GameObject[] OFB;
	private Collider2D[] OFBC;
	private GameObject[] OFP;
	private Collider2D[] OFPC;
	private PolygonCollider2D ThisCollider;
	private Rigidbody2D ThisRigidbody;
	
	public void SetBackToSpawnPosition()
	{
		transform.position = new Vector3 (StartXPosition, StartYPosition, 0f);
		ThisRigidbody.isKinematic = true;
		SetBackToNormalStance();
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}

	public void SetBackToNormalStance()
	{
		ThisRigidbody.velocity = new Vector2 (0f, 0f);
		ThisRigidbody.freezeRotation = true;
		ThisRigidbody.freezeRotation = false;
	}

	void Start()
	{
		ThisCollider = this.GetComponent<PolygonCollider2D>();
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

		SetColour();

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
        }
    }

	void SetColour ()
	{
		if (SubjectType == 1)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(1f, .125f, 0f, 1f);
		}

		if (SubjectType == 2)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(.537f, .125f, .721f, 1f);
		}

		if (SubjectType == 3)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(0f, .125f, .721f, 1f);
		}

		if (SubjectType == 4)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(0f, .8f, 0f, 1f);
		}

		if (SubjectType == 5)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(1f, .486f, 0f, 1f);
		}

		if (SubjectType == 6)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(.004f, .686f, .784f, 1f);
		}

		if (SubjectType == 7)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(1f, .98f, 0f, 1f);
		}

		if (SubjectType == 0)
		{
			this.GetComponent<SpriteRenderer>().color = new Color(.964f, .129f, .576f, 1f);
		}
	}

	public bool IsMemorized ()
	{
		if (transform.position.x == StartXPosition && transform.position.y == StartYPosition)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}