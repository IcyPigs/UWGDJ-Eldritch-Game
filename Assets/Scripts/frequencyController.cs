using UnityEngine;

public class frequencyController : MonoBehaviour
{
    [SerializeField] LineRenderer targetWave;
    [SerializeField] LineRenderer playerWave;

    public float[] targetFrequency = new float[3];
    public float[] playerFrequency = new float[3];

    public int targetVariable = 0;
    public float leniencyPercent;

    void Start()
    {
        RandomizeTargetFrequency();
    }

    // Update is called once per frame
    void Update()
    {
        SetFrequency(targetWave, targetFrequency[0], targetFrequency[1], targetFrequency[2]);
        SetFrequency(playerWave, playerFrequency[0], playerFrequency[1], playerFrequency[2]);
    }

    void RandomizeTargetFrequency()
    {
        targetFrequency[0] = Random.Range(0.1f, 1.2f);
        targetFrequency[1] = Random.Range(1f, 5f);
        targetFrequency[2] = Random.Range(0f, 3f);
    }

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

    public void SetTargetVariable(int target)
    {
        targetVariable = target;
        Debug.Log(CheckPlayerAccuracy());

    }

    public void SliderModifyVariable(float value)
    {
        playerFrequency[targetVariable] = value;
        CheckPlayerAccuracy();
    }

    float CheckPlayerAccuracy()
    {
        float ampAccuracy = Mathf.Abs((playerFrequency[0] - targetFrequency[0]) / targetFrequency[0]) * 100f;
        float widthAccuracy = Mathf.Abs((playerFrequency[1] - targetFrequency[1]) / targetFrequency[1]) * 100f;
        float offsetAccuracy = Mathf.Abs((playerFrequency[2] - targetFrequency[2]) / targetFrequency[2]) * 100f;

        float totalAccuracy = (ampAccuracy + widthAccuracy + offsetAccuracy) / 3;

        if(totalAccuracy <= (leniencyPercent))
        {
            RandomizeTargetFrequency();
        }

        return totalAccuracy;
    }
}
