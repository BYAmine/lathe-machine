using UnityEngine;

public class ItemDrag : MonoBehaviour
{
    private bool isDragging;
    private Plane dragPlane; // Plane to project the mouse ray onto
    private float distanceToCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                // Create a plane at the object's position, facing the camera
                dragPlane = new Plane(Camera.main.transform.forward, transform.position);
                distanceToCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
            }
        }

        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}