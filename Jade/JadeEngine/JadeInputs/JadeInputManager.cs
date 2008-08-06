using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace JadeEngine.JadeInputs
{
    public class JadeInputManager
    {
        private static Form ParentWindow { get; set; }
        private static Collection<JadeInputDevice> _devices;

        private static Collection<JadeInputDevice> Devices
        {
            get
            {
                if(_devices == null)
                    _devices = new Collection<JadeInputDevice>();

                return _devices;
            }

            set { _devices = value; }
        }

        public static void AddDevice(JadeInputDevice device)
        {
            Devices.Add(device);
        }

        public static void RemoveDevice(JadeInputDevice device)
        {
            Devices.Remove(device);
        }

        internal static void Update()
        {
            if(ParentWindow.Focused)
            {
                foreach(JadeInputDevice device in Devices)
                    device.Update();
            }
        }

        internal static void Initialize(JadeGame game)
        {
            ParentWindow = (Form) Form.FromHandle(game.Window.Handle);

            foreach(JadeInputDevice device in Devices)
                device.Initialize();
        }
    }
}
