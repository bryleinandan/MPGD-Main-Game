using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColour, deselectedColour;
    
    public void Awake() {
        Deselect();
    }
    public void Select() {
        image.color = selectedColour;
    }
    public void Deselect() {
        image.color = deselectedColour;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
            draggableItem.parentAfterDrag = transform;
        }
    }
}
