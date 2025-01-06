using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue : MonoBehaviour
{

    public Image image; 
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public float lineSpeed;

    private int index;

    void Start() {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update() {
        
    }

    void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
        SetImageAlpha(.4f);
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(lineSpeed);
        NextLine();

    }

    void NextLine() {
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            if (lines[index]=="") {
                SetImageAlpha(0f);
            } else {
                SetImageAlpha(.4f);
            }
            StartCoroutine (TypeLine());
        } else {
            gameObject.SetActive(false);
        }
    }

    void SetImageAlpha(float alphaValue) {
        if (image != null) {
            Color currentColor = image.color;
            currentColor.a = alphaValue;
            image.color = currentColor;
        }
    }


}
