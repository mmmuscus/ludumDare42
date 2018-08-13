using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	private int MaxLevel = 10;
	private int CurentLevel;
	private int LevelHazardChance;
	private bool IsHazard;
	private int NumberToGenerate;
	private int ClosestDivFive;
	private int[] day;
	private int[,] Tests;
	private int RandomIndexForTests;
	private int[] max;
	private bool EndOfNeeded = false;
	private float Column;
	private int RandomIndex;
	private GameObject[] DestroyLastWeekLessons;

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

	private float WeekendTime = 15f;
	private float WeekTime = 10f;
	private float CurTime = 0f;
	private int CurDay;
	private float TimeRemaining;
	private float LastTimeRemaining;

	private bool isGO = false;
	private bool isVict = false;
	private bool isVictory = false;
	private bool isGame = false;
	public bool isIntro = true;
	private bool isIntermission = false;
	private GameObject[] SpaceLessons;

	public GameObject[] LessonList;
	private GameObject LessonHandle;

	private GameObject[] Lessons;

	private GameObject[] ULs = new GameObject[8];
	private SpriteRenderer[] ULSs = new SpriteRenderer[8];
	private SpriteRenderer[,] Ns = new SpriteRenderer[8,2];

	private float transitionTime = 1f;
	private float Papert = 0f;
	public GameObject F1;
	public GameObject F2;
	private GameObject F1Handle;
	private GameObject F2Handle;
	private bool F1active = false;
	private bool F2active = false;

	public GameObject Paper;
	public GameObject IMSSN;
	public GameObject VTY;
	public GameObject FAIL;
	private int Sum;
	private int Num;
	private float Average;
	private bool isFailure = false;

	void StartNewGame ()
	{
		isGO = false;
		isVict = false;
		isIntermission = false;
		isIntro = false;
		isGame = true;

		for (int i = 0; i < 8; i++)
		{
			for (int k = 0; k < 2; k++)
			{
				AllTime[i,k] = 0;
			}
		}

		CurentLevel = 1;
		SetUpLevel(CurentLevel);
	}

	void PlayIntro ()
	{
		isGO = false;
		isVict = false;
		isIntermission = false;
		isIntro = true;
		isGame = false;
	}

	void PlayInterMission ()
	{
		Instantiate(IMSSN);
		isGO = false;
		isVict = false;
		isIntermission = true;
		isIntro = false;
		isGame = false;
	}

	void PlayVictory ()
	{
		isVict = true;
		isGO = false;
		isIntermission = false;
		isIntro = false;
		isGame = false;
		Instantiate(VTY);
	}

	void PlayFaliure ()
	{
		isGO = true;
		isVict = false;
		isIntermission = false;
		isIntro = false;
		isGame = false;
		Instantiate(FAIL);
	}

	void SetUpLevel (int level)
	{
		DestroyLastWeek();

		ResetSetUpVariables();
		DetermineTestsAndHazards(level);
		DetermineDays(NumberToGenerate);      //How many tests a day
		SpawnWeek();                          //What kind of tests + spawns Subject prefabs //!!! shuld also spawn week table 

		CurDay = 0;
		GameObject.Find("HL0").GetComponent<SpriteRenderer>().color = new Color(1f, .125f, 0f, 0.4f);
		CurTime = 0;
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

		Math1.Clear();
		Lit2.Clear();
		Fra3.Clear();
		Hist4.Clear();
		Phys5.Clear();
		Inf6.Clear();
		Chem7.Clear();
		Geo0.Clear();

		Instantiate(Paper);

		isFailure = false;

		for (int i = 0; i < 8; i++)
		{
			AllTime[i,0] += ThisWeek[i,0];
			AllTime[i,1] += ThisWeek[i,1];

			//Debug.Log(i + ". subj sum: " + AllTime[i,0] + " num ber: " + AllTime[i,1]);

			// Average = Alltime[i,0]
			

			if (AllTime[i,1] != 0)
			{
				Average = (float)AllTime[i,0] / (float)AllTime[i,1];

				GameObject.FindWithTag(i + "NE").GetComponent<Evaluator>().Value = (int) Mathf.Floor(Average);
				GameObject.FindWithTag(i + "NT").GetComponent<Evaluator>().Value = (int) Mathf.Round((Average - Mathf.Floor(Average)) * 10);

				if(Average < 1.5f)
				{
					isFailure = true;
					GameObject.FindWithTag("UL" + i).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				}
				// if(AllTime[i,1] / AllTime[i,0] <= 1.5f)
				// {
				// 	PlayFaliure();
				// }
			}
		}

		Sum = 0;
		Num = 0;

		for (int i = 0; i < 8; i++)
		{
			Sum += AllTime[i,0];
			Num += AllTime[i,1];
		}

		Average = (float)Sum / (float)Num;

		GameObject.FindWithTag("OANE").GetComponent<Evaluator>().Value = (int) Mathf.Floor(Average);
		GameObject.FindWithTag("OANT").GetComponent<Evaluator>().Value = (int) Mathf.Round((Average - Mathf.Floor(Average)) * 10);

		if(Average < 1.5f)
		{
			isFailure = true;
			GameObject.FindWithTag("ULOA").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}

		if (isFailure)
		{
			PlayFaliure();
		}

		CurentLevel++;
		if (CurentLevel > MaxLevel && isGame)
		{
			isVictory = false;
			for (int i = 0; i < 8; i++)
			{
				if (AllTime[i, 0] != 0)
				{
					isVictory = true;
				}
			}

			if (isVictory)
			{
				PlayVictory();
			}
			else
			{
				PlayFaliure();
			}
		}
		else
		{
			if (isGame)
			{
				SetUpLevel(CurentLevel);
				PlayInterMission();
			}
		}
	}

	void LoadTempIntoThisWeek (int id)
	{
		// Debug.Log("Subject: " + id);
		ThisWeek[id,1] = TemporoaryArray.Length;
		// Debug.Log("How many there was this week: " + ThisWeek[id,1]);
		ThisWeek[id,0] = 0;
		for (int i = 0; i < TemporoaryArray.Length; i++)
		{
			ThisWeek[id,0] += TemporoaryArray[i];
		}
		// Debug.Log("And the sum of theese is: " + ThisWeek[id,0]);

		TemporoaryArray = new int[0];
	}

	void EndOfDay (int dayy)
	{
		if (dayy != 4)
		{
			GameObject.FindWithTag("timer").GetComponent<AudioSource>().Play();
		}

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
			if (Tests[dayy, i] != 0)
			{
				Grade = 0;
				
				if ((float)(Evaluated[i] / (float)Tests[dayy, i]) >= 1)
				{
					Grade = 5;
				}
				else
				{
					Grade = (int)Mathf.Round(5 * (float)(Evaluated[i] / (float)Tests[dayy, i]));

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
	}

	void SpawnWeek ()
	{
		for (int i = 0; i < 5; i++)
		{
			for (int k = 0; k < day[i]; k++)
			{
				RandomIndexForTests = Random.Range(0,8);

				while (Tests[i,RandomIndexForTests] >= 10)
				{
					RandomIndexForTests++;

					if (RandomIndexForTests == 8)
					{
						RandomIndexForTests = 0;
					}
				}

				Tests[i,RandomIndexForTests]++;
			}
		}

		for (int i = 0; i < 5; i++)
		{
			for (int k = 0; k < 8; k++)
			{
				if (k == 0)
				{
					GameObject.Find("Number" + (i + 1) + "8").GetComponent<Evaluator>().Value = Tests[i,k];
				}
				else
				{
					GameObject.Find("Number" + (i + 1) + k).GetComponent<Evaluator>().Value = Tests[i,k];
				}
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
				Column = -3.5f;
			}

			for (int i = 0; i < 15; i++)
			{
				LessonHandle = Instantiate(LessonList[Random.Range(0, 23)], new Vector3(Column, (3.5f - (i * 0.5f)), 15 - i), Quaternion.identity);
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

	void DestroyLastWeek ()
	{
		DestroyLastWeekLessons = GameObject.FindGameObjectsWithTag("Subject");
		foreach (GameObject subj in DestroyLastWeekLessons)
		{
			Destroy(subj);
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
			NumberToGenerate = Random.Range((level + 4) * 2, (level + 5) * 2);
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
		Math1 = new List<int>();
		Lit2 = new List<int>();
		Fra3 = new List<int>();
		Hist4 = new List<int>();
		Phys5 = new List<int>();
		Inf6 = new List<int>();
		Chem7 = new List<int>();
		Geo0 = new List<int>();

		StartNewGame();
		PlayIntro();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			SceneManager.LoadScene("Main");
		}
		
		if (isGame)
		{
			CurTime += Time.deltaTime;

			if (CurDay == 0)
			{
				TimeRemaining = Mathf.Round(WeekendTime - CurTime);
			}
			else
			{
				TimeRemaining = Mathf.Round(WeekTime - CurTime);
			}

			if (TimeRemaining != (GameObject.FindWithTag("tizesek").GetComponent<Evaluator>().Value * 10) + GameObject.FindWithTag("egyeseek").GetComponent<Evaluator>().Value)
			{
				if (TimeRemaining % 2 == 1)
				{
					GameObject.FindWithTag("tizesek").GetComponent<AudioSource>().Play();
				}
				else
				{
					GameObject.FindWithTag("egyeseek").GetComponent<AudioSource>().Play();
				}
			}

			if (TimeRemaining >= 10)
			{
				GameObject.FindWithTag("tizesek").GetComponent<Evaluator>().Value = 1;
				GameObject.FindWithTag("egyeseek").GetComponent<Evaluator>().Value = (int)(TimeRemaining - 10);
			}
			else
			{
				GameObject.FindWithTag("tizesek").GetComponent<Evaluator>().Value = 0;
				GameObject.FindWithTag("egyeseek").GetComponent<Evaluator>().Value = (int)TimeRemaining;
			}

			if ((CurTime >= WeekendTime && CurDay == 0) || (CurTime >= WeekTime && CurDay != 0) || (Input.GetKeyDown(KeyCode.S)))
			{
				EndOfDay(CurDay);
				GameObject.Find("HL" + CurDay).GetComponent<SpriteRenderer>().color = new Color (0.9411f, 0.9019f, 0.7843f, 0.4f);
				CurDay++;
				if (CurDay >= 5)
				{
					EndOfWeek();
				}
				else
				{
					GameObject.Find("HL" + CurDay).GetComponent<SpriteRenderer>().color = new Color(1f, .125f, 0f, 0.4f);
				}

				CurTime = 0f;
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				SpaceLessons = GameObject.FindGameObjectsWithTag("Subject");
				foreach (GameObject subj in SpaceLessons)
				{
					subj.GetComponent<DragScript>().SetBackToSpawnPosition();
				}
			}
		}

		if (isIntro)
		{
			if (F2active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle.GetComponent<Rigidbody2D>().isKinematic = false;
				}

				if (F2Handle.transform.position.y <= -8.6f)
				{
					isIntro = false;
					isGame = true;
					Destroy(F2Handle);
					F2active = false;
					Papert = 0;
				}
			}

			if (F1active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle = Instantiate(F2);
					Destroy(F1Handle);
					F1active = false;
					F2active = true;
					Papert = 0;
				}
			}

			if (Input.GetKeyDown(KeyCode.Space) && !F1active && !F2active)
			{
				F1Handle = Instantiate(F1);
				Destroy(GameObject.FindWithTag("IP"));		
				F1active = true;
				Papert = 0;
			}
		}

		if (isGO)
		{
			if (F2active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle.GetComponent<Rigidbody2D>().isKinematic = false;
				}

				if (F2Handle.transform.position.y <= -8.6f)
				{
					StartNewGame();
					Destroy(F2Handle);
					F2active = false;
					Papert = 0;
				}
			}

			if (F1active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle = Instantiate(F2);
					Destroy(F1Handle);
					F1active = false;
					F2active = true;
					Papert = 0;
				}
			}

			if (Input.GetKeyDown(KeyCode.Space) && !F1active && !F2active)
			{
				F1Handle = Instantiate(F1);
				Destroy(GameObject.FindWithTag("FL"));
				Destroy(GameObject.FindWithTag("papr"));		
				F1active = true;
				Papert = 0;
			}
		}

		if (isVict)
		{
			if (F2active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle.GetComponent<Rigidbody2D>().isKinematic = false;
				}

				if (F2Handle.transform.position.y <= -8.6f)
				{
					StartNewGame();
					Destroy(F2Handle);
					F2active = false;
					Papert = 0;
				}
			}

			if (F1active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle = Instantiate(F2);
					Destroy(F1Handle);
					F1active = false;
					F2active = true;
					Papert = 0;
				}
			}

			if (Input.GetKeyDown(KeyCode.Space) && !F1active && !F2active)
			{
				F1Handle = Instantiate(F1);
				Destroy(GameObject.FindWithTag("papr"));
				Destroy(GameObject.FindWithTag("Vt"));	
				F1active = true;
				Papert = 0;
			}
		}

		if (isIntermission)
		{
			if (F2active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle.GetComponent<Rigidbody2D>().isKinematic = false;
				}

				if (F2Handle.transform.position.y <= -8.6f)
				{
					isIntermission = false;
					isGame = true;
					Destroy(F2Handle);
					F2active = false;
					Papert = 0;
				}
			}

			if (F1active)
			{
				Papert += Time.deltaTime;
				if (Papert >= 1f)
				{
					F2Handle = Instantiate(F2);
					Destroy(F1Handle);
					F1active = false;
					F2active = true;
					Papert = 0;
				}
			}

			if (Input.GetKeyDown(KeyCode.Space) && !F1active && !F2active)
			{
				F1Handle = Instantiate(F1);
				Destroy(GameObject.FindWithTag("papr"));
				Destroy(GameObject.FindWithTag("IMSN"));
				F1active = true;
				Papert = 0;
			}
		}	
	}
}