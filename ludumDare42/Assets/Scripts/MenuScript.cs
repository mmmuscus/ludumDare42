using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	void OnMouseEnter()
	{
		
	}

	void OnMouseDown()
	{
		SceneManager.LoadScene("Week");
	}
}
