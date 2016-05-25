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
    public class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_TEXTURE |
            ComponentTypes.COMPONENT_ROTATION | ComponentTypes.COMPONENT_TRANSLATION);

        private Effect effect;

        public SystemRender(Effect effect)
        {
            this.effect = effect;
        }

        public string Name
        {
            get { return "SystemRender"; }
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

                IComponent positionComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                IComponent rotationComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
                });
                var rotation = ((ComponentRotation)rotationComponent).AllAxes;

                IComponent translationComponent = components.FirstOrDefault(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TRANSLATION;
                });
                Vector3 translation = ((ComponentTranslation)translationComponent).Translation;

                Matrix world = Matrix.Identity;
                if (rotation.X != 0.0f)
                {
                    world *= Matrix.CreateRotationX(rotation.X);
                }
                if (rotation.Y != 0.0f)
                {
                    world *= Matrix.CreateRotationY(rotation.Y);
                }
                if (rotation.Z != 0.0f)
                {
                    world *= Matrix.CreateRotationZ(rotation.Z);
                }
                world *= Matrix.CreateTranslation(position + translation);

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
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(CoolGameBase.view);
            effect.Parameters["Projection"].SetValue(CoolGameBase.projection);
            effect.Parameters["UserTexture"].SetValue(texture);
            effect.Parameters["lDir"].SetValue(new Vector3(0.0f, -1.0f, 0.0f));
            effect.Parameters["innerC"].SetValue(1.39626f);
            effect.Parameters["outerC"].SetValue(1.5708f);

            effect.Parameters["LightPosition"].SetValue(new Vector3(CoolGameBase.cp.X,CoolGameBase.cp.Y,CoolGameBase.cp.Z));
            effect.Parameters["LightPosition2"].SetValue(CoolGameBase.sp1);
            effect.Parameters["LightPosition3"].SetValue(CoolGameBase.sp2);

            effect.Parameters["LightDiffuseColor"].SetValue(new Vector3(0.8f, 0.4f, 0.4f));
            effect.Parameters["LightDiffuseColor2"].SetValue(new Vector3(1.0f, 1.0f, 0.0f));
            effect.Parameters["LightDiffuseColor3"].SetValue(new Vector3(0.0f, 0.0f, 1.0f));

            effect.Parameters["LightDistanceSquared"].SetValue(50f);
            effect.Parameters["LightDistanceSquared2"].SetValue(10f);

            CoolGameBase.gameInstance.GraphicsDevice.SetVertexBuffer(geometry.VertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                CoolGameBase.gameInstance.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, geometry.NumberOfTriangles);
            }
        }

    }
}
