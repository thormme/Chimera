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
            string firstLevel = args.Length > 0 ? args[0] : null;
            using (ChimeraGame game = new ChimeraGame(firstLevel))
            {
                game.Run();
            }
        }
    }
#endif
}

