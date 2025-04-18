using UnityEngine;
public class ChuckAttachment : MonoBehaviour
{
    public Transform attachmentPoint; // Where the item will snap
    public GameObject previewObject;  // Preview GameObject
    public Transform jaw1, jaw2, jaw3; // The three jaws (assign in Inspector)
    private GameObject currentItem;
    private Vector3[] initialJawPositions; // To store initial positions of jaws
    private Vector3[] initialJawScales;    // To store initial scales of jaws

    private void Start()
    {
        if (previewObject != null)
        {
            previewObject.SetActive(false);
            previewObject.transform.localScale = Vector3.one;
        }

        if (jaw1 != null && jaw2 != null && jaw3 != null)
        {
            initialJawPositions = new Vector3[3];
            initialJawPositions[0] = jaw1.localPosition;
            initialJawPositions[1] = jaw2.localPosition;
            initialJawPositions[2] = jaw3.localPosition;

            initialJawScales[0] = jaw1.localScale;
            initialJawScales[1] = jaw2.localScale;
            initialJawScales[2] = jaw3.localScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AttachableItem>() != null)
        {
            currentItem = other.gameObject;
            ShowPreview(currentItem);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AttachableItem>() != null)
        {
            HidePreview();
            currentItem = null;
            ResetJaws();
        }
    }

    private void Update()
    {
        if (currentItem != null && Input.GetMouseButtonUp(0))
        {
            AttachItem(currentItem);
        }
    }

    private void ShowPreview(GameObject item)
    {
        if (previewObject == null || attachmentPoint == null) return;
        previewObject.SetActive(true);
        previewObject.transform.position = attachmentPoint.position;
        previewObject.transform.rotation = attachmentPoint.rotation * Quaternion.Euler(-90, 0, 0);

        ResizePreview(item);
    }

    private void HidePreview()
    {
        if (previewObject != null)
            previewObject.SetActive(false);
    }

    private void ResizePreview(GameObject item)
    {
        Renderer itemRenderer = item.GetComponent<Renderer>();
        if (itemRenderer == null) return;

        Vector3 itemSize = itemRenderer.bounds.size;
        float diameter = Mathf.Max(itemSize.x, itemSize.z);
        float referenceSize = 1f;
        float scaleFactor = diameter / referenceSize;

        previewObject.transform.localScale = Vector3.one * scaleFactor;
    }

    private void AttachItem(GameObject item)
    {
        item.transform.position = attachmentPoint.position;
        item.transform.rotation = attachmentPoint.rotation * Quaternion.Euler(-90, 0, 0);
        item.transform.SetParent(attachmentPoint);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        AdjustJaws(item);
        HidePreview();
        currentItem = null;
    }

    private void AdjustJaws(GameObject item)
    {
        if (jaw1 == null || jaw2 == null || jaw3 == null) return;

        // Detect the cylinder’s scale
        Vector3 itemScale = item.transform.localScale;
        Debug.Log($"Detected item scale: {itemScale}");

        // Define the base movement distance for each scale
        float moveDistance = 0f;
        float gripAdjustment = 0.9f; // Adjust for tighter grip (lower = tighter)

        if (Mathf.Approximately(itemScale.x, 1f) && Mathf.Approximately(itemScale.y, 1f) && Mathf.Approximately(itemScale.z, 1f))
        {
            float diameter = 1f; // Default cylinder diameter at scale 1
            moveDistance = (diameter / 2f) * gripAdjustment; // Radius adjusted for grip
        }
        else if (Mathf.Approximately(itemScale.x, 0.5f) && Mathf.Approximately(itemScale.y, 0.5f) && Mathf.Approximately(itemScale.z, 0.5f))
        {
            float diameter = 0.5f; // Diameter at scale 0.5
            moveDistance = (diameter / 2f) * gripAdjustment;
        }
        else if (Mathf.Approximately(itemScale.x, 1.5f) && Mathf.Approximately(itemScale.y, 1.5f) && Mathf.Approximately(itemScale.z, 1.5f))
        {
            float diameter = 1.5f; // Diameter at scale 1.5
            moveDistance = (diameter / 2f) * gripAdjustment;
        }
        else
        {
            Debug.LogWarning($"Item scale {itemScale} not recognized. Resetting jaws to initial positions.");
            ResetJaws();
            return;
        }

        // Move jaws inward/outward from their initial positions
        jaw1.localPosition = initialJawPositions[0] + new Vector3(-moveDistance, 0, 0);
        jaw2.localPosition = initialJawPositions[1] + new Vector3(moveDistance * 0.5f, -moveDistance * 0.866f, 0);
        jaw3.localPosition = initialJawPositions[2] + new Vector3(moveDistance * 0.5f, moveDistance * 0.866f, 0);
    }

    private void ResetJaws()
    {
        if (jaw1 == null || jaw2 == null || jaw3 == null) return;

        jaw1.localPosition = initialJawPositions[0];
        jaw2.localPosition = initialJawPositions[1];
        jaw3.localPosition = initialJawPositions[2];

        jaw1.localScale = initialJawScales[0];
        jaw2.localScale = initialJawScales[1];
        jaw3.localScale = initialJawScales[2];
    }
}