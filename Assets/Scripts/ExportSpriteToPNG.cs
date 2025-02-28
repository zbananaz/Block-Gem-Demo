using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportSpriteToPNG
{
    [MenuItem("Tools/Export Sprite to PNG")]
    static void ExportSelectedSprite()
    {
        // Lấy object được chọn
        Object selected = Selection.activeObject;
        if (selected == null || !(selected is Texture2D))
        {
            Debug.LogError("Hãy chọn một Texture2D trong Project!");
            return;
        }

        Texture2D texture = selected as Texture2D;
        string path = EditorUtility.SaveFilePanel("Save PNG", "", texture.name + ".png", "png");

        if (!string.IsNullOrEmpty(path))
        {
            // Chuyển Texture2D thành PNG
            byte[] pngData = texture.EncodeToPNG();
            File.WriteAllBytes(path, pngData);
            Debug.Log("Saved to: " + path);
        }
    }
}
