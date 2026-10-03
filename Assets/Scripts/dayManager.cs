using UnityEngine;
using UnityEngine.UI;

public class dayManager : MonoBehaviour
{
    public static dayManager instance { get; private set; }

    [Space]

    public Day[] days; // Stores each possible day
    public bool[] completionStatus = new bool[5]; // Tracks if the player has completed each day
    
    [Space]

    public bool clockOut; // Indicates if the player is in gameplay
    public int score; // 1-5? for preformance, gets at the end
    public int currentDay; // Track which day the player is on

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
        StartDay();
    }
    
    void StartDay()
    {
        // Indicates gameplay has started
        clockOut = false;

        // Set Up Gameplay Managers
        dialogueManager.instance.LoadDialogue(days[currentDay].dialogue);  
        dialogueManager.instance.UpdateDisturbanceLevel(0);

        // Set up character profiles
        dialogueManager.instance.characterProfiles[0].sprite = days[currentDay].characters[0].profile;
        dialogueManager.instance.characterProfiles[1].sprite = days[currentDay].characters[1].profile;

        // Set up Patience
        frequencyController.instance.StartDay();
    }

    public void EndDay(bool successState)
    {
        clockOut = true;

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
    }
}
