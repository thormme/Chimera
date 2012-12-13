using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using finalProject.Parts;
using finalProject.AI;

namespace finalProject.Creatures
{
    public class Turtle : NonPlayerCreature
    {

        public static GameTip mTip = new GameTip(
            new string[] 
            {
                "You have encountered a turtle.",
                "Turtles are able to hide in their shell to avoid damage."
            },
            10.0f);

        public Turtle(Vector3 position, Spawner spawn)
            : base(
                position,
                2.0f,                                           // Height
                2.0f,                                           // Radius
                6.0f,                                           // Mass
                CreatureConstants.TurtleSensitivityRadius,      // Sensitivity Radius
                new ActivationAI(),                             // AI
                new AnimateModel("turtle", "stand"),            // Model
                135,                                            // Vision Angle
                CreatureConstants.TurtleListeningSensitivity,   // Listening Sensitivity
                CreatureConstants.TurtleSneak,                  // Sneak
                CreatureConstants.TurtleIntimidation,           // Intimidation
                new TurtleShell(),                               // Part
                spawn
                )
        {

        }

        protected override List<PartBone> GetUsablePartBones()
        {
            List<PartBone> bones = new List<PartBone>();
            bones.Add(PartBone.Spine1Cap);
            return bones;
        }

        protected override GameTip Tip
        {
            get
            {
                return mTip;
            }
        }

    }
}
