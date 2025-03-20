
using UnityEditor;

public class TextureSizeLimiter : EditorWindow
{
    [MenuItem("Tools/Limit Texture Size to 1080")]
    static void LimitTextureSize()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D"); // 获取所有纹理
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (textureImporter != null)
            {
                if (textureImporter.maxTextureSize > 1024) // 限制最大大小
                {
                    textureImporter.maxTextureSize = 1024;
                    textureImporter.SaveAndReimport();
                }
            }
        }
    }
}
