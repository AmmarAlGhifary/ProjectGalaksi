using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class QuizQuestion
{
    public string questionText;
    public string[] answers = new string[4];
    public int correctAnswerIndex;
}

public class QuizManager : MonoBehaviour
{
    [Header("Quiz UI Elements")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public Button[] answerButtons;
    public Button closeQuizButton;
    public GameObject mainHUDPanel;

    [Header("Quiz Data")]
    public List<QuizQuestion> questions;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private bool quizFinished = false;

    void Start()
    {
        Debug.Log("[QuizManager] Start() called on GameObject: " + gameObject.name);
        
        // Add listeners to buttons safely
        if (answerButtons != null)
        {
            Debug.Log("[QuizManager] answerButtons array length: " + answerButtons.Length);
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] != null)
                {
                    Debug.Log("[QuizManager] Answer button " + i + " found: " + answerButtons[i].gameObject.name);
                    int index = i; // local copy for closure
                    answerButtons[i].onClick.AddListener(() => OnAnswerClicked(index));
                }
                else
                {
                    Debug.LogWarning("[QuizManager] Answer button " + i + " is NULL!");
                }
            }
        }
        else
        {
            Debug.LogError("[QuizManager] answerButtons array is NULL!");
        }

        if (closeQuizButton != null)
        {
            Debug.Log("[QuizManager] closeQuizButton found: " + closeQuizButton.gameObject.name);
            closeQuizButton.onClick.AddListener(CloseQuiz);
        }
        else
        {
            Debug.LogError("[QuizManager] closeQuizButton is NULL! Exit will not work!");
        }
        
    }

    private void LoadDefaultQuestions()
    {
        // Indonesian quiz questions about the Solar System (Fallback)
        questions = new List<QuizQuestion>()
        {
            new QuizQuestion {
                questionText = "Apa pusat dari tata surya kita?",
                answers = new string[] { "Bumi", "Bulan", "Matahari", "Jupiter" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Berapa lama waktu yang dibutuhkan Bumi untuk mengelilingi Matahari satu putaran penuh?",
                answers = new string[] { "24 Jam", "365 Hari", "30 Hari", "12 Jam" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Bulan adalah satu-satunya satelit alami milik planet apa?",
                answers = new string[] { "Mars", "Venus", "Saturnus", "Bumi" },
                correctAnswerIndex = 3
            },
            new QuizQuestion {
                questionText = "Gas apa yang paling banyak menyusun Matahari?",
                answers = new string[] { "Oksigen", "Hidrogen", "Karbon Dioksida", "Nitrogen" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Mengapa kita hanya bisa melihat satu sisi permukaan Bulan dari Bumi?",
                answers = new string[] { "Rotasi dan revolusi Bulan sama", "Sisi lainnya gelap gulita", "Bulan tidak berputar sama sekali", "Tertutup oleh awan luar angkasa" },
                correctAnswerIndex = 0
            },
            new QuizQuestion {
                questionText = "Planet manakah yang dikenal sebagai planet biru?",
                answers = new string[] { "Mars", "Venus", "Bumi", "Neptunus" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Berapa suhu permukaan Matahari kira-kira?",
                answers = new string[] { "1.000 derajat Celcius", "5.500 derajat Celcius", "100.000 derajat Celcius", "500 derajat Celcius" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Apa nama fenomena ketika Bulan menutupi Matahari sepenuhnya?",
                answers = new string[] { "Gerhana Bulan", "Gerhana Matahari Total", "Supermoon", "Komet" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Lapisan atmosfer Bumi yang melindungi kita dari sinar ultraviolet adalah?",
                answers = new string[] { "Troposfer", "Stratosfer (Lapisan Ozon)", "Mesosfer", "Termosfer" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Berapa jarak rata-rata antara Bumi dan Matahari?",
                answers = new string[] { "150 juta km", "50 juta km", "500 juta km", "1 miliar km" },
                correctAnswerIndex = 0
            },
        };
    }

    public void StartQuiz(List<QuizQuestion> customQuestions = null)
    {
        if (customQuestions != null && customQuestions.Count > 0)
        {
            questions = customQuestions;
        }
        else
        {
            LoadDefaultQuestions();
        }

        currentQuestionIndex = 0;
        score = 0;
        quizFinished = false;
        
        // Make sure close button text says "Keluar Kuis"
        if (closeQuizButton != null)
        {
            TextMeshProUGUI closeBtnText = closeQuizButton.GetComponentInChildren<TextMeshProUGUI>();
            if (closeBtnText != null) closeBtnText.text = "Keluar Kuis";
        }
        
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            QuizQuestion q = questions[currentQuestionIndex];
            if (questionText != null) questionText.text = q.questionText;
            if (scoreText != null) scoreText.text = $"Skor: {score} / {questions.Count}";

            if (answerButtons != null)
            {
                for (int i = 0; i < answerButtons.Length; i++)
                {
                    if (answerButtons[i] != null)
                    {
                        answerButtons[i].gameObject.SetActive(true);
                        TextMeshProUGUI btnText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                        if (btnText != null) btnText.text = q.answers[i];
                    }
                }
            }
        }
        else
        {
            // Quiz finished!
            quizFinished = true;
            if (questionText != null) questionText.text = "Kuis Selesai!";
            if (scoreText != null) scoreText.text = $"Skor Akhir: {score} / {questions.Count}";
            
            // Hide answer buttons
            if (answerButtons != null)
            {
                foreach (var btn in answerButtons)
                {
                    if (btn != null) btn.gameObject.SetActive(false);
                }
            }
            
            // Change close button text to "Kembali ke Menu"
            if (closeQuizButton != null)
            {
                TextMeshProUGUI closeBtnText = closeQuizButton.GetComponentInChildren<TextMeshProUGUI>();
                if (closeBtnText != null) closeBtnText.text = "Kembali ke Menu";
            }
        }
    }

    private void OnAnswerClicked(int buttonIndex)
    {
        if (currentQuestionIndex < questions.Count)
        {
            if (buttonIndex == questions[currentQuestionIndex].correctAnswerIndex)
            {
                score++;
            }
            
            currentQuestionIndex++;
            DisplayQuestion();
        }
    }

    private void CloseQuiz()
    {
        Debug.Log("[QuizManager] CloseQuiz() called!");
        
        // Find QuizPanel by searching the ENTIRE SCENE (not by walking up parents)
        // because QuizManager lives on GameManager, NOT on QuizPanel
        GameObject quizPanel = null;
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.name == "QuizPanel" && t.gameObject.scene.isLoaded)
            {
                quizPanel = t.gameObject;
                break;
            }
        }

        if (quizPanel != null)
        {
            Debug.Log("[QuizManager] Found and hiding QuizPanel");
            quizPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("[QuizManager] Could not find QuizPanel!");
        }
        
        // Find MainMenuManager by searching ALL objects including inactive ones
        MainMenuManager menuManager = null;
        MainMenuManager[] allMenuManagers = Resources.FindObjectsOfTypeAll<MainMenuManager>();
        foreach (MainMenuManager mm in allMenuManagers)
        {
            if (mm.gameObject.scene.isLoaded)
            {
                menuManager = mm;
                break;
            }
        }
        
        Debug.Log("[QuizManager] MainMenuManager found: " + (menuManager != null));
        if (menuManager != null)
        {
            Debug.Log("[QuizManager] Calling ShowMainMenu()...");
            menuManager.ShowMainMenu();
        }
        else
        {
            Debug.LogError("[QuizManager] MainMenuManager NOT FOUND!");
        }
    }
}
