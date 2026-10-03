using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dayManager : MonoBehaviour
{
    public static dayManager instance { get; private set; }

    [Space]

    public Day[] days; // Stores each possible day
    public int[] dayRanges = new int[5]; // Each value stores how many days are valid for each day
    // dayRanges[0] is valid mondays, dayRanges[1] is valid tuesdays... so on
    public bool[] completionStatus = new bool[5]; // Tracks if the player has completed each day
    
    [Space]

    public bool clockOut; // Indicates if the player is in gameplay
    public int score; // 1-5? for preformance, gets at the end
    public int currentDay; // Track which day the player is on
    
    [Space]

    int dayIndex; // Which day from days[] will be called by StartDay()
    public int finalCompletions; // Used for score card
    public float finalPatience; // Used for score card

    void Awake()
    {
        if(dayManager.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dayIndex = Random.Range(0, dayRanges[0] - 1);
        StartDay();
    }
    
    public void StartDay()
    {
        // Indicates gameplay has started
        clockOut = false;

        // Set Up Gameplay Managers
        dialogueManager.instance.LoadDialogue(days[dayIndex].dialogue);  
        dialogueManager.instance.UpdateDisturbanceLevel(0);

        // Set up character profiles
        dialogueManager.instance.characterProfiles[0].sprite = days[dayIndex].characters[0].profile;
        dialogueManager.instance.characterProfiles[1].sprite = days[dayIndex].characters[1].profile;

        // Set up Patience
        frequencyController.instance.StartDay();
    }

    public void SetDayIndex(int newIndex) { dayIndex = newIndex; }
    public Day GetCurrentDay() { return days[dayIndex]; }

    public void EndDay(bool successState)
    {
        clockOut = true;

        finalCompletions = frequencyController.instance.completions;
        finalPatience = Mathf.Round(frequencyController.instance.patience) / 10;

        if(finalPatience < 0f)
            finalPatience = 0;

        if(successState)
        {
            completionStatus[currentDay] = true;
            Debug.Log("Day " + (currentDay + 1) + ": Pass");
        }
        else
        {
            completionStatus[currentDay] = false;
            Debug.Log("Day " + (currentDay + 1) + ": Fail");
        }

        currentDay++;

        SceneManager.LoadScene("Completion");
    }
}
