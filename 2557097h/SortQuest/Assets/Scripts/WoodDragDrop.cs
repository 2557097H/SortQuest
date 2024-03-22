using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WoodDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag; // Store the parent of the object after dragging
    private CanvasGroup canvasGroup;

    void Start()
    {
        parentAfterDrag = transform.parent; // Initialize the parentAfterDrag variable with the current parent
        canvasGroup = GetComponent<CanvasGroup>(); // Get the CanvasGroup component attached to the object
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent; // Update the parentAfterDrag variable to store the current parent before dragging
        transform.SetParent(transform.root); // Set the parent of the object to the root of the hierarchy
        transform.SetAsLastSibling(); //Move the object to the front of the canvas

        // Disable raycasts on the dragged object
        canvasGroup.blocksRaycasts = false; // Disable raycasts to prevent interaction with the object during dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Update the position of the object to match the pointer position during dragging
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Revert to the original parent
        transform.SetParent(parentAfterDrag); // Set the parent of the object back to its original parent after dragging

        // Enable raycasts again
        canvasGroup.blocksRaycasts = true; // Enable raycasts to allow interaction with the object after dragging
    }
}
