﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Materia.Textures;
using Materia.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Materia.Imaging.GLProcessing
{
    public class LevelsProcessor : ImageProcessor
    {
        GLShaderProgram shader;

        public Vector3 Min { get; set; }
        public Vector3 Mid { get; set; }
        public Vector3 Max { get; set; }

        public LevelsProcessor() : base()
        {
            shader = GetShader("image.glsl", "levels.glsl");
        }

        public override void Process(int width, int height, GLTextuer2D tex, GLTextuer2D output)
        {
            base.Process(width, height, tex, output);

            if (shader != null)
            {
                Vector3 min = Min;
                Vector3 max = Max;
                Vector3 mid = Mid;
                Vector2 tiling = new Vector2(TileX, TileY);

                shader.Use();
                shader.SetUniform2("tiling", ref tiling);
                shader.SetUniform("MainTex", 0);
                GL.ActiveTexture(TextureUnit.Texture0);
                tex.Bind();
                shader.SetUniform3("maxValues", ref max);
                shader.SetUniform3("minValues", ref min);
                shader.SetUniform3("midValues", ref mid);

                if (renderQuad != null)
                {
                    renderQuad.Draw();
                }

                GLTextuer2D.Unbind();
                output.Bind();
                output.CopyFromFrameBuffer(width, height);
                GLTextuer2D.Unbind();
            }
        }
    }
}
