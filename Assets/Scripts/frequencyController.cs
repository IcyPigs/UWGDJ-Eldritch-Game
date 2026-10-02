using UnityEngine;

public class frequencyController : MonoBehaviour
{
    [SerializeField] LineRenderer targetWave;
    [SerializeField] LineRenderer playerWave;

    // Frequency is [amplitude, width, offset]
    public float[] targetFrequency = new float[3];
    public float[] playerFrequency = new float[3];

    // target variable is used for sliders
    public int targetVariable = 0;
    // leniencyPercent is how close the player must be to the target frequency
    public float leniencyPercent;

    // Counts how many times the player has matched the target frequency
    public int completions = 0;

    void Start()
    {
        RandomizeTargetFrequency();
    }

    // Update is called once per frame
    void Update()
    {
        // Updates display of frequency waves
        SetFrequency(targetWave, targetFrequency[0], targetFrequency[1], targetFrequency[2]);
        SetFrequency(playerWave, playerFrequency[0], playerFrequency[1], playerFrequency[2]);
    }

    // Randomizes the target frequency values
    void RandomizeTargetFrequency()
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
    float CheckPlayerAccuracy()
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

        // If the player has completed 3 matches, they win
        if(completions >= 3)
        {
            Debug.Log("Win Condition Met");
        }
    }
}
