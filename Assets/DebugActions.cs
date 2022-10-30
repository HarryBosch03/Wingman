using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class DebugActions : MonoBehaviour
{
    string consoleText;
    bool open;

    int historyIndex;
    List<string> commandHistory = new List<string>();

    private void Update()
    {
        if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
        {
            if (open)
            {
                historyIndex = -1;
                consoleText = string.Empty;
                open = false;
                PlayerController.InputBlockers.Remove(this);
            }
            else
            {
                open = true;
                PlayerController.InputBlockers.Add(this);
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame && open)
        {
            ParseCommand();
            commandHistory.Insert(0, consoleText);
            historyIndex = -1;
            consoleText = string.Empty;
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            historyIndex++;
            if (historyIndex >= commandHistory.Count)
            {
                if (commandHistory.Count > 0)
                {
                    historyIndex = commandHistory.Count - 1;
                }
                else
                {
                    historyIndex = -1;
                }
            }

            consoleText = historyIndex == -1 ? string.Empty : commandHistory[historyIndex];
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            historyIndex--;
            if (historyIndex < 0)
            {
                historyIndex = -1;
            }

            consoleText = historyIndex == -1 ? string.Empty : commandHistory[historyIndex];
        }
    }

    private void ParseCommand()
    {
        string[] args = consoleText.Split(' ');
        if (args.Length == 0) return;
        switch (args[0])
        {
            case "dmg":
                if (args.Length == 2)
                {
                    PlayerInput player = FindObjectOfType<PlayerInput>();
                    if (!player) break;

                    PlayerController controller = player.GetComponent<PlayerController>();
                    if (!controller) break;
                    if (!controller.Avatar) break;

                    Health playerHealth = controller.Avatar.GetComponent<Health>();
                    if (!playerHealth) break;

                    if (float.TryParse(args[1], out float damage))
                    {
                        playerHealth.Damage(new DamageArgs(null, damage));
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnGUI()
    {
        if (open)
        {
            int consoleHeight = 50;
            GUI.SetNextControlName("debugActions");
            consoleText = GUI.TextField(new Rect(0, Screen.height - consoleHeight, Screen.width, consoleHeight), consoleText);
            GUI.FocusControl("debugActions");
        }
    }
}
