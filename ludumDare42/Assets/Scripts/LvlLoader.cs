using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlLoader : MonoBehaviour 
{
	public Animator animator;

	Scene SC;
	
	void Update () 
	{
		SC = SceneManager.GetActiveScene();	
	}

	public void OnFadeComplete ()
	{
		if (SC.name == "Main")
		{
			SceneManager.LoadScene("Week");
		}
		else
		{
			SceneManager.LoadScene("Main");
		}
	}
}
