using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IncomeManager : MonoBehaviour
{
    public Button[] buttons; // Array of buttons
    public Text[] stackTexts; // Array of text fields to display the stack counts
    public Text goldText; // Text field to display the current gold amount

    private int[] buttonStacks; // Array to track button stacks
    private float[] buttonClickCooldowns; // Array to track button click cooldowns
    private float[] stackRestoreTimers; // Array to track stack restoration timers

    public int currentIncome; // Current income amount
    public int currentGold; // Current gold amount

    private void Start()
    {
        buttonStacks = new int[buttons.Length];
        buttonClickCooldowns = new float[buttons.Length];
        stackRestoreTimers = new float[buttons.Length];

        // Initialize button stacks, cooldowns, and timers
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonStacks[i] = 10;
            buttonClickCooldowns[i] = 0f;
            stackRestoreTimers[i] = 3f;
            stackTexts[i].text = buttonStacks[i].ToString(); // Update the displayed stack count
        }

        StartCoroutine(AddGold());
        StartCoroutine(RestoreStacks());
    }

    private IEnumerator AddGold()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Generate income every 5 seconds
            GenerateIncome();
            goldText.text = currentGold.ToString(); // Update the displayed gold amount
        }
    }

    private IEnumerator RestoreStacks()
    {
        while (true)
        {
            yield return null;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttonStacks[i] < 10)
                {
                    stackRestoreTimers[i] -= Time.deltaTime;

                    if (stackRestoreTimers[i] <= 0f)
                    {
                        buttonStacks[i]++;
                        stackTexts[i].text = buttonStacks[i].ToString(); // Update the displayed stack count
                        stackRestoreTimers[i] = 3f; // Reset the stack restoration timer
                    }
                }
            }
        }
    }

    private void Update()
    {
        // Update button click cooldowns
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttonClickCooldowns[i] > 0)
            {
                buttonClickCooldowns[i] -= Time.deltaTime;
                if (buttonClickCooldowns[i] < 0)
                {
                    buttonClickCooldowns[i] = 0f;
                }
            }
        }
    }

    private void GenerateIncome()
    {
        currentGold += currentIncome;
    }

    public void OnButtonClick(int buttonIndex)
    {
        if (buttonStacks[buttonIndex] > 0 && buttonClickCooldowns[buttonIndex] <= 0f)
        {
            currentIncome += buttons[buttonIndex].GetComponent<ButtonIncome>().incomeValue;
            buttonStacks[buttonIndex]--;
            stackTexts[buttonIndex].text = buttonStacks[buttonIndex].ToString(); // Update the displayed stack count

            buttonClickCooldowns[buttonIndex] = 1f; // Set the button click cooldown for the clicked button
            stackRestoreTimers[buttonIndex] = 3f; // Start the stack restoration timer for the clicked button
        }
    }
}