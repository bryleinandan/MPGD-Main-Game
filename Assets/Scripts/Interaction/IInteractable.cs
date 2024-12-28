using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using System.Numerics;

public interface IInteractable {

    // interface means whatever is defined here should be re-defined / overwritten in the child

    //text shown when you get close.
    // [SerializeField] private string _prompt;
    // public string InteractionPrompt => _prompt;
    public String interactionPromptStr { get; set;}

    // function that plays when you press the interaction key.
    public bool Interact(Interactor interactor); // takes as input the thing that initiated

    //private Camera mainCam = GameObject.Find("InventoryManager").GetComponent<Camera>();
    abstract Transform playerTransform { get; } // dont forget to constantly update this
    abstract TextMeshPro promptTextMesh { get; set;}
    public bool promptIsVisible { get; set; }
    public Vector3 textOriginalScale { get; set; }
    //  promptTextMesh.GetComponent<RectTransform>().localScale
    public float smoothSpeed { get; } // i use 2.5f // considering a setter. probably not.
    

    void UpdateVisibility() { // put this in update()
        // no point in making these public
        var vec0 = new Vector3(0.0f, 0.0f, 0.0f);
        float smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);

        //Debug.Log("Prompt is visible:" +promptIsVisible);
        RectTransform txtScale = promptTextMesh.GetComponentInChildren<RectTransform>();
        if (promptIsVisible) {
            //txtScale.localScale = textOriginalScale;
            txtScale.localScale = Vector3.Lerp(txtScale.localScale, textOriginalScale, smooth);
        } else {
            //txtScale.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            // ease:
            txtScale.localScale = Vector3.Lerp(txtScale.localScale, vec0, smooth);
        }
    }

    void LateUpdateLabelRotation() {
        //promptTextMesh.transform.rotation = player.transform.rotation;
        promptTextMesh.transform.rotation = playerTransform.rotation;
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

    void SetLabelPosition(float posX, float posY, float posZ) {
        // var rect = promptTextMesh.GetComponent<RectTransform>();
        // rect.anchoredPosition3D = new Vector3(posX, posY, posZ);
        // so this really only scales the y value to mesh height
        //var offset = 0.3f;
        //promptTextMesh.margin = new Vector4(8, 0, 8, posY - offset);
        //promptTextMesh.margin = new Vector4(0, 0, 0, posY-offset);

        var offset = 2.0f; // screw it. player is 2 units tall
        promptTextMesh.margin = new Vector4(8, 0, 8, posY + offset);
        
    }
}
