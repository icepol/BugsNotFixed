using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            OnPlayButtonClick();
        }
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("Game");
    }
}
