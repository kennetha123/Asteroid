using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if DEBUG

public class DebugController : MonoBehaviour
{
    #region Public/Serializeable Variable

    public float boxHeight = 20;

    public static DebugCommand<int> SetScore;
    public static DebugCommand<int> SpawnUFO;
    public static DebugCommand GodMode;

    public List<object> commandList;

    #endregion

    #region Private/Hidden Variable

    // command selected in commmand list button (simple intelisense)
    private int selectedCommand = -1;

    private bool showConsole;

    private string input = "";

    private List<string> allCommandSelection = new List<string>();  // All Command List In String
    private List<string> commandSelection = new List<string>();     // All Command List That contain what in input (like in input "a", then every command that have "a", will show)

    private Vector2 scrollPos;

    private GameManager manager;

    #endregion

    private void Awake()
    {
        manager = GetComponent<GameManager>();

        #region Command

        // create new command called "test"
        SetScore = new DebugCommand<int>("set_score", "change current score value", "set_score <amount>", (x) =>
        {
            manager.currentScore = x;
        });

        // create new command that have parameter called "print"
        SpawnUFO = new DebugCommand<int>("spawn_ufo", "spawn ufo into the scene", "spawn_ufo <type 1 or 2>", (x) =>
        {
            manager.CreateUFO(x - 1);
        });

        GodMode = new DebugCommand("god_mode", "make spaceship invulnerable from any damage", "god_mode", () =>
        {
            Collider2D col = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            col.enabled = !col.enabled;
        });

        #endregion

        commandList = new List<object>
        {
            SetScore,
            SpawnUFO,
            GodMode
        };

        // get all command format in the command list
        allCommandSelection = commandList.Select(x => (x as DebugCommandBase).commandFormat).ToList();

        // set default command selection to all
        commandSelection = allCommandSelection;
    }

    private void Update()
    {
        // open / close debug console
        if (Input.GetKeyDown(KeyCode.BackQuote) || input.Contains("`"))
        {
            input = "";
            selectedCommand = -1;
            showConsole = !showConsole;
        }
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            input = "";
            selectedCommand = -1;
            showConsole = !showConsole;
        }

        if (showConsole)
        {
            #region GUI

                #region Command Field

                    GUI.SetNextControlName("Command Field");

                        input = GUI.TextField(new Rect(5f, 5f, Screen.width - 10f, boxHeight), input);
            
                    GUI.FocusControl("Command Field");

                #endregion

                // Simple Intelisense
                #region Intelisense

                    #region Listing

                        // create new gui layout area for positioning
                        GUILayout.BeginArea(new Rect(0, boxHeight + 10f, Screen.width / 2, 100));

                            // create horizontal layout (make it easier if want to add a "detail command" in future)
                            GUILayout.BeginHorizontal();

                                #region ScrollView

                                scrollPos = GUILayout.BeginScrollView(
                                    scrollPos,
                                    GUILayout.Width(Screen.width / 4)
                                );

                                    GUI.skin.button.alignment = TextAnchor.MiddleLeft;

                                    selectedCommand = GUILayout.SelectionGrid(
                                        selectedCommand,
                                        commandSelection.ToArray(),
                                        1
                                    );

                                    GUI.skin.button.alignment = TextAnchor.MiddleCenter;

                                GUILayout.EndScrollView();

                                #endregion

                            GUILayout.EndHorizontal();

                        GUILayout.EndArea();

                        if(selectedCommand != -1)
                        {
                            input = commandSelection[selectedCommand].Split(' ')[0];
                            selectedCommand = -1;
                        }

                    #endregion

                    if (input != "")
                    {
                        string[] properties = input.Split(' ');
                        commandSelection = allCommandSelection.Where(x => x.Contains(properties[0])).ToList();
                    }
                    else
                    {
                        commandSelection = allCommandSelection;
                    }

                #endregion

            #endregion

            if (Event.current.keyCode == KeyCode.Return)
            {
                HandleInput();
                input = "";
            }
        }
    }

    /// <summary>
    /// Call Function Based on Input
    /// </summary>
    private void HandleInput()
    {
        // split command properties written in input
        string[] properties = input.Split(' ');

        for(int i = 0; i < commandList.Count; i++)
        {
            // check command properties to all command list id
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (properties[0] == commandBase.commandId)
            {
                // call the command
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }

}

#endif