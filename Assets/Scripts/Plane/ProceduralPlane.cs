using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralPlane : MonoBehaviour
{
    public float width = 5f;  // Kích thước tổng theo X
    public float height = 10f; // Kích thước tổng theo Y
    public int gridWidth = 4;  // Số lượng ô theo X
    public int gridHeight = 8; // Số lượng ô theo Y
    public Material material;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    public float cellSize; // Kích thước của mỗi ô (luôn vuông)

    void Awake()
    {
        CalculateCellSize(); // Tính toán cellSize trước khi tạo Plane
        GeneratePlane();
        SetMaterial();
        SetPivotToCenter();
        SetPosition();
    }

    void CalculateCellSize()
    {
        float cellSizeX = width / gridWidth; // Kích thước ô theo chiều rộng
        float cellSizeY = height / gridHeight; // Kích thước ô theo chiều cao

        // Chọn kích thước nhỏ nhất để đảm bảo ô vuông
        cellSize = Mathf.Min(cellSizeX, cellSizeY);

        // Cập nhật lại `width` và `height` để khớp với ô vuông
        width = cellSize * gridWidth;
        height = cellSize * gridHeight;
    }


    
    void GeneratePlane()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int vertCountX = gridWidth + 1;  // Số điểm đỉnh theo X
        int vertCountY = gridHeight + 1; // Số điểm đỉnh theo Y
        vertices = new Vector3[vertCountX * vertCountY];

        uvs = new Vector2[vertices.Length];
        triangles = new int[gridWidth * gridHeight * 6];

        // Tạo các đỉnh (vertices)
        int vertIndex = 0;
        for (int y = 0; y < vertCountY; y++)
        {
            for (int x = 0; x < vertCountX; x++)
            {
                vertices[vertIndex] = new Vector3(x * cellSize - width / 2, 0, y * cellSize - height / 2);
                uvs[vertIndex] = new Vector2((float)x / gridWidth, (float)y / gridHeight);
                vertIndex++;
            }
        }

        // Tạo các tam giác (triangles)
        int triIndex = 0;
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int topLeft = y * vertCountX + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + vertCountX;
                int bottomRight = bottomLeft + 1;

                // Tam giác 1
                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topRight;

                // Tam giác 2
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = bottomRight;
            }
        }

        // Gán Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals(); // Giúp ánh sáng hiển thị đúng
    }

    private void SetMaterial()
    {
        GetComponent<MeshRenderer>().material = material;
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

    void SetPosition() 
    {
        transform.position = new Vector3(0, 0.1f, 0);
    }
}
