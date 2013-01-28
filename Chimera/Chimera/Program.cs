using System;

namespace Chimera
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ChimeraGame game = new ChimeraGame())
            {
                game.Run();
            }
        }
    }
#endif
}

