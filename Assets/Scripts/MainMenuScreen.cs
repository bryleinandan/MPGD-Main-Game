using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    public void PlayButton() {
        Debug.Log("scene should be main");
        SceneManager.LoadScene("Main");
    }
}
