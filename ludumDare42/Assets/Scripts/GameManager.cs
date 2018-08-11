using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public int MaxLevel = 10;
	private int CurrentLevel;
	private int LevelHazardChance;
	private bool IsHazard;
	private int NumberToGenerate;
	private int ClosestDivFive;
	private int[] day;
	// private int index;

	private int[] Math;
	private int[] Lit;
	private int[] Fra;
	private int[] Hist;
	private int[] Phys;
	private int[] Inf;
	private int[] Chem;
	private int[] Geo;

	void SetUpLevel (int level)
	{
		ResetSetUpVariables();
		DetermineTestsAndHazards(level);
		DetermineDays(NumberToGenerate);
		DetermineColour(NumberToGenerate);
	}

	void DetermineColour (int number)
	{
		//kisorsolod azt h melyik tantárgy szépen végigmész a listákon s egy a nyolchoz vagy valami okos súlyozás idk
	}

	void DetermineDays (int number)
	{
		if (number % 2 == 1)
		{
			if (((number + 1) / 2) % 5 == 0)
			{
				ClosestDivFive = (number + 1) / 2;
			}
			else
			{
				ClosestDivFive = ((number + 1) / 2) + (5 - (((number + 1) / 2) % 5));
			}
		}
		else
		{
			if ((number / 2) % 5 == 0)
			{
				ClosestDivFive = number / 2;
			}
			else
			{
				ClosestDivFive = (number / 2) + (5 - ((number / 2) % 5));
			}
		}

		for (int i = 0; i < 5; i++)
		{
			day[i] += ClosestDivFive / 5;
		}

		number -= ClosestDivFive;

		for (int i = 0; i < number; i++)
		{
			// index = Random.Range(1, 5);
			day[Random.Range(1, 5)]++;
		}
	}

	void ResetSetUpVariables ()
	{
		for (int i = 0; i < 5; i++)
		{
			day[i] = 0;
		}
		IsHazard = false;
	}

	void DetermineTestsAndHazards (int level)
	{
		if (level >= 6)
		{
			LevelHazardChance = Random.Range(0, 5);
			if ((level - LevelHazardChance) >= 5)
			{
				IsHazard = true;
			}

			NumberToGenerate = Random.Range((level + 9) * 2, (level + 10) *2);
		}

		if (level == 1)
		{
			NumberToGenerate = Random.Range(5, 7);
		}

		if (level == 2)
		{
			NumberToGenerate = Random.Range(8, 10);
		}

		if (level == 3)
		{
			NumberToGenerate = Random.Range(11, 14);
		}

		if (level == 4)
		{
			NumberToGenerate = Random.Range(17, 20);
		}

		if (level == 5)
		{
			NumberToGenerate = Random.Range(23, 29);
		}
	}

	void Start () 
	{
		day = new int[5];

		CurrentLevel = 1;
		SetUpLevel(CurrentLevel);
	}

	void Update ()
	{

	}
}
