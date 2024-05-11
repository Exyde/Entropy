
using Entropy.Rendering;
using Entropy.Utility;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


//https://opentk.net/learn/chapter1/2-hello-triangle.html?tabs=onload-opentk4%2Conrender-opentk4%2Cresize-opentk4

namespace Entropy.Application
{
    public static class Application
    {
        static void Main(string[] args)
        {
            using Game game = new Game(800 ,600,  "Entropy Engine");
            Console.WriteLine("Welcome !");
            game.Run();
        }
    }
    
    public class Game : GameWindow
    {
        private int m_vertexBuffer;

        private Shader? m_shader;
        
        public Game(int width, int height, string title) :
            base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title})
        {
            
        }


        /// <summary>
        /// Called once on window first open
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(ColorUtility.Red);

            m_shader = new Shader("shader.vert", "shader.frag");
            
            float[] vertices = {
                -0.5f, -0.5f, 0f,
                0.5f, -0.5f, 0f,
                0.5f, 0.5f, 0f
            };

            unsafe
            {
                fixed (float* vertexPtr = &vertices[0])
                {
                    m_vertexBuffer = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, m_vertexBuffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertexPtr, BufferUsage.StaticDraw);
                }
            }

        }

        protected override void OnUnload()
        {
            base.OnUnload();
            
            //m_shader?.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);


            
            SwapBuffers();
            
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
