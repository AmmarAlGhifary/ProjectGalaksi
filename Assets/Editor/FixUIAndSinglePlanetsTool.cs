using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class FixUIAndSinglePlanetsTool
{
    [MenuItem("Tools/9. Fix UI and Single Planets")]
    public static void Fix()
    {
        // 1. FIX THE UI BUTTON OVERLAP
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform buttonGroup = canvas.transform.Find("MainHUDPanel/ButtonGroup");
            if (buttonGroup != null)
            {
                // Ensure the group has a HorizontalLayoutGroup
                HorizontalLayoutGroup hlg = buttonGroup.GetComponent<HorizontalLayoutGroup>();
                if (hlg == null) hlg = buttonGroup.gameObject.AddComponent<HorizontalLayoutGroup>();
                
                hlg.childControlWidth = false;
                hlg.childControlHeight = false;
                hlg.childForceExpandWidth = false;
                hlg.childForceExpandHeight = false;
                hlg.spacing = 20;
                hlg.childAlignment = TextAnchor.MiddleCenter;

                // Fix the individual buttons
                Button[] buttons = buttonGroup.GetComponentsInChildren<Button>();
                foreach (Button btn in buttons)
                {
                    // Add ContentSizeFitter to button so it hugs the text
                    ContentSizeFitter csf = btn.GetComponent<ContentSizeFitter>();
                    if (csf == null) csf = btn.gameObject.AddComponent<ContentSizeFitter>();
                    csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                    // Remove fixed width/height LayoutElements that were squishing them
                    LayoutElement le = btn.GetComponent<LayoutElement>();
                    if (le != null) Object.DestroyImmediate(le);

                    // Add a HorizontalLayoutGroup to the button to act as Padding!
                    HorizontalLayoutGroup btnLayout = btn.GetComponent<HorizontalLayoutGroup>();
                    if (btnLayout == null) btnLayout = btn.gameObject.AddComponent<HorizontalLayoutGroup>();
                    btnLayout.padding = new RectOffset(30, 30, 15, 15);
                    btnLayout.childControlWidth = true;
                    btnLayout.childControlHeight = true;

                    // Fix the Text size
                    TextMeshProUGUI tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
                    if (tmp != null)
                    {
                        tmp.fontSize = 28; // Down from 45!
                        tmp.enableWordWrapping = false; // Prevent it from stacking
                    }
                }
            }
        }

        // 2. FIX THE SINGLE PLANET SLIDERS & ORBIT DISTANCE
        string[] targetNames = { "ImageTarget_Earth", "ImageTarget_Moon", "ImageTarget_Sun" };
        foreach (string targetName in targetNames)
        {
            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                PlanetInteractable pi = target.GetComponentInChildren<PlanetInteractable>();
                if (pi != null)
                {
                    // The Animator is fighting the scale of the object. 
                    // Tell the script to scale the PARENT container instead!
                    if (pi.transform.parent != null && pi.transform.parent.name == "Tata Surya Albi try fix")
                    {
                        pi.targetToScale = pi.transform.parent;
                        Debug.Log($"Fixed slider scale target for {pi.planetName}");
                    }
                }
            }
        }

        // 3. SEPARATE EARTH AND MOON FURTHER FROM SUN
        GameObject solarTarget = GameObject.Find("ImageTarget_SolarSystem");
        if (solarTarget != null)
        {
            Transform container = solarTarget.transform.Find("ScaleWrapper/Tata Surya Albi try fix");
            if (container != null)
            {
                Transform earth = container.Find("Earth");
                Transform moon = container.Find("Moon");
                // Push them further out so they don't visually overlap the massive sun
                if (earth != null) earth.localPosition = new Vector3(-3.5f, 0, 0);
                if (moon != null) moon.localPosition = new Vector3(-5.0f, 0.25f, 0);
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("UI fixed and single planet sliders restored!");
    }
}
