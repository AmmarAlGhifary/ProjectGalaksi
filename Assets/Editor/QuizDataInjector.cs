using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class QuizDataInjector : EditorWindow
{
    [MenuItem("Tools/3. Inject Quiz Data Automatically")]
    public static void InjectData()
    {
        PlanetInteractable[] planets = Resources.FindObjectsOfTypeAll<PlanetInteractable>();
        int modifiedCount = 0;

        foreach (PlanetInteractable p in planets)
        {
            if (p.gameObject.scene.isLoaded) // Only modify objects in the active scene
            {
                if (p.planetName.Equals("Solar System", System.StringComparison.OrdinalIgnoreCase))
                {
                    p.specificQuizQuestions = GetSolarSystemQuiz();
                    p.generalInformation = "Selamat datang di Tata Surya kita! Tata Surya adalah sekumpulan benda langit yang menakjubkan, yang semuanya terikat kuat oleh gaya gravitasi Matahari.\n\nMatahari adalah sebuah bintang pijar raksasa yang menjadi pusat peredaran bagi delapan planet utama. Mulai dari Merkurius yang paling dekat dan panas, Venus yang bersinar terang, Bumi rumah kita yang penuh kehidupan, Mars si planet merah, Jupiter raksasa gas terbesar, Saturnus dengan cincin esnya yang indah, Uranus, hingga Neptunus di bagian paling luar yang dingin dan gelap.\n\nSelain planet-planet besar, Tata Surya kita juga dipenuhi oleh jutaan asteroid, komet pelancong, serta satelit alami seperti Bulan kita. Silakan putar dan perbesar model Tata Surya ini untuk melihat bagaimana semua keajaiban kosmik ini bergerak dalam harmoni yang sempurna!";
                    EditorUtility.SetDirty(p);
                    modifiedCount++;
                }
                else if (p.planetName.Equals("Moon", System.StringComparison.OrdinalIgnoreCase))
                {
                    p.specificQuizQuestions = GetMoonQuiz();
                    p.generalInformation = "Bulan adalah satu-satunya satelit alami Bumi. Ia merupakan benda langit paling terang kedua di langit setelah Matahari dan gravitasi Bulan sangat mempengaruhi pasang surut air laut di Bumi.";
                    EditorUtility.SetDirty(p);
                    modifiedCount++;
                }
                else if (p.planetName.Equals("Sun", System.StringComparison.OrdinalIgnoreCase))
                {
                    p.specificQuizQuestions = GetSunQuiz();
                    p.generalInformation = "Matahari adalah bintang yang berada tepat di pusat Tata Surya kita. Ia merupakan bola plasma panas raksasa yang bercahaya dan menjadi sumber energi terpenting bagi seluruh kehidupan di Bumi.";
                    EditorUtility.SetDirty(p);
                    modifiedCount++;
                }
                else if (p.planetName.Equals("Earth", System.StringComparison.OrdinalIgnoreCase))
                {
                    p.specificQuizQuestions = GetEarthQuiz();
                    p.generalInformation = "Bumi adalah planet ketiga dari Matahari dan satu-satunya planet di alam semesta yang diketahui memiliki kehidupan. Sekitar 71% dari permukaan Bumi ditutupi oleh air laut biru yang luas.";
                    EditorUtility.SetDirty(p);
                    modifiedCount++;
                }
            }
        }

        if (modifiedCount > 0)
        {
            EditorSceneManager.MarkAllScenesDirty();
            Debug.Log($"[QuizDataInjector] Berhasil memasukkan soal kuis ke {modifiedCount} objek!");
        }
        else
        {
            Debug.LogWarning("[QuizDataInjector] Tidak ada PlanetInteractable yang ditemukan atau nama planet tidak cocok.");
        }
    }

    private static List<QuizQuestion> GetSolarSystemQuiz()
    {
        return new List<QuizQuestion>
        {
            new QuizQuestion {
                questionText = "Apa nama galaksi tempat Tata Surya kita berada?",
                answers = new string[] { "Andromeda", "Bima Sakti", "Sombrero", "Triangulum" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Planet manakah yang memiliki ukuran paling besar di Tata Surya?",
                answers = new string[] { "Saturnus", "Uranus", "Jupiter", "Neptunus" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Apa nama sabuk di tepi luar Tata Surya kita (melewati orbit Neptunus)?",
                answers = new string[] { "Sabuk Utama", "Sabuk Kuiper", "Awan Oort", "Sabuk Orion" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Berapa jumlah planet yang secara resmi diakui dalam Tata Surya kita saat ini?",
                answers = new string[] { "7", "8", "9", "10" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Planet mana yang memiliki orbit paling dekat dengan Matahari?",
                answers = new string[] { "Venus", "Bumi", "Mars", "Merkurius" },
                correctAnswerIndex = 3
            }
        };
    }

    private static List<QuizQuestion> GetMoonQuiz()
    {
        return new List<QuizQuestion>
        {
            new QuizQuestion {
                questionText = "Apa fase bulan saat permukaannya gelap dan tidak mendapat cahaya matahari?",
                answers = new string[] { "Bulan Sabit", "Bulan Purnama", "Bulan Baru (New Moon)", "Bulan Cembung" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Siapakah manusia pertama yang berjalan di permukaan Bulan?",
                answers = new string[] { "Yuri Gagarin", "Buzz Aldrin", "Neil Armstrong", "Alan Shepard" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Berapa lama waktu yang dibutuhkan Bulan untuk mengelilingi Bumi?",
                answers = new string[] { "Sekitar 7 hari", "Sekitar 14 hari", "Sekitar 27,3 hari", "Sekitar 365 hari" },
                correctAnswerIndex = 2
            }
        };
    }

    private static List<QuizQuestion> GetSunQuiz()
    {
        return new List<QuizQuestion>
        {
            new QuizQuestion {
                questionText = "Reaksi apa yang terjadi di inti Matahari yang menghasilkan panas dan cahaya?",
                answers = new string[] { "Fisi Nuklir", "Reaksi Kimia", "Fusi Nuklir", "Elektromagnetik" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Apa nama lapisan atmosfer paling luar dari Matahari saat terjadi gerhana?",
                answers = new string[] { "Kromosfer", "Fotosfer", "Korona", "Zona Radiatif" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Unsur gas apa yang paling banyak kedua di Matahari setelah Hidrogen?",
                answers = new string[] { "Oksigen", "Karbon", "Helium", "Nitrogen" },
                correctAnswerIndex = 2
            }
        };
    }

    private static List<QuizQuestion> GetEarthQuiz()
    {
        return new List<QuizQuestion>
        {
            new QuizQuestion {
                questionText = "Berapa persentase rata-rata permukaan Bumi yang tertutup oleh perairan?",
                answers = new string[] { "30%", "50%", "71%", "85%" },
                correctAnswerIndex = 2
            },
            new QuizQuestion {
                questionText = "Apa nama lapisan struktur paling luar dari Bumi tempat kita berpijak?",
                answers = new string[] { "Inti Dalam", "Kerak Bumi", "Mantel Bumi", "Inti Luar" },
                correctAnswerIndex = 1
            },
            new QuizQuestion {
                questionText = "Apa gas penyusun utama yang paling melimpah di atmosfer Bumi?",
                answers = new string[] { "Oksigen", "Karbon Dioksida", "Argon", "Nitrogen" },
                correctAnswerIndex = 3
            }
        };
    }
}
