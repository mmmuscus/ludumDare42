using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	public Sprite Hover;
	public Sprite UnHover;

	public Animator animator;

	void OnMouseEnter()
	{
		this.GetComponent<SpriteRenderer>().sprite = Hover;
		this.GetComponent<AudioSource>().Play();
	}

	void OnMouseExit()
	{
		this.GetComponent<SpriteRenderer>().sprite = UnHover;
	}

	void OnMouseDown()
	{
		animator.SetTrigger("fadeOUT");
		this.GetComponent<BoxCollider2D>().enabled = false;
		// SceneManager.LoadScene("Week");
	}
}
