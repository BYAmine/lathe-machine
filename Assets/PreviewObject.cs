using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public GameObject cubePreview;     // Child GameObject for cube shape
    public GameObject cylinderPreview; // Child GameObject for cylinder shape

    private void Start()
    {
        if (cubePreview != null) cubePreview.SetActive(false);
        if (cylinderPreview != null) cylinderPreview.SetActive(false);
    }

    public void SetPreviewType(string itemType)
    {
        if (cubePreview != null) cubePreview.SetActive(itemType == "Cube");
        if (cylinderPreview != null) cylinderPreview.SetActive(itemType == "Cylinder");
    }
}