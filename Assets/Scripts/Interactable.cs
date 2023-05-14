using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //The message which is shown when player look at interactable object
    public string promptMessage;

    //Function which will be called from player script
    public void BaseInteract()
    {
        
    }
    
    //Template function which will be overriden by subclasses
    public virtual void Interact()
    {
        
    }
}
