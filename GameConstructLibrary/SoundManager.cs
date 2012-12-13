using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace GameConstructLibrary
{
    public static class SoundManager
    {
        static private Dictionary<string, SoundEffect> mUniqueSoundEffectLibrary = new Dictionary<string, SoundEffect>();

        /// <summary>
        /// Loads all sound effecs from content/sound/ into memory.
        /// </summary>
        /// <param name="content">Global game content manager.</param>
        static public void LoadContent(ContentManager content)
        {
            // Load models.
            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\" + "sounds");
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find sounds/ directory in content.");
            }

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                FileInfo[] files = subDir.GetFiles("*");
                foreach (FileInfo file in files)
                {
                    string soundName = Path.GetFileNameWithoutExtension(file.Name);
                    string subDirPath = Path.GetFileNameWithoutExtension(subDir.FullName);
                    mUniqueSoundEffectLibrary.Add(soundName, content.Load<SoundEffect>("sounds/" + subDirPath + "/" + soundName));
                }
            }
        }

        /// <summary>
        /// Retrieves sounds from database.  Throws KeyNotFoundException if soundName does not exist in database.
        /// </summary>
        /// <param name="soundName">Name of the sound.</param>
        static public SoundEffect LookupSound(string soundName)
        {
            SoundEffect result;
            if (mUniqueSoundEffectLibrary.TryGetValue(soundName, out result))
            {
                return result;
            }
            throw new KeyNotFoundException("Unable to find sound key: " + soundName);
        }
    }
}
