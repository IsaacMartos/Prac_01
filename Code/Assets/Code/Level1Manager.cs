﻿using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("Level2Scene");
        }
    }
            

}