using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        if (!gameObject.name.Contains("(Clone)"))
        {
            // Check if all other sibling slots contain a game object
            bool allSiblingsOccupied = AllSiblingsOccupied();

            if (allSiblingsOccupied)
            {
                GameObject duplicatedKey = Instantiate(gameObject);
                duplicatedKey.transform.SetParent(parentAfterDrag);
            }
        }
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Disable raycasts on the dragged object
        canvasGroup.blocksRaycasts = false;

    }
    private bool AllSiblingsOccupied()
    {
        Transform parent = transform.parent.parent.parent;
        foreach (Transform sibling in parent)
        {
            foreach(Transform child in sibling)
            {
                if (child.childCount == 0)
                {
                    return false; // At least one sibling slot is not occupied
                }

            }
        }
        return true; // All sibling slots are occupied
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
