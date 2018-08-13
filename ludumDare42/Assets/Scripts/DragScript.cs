using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    private bool dragging = false;
	public bool PublicDragging = false;
	private bool CanBeDragged = false;
    private float distance;

	public float StartXPosition = -0f;
	public float StartYPosition = 0f;
	public int SubjectType;

	private GameObject[] OFB;
	private Collider2D[] OFBC;
	private GameObject[] OFP;
	private Collider2D[] OFPC;
	private GameObject[] PC;
	private PolygonCollider2D[] PCC;
	private DragScript[] PCS;
	private SpriteRenderer[] PCR;
	private int PCCounter = 0;
	private PolygonCollider2D ThisCollider;
	private Rigidbody2D ThisRigidbody;
	private GameObject GM;
	
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
		ThisRigidbody.freezeRotation = false;
	}

	void Start()
	{
		ThisCollider = this.GetComponent<PolygonCollider2D>();
		ThisRigidbody = this.GetComponent<Rigidbody2D>();

		OFB = GameObject.FindGameObjectsWithTag("OFB");
		OFP = GameObject.FindGameObjectsWithTag("OFP");
		PC = GameObject.FindGameObjectsWithTag("Subject");

		OFBC = new Collider2D[OFB.Length];
		OFPC = new Collider2D[OFP.Length];
		PCC = new PolygonCollider2D[PC.Length];
		PCR = new SpriteRenderer[PC.Length];
		PCS = new DragScript[PC.Length];
		GM = GameObject.FindWithTag("GameController");

		for (int i = 0; i < OFB.Length; i++)
		{
			OFBC[i] = OFB[i].GetComponent<Collider2D>();
		}

		for (int i = 0; i < OFP.Length; i++)
		{
			OFPC[i] = OFP[i].GetComponent<Collider2D>();
		}

		for (int i = 0; i < PC.Length; i++)
		{
			PCC[i] = PC[i].GetComponent<PolygonCollider2D>();
		}

		for (int i = 0; i < PC.Length; i++)
		{
			PCR[i] = PC[i].GetComponent<SpriteRenderer>();
		}

		for (int i = 0; i < PC.Length; i++)
		{
			PCS[i] = PC[i].GetComponent<DragScript>();
		}

		SetColour();

		SetBackToSpawnPosition();
	}

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);

		dragging = true;
		PublicDragging = true;
		CanBeDragged = true;

		ThisCollider.isTrigger = true;

		SetBackToNormalStance();
    }
 
    void OnMouseUp()
    {
        dragging = false;
		PublicDragging = false;

		foreach (Collider2D other in OFBC)
		{
			if (ThisCollider.IsTouching(other))
			{
				SetBackToSpawnPosition();
			}
		}

		foreach (PolygonCollider2D other in PCC)
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
        if (dragging && CanBeDragged && !GM.GetComponent<GameManager>().isIntro)
        {
			for (int i = 0; i < PCS.Length; i++)
			{
				if (PCS[i].PublicDragging && (PCR[i].sortingOrder < this.GetComponent<SpriteRenderer>().sortingOrder))
				{
					CanBeDragged = false;
				}
			}

			if (CanBeDragged)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 rayPoint = ray.GetPoint(distance);
	    		transform.position = rayPoint;

				if (Input.GetKey(KeyCode.A)) 
				{
   			    	transform.Rotate(Vector3.forward * 3f);
				}

				if (Input.GetKey(KeyCode.D)) 
				{
   	    			transform.Rotate(Vector3.back * 3f);
   		 		}
			}
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