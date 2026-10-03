using UnityEngine;
using UnityEngine.UI;

public class dayManager : MonoBehaviour
{
    public static dayManager instance { get; private set; }

    public Slider patienceBar; // Reference to patience bar

    public Day[] days; // Stores each possible day
    public bool[] completionStatus = new bool[5]; // Tracks if the player has completed each day
    [Space]

    public bool clockOut; // Indicates if the player is in gameplay
    public int score; // 1-5? for preformance, gets at the end
    public float patience; // Tracks the client patience level
    public int currentDay; // Track which day the player is on

    float decayTimer = 0f; // Timer for patience decay
    float decayRate = 1f; // Rate at which patience decays, referenced by Day[x].patienceDecayRate

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
        resetPatience();
        dialogueManager.instance.LoadDialogue(days[currentDay].dialogue);  

        decayRate = days[currentDay].patienceDecayRate; 
        decayTimer = decayRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(!clockOut)
            decayTimer -= Time.deltaTime;

        if(decayTimer <= 0f)
        {
            decay();
            decayTimer = decayRate;
        }
    }
    
    void resetPatience() {
        patience = 1000;
    }

    void decay()
    {
        patience -= 10f  * (1f + Mathf.Log(1f + frequencyController.instance.CheckPlayerAccuracy())) / days[currentDay].leniency;
        patienceBar.value = patience;
        
        // If patience is 0 or less, end the day with a failure state
        if(patience <= 0f)
            EndDay(false);
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
