using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	public Sprite Hover;
	public Sprite UnHover;

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
		SceneManager.LoadScene("Week");
	}
}
