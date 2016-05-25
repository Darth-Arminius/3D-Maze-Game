using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemRenderUI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_UI | ComponentTypes.COMPONENT_TEXTURE 
            | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_ROTATION);

        private Effect effect;

        public SystemRenderUI(Effect effect)
        {
            this.effect = effect;
        }

        public string Name
        {
            get { return "SystemRenderUI"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                var components = entity.Components;

                IComponent geometryComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry;

                IComponent rotationComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
                });
                float rotation = ((ComponentRotation)rotationComponent).YAxis;

                IComponent UIComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_UI;
                });
                Vector2 position = ((ComponentUI)UIComponent).Position;
                float scale = ((ComponentUI)UIComponent).Scale;

                Matrix world = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f));
               
                world = Matrix.CreateScale(scale);

                Matrix trans = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f));
                world = world * Matrix.CreateRotationZ(rotation);
                world *= trans; //this is needed but I have 0% of an idea why.


                IComponent textureComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                Texture2D texture = ((ComponentTexture)textureComponent).Texture;

                Draw(world, geometry, texture);

            }
        }

        public void Draw(Matrix world, Geometry geometry, Texture2D texture)
        {
            effect.Parameters["WorldViewProj"].SetValue(world);
            effect.Parameters["UserTexture"].SetValue(texture);

            CoolGameBase.gameInstance.GraphicsDevice.SetVertexBuffer(geometry.VertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                CoolGameBase.gameInstance.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, geometry.NumberOfTriangles);
            }
        }

    }
}
