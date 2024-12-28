using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Health playerHealth;
    void Start() {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero; // hide game over screen
    }
    void Update() {
        if (playerHealth.currentHealth <= 0) { // when player health 0
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make cursor visible
            gameObject.transform.localScale = Vector3.one; // show game over screen
        }
    }


    public void RestartButton() {
        Debug.Log("scene should be main");
        SceneManager.LoadScene("Main"); // replace with final game scene name
    }
    public void ExitButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
