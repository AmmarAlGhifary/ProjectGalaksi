using UnityEngine;
using Vuforia;

[RequireComponent(typeof(ObserverBehaviour))]
public class AutoSelectOnTrack : MonoBehaviour
{
    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        ARUIManager uiManager = FindObjectOfType<ARUIManager>();
        
        // Only show when fully tracked. If it's EXTENDED_TRACKED, it means the camera lost it 
        // but is guessing where it is. We want to hide the UI immediately if it's out of view!
        if (targetStatus.Status == Status.TRACKED)
        {
            if (uiManager != null && uiManager.mainHUDPanel != null)
            {
                uiManager.mainHUDPanel.SetActive(true);
            }

            PlanetInteractable planet = GetComponentInChildren<PlanetInteractable>();

            if (planet != null && uiManager != null)
            {
                uiManager.SelectPlanet(planet);
                Debug.Log("[AutoSelect] Automatically selected: " + planet.planetName);
                
                if (uiManager.quizButton != null) uiManager.quizButton.gameObject.SetActive(true);
            }
            else if (gameObject.name == "ImageTarget_SolarSystem")
            {
                GameObject playAllBtn = FindObjectByName("PlayAllButton");
                if (playAllBtn != null) playAllBtn.SetActive(true);
            }
        }
        else if (targetStatus.Status == Status.EXTENDED_TRACKED || targetStatus.Status == Status.NO_POSE)
        {
            if (uiManager != null)
            {
                if (uiManager.actionButton != null) uiManager.actionButton.gameObject.SetActive(false);
                if (uiManager.infoButton != null) uiManager.infoButton.gameObject.SetActive(false);
                if (uiManager.quizButton != null) uiManager.quizButton.gameObject.SetActive(false);
                if (uiManager.scaleRotationSlider != null) uiManager.scaleRotationSlider.gameObject.SetActive(false);
            }
            
            // Hide the "Animasi Penuh" button
            GameObject playAllBtn = FindObjectByName("PlayAllButton");
            if (playAllBtn != null) playAllBtn.SetActive(false);
        }
    }

    private GameObject FindObjectByName(string name)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.name == name && t.gameObject.scene.isLoaded)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    void OnDestroy()
    {
        if (observer != null)
        {
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}
