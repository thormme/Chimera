using System;

namespace WorldEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (Editor game = new Editor())
            {
                game.Run();
            }
        }
    }
#endif
}
