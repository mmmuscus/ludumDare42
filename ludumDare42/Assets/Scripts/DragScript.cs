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
	private Collider2D ThisCollider;
	private Rigidbody2D ThisRigidbody;

	public void SetBackToSpawnPosition()
	{
		transform.position = new Vector3 (StartXPosition, StartYPosition, 0f);
		ThisRigidbody.isKinematic = true;
		ThisRigidbody.velocity = new Vector2 (0f, 0f);
	}

	void Start()
	{
		ThisCollider = this.GetComponent<Collider2D>();
		ThisRigidbody = this.GetComponent<Rigidbody2D>();

		if (OFB == null)
		{
			OFB = GameObject.FindGameObjectsWithTag("OFB");
		}

		OFBC = new Collider2D[OFB.Length];

		for (int i = 0; i < OFB.Length; i++)
		{
			OFBC[i] = OFB[i].GetComponent<Collider2D>();
		}

		SetBackToSpawnPosition();
	}

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
		dragging = true;
		ThisCollider.isTrigger = true;
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
}
