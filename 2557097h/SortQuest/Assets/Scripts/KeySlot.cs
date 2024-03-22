using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class KeySlot : MonoBehaviour, IDropHandler
{
    // Property to store the array index of the key slot
    public int ArrayIndex { get; private set; }

    void Start()
    {
        // Set the array index based on the order in the hierarchy
        ArrayIndex = transform.GetSiblingIndex();
    }

    // Called when a draggable object is dropped onto this slot
    public void OnDrop(PointerEventData eventData)
    {
        // Get the object that was dropped
        GameObject dropped = eventData.pointerDrag;

        // Get the KeyDragDrop component of the dropped object
        KeyDragDrop keyDragDrop = dropped.GetComponent<KeyDragDrop>();

        // Set the parent of the dropped object to this slot's transform
        keyDragDrop.parentAfterDrag = transform;
    }
}
