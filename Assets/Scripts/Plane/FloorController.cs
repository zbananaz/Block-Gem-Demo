using UnityEngine;

public class FloorController : MonoBehaviour
{
    void Start()
    {
        SetPivotToCenter(); // Gán material để hiển thị
    }

    void SetPivotToCenter()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("Object này không có MeshFilter hoặc Mesh!");
            return;
        }

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        // Lấy vị trí trung tâm hiện tại của Mesh
        Vector3 meshCenter = mesh.bounds.center;

        // Dịch chuyển tất cả vertices để Pivot nằm giữa
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= meshCenter;
        }

        // Cập nhật Mesh
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        // Dịch chuyển Object để bù trừ lại vị trí mới của Pivot
        transform.position += meshCenter;
    }
}
