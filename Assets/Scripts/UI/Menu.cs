using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
	}

	public void Play()
	{
		SceneManager.LoadScene("GameScene");
	}
}
