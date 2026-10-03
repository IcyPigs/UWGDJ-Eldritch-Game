using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class menuManager : MonoBehaviour
{
    // References to gameobjects
    public TextMeshProUGUI completionText;
    public TextMeshProUGUI patienceText;

    // Loads the called scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Completion")
        {
            if(dayManager.instance == null)
            {
                GameObject manager = new GameObject("Day Manager");
                manager.AddComponent<dayManager>();
            }

            SetCompletionStats();
        }
    }

    void SetCompletionStats()
    {
        completionText.text = ("Errors Resolved: " + dayManager.instance.finalCompletions + " / 3");
        patienceText.text = ("Final Patience: " + dayManager.instance.finalPatience + "%");
    }

    public void NextDay() 
    {
        int newIndex = 0;
        int currentDay = dayManager.instance.currentDay;

        int startOfIndex = 0;
        for(int i = 0; i < currentDay; i++)
        {
            startOfIndex += dayManager.instance.dayRanges[i];
        }

        newIndex = Random.Range(startOfIndex, startOfIndex - 1 + dayManager.instance.dayRanges[currentDay]);

        dayManager.instance.SetDayIndex(newIndex);
        SceneManager.LoadScene("UWGDJ");
        dayManager.instance.StartDay();
    }
}
