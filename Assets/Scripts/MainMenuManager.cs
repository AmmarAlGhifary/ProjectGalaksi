using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject arHUDPanel;
    public GameObject quizPanel;
    public GameObject generalInfoPanel;

    [Header("Menu Buttons")]
    public Button playARButton;
    public Button playQuizButton;
    public Button exitButton;

    private GameObject arExitButton;

    void Start()
    {
        // Add button listeners
        if (playARButton != null) playARButton.onClick.AddListener(StartARMode);
        if (playQuizButton != null) playQuizButton.onClick.AddListener(StartQuizMode);
        if (exitButton != null) exitButton.onClick.AddListener(ExitApp);

        // Find the AR exit button by name
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.name == "ARExitButton" && t.gameObject.scene.isLoaded)
            {
                arExitButton = t.gameObject;
                Button btn = arExitButton.GetComponent<Button>();
                if (btn != null) btn.onClick.AddListener(ShowMainMenu);
                break;
            }
        }

        // Always start at the Main Menu when the app opens
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        // 1. Turn off the AR Camera to save battery and hide the real world
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = false;
        }
            
        // 2. Show only the Main Menu
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (arHUDPanel != null) arHUDPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);
        if (generalInfoPanel != null) generalInfoPanel.SetActive(false);
        if (arExitButton != null) arExitButton.SetActive(false);
    }

    public void StartARMode()
    {
        // Hide menu
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        
        // Show the back button so user can always exit AR
        if (arExitButton != null) arExitButton.SetActive(true);
        
        // Turn on the AR Camera
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = true;
        }

        // Note: We don't turn on arHUDPanel here. 
        // Vuforia's Image Target will turn it on when it sees the physical marker!
    }

    public void StartQuizMode()
    {
        // Hide menu, ensure camera is off
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (VuforiaBehaviour.Instance != null) VuforiaBehaviour.Instance.enabled = false;
        
        // Find QuizManager anywhere in the scene (it might be on a different GameManager)
        QuizManager qm = FindObjectOfType<QuizManager>();
        if (qm != null && quizPanel != null)
        {
            quizPanel.SetActive(true);
            qm.StartQuiz();
        }
        else
        {
            // Fallback: if quizPanel reference is broken, try to find it
            if (quizPanel == null)
            {
                Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
                foreach(Transform t in allTransforms)
                {
                    if (t.name == "QuizPanel")
                    {
                        quizPanel = t.gameObject;
                        break;
                    }
                }
            }
            if (quizPanel != null)
            {
                quizPanel.SetActive(true);
                if (qm != null) qm.StartQuiz();
            }
            else
            {
                Debug.LogError("Could not find QuizPanel anywhere!");
                // Go back to menu so user isn't stuck on black screen
                ShowMainMenu();
            }
        }
    }

    public void ExitApp()
    {
        Application.Quit();
        Debug.Log("App Exited!"); // This only shows in the Unity Editor
    }
}
