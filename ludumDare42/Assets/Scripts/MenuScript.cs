using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	private bool MouseIn = false;
	private bool MouseClick = false;

	void OnMouseEnter()
	{
		MouseIn = true;
	}

	void OnMouseLeave()
	{
		MouseIn = false;
	}

	void OnMouseDown()
	{
		MouseClick = true;
	}

	void OnMouseUp()
	{
		MouseClick = false;
	}

	void Update () 
	{
		if (MouseIn && MouseClick)
		{
			SceneManager.LoadScene("Week");
		}
	}
}
