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
using Utility;

namespace MapEditor
{

    public enum Edit { None, Height, Object };
    public enum States { None, Height, Object, Parameters, New, Save, Load };

    public static class GameMapEditor
    {

        public static Vector3 MapScale = Utils.WorldScale;

        public static InputManager Input;
        public static GuiManager GUI;

        public static Screen Screen;
        public static Viewport Viewport;
        public static FPSCamera Camera;

        public static States CurrentState;
        public static Edit EditMode;
        
        public static Dialog Dialog;
        public static ParametersDialog Parameters = new ParametersDialog(new List<string>());

        public static Boolean Displayed;
        public static Dialog Reminder;

        public static Boolean Placeable;

        // Placing properties
        public static int Size;
        public static int Intensity;
        public static bool Feather;
        public static bool Set;

        public static DummyObject Dummy;
        public static bool RandomOrientation;
        public static float RandomScale;

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
                CurrentState = States.Load;
                Dialog = new LoadMapDialog();
            }

            if (CurrentState != States.Parameters) Screen.Desktop.Children.Add(Dialog);

        }

        public static void ReturnState()
        {
            
            Screen.Desktop.Children.Remove(Parameters);
            Screen.Desktop.Children.Add(Dialog);

            Size = 0;
            Intensity = 0;
            Feather = false;
            Set = false;

            Dummy = new DummyObject();
            Dummy.Parameters = new string[0];

        }

        public static void ToggleReminder()
        {
            if (Displayed) Reminder.Hide();
            else Reminder.Show();
            Displayed = !Displayed;
        }

        public static void Pressed()
        {

            

            MouseState mouse = Mouse.GetState();

            if (mouse.X <= Parameters.Bounds.Left.Offset || mouse.X >= Parameters.Bounds.Right.Offset ||
                mouse.Y <= Parameters.Bounds.Top.Offset || mouse.Y >= Parameters.Bounds.Bottom.Offset)
            {
                if (Placeable)
                {
                    Editor.AddState(Map);
                    if (EditMode == Edit.Object)
                    {
                        Map.AddObject();
                    }
                }

                if (EditMode == Edit.None)
                {
                    SelectClick = new Vector2(mouse.X, mouse.Y);
                    SelectRelease = new Vector2(mouse.X, mouse.Y);
                }

            }

        }

        public static void Hold()
        {

            MouseState mouse = Mouse.GetState();

            if (mouse.X <= Parameters.Bounds.Left.Offset || mouse.X >= Parameters.Bounds.Right.Offset ||
                mouse.Y <= Parameters.Bounds.Top.Offset || mouse.Y >= Parameters.Bounds.Bottom.Offset)
            {
                if (Placeable)
                {
                    if (EditMode == Edit.Height)
                    {
                        Map.ModifyVertices();
                    }
                }

                if (EditMode == Edit.None)
                {
                    SelectRelease = new Vector2(mouse.X, mouse.Y);
                }

            }

        }

        public static void Released()
        {
            if (EditMode == Edit.None)
            {
                SelectClick = new Vector2(0.0f, 0.0f);
                SelectRelease = new Vector2(0.0f, 0.0f);
                Selected = Map.Select(TopLeft, BottomRight);
            }
        }


        public static void Delete()
        {
            Map.Delete(Selected);
        }

        public static void Move(Vector3 movement)
        {
            Map.Move(Selected, movement);
        }

        public static void Scale(Boolean direction)
        {
            Map.Scale(Selected, direction);
        }

        public static void Rotate(Boolean direction)
        {
            Map.Rotate(Selected, direction);
        }

        private static void MakeBox()
        {
            TopLeft = SelectClick;
            BottomRight = SelectClick;

            if (SelectClick.X < SelectRelease.X)
            {
                TopLeft.X = SelectClick.X;
                BottomRight.X = SelectRelease.X;
            }
            else
            {
                TopLeft.X = SelectRelease.X;
                BottomRight.X = SelectClick.X;
            }

            if (SelectClick.Y < SelectRelease.Y)
            {
                TopLeft.Y = SelectClick.Y;
                BottomRight.Y = SelectRelease.Y;
            }
            else
            {
                TopLeft.Y = SelectRelease.Y;
                BottomRight.Y = SelectClick.Y;
            }

            Rectangle = new Rectangle((int)TopLeft.X,
                                      (int)TopLeft.Y,
                                      (int)(BottomRight.X - TopLeft.X),
                                      (int)(BottomRight.Y - TopLeft.Y));
        }

        public static void Update(GameTime gameTime)
        {

            Map.Update(gameTime);
            Entity.Update(gameTime);
            Editor.Update(gameTime);
            MakeBox();
        }

        public static void Render()
        {
            Map.Render();
            Entity.Render();
        }

        public static void RenderBox(GraphicsDevice graphics, SpriteBatch sprites)
        {

            if (Rectangle.Width > 0 && Rectangle.Height > 0)
            {

                Texture2D texture = new Texture2D(graphics, Rectangle.Width, Rectangle.Height);
                Color[] data = new Color[Rectangle.Width * Rectangle.Height];
                for (int count = 0; count < data.Length; count++) data[count] = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                texture.SetData(data);
                sprites.Begin();
                sprites.Draw(texture, TopLeft, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                sprites.End();
            }
        }

    }
}