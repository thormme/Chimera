using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace finalProject
{
    /// <summary>
    /// Gets a Random instance. This is used so we don't reseed a ton of times.
    /// </summary>
    static public class Rand
    {
        static public Random rand = new Random();
        static public float NextFloat(float max)
        {
            float ret = ((float)rand.Next() / (float)int.MaxValue) * max;
            //Console.WriteLine(max + " " + ret);
            return ret;
        }
    }
}
