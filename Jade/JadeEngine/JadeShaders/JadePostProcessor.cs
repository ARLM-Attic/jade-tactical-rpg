using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JadeEngine.JadeShaders
{
    public class JadePostProcessor : JadeShader
    {
        private VertexPositionTexture[] _verts;
        private VertexDeclaration Declaration{ get; set; }
        private ResolveTexture2D Resolved { get; set; }

        public JadePostProcessor(string asset) : base(asset) { }

        internal override void LoadContent(GraphicsDevice gd, Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            base.LoadContent(gd, cm);

            _verts = new VertexPositionTexture[]
                         {
                             new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
                             new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
                             new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
                             new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0))
                         };

            Declaration = new VertexDeclaration(gd, VertexPositionTexture.VertexElements);

            LoadResolveTarget(gd);
        }

        private void LoadResolveTarget(GraphicsDevice gd)
        {
            Resolved = new ResolveTexture2D(gd, gd.Viewport.Width, gd.Viewport.Height, 1, gd.DisplayMode.Format);
        }

        internal void Draw(GraphicsDevice gd)
        {
            if(Resolved.Width != gd.Viewport.Width || Resolved.Height != gd.Viewport.Height || Resolved.Format != gd.DisplayMode.Format)
                LoadResolveTarget(gd);

            gd.ResolveBackBuffer(Resolved);
            gd.Textures[0] = Resolved;

            SetParameters(null);
            Effect.Begin();
            foreach(EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                gd.VertexDeclaration = Declaration;
                gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, _verts, 0, 2);
                pass.End();
            }
            Effect.End();
        }
    }
}
