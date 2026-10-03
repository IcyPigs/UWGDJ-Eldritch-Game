using UnityEngine;
using UnityEngine.UI;

public class frequencyController : MonoBehaviour
{
    public static frequencyController instance { get; private set;}

    public Slider patienceBar; // Reference to patience bar

    [SerializeField] LineRenderer targetWave;
    [SerializeField] LineRenderer playerWave;

    // Frequency is [amplitude, width, offset]
    public float[] targetFrequency = new float[3];
    public float[] playerFrequency = new float[3];

    // target variable is used for sliders
    public int targetVariable = 0;
    // leniencyPercent is how close the player must be to the target frequency
    public float leniencyPercent;

    public float patience; // Tracks the client patience level
    public float characterLeniency;

    float decayTimer = 0f; // Timer for patience decay
    float decayRate = 1f; // Rate at which patience decays, referenced by Day[x].patienceDecayRate


    // Counts how many times the player has matched the target frequency
    public int completions = 0;

    void Awake()
    {
        if(frequencyController.instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    public void StartDay()
    {
        // Randomize starting target frequency
        RandomizeTargetFrequency();

        // Set up Patience
        resetPatience();
        decayRate = dayManager.instance.days[dayManager.instance.currentDay].patienceDecayRate; 
        decayTimer = decayRate;

        // Set gameplay variables
        characterLeniency = dayManager.instance.days[dayManager.instance.currentDay].leniency;
    }

    // Update is called once per frame
    void Update()
    {
        // Updates display of frequency waves
        SetFrequency(targetWave, targetFrequency[0], targetFrequency[1], targetFrequency[2]);
        SetFrequency(playerWave, playerFrequency[0], playerFrequency[1], playerFrequency[2]);

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
        patience -= 10f  * (1f + Mathf.Log(1f + CheckPlayerAccuracy())) / characterLeniency;
        patienceBar.value = patience;
        
        // If patience is 0 or less, end the day with a failure state
        if(patience <= 0f)
            dayManager.instance.EndDay(false);
    }

    // Randomizes the target frequency values
    public void RandomizeTargetFrequency()
    {
        targetFrequency[0] = Random.Range(0.1f, 1.2f);
        targetFrequency[1] = Random.Range(1f, 5f);
        targetFrequency[2] = Random.Range(0f, 3f);
    }

    // Sets the frequency of a LineRenderer based on amplitude, width, and offset
    void SetFrequency(LineRenderer line, float amp, float width, float offset)
    {
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);

        for (int x = 0; x < positions.Length; x++)
        {
            float localx = x / 10f;
            Vector3 newPos = new Vector3(localx, amp * Mathf.Sin((localx + offset + Time.time) * width), 0);
            line.SetPosition(x, newPos);
        }
    }

    // Used by sliders for player controls
    public void SetTargetVariable(int target)
    {
        targetVariable = target;

    }

    // Used by sliders for player controls
    public void SliderModifyVariable(float value)
    {
        playerFrequency[targetVariable] = value;
        // Check if the player has matched the target frequency
        CheckPlayerAccuracy();
    }

    // Averages how accurate the player amplitude, width and offset are to see if they player is close enough to the target frequency to count as a match. If they are close enough, it adds a completion step and randomizes the target frequency.
    public float CheckPlayerAccuracy()
    {
        // Get the accuracy of each variable as a percentage
        float ampAccuracy = Mathf.Abs((playerFrequency[0] - targetFrequency[0]) / targetFrequency[0]) * 100f;
        float widthAccuracy = Mathf.Abs((playerFrequency[1] - targetFrequency[1]) / targetFrequency[1]) * 100f;
        float offsetAccuracy = Mathf.Abs((playerFrequency[2] - targetFrequency[2]) / targetFrequency[2]) * 100f;

        // Get the average accuracy of the three variables
        float totalAccuracy = (ampAccuracy + widthAccuracy + offsetAccuracy) / 3;

        // If average accuracy is within the leniency percent, add a completion step
        if(totalAccuracy <= (leniencyPercent))
        {
            AddCompletionStep();
        }

        return totalAccuracy;
    }

    // Adds a completion step and randomizes the target frequency
    void AddCompletionStep()
    {
        completions++;
        RandomizeTargetFrequency();
        dialogueManager.instance.UpdateDisturbanceLevel(completions);
        FindAnyObjectByType<audioManager>().Play("Robot Affirm");
        Debug.Log("Completion Step");

        // If the player has completed 3 matches, they win
        if(completions >= 3)
        {
            dayManager.instance.EndDay(true);
        }
    }
}
