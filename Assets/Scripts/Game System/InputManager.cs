using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject selectedObject = null; // Vật thể đang được chọn
    private Vector3 targetPosition; // Vị trí mục tiêu khi kéo
    private Plane groundPlane; // Mặt phẳng để chọn vị trí
    private Rigidbody rb;
    
    [SerializeField]
    private float speed = 10.0f; // Tốc độ di chuyển
    private bool isDragging = false; // Biến kiểm tra trạng thái kéo
    void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero); // Mặt phẳng cố định ở Y = 0
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPosition = GetWorldPositionFromTouch(touch);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TrySelectObject(touch);
                    if (selectedObject != null)
                    {
                        targetPosition = touchWorldPosition;
                        Outline outline = selectedObject.GetComponent<Outline>();
                        if (outline == null)
                        {
                            outline = selectedObject.AddComponent<Outline>();
                        }
                        outline.enabled = true;
                        outline.OutlineColor = Color.white;
                        outline.OutlineWidth = 10.0f;

                        rb = selectedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            // rb.isKinematic = true; // Giữ nguyên trạng thái khi kéo
                            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Tránh xuyên vật thể
                        }

                        isDragging = true; // Bắt đầu kéo
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging && selectedObject != null)
                    {
                        targetPosition = touchWorldPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging && selectedObject != null)
                    {
                        Outline outline = selectedObject.GetComponent<Outline>();
                        if (outline != null)
                        {
                            outline.enabled = false;
                        }

                        if (rb != null)
                        {
                            // rb.isKinematic = false; // Trả lại trạng thái vật lý
                            rb.linearVelocity = Vector3.zero; // Dừng di chuyển
                            rb.angularVelocity = Vector3.zero;
                        }
                        targetPosition = touchWorldPosition;
                        isDragging = false; // Dừng kéo
                        selectedObject = null;
                        rb = null;
                    }
                    break;
            }
        }
    }

    // 🛠 Di chuyển bằng Rigidbody ngay khi kéo
    void FixedUpdate()
    {
        if (isDragging && selectedObject != null && rb != null)
        {
           
            //  Vector3 mousePos = Vector3.Lerp(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.linearVelocity = 
            (targetPosition - selectedObject.transform.position).normalized * speed;
            Debug.Log(rb.linearVelocity);
        }
    }

    private void TrySelectObject(Touch touch)
    {
        Ray ray = mainCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Block")) // Chỉ chọn object có tag "Block"
            {
                selectedObject = hit.transform.gameObject;
                rb = selectedObject.GetComponent<Rigidbody>();
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
}
