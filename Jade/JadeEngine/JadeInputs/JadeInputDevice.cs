namespace JadeEngine.JadeInputs
{
    public enum JadeMouseButton
    {
        LeftButton = 0,
        MiddleButton = 1,
        RightButton = 2,
        XButton1 = 3,
        Xbutton2 = 4
    }

    public enum JadeGamePadButton
    {
        A = 0,
        B = 1, 
        Back = 2, 
        LeftShoulder = 3,
        LeftStick = 4, 
        RightShoulder = 5, 
        RightStick = 6, 
        Start = 7, 
        X = 8, 
        Y = 9
    }

    public enum JadeGamePadDPadDirection { Down = 0, Left = 1, Right = 2, Up = 3 }
    public enum JadePressedState { Pressed, Released, Held, Idle }

    public class JadeInputDevice
    {
        internal virtual void Update() {}
        internal virtual void Initialize() {}
    }
}
