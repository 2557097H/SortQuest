using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class KeySlot : MonoBehaviour, IDropHandler
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
        KeyDragDrop keyDragDrop = dropped.GetComponent<KeyDragDrop>();
        keyDragDrop.parentAfterDrag = transform;
    }
}
