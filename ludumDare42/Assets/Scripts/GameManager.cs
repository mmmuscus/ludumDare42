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
	private int[,] Tests;
	private int[] max; 
	private bool EndOfNeeded = false;
	private float Column;
	private int RandomIndex;

	private int[] Evaluated;

	public float WeekendTime = 15f;
	public float WeekTime = 5f;
	private float CurrTime;

	public GameObject[] LessonList;
	private GameObject LessonHandle;

	private GameObject[] Lessons;

	void SetUpLevel (int level)
	{
		ResetSetUpVariables();
		DetermineTestsAndHazards(level);
		DetermineDays(NumberToGenerate);
		SpawnWeek();
	}

	// void EndOfDay ()
	// {
	// 	for (int i = 0; i < 8; i++)
	// 	{
	// 		Evaluated[i] = 0;
	// 	}

	// 	Lessons = GameObject.FindGameObjectsWithTag("Subject");
	// 	foreach (GameObject subj in Lessons)
	// 	{
	// 		if (subj.transform.position != new Vector3(subj.GetComponent<DragScript>().StartXPosition, subj.GetComponent<DragScript>().StartYPosition, subj.transform.position.z)
	// 		{
	// 			Evaluated[subj.GetComponent<DragScript>().SubjectType]++;
	// 		}
	// 	}
	// }

	void SpawnWeek ()
	{
		for (int i = 1; i < 5; i++)
		{
			for (int k = 0; k < day[i]; k++)
			{
				Tests[i,Random.Range(0, 7)]++;
			}
		}

		for (int k = 0; k < 8; k++)
		{
			max[k] = 0;
			for (int i = 0; i < 5; i++)
			{
				if (Tests[i,k] > max[k])
				{
					max[k] = Tests[i,k];
				}
			}
		}

		for (int c = 0; c < 2; c++)
		{
			if (c == 0)
			{
				Column = -7f;
			}
			else
			{
				Column = -4f;
			}
			Debug.Log (Column);

			for (int i = 0; i < 15; i++)
			{
				LessonHandle = Instantiate(LessonList[Random.Range(0, 23)], new Vector3(Column, (3.5f - (i * 0.5f)), 0f), Quaternion.identity);
				LessonHandle.GetComponent<DragScript>().StartXPosition = Column;
				LessonHandle.GetComponent<DragScript>().StartYPosition = 3.5f - (i * 0.5f);
				LessonHandle.GetComponent<SpriteRenderer>().sortingOrder = i;

				if (EndOfNeeded)
				{
					LessonHandle.GetComponent<DragScript>().SubjectType = Random.Range(0, 7);
				}

				EndOfNeeded = true;
				for (int k = 0; k < 8; k++)
				{
					if (max[k] > 0)
					{
						EndOfNeeded = false;
					}
				}

				if(!EndOfNeeded)
				{
					RandomIndex = Random.Range(0, 7);
					while (max[RandomIndex] != 0)
					{
						RandomIndex++;

						if (RandomIndex == 8)
						{
							RandomIndex = 0;
						}
					}

					LessonHandle.GetComponent<DragScript>().SubjectType = RandomIndex;
				}
			}
		}
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

		for (int i = 0; i < 5; i++)
		{
			for (int k = 0; k < 8; k++)
			{
				Tests[i,k] = 0;
			}
		}

		for (int i = 0; i < 8; i++)
		{
			max[i] = 0;
		}
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
		}

		if (level == 1)
		{
			NumberToGenerate = Random.Range(5, 6);
		}

		if (level == 2)
		{
			NumberToGenerate = Random.Range(7, 10);
		}

		if (level == 3)
		{
			NumberToGenerate = Random.Range(11, 14);
		}

		if (level == 4)
		{
			NumberToGenerate = Random.Range(15, 17);
		}

		if (level >= 5)
		{
			NumberToGenerate = Random.Range((level + 4) * 2, ((level + 4) * 2) + 1);
		}
	}

	void Start () 
	{
		day = new int[5];
		Tests = new int[5,8];
		max = new int[8];
		Evaluated = new int[8];

		CurrentLevel = 1;
		SetUpLevel(CurrentLevel);
	}

	void Update ()
	{

	}
}
