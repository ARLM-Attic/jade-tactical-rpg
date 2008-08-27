using Microsoft.Xna.Framework;

namespace JadeEngine.JadeObjects.JadeObjectComponents
{
    public interface IJadeHasMaterial : IJadeObjectComponent
    {
    	void SetMaterialDiffuseColor(Vector4 color);
        void SetMaterialSpecularColor(Vector4 color);
    	void SetMaterialSpecularPower(float power);

        void SetMaterialProperties();
    }
}
