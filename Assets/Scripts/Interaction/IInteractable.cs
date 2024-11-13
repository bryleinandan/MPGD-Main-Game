using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    // interface means whatever is defined here should be re-defined / overwritten in the child

    //text shown when you get close.
    // [SerializeField] private string _prompt;
    // public string InteractionPrompt => _prompt;
    public String InteractionPrompt { get; }

    // function that plays when you press the interaction key.
    public bool Interact(Interactor interactor);

}
