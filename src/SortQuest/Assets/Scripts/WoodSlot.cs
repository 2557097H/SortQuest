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

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        WoodDragDrop woodDragDrop = dropped.GetComponent<WoodDragDrop>();
        woodDragDrop.parentAfterDrag = transform;
    }
}
