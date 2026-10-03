using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogueManager : MonoBehaviour
{
    // Make dialogue manager always accessible
    [HideInInspector]
    public static dialogueManager instance { get; private set;}

    // Dialogue to be played
    public Dialogue dialogue;

    [Space]
    [Header("UI Elements")]

    public Image profile;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statementText;

    [Space]
    [Header("Disturbance Settings")]

    // How connected the call is, causes distrubance to dialogue
    public float timeBetweenDisturbance = 2f;
    public float connection = 50f;

    int currentLine = 0;
    float timerToScroll = 0f;
    float timerToDisturb = 0f;
    string currentStatement = "";

    // Make sure only one instance of dialogueManager exists at a time
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        // Scrolls through the dialogue according to timeToRead
        timerToScroll -= Time.deltaTime;

        if (timerToScroll <= 0f)
        {
            // Update the current line, if the line is not the last line, display the next line
            currentLine++;
            if (currentLine < dialogue.lines.Length)
            {
                NextLine(currentLine);
            }
        }

        // Disturbs the dialogue according to connection
        timerToDisturb -= Time.deltaTime;

        if (timerToDisturb <= 0f)
        {
            timerToDisturb = Random.Range(0, timeBetweenDisturbance);
            statementText.text = DisturbDialogue(currentStatement);
        }
    }

    // Allows other scripts to load a new dialogue into the dialogue manager
    public void LoadDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;

        // Ensures first dialogue is displayed
        NextLine(0);
    }

    // Displays the next line at the lineIndex
    void NextLine(int lineIndex)
    {
        Line newLine = dialogue.lines[lineIndex];

        timerToScroll = newLine.timeToRead;
        DisplayLine(newLine);
    }

    // Displays the current line of dialogue in the UI
    void DisplayLine(Line line)
    {
        profile.sprite = line.character.profile;
        nameText.text = line.character.name;
        currentStatement = line.statement;
        timerToDisturb = 0;
    }

    // Skips to next line of dialogue, cycles to start if the dialogue has ended
    public void SkipDialogue()
    {
        if (currentLine >= dialogue.lines.Length - 1)
        {
            currentLine = 0;
            NextLine(currentLine);
        }
        else
        {
            currentLine++;
            NextLine(currentLine);
        }
    }

    string DisturbDialogue(string statement)
    {
        // Calculate how many characters to disturb based on connection percentage
        int charactersToDisturb = statement.Length - Mathf.FloorToInt(statement.Length * (connection / 100f));

        string newStatement = statement;

        // If not changing any characters, return the original statement
        if (charactersToDisturb <= 0)
        {
            return newStatement;
        }

        // Disturbs the dialogue by changing random characters to random code looking characters
        for(int i = 0; i < charactersToDisturb; i++)
        {
            // Grabs a random character in the string
            int randomIndex = Random.Range(0, statement.Length);

            // Get a random code looking character from the ASCII table
            char randomChar = (char)Random.Range(33, 64);

            // Changes the random character to the random letter
            newStatement = newStatement.Remove(randomIndex, 1).Insert(randomIndex, randomChar.ToString());
        }

        // Returns the disturbed statement
        return newStatement;
    }
}
