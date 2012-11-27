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

    public enum States { None, Height, Object, New, Save, Load };

    public static class GameMapEditor
    {

        public static InputManager Input;
        public static GuiManager GUI;

        public static Screen Screen;
        public static Viewport Viewport;
        public static FPSCamera Camera;

        public static States CurrentState;
        public static Boolean EditMode;
        public static Dialog Dialog;

        public static Boolean Displayed;
        public static Dialog Reminder;

        public static Boolean Placeable;
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
            EditMode = false;

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

            Screen.Desktop.Children.Add(Dialog);

        }

        public static void ToggleReminder()
        {
            if (Displayed) Reminder.Hide();
            else Reminder.Show();
            Displayed = !Displayed;
        }

        public static void Pressed()
        {

            Editor.AddState(Map);

            if (EditMode)
            {

                if (CurrentState == States.None)
                {
                    MouseState mouse = Mouse.GetState();
                    SelectClick = new Vector2(mouse.X, mouse.Y);
                    SelectRelease = new Vector2(mouse.X, mouse.Y);
                }

                if (Placeable)
                {
                    if (CurrentState == States.Object)
                    {
                        Map.AddObject(Position);
                    }
                }
            }
            else
            {
                MouseState mouse = Mouse.GetState();
                SelectClick = new Vector2(mouse.X, mouse.Y);
                SelectRelease = new Vector2(mouse.X, mouse.Y);
            }
        }

        public static void Hold()
        {
            if (EditMode)
            {

                if (CurrentState == States.None)
                {
                    MouseState mouse = Mouse.GetState();
                    SelectRelease = new Vector2(mouse.X, mouse.Y);
                }

                if (Placeable)
                {
                    if (CurrentState == States.Height)
                    {
                        Map.ModifyVertices(Position);
                    }
                }
            }
            else
            {
                MouseState mouse = Mouse.GetState();
                SelectRelease = new Vector2(mouse.X, mouse.Y);
            }

        }

        public static void Released()
        {
            if (EditMode)
            {

                if (CurrentState == States.None)
                {
                    SelectClick = new Vector2(0.0f, 0.0f);
                    SelectRelease = new Vector2(0.0f, 0.0f);
                    Selected = Map.Select(TopLeft, BottomRight);
                }
            }
            else
            {
                SelectClick = new Vector2(0.0f, 0.0f);
                SelectRelease = new Vector2(0.0f, 0.0f);
                Selected = Map.Select(TopLeft, BottomRight);
            }
        }

        public static void ToggleEditMode()
        {
            EditMode = !EditMode;
            if (EditMode)
            {
                Dialog.Hide();
                if (CurrentState == States.Object)
                {
                    ObjectEditorDialog tempDialog = Dialog as ObjectEditorDialog;
                    tempDialog.EnableParameters();
                }
            }
            else
            {
                Dialog.Show();
                if (CurrentState == States.Object)
                {
                    ObjectEditorDialog tempDialog = Dialog as ObjectEditorDialog;
                    tempDialog.DisableParameters();
                }
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
