using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator : MonoBehaviour 
{
	public int Value = 0;
	public Sprite[] Numbers;
	private SpriteRenderer Renderer;

	void Start ()
	{
		Renderer = this.GetComponent<SpriteRenderer>();
	}
	
	void Update () 
	{
		if (Value >= 0 && Value <= 9);
		{
			Renderer.sprite = Numbers[Value];
		}
	}
}
