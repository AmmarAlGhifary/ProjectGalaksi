using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainHUDPanel;
    public GameObject generalInfoPanel;
    public GameObject quizPanel;

    [Header("Main HUD Elements")]
    public Slider scaleRotationSlider;
    public TextMeshProUGUI planetNameText;
    public Button actionButton;
    public Button infoButton;
    public Button quizButton;

    [Header("Info Panel Elements")]
    public TextMeshProUGUI infoTitleText;
    public TextMeshProUGUI infoBodyText;
    public Button closeInfoButton;

    private PlanetInteractable currentSelectedPlanet;
    private AudioNarrator audioNarrator;
    private QuizManager quizManager;

    void Start()
    {
        audioNarrator = FindObjectOfType<AudioNarrator>();
        quizManager = FindObjectOfType<QuizManager>();

        // Set up button listeners
        if (actionButton != null) actionButton.onClick.AddListener(OnActionClicked);
        if (infoButton != null) infoButton.onClick.AddListener(OnInfoClicked);
        if (quizButton != null) quizButton.onClick.AddListener(OnQuizClicked);
        if (closeInfoButton != null) closeInfoButton.onClick.AddListener(CloseInfoPanel);
        if (scaleRotationSlider != null) scaleRotationSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Hide panels initially
        if (generalInfoPanel != null) generalInfoPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);
        
        // Hide interactive buttons until a planet is selected
        if (actionButton != null) actionButton.gameObject.SetActive(false);
        if (infoButton != null) infoButton.gameObject.SetActive(false);
        if (scaleRotationSlider != null) scaleRotationSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        // Simple touch raycast detection (also works with mouse click in Editor)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                PlanetInteractable planet = hit.collider.GetComponent<PlanetInteractable>();
                if (planet != null)
                {
                    SelectPlanet(planet);
                }
            }
        }
    }

    public void SelectPlanet(PlanetInteractable planet)
    {
        currentSelectedPlanet = planet;
        if (planetNameText != null) planetNameText.text = planet.planetName;

        // Show buttons
        if (actionButton != null) actionButton.gameObject.SetActive(true);
        if (infoButton != null) infoButton.gameObject.SetActive(true);
        if (scaleRotationSlider != null) 
        {
            scaleRotationSlider.gameObject.SetActive(true);
            scaleRotationSlider.value = 0.5f; 
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (currentSelectedPlanet != null)
        {
            currentSelectedPlanet.UpdateTransformFromSlider(value);
        }
    }

    private void OnActionClicked()
    {
        if (currentSelectedPlanet != null)
        {
            currentSelectedPlanet.PlayActionAnimation();
        }
    }

    private void OnInfoClicked()
    {
        if (currentSelectedPlanet != null)
        {
            if (mainHUDPanel != null) mainHUDPanel.SetActive(false);
            if (generalInfoPanel != null) generalInfoPanel.SetActive(true);
            
            if (infoTitleText != null) infoTitleText.text = currentSelectedPlanet.planetName;
            if (infoBodyText != null) infoBodyText.text = currentSelectedPlanet.generalInformation;

            // NEW TASK: Play narration voice when user clicks Read Info
            if (currentSelectedPlanet.narrationAudio != null && audioNarrator != null)
            {
                audioNarrator.PlayNarration(currentSelectedPlanet.narrationAudio);
            }
        }
    }

    private void CloseInfoPanel()
    {
        if (generalInfoPanel != null) generalInfoPanel.SetActive(false);
        if (mainHUDPanel != null) mainHUDPanel.SetActive(true);

        // Stop narration when closing info panel
        if (audioNarrator != null)
        {
            audioNarrator.StopNarration();
        }
    }

    private void OnQuizClicked()
    {
        if (mainHUDPanel != null) mainHUDPanel.SetActive(false);
        if (generalInfoPanel != null) generalInfoPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(true);
        
        if (quizManager != null) 
        {
            if (currentSelectedPlanet != null && currentSelectedPlanet.specificQuizQuestions != null && currentSelectedPlanet.specificQuizQuestions.Count > 0)
            {
                quizManager.StartQuiz(currentSelectedPlanet.specificQuizQuestions);
            }
            else
            {
                quizManager.StartQuiz();
            }
        }
    }
}
