using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace JadeEngine.JadeInputs
{
    public delegate void JadeMouseClickHandler(Point position, Collection<JadeMouseButton> buttons);
    public delegate void JadeMouseHeldHandler(Point position, Collection<JadeMouseButton> buttons);
    public delegate void JadeMouseReleaseHandler(Point position, Collection<JadeMouseButton> buttons);
    public delegate void JadeMouseMoveHandler(Vector2 move);
    public delegate void JadeMouseScrollHandler(int ticks);

    public class JadeMouseDevice : JadeInputDevice
    {
        private bool _freezeMouse;

        private Collection<JadePressedState> ButtonStates { get; set; }
        private Collection<JadePressedState> PressedStates { get; set; }
        private Collection<JadeMouseButton> Pressed { get; set; }
        private Collection<JadeMouseButton> Held { get; set; }
        private Collection<JadeMouseButton> Released { get; set; }

        public event JadeMouseClickHandler OnClick;
        public event JadeMouseHeldHandler OnMouseHeld;
        public event JadeMouseReleaseHandler OnRelease;
        public event JadeMouseMoveHandler OnMove;
        public event JadeMouseScrollHandler OnScroll;

        private MouseState MouseState { get; set; }
        private Point InitalMousePosition { get; set; }
        private int LastScrollPosition { get; set; }

        public bool Freeze
        {
            get { return _freezeMouse; }
            set
            {
                _freezeMouse = value;
                if(_freezeMouse)
                {
                    MouseState = Mouse.GetState();
                    InitalMousePosition = new Point(MouseState.X, MouseState.Y);
                }
            }
        }

        internal override void Initialize()
        {
            MouseState = Mouse.GetState();
            InitalMousePosition = new Point(MouseState.X, MouseState.Y);
            LastScrollPosition = MouseState.ScrollWheelValue;
            Freeze = false;

            ButtonStates = new Collection<JadePressedState>();
            
            for(int i = 0; i < 5; i++)
                ButtonStates.Add(JadePressedState.Idle);
        }

        internal override void Update()
        {
            int scrollWheelValue = MouseState.ScrollWheelValue;
            int scrollWheelMoved = LastScrollPosition - scrollWheelValue;
            LastScrollPosition = scrollWheelValue;

            MouseState = Mouse.GetState();
            Point currentMousePosition = new Point(MouseState.X, MouseState.Y);
            Vector2 mouseMoved = new Vector2(currentMousePosition.X - InitalMousePosition.X,
                                                currentMousePosition.Y - InitalMousePosition.Y);

            if (Freeze)
                Mouse.SetPosition(InitalMousePosition.X, InitalMousePosition.Y);
            else
                InitalMousePosition = currentMousePosition;

            Pressed = new Collection<JadeMouseButton>();
            Held = new Collection<JadeMouseButton>();
            Released = new Collection<JadeMouseButton>();
            PressedStates = JadeInputFunctions.MousePressedStateArray(MouseState);

            for (int i = 0; i < 5; i++)
                ButtonStates[i] = JadeInputFunctions.CheckPressedState(PressedStates[i], ButtonStates[i]);

            for (int i = 0; i < 5; i++)
            {
                if (ButtonStates[i] == JadePressedState.Pressed) Pressed.Add((JadeMouseButton)i);
                else if (ButtonStates[i] == JadePressedState.Held) Held.Add((JadeMouseButton)i);
                else if (ButtonStates[i] == JadePressedState.Released) Released.Add((JadeMouseButton)i);
            }

            if (scrollWheelMoved != 0 && OnScroll != null) OnScroll(scrollWheelMoved);
            if (mouseMoved.Length() > 0 && OnMove != null) OnMove(mouseMoved);
            if (Pressed.Count > 0 && OnClick != null) OnClick(currentMousePosition, Pressed);
            if (Held.Count > 0 && OnMouseHeld != null) OnMouseHeld(currentMousePosition, Held);
            if (Released.Count > 0 && OnRelease != null) OnRelease(currentMousePosition, Released);
        }
    }
}
