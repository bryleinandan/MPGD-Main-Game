using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public String InteractionPrompt { get; }

    public bool Interact(Interactor interactor);
    
}
