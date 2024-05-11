using OpenTK.Graphics.OpenGL;
using ProgramParameterPName = OpenTK.Graphics.OpenGLES2.ProgramParameterPName;

namespace Entropy.Rendering;

public class Shader : IDisposable
{
    private int Program;
    private bool disposed;

    public Shader(string vertexPath, string fragmentPath)
    {
        Program = GL.CreateProgram();

        var vertexShader = Load(vertexPath, ShaderType.VertexShader);
        var fragmentShader = Load(fragmentPath, ShaderType.FragmentShader);
        
        GL.AttachShader(Program, vertexShader );
        GL.AttachShader(Program, fragmentShader);

        GL.LinkProgram(Program);

        var success = -1;
        GL.GetProgrami(Program, ProgramProperty.LinkStatus, ref success);

        if (success == 0)
        {
            GL.GetProgramInfoLog(Program, out var log);
            Console.WriteLine(log);
        }
        
        GL.DetachShader(Program, vertexShader );
        GL.DetachShader(Program, fragmentShader );
        
        GL.DeleteShader( vertexShader );
        GL.DeleteShader( fragmentShader );
    }

    ~Shader()
    {
        if (!disposed)
        {
            Console.WriteLine("GPU Leak. Shader.Dispose() wasn't called.");
        }
    }
    
    public void Use()
    {
        GL.UseProgram(Program);
    }

    private int Load(string path, ShaderType type)
    {
        int shader;
        string source = File.ReadAllText(path);

        shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        
        GL.CompileShader(shader);


        var success = -1;
        GL.GetShaderi(shader, ShaderParameterName.CompileStatus, ref success);

        if (success == 0)
        {
            GL.GetShaderInfoLog(shader, out string log);
            Console.WriteLine(log);
        }

        return shader;

    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            GL.DeleteProgram(Program);
            disposed = true;
        }
    }
}