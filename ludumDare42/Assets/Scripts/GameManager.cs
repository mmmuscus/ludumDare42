﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private int Grade;
	private int[,] ThisWeek;     //!!! thing happens or whatevz
	private int[,] AllTime;
	private int[] TemporoaryArray;
	List <int> Math1;
	List <int> Lit2;
	List <int> Fra3;
	List <int> Hist4;
	List <int> Phys5;
	List <int> Inf6;
	List <int> Chem7;
	List <int> Geo0;

	public float WeekendTime = 15f;
	public float WeekTime = 5f;
	private float CurrTime;

	private bool isGameOver = false;
	private GameObject[] SpaceLessons;

	public GameObject[] LessonList;
	private GameObject LessonHandle;

	private GameObject[] Lessons;

	void SetUpLevel (int level)
	{
		ResetSetUpVariables();
		DetermineTestsAndHazards(level);
		DetermineDays(NumberToGenerate);      //How many tests a day
		SpawnWeek();                          //What kind of tests + spawns Subject prefabs //!!! shuld also spawn week table 
	}

	void EndOfWeek ()
	{
		TemporoaryArray = Math1.ToArray();
		LoadTempIntoThisWeek (1);
		TemporoaryArray = Lit2.ToArray();
		LoadTempIntoThisWeek (2);
		TemporoaryArray = Fra3.ToArray();
		LoadTempIntoThisWeek (3);
		TemporoaryArray = Hist4.ToArray();
		LoadTempIntoThisWeek (4);
		TemporoaryArray = Phys5.ToArray();
		LoadTempIntoThisWeek (5);
		TemporoaryArray = Inf6.ToArray();
		LoadTempIntoThisWeek (6);
		TemporoaryArray = Chem7.ToArray();
		LoadTempIntoThisWeek (7);
		TemporoaryArray = Geo0.ToArray();
		LoadTempIntoThisWeek (0);

		for (int i = 0; i < 8; i++)
		{
			AllTime[i,0] += ThisWeek[i,0];
			AllTime[i,1] += ThisWeek[i,1];

			if(AllTime[i,1] / AllTime[i,0] <= 1.5f)
			{
				isGameOver = true;
			}
		}
	}

	void LoadTempIntoThisWeek (int id)
	{
		ThisWeek[id,1] = TemporoaryArray.Length;
		for (int i = 0; i < TemporoaryArray.Length; i++)
		{
			ThisWeek[id,0] += TemporoaryArray[i];
		}
	}

	void EndOfDay (int dayy)
	{
		for (int i = 0; i < 8; i++)
		{
			Evaluated[i] = 0;
		}

		Lessons = GameObject.FindGameObjectsWithTag("Subject");
		foreach (GameObject subj in Lessons)
		{
			if (subj.transform.position != new Vector3(subj.GetComponent<DragScript>().StartXPosition, subj.GetComponent<DragScript>().StartYPosition, subj.transform.position.z))
			{
				Evaluated[subj.GetComponent<DragScript>().SubjectType]++;
			}
		}

		for (int i = 0; i < 8; i++)
		{
			Grade = 0;

			if ((Evaluated[i] / Tests[dayy, i]) >= 1)
			{
				Grade = 5;
			}
			else
			{
				Grade = (int)Mathf.Round(5 * (Evaluated[i] / Tests[dayy, i]));

				if (Grade < 0)
				{
					Grade = 0;
				}
			}

			if (i == 1)
			{
				Math1.Add(Grade);
			}

			if (i == 2)
			{
				Lit2.Add(Grade);
			}

			if (i == 3)
			{
				Fra3.Add(Grade);
			}

			if (i == 4)
			{
				Hist4.Add(Grade);
			}

			if (i == 5)
			{
				Phys5.Add(Grade);
			}

			if (i == 6)
			{
				Inf6.Add(Grade);
			}

			if (i == 7)
			{
				Chem7.Add(Grade);
			}

			if (i == 0)
			{
				Geo0.Add(Grade);
			}
		}
	}

	void SpawnWeek ()
	{
		for (int i = 1; i < 5; i++)
		{
			for (int k = 0; k < day[i]; k++)
			{
				Tests[i,Random.Range(0, 8)]++;
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

		EndOfNeeded = false;

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

			for (int i = 0; i < 15; i++)
			{
				LessonHandle = Instantiate(LessonList[Random.Range(0, 23)], new Vector3(Column, (3.5f - (i * 0.5f)), 0f), Quaternion.identity);
				LessonHandle.GetComponent<DragScript>().StartXPosition = Column;
				LessonHandle.GetComponent<DragScript>().StartYPosition = 3.5f - (i * 0.5f);
				LessonHandle.GetComponent<SpriteRenderer>().sortingOrder = i;

				if (EndOfNeeded)
				{
					LessonHandle.GetComponent<DragScript>().SubjectType = Random.Range(0, 8);
				}
				else
				{
					RandomIndex = Random.Range(0, 8);
					while (max[RandomIndex] == 0)
					{
						RandomIndex++;

						if (RandomIndex == 8)
						{
							RandomIndex = 0;
						}
					}

					LessonHandle.GetComponent<DragScript>().SubjectType = RandomIndex;
					max[RandomIndex]--;

					EndOfNeeded = true;

					for (int g = 0; g < 8; g++)
					{
						if (max[g] > 0)
						{
							EndOfNeeded = false;
						}
					}
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
		ThisWeek = new int[8,2];
		AllTime = new int[8,2];

		CurrentLevel = 1;
		SetUpLevel(CurrentLevel);
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Main");
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			SpaceLessons = GameObject.FindGameObjectsWithTag("Subject");
			foreach (GameObject subj in SpaceLessons)
			{
				subj.GetComponent<DragScript>().SetBackToSpawnPosition();
			}
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			Debug.Log("WE WILL RETRUN HERE");
		}
	}
}