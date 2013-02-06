﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;
using GameConstructLibrary;

namespace Chimera
{
    /// <summary>
    /// Console used to input debug commands.
    /// </summary>
    public class DebugConsole : WindowControl
    {
        public delegate void ConsoleCommand(List<string> parameters);
        private static Dictionary<string, ConsoleCommand> mCommands = new Dictionary<string, ConsoleCommand>();
        public static bool IsVisible
        {
            get
            {
                return ConsoleInput.Enabled;
            }
        }

        public static InputControl ConsoleInput = new InputControl();

        private static List<string> mCommandHistory = new List<string>();
        private static int mCommandHistoryIndex = 0;
        
        /// <summary>
        /// Construct a new DebugConsole.
        /// </summary>
        public DebugConsole()
        {
            ConsoleInput.Text = "";
            mCommandHistory.Add("");
            ConsoleInput.Bounds = new UniRectangle(120.0f, 40.0f, 140.0f, 30.0f);
            Children.Add(ConsoleInput);
            Bounds.Size = new UniVector(0, 0);
            Bounds.Top = new UniScalar(-100);
        }

        /// <summary>
        /// Add a new command to the console.
        /// </summary>
        /// <param name="commandName">The string used to invoke the command.</param>
        /// <param name="command">The command that should be invoked.</param>
        public static void AddCommand(string commandName, ConsoleCommand command)
        {
            mCommands.Add(commandName, command);
        }

        /// <summary>
        /// Remove a command from the console.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        public static void RemoveCommand(string commandName)
        {
            mCommands.Remove(commandName);
        }

        /// <summary>
        /// Run a particular command with given parameters.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="parameters">The parameters as a list of strings.</param>
        public static void CallCommand(string commandName, List<string> parameters)
        {
            if (mCommands.ContainsKey(commandName))
            {
                mCommands[commandName].Invoke(parameters);
            }
        }

        /// <summary>
        /// Hide the console.
        /// </summary>
        public static void Hide()
        {
            ConsoleInput.Bounds.Location = new UniVector(0, -100);
            ConsoleInput.Enabled = false;
        }
        
        /// <summary>
        /// Show the console.
        /// </summary>
        public static void Show()
        {
            ConsoleInput.Bounds.Location = new UniVector(0, 100);
            ConsoleInput.Enabled = true;
            ConsoleInput.Text = "";
        }

        /// <summary>
        /// Run the command currently entered in the console.
        /// </summary>
        public static void RunEnteredCommand()
        {
            string command = ConsoleInput.Text.Replace("\n", "");
            if (command.Length > 0)
            {
                // Trim command to text before and newlines
                ConsoleInput.Text = command;
                // Add command to history
                mCommandHistory.Insert(mCommandHistory.Count - 1, command);
                string[] commandArgs = ConsoleInput.Text.Split(new char[] { ' ' });
                if (commandArgs.Length > 0)
                {
                    List<string> parameters = new List<string>();
                    int count = 0;
                    foreach (string parameter in commandArgs)
                    {
                        if (count > 0)
                        {
                            parameters.Add(parameter);
                        }
                        count++;
                    }
                    CallCommand(commandArgs[0], parameters);
                }
                ConsoleInput.Text = "";
                mCommandHistoryIndex = mCommandHistory.Count - 1;
            }
        }

        /// <summary>
        /// Navigates to the previously entered command, if any.
        /// </summary>
        public static void NavigateToPreviousCommand()
        {
            mCommandHistoryIndex = mCommandHistoryIndex > 0 ? mCommandHistoryIndex - 1 : mCommandHistoryIndex;
            ConsoleInput.Text = mCommandHistory[mCommandHistoryIndex];
        }

        /// <summary>
        /// Navigates to the command entered after the current, if any.
        /// </summary>
        public static void NavigateToNextCommand()
        {
            mCommandHistoryIndex = mCommandHistoryIndex < mCommandHistory.Count - 1 ? mCommandHistoryIndex + 1 : mCommandHistoryIndex;
            ConsoleInput.Text = mCommandHistory[mCommandHistoryIndex];
        }
    }
}
