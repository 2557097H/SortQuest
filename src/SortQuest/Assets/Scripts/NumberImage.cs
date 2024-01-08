using UnityEngine;
using UnityEngine.EventSystems;

public class NumberImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool isDragging = false;
    private int index;
    private GameObject draggedObject;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        // Check which object is being interacted with using the EventSystem
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;

        // Perform a raycast to determine the GameObject being interacted with
        RaycastResult raycastResult = new RaycastResult();
        EventSystem.current.RaycastAll(pointerEventData, new System.Collections.Generic.List<RaycastResult> { raycastResult });
        Debug.Log(raycastResult.gameObject);
        if (raycastResult.gameObject != null)
        {
            draggedObject = raycastResult.gameObject;
            Debug.Log(draggedObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        draggedObject = null;
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public int GetIndex()
    {
        return index;
    }

    public GameObject GetDraggedObject()
    {
        return draggedObject;
    }
}
