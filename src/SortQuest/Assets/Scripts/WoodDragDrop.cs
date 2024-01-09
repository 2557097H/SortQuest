using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WoodDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;
   

    void Start()
    {
        parentAfterDrag = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Disable raycasts on the dragged object
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Revert to the original parent
        transform.SetParent(parentAfterDrag);

        // Enable raycasts again
        canvasGroup.blocksRaycasts = true;
    }
}
