using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private Camera mainCamera;

    private Plane groundPlane;
    private GameObject selectedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        while (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPosition = GetWorldPositionFromTouch(touch);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (selectedObject != null)
                    {
                        TrySelectObject(touch);
                    }

                    break;

                case TouchPhase.Moved:
                    if (selectedObject != null)
                    {
                        if (selectedObject.CompareTag("Block"))
                        {
                            EventBroker.instance.OnBlockMoving?.Invoke(touchWorldPosition);
                        }
                    }

                    break;
                    
                case TouchPhase.Ended:
                    break;

                case TouchPhase.Canceled:
                    if (selectedObject != null)
                    {
                        if (selectedObject.CompareTag("Block"))
                        {
                            EventBroker.instance.OnBlockDeselected?.Invoke(touchWorldPosition);
                        }
                        selectedObject = null;
                    }

                    break;
            }
        }
    }

    private Vector3 GetWorldPositionFromTouch(Touch touch)
    {
        Ray ray = mainCamera.ScreenPointToRay(touch.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            return new Vector3(worldPosition.x, 0.21f, worldPosition.z); // Giữ nguyên Y
        }

        return transform.position;
    }

    private void TrySelectObject(Touch touch)
    {
        Ray ray = mainCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Block"))
            {
                selectedObject = hit.collider.gameObject;
                EventBroker.instance.OnBlockSelected?.Invoke(selectedObject);
            }
        }
    }

}
