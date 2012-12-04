using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.Input;
using Nuclex.UserInterface;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Input;

namespace MapEditor
{

    public enum Edit { None, Height, Object };
    public enum States { None, Height, Object, Parameters, New, Save, Load };

    public static class GameMapEditor
    {

        public static Vector3 MapScale = new Vector3(8.0f, 0.25f, 8.0f);

        public static InputManager Input;
        public static GuiManager GUI;

        public static Screen Screen;
        public static Viewport Viewport;
        public static FPSCamera Camera;

        public static States CurrentState;
        public static Edit EditMode;
        
        public static Dialog Dialog;
        public static Dialog Parameters = new ParametersDialog(new List<string>());

        public static Boolean Displayed;
        public static Dialog Reminder;

        public static Boolean Placeable;

        // Placing properties
        public static int Size;
        public static int Intensity;
        public static bool Feather;
        public static bool Set;

        public static DummyObject Dummy;

        //public static 

        public static Vector3 Position;

        public static DummyMap Map;
        public static MapEditorEntity Entity;
        public static MapEditor Editor;

        private static Vector2 SelectClick;
        private static Vector2 SelectRelease;
        private static Vector2 TopLeft;
        private static Vector2 BottomRight;
        private static Rectangle Rectangle;
        private static List<DummyObject> Selected;

        public static void Initialize()
        {

            // Add dialog to windows
            ToggleState(States.None);
            EditMode = Edit.None;

            Size = 0;
            Intensity = 0;
            Feather = false;
            Set = false;

            Dummy = new DummyObject();
            Dummy.Parameters = new string[0];

            // Create default dummy map
            Map = new DummyMap(100, 100);

            // Create new entity
            Entity = new MapEditorEntity();

            // Create new editor
            Editor = new MapEditor();

            // Create empty selected list
            Selected = new List<DummyObject>();

            // Create reminder dialog
            Displayed = true;
            Reminder = new HotkeyDialog();
            Screen.Desktop.Children.Add(Reminder);

        }

        public static void ToggleState(States state)
        {

            Screen.Desktop.Children.Remove(Dialog);

            if (state == States.None)
            {
                CurrentState = States.None;
                Dialog = new MapEditorDialog();
            }
            else if (state == States.Height)
            {
                CurrentState = States.Height;
                Dialog = new HeightEditorDialog();
            }
            else if (state == States.Object)
            {
                CurrentState = States.Object;
                Dialog = new ObjectEditorDialog();
            }
            else if (state == States.Parameters)
            {
                CurrentState = States.Parameters;
                Parameters = new ParametersDialog(Dummy.Parameters.ToList<string>());
                Screen.Desktop.Children.Add(Parameters);
            }
            else if (state == States.New)
            {
                CurrentState = States.New;
                Dialog = new NewMapDialog();
            }
            else if (state == States.Save)
            {
                CurrentState = States.Save;
                Dialog = new SaveMapDialog();
            }
            else if (state == States.Load)
            {
                CurrentState = Stat