using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IInteractable {

    // interface means whatever is defined here should be re-defined / overwritten in the child

    //text shown when you get close.
    // [SerializeField] private string _prompt;
    // public string InteractionPrompt => _prompt;
    public String interactionPromptStr { get; set;}

    // function that plays when you press the interaction key.
    public bool Interact(Interactor interactor); // takes as input the thing that initiated

    //private Camera mainCam = GameObject.Find("InventoryManager").GetComponent<Camera>();
    abstract Camera mainCam { get; }
    //abstract TextMeshProUGUI promptTextMesh { get; set;}
    abstract TextMeshPro promptTextMesh { get; set;}
    public bool promptIsVisible { get; set; }
    public Vector3 textOriginalScale { get; set; }
    //  promptTextMesh.GetComponent<RectTransform>().localScale

    void UpdateVisibility() { // put this in update()
        Debug.Log("Prompt is visible:" +promptIsVisible);
        RectTransform txtScale = promptTextMesh.GetComponent<RectTransform>();
        if (promptIsVisible) {
            txtScale.localScale = textOriginalScale;
        } else {
            txtScale.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    void ShowPrompt(string setTo = "DEFAULT_") {
        if (promptTextMesh == null) {
            Debug.Log("there is no text mesh to display in:");
            Debug.Log(this);
        } else {
            if (setTo == "DEFAULT_") {
                setTo = interactionPromptStr;
            }
            promptTextMesh.text = setTo;
            promptIsVisible = true;
        }
    }

    void HidePrompt() {
        promptIsVisible = false;
    }

}
