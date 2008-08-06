using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JadeEngine.JadeInputs
{
    public delegate void JadeKeyPressHandler(Collection<Keys> keys);
    public delegate void JadeKeyHeldHandler(Collection<Keys> keys);
    public delegate void JadeKeyReleaseHandler(Collection<Keys> keys);
}