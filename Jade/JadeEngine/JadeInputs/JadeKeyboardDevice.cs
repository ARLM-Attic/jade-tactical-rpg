using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;

namespace JadeEngine.JadeInputs
{
    public class JadeKeyboardDevice : JadeInputDevice
    {
        private KeyboardState LastState { get; set; }
        private Collection<Keys> LastPressedKeys { get; set; }

        private Collection<Keys> Pressed { get; set; }
        private Collection<Keys> Held { get; set; }
        private Collection<Keys> Released { get; set; }

        public event JadeKeyPressHandler OnKeyPress;
        public event JadeKeyHeldHandler OnKeyHeld;
        public event JadeKeyReleaseHandler OnKeyRelease;

        internal override void Initialize()
        {
            LastState = Keyboard.GetState();
        }

        internal override void Update()
        {
            KeyboardState currentState = Keyboard.GetState();
            Collection<Keys> currentPressedKeys = new Collection<Keys>(currentState.GetPressedKeys());
            LastPressedKeys = new Collection<Keys>(LastState.GetPressedKeys());
            LastState = currentState;

            Pressed = new Collection<Keys>();
            Held = new Collection<Keys>();
            Released = new Collection<Keys>();

            foreach (Keys key in currentPressedKeys)
            {
                if(LastPressedKeys.Contains(key))
                    Held.Add(key);
                else
                    Pressed.Add(key);
            }

            foreach(Keys key in LastPressedKeys)
                if(!currentPressedKeys.Contains(key)) Released.Add(key);

            if (Pressed.Count > 0 && OnKeyPress != null) OnKeyPress(Pressed);
            if (Held.Count > 0 && OnKeyHeld != null) OnKeyHeld(Held);
            if (Released.Count > 0 && OnKeyRelease != null) OnKeyRelease(Released);
        }
    }
}
