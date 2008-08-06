using Microsoft.Xna.Framework;

namespace JadeEngine.JadeObjects.JadeObjectComponents
{
    public interface IJadeHasMaterial : IJadeObjectComponent
    {
        void SetAmbientLightColor(Vector3 color);
        void SetDiffuseLightColor(Vector3 color);
        void SetSpecularPower(float power);

        void SetMaterialProperties();
    }
}
