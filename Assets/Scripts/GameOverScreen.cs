using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Player player;
    void Start() {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero; // hide game over screen
    }
    void Update() {
        if (player.currentHealth <= 0) { // when player health 0, show game over screen
            gameObject.transform.localScale = Vector3.one;
        }
    }


    public void RestartButton() {
        Debug.Log("replace scene name to final game scene name");
        SceneManager.LoadScene("Sahra2"); // replace with final game scene name
    }
    public void ExitButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
