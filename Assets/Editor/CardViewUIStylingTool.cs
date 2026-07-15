using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class CardViewUIStylingTool
{
    [MenuItem("Tools/2. Apply CardView Styling")]
    public static void ApplyStyling()
    {
        // 1. Generate Rounded Rectangle Sprite
        string spritePath = "Assets/RoundedCard.png";
        if (!File.Exists(spritePath))
        {
            GenerateRoundedRectTexture(spritePath);
        }
        
        Sprite roundedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (roundedSprite == null)
        {
            // If it loaded as Texture2D, we need to change its import settings
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(spritePath);
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteBorder = new Vector4(20, 20, 20, 20); // 9-slicing
                importer.SaveAndReimport();
                roundedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            }
        }

        // 2. Apply to Buttons
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button btn in buttons)
        {
            if (btn.gameObject.scene.isLoaded)
            {
                StyleAsCardView(btn.gameObject, roundedSprite, new Color(0.1f, 0.5f, 0.9f, 0.9f));
                
                // Make text inside button white and bold
                var txt = btn.GetComponentInChildren<UnityEngine.UI.Text>();
                if (txt != null)
                {
                    txt.color = Color.white;
                    txt.fontStyle = FontStyle.Bold;
                }
                var tmpro = btn.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (tmpro != null)
                {
                    tmpro.color = Color.white;
                    tmpro.fontStyle = TMPro.FontStyles.Bold;
                }
            }
        }

        // 3. Apply to Info Panels
        string[] panelsToStyle = { "GeneralInfoPanel", "QuizPanel", "MainMenuPanel" };
        foreach (string panelName in panelsToStyle)
        {
            Transform[] all = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform t in all)
            {
                if (t.name == panelName && t.gameObject.scene.isLoaded)
                {
                    StyleAsCardView(t.gameObject, roundedSprite, new Color(0.1f, 0.1f, 0.15f, 0.85f)); // Dark glassmorphism
                }
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("CardView styling applied!");
    }

    private static void StyleAsCardView(GameObject obj, Sprite sprite, Color color)
    {
        Image img = obj.GetComponent<Image>();
        if (img == null) img = obj.AddComponent<Image>();

        img.sprite = sprite;
        img.type = Image.Type.Sliced; // Important for 9-slice rounded corners
        img.color = color;

        // Add a subtle drop shadow
        Shadow shadow = obj.GetComponent<Shadow>();
        if (shadow == null) shadow = obj.AddComponent<Shadow>();
        
        shadow.effectColor = new Color(0, 0, 0, 0.5f);
        shadow.effectDistance = new Vector2(0, -5f); // Shadow falls downwards
    }

    private static void GenerateRoundedRectTexture(string path)
    {
        int size = 128;
        int radius = 32;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color transparent = new Color(0, 0, 0, 0);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Calculate distance from closest corner
                float cx = Mathf.Max(0, radius - x, x - (size - radius - 1));
                float cy = Mathf.Max(0, radius - y, y - (size - radius - 1));
                float dist = Mathf.Sqrt(cx * cx + cy * cy);

                if (dist > radius)
                {
                    tex.SetPixel(x, y, transparent);
                }
                else
                {
                    tex.SetPixel(x, y, Color.white);
                }
            }
        }
        
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();
    }
}
