using System;
using Tao.FreeGlut;
using OpenGL;
namespace IrrationalSpace
{
    public static class WindowPreferences
    {
        public static int widght = 800, height = 600;
       

        public static void InitWindow(Glut.IdleCallback idleCallback,Glut.DisplayCallback displayCallback,Glut.KeyboardCallback keybordCallback,Glut.KeyboardUpCallback keyboardUpcallback,Glut.CloseCallback closeCallback,Glut.ReshapeCallback reshapeCallback,int _widght=800, int _height=600)
        {
            widght = _widght;
            height = _height;
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(widght, height);
            Glut.glutCreateWindow("Irrational");

            Glut.glutIdleFunc(idleCallback);//OnRenderFrame
            Glut.glutDisplayFunc(displayCallback);//OnDisplay
            Glut.glutKeyboardFunc(keybordCallback);
            Glut.glutKeyboardUpFunc(keyboardUpcallback);
           
            Glut.glutCloseFunc(closeCallback);
            Glut.glutReshapeFunc(reshapeCallback);//OnReshape
            Gl.Enable(EnableCap.DepthTest);
            Gl.Disable(EnableCap.Blend);

            Gl.BlendFunc(BlendingFactorSrc.OneMinusConstantAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }
    }
}
