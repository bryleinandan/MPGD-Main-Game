using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    public void PlayButton() {
        Debug.Log("replace scene name to final game scene name");
        SceneManager.LoadScene("Sahra2");
    }
}
