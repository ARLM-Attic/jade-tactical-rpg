using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;

namespace JadeEngine.JadeInputs
{
    public class JadeInputFunctions
    {
        internal static Collection<JadePressedState> MousePressedStateArray(MouseState mouse)
        {
            Collection<JadePressedState> states = new Collection<JadePressedState>();
            states.Add(ConvertState(mouse.LeftButton));
            states.Add(ConvertState(mouse.MiddleButton));
            states.Add(ConvertState(mouse.RightButton));
            states.Add(ConvertState(mouse.XButton1));
            states.Add(ConvertState(mouse.XButton2));
            return states;
        }

        internal static Collection<JadePressedState> GamePadPressedStateArray(GamePadState gamePad)
        {
            Collection<JadePressedState> states = new Collection<JadePressedState>(); 
            states.Add(ConvertState(gamePad.Buttons.A)); 
            states.Add(ConvertState(gamePad.Buttons.B)); 
            states.Add(ConvertState(gamePad.Buttons.Back)); 
            states.Add(ConvertState(gamePad.Buttons.LeftShoulder)); 
            states.Add(ConvertState(gamePad.Buttons.LeftStick)); 
            states.Add(ConvertState(gamePad.Buttons.RightShoulder)); 
            states.Add(ConvertState(gamePad.Buttons.RightStick)); 
            states.Add(ConvertState(gamePad.Buttons.Start)); 
            states.Add(ConvertState(gamePad.Buttons.X)); 
            states.Add(ConvertState(gamePad.Buttons.Y)); 
            return states;
        }

        internal static Collection<JadePressedState> DPadPressedStateArray(GamePadDPad dpad)
        {
            Collection<JadePressedState> states = new Collection<JadePressedState>(); 
            states.Add(ConvertState(dpad.Down)); 
            states.Add(ConvertState(dpad.Left)); 
            states.Add(ConvertState(dpad.Right)); 
            states.Add(ConvertState(dpad.Up)); 
            return states;
        }

        public static JadePressedState ConvertState(ButtonState state)
        {
            switch(state)
            {
                case ButtonState.Pressed:
                    return JadePressedState.Pressed;
                case ButtonState.Released:
                    return JadePressedState.Released;
            }

            return JadePressedState.Idle;
        }

        internal static JadePressedState CheckPressedState(JadePressedState currentState, JadePressedState lastState)
        {
            if(currentState == JadePressedState.Pressed)
            {
                if (lastState != JadePressedState.Released)
                    return JadePressedState.Pressed;
                else
                    return JadePressedState.Held;
            }
            else
            {
                if (lastState != JadePressedState.Released && lastState != JadePressedState.Idle)
                    return JadePressedState.Released;
            }

            return JadePressedState.Idle;
        }
    }
}
