using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;
using GameConstructLibrary;

namespace finalProject
{
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

        public DebugConsole()
        {
            ConsoleInput.Text = "";
            ConsoleInput.Bounds = new UniRectangle(120.0f, 40.0f, 140.0f, 30.0f);
            Children.Add(ConsoleInput);
            Bounds.Size = new UniVector(0, 0);
            Bounds.Top = new UniScalar(-100);
        }

        public static void AddCommand(string commandName, ConsoleCommand command)
        {
            mCommands.Add(commandName, command);
        }

        public static void RemoveCommand(string commandName)
        {
            mCommands.Remove(commandName);
        }

        public static void CallCommand(string commandName, List<string> parameters)
        {
            if (mCommands.ContainsKey(commandName))
            {
                mCommands[commandName].Invoke(parameters);
            }
        }

        public static void Hide()
        {
            ConsoleInput.Bounds.Location = new UniVector(0, -100);
            ConsoleInput.Enabled = false;
        }

        public static void Show()
        {
            ConsoleInput.Bounds.Location = new UniVector(0, 100);
            ConsoleInput.Enabled = true;
            ConsoleInput.Text = "";
        }

        public static void RunEnteredCommand()
        {
            string[] lines = ConsoleInput.Text.Split(new char[] { '\r', '\n' });
            if (lines.Length > 0)
            {
                ConsoleInput.Text = lines[0];
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
            }
        }
    }
}
