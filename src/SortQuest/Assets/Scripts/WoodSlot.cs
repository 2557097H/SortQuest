using UnityEngine;
using UnityEngine.EventSystems;

public class WoodSlot : MonoBehaviour, IDropHandler
{
    public int ArrayIndex { get; private set; }

    void Start()
    {
        // Set the array index based on the order in the hierarchy
        ArrayIndex = transform.GetSiblingIndex();
    }

    // Implementing the OnDrop method from IDropHandler interface
    public void OnDrop(PointerEventData eventData)
    {
        // Get the GameObject being dragged
        GameObject dropped = eventData.pointerDrag;

        // Get the WoodDragDrop component attached to the dropped GameObject
        WoodDragDrop woodDragDrop = dropped.GetComponent<WoodDragDrop>();

        // Set the parentAfterDrag property of the WoodDragDrop component to this transform
        woodDragDrop.parentAfterDrag = transform;
    }
}
