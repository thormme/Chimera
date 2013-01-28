using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Chimera
{
    public class StateTimer<T>
    {
        public double Timer
        {
            get;
            set;
        }

        private T mIdleState;

        private bool mLoop;
        public void Loop()
        {
            mLoop = true;
        }

        private bool mNewState;
        public T NewState()
        {
            if (mNewState)
            {
                return mState;
            }
            return default(T);
        }

        private T mState;
        public T State
        {
            get
            {
                return mState;
            }
            set
            {
                Stop();
                mState = value;
                mNewState = true;
            }
        }

        public StateTimer(T idleState)
        {
            mIdleState = idleState;
            Reset();
        }

        public void Reset()
        {
            mState = mIdleState;
            ResetNewState();
            Stop();
        }

        public void Stop()
        {
            Timer = -1.0f;
            mLoop = false;
        }

        public void Next()
        {
            int i = (int)(object)mState;
            mState = (T)(object)((1 + i) % (Enum.GetNames(typeof(T))).Length);
            if (mLoop && (int)(object)mState == (int)(object)mIdleState)
            {
                Next();
            }
            mNewState = true;
        }

        public void NextIn(double time)
        {
            Timer = time;
        }

        public void NextUntil(double time)
        {
            Next();
            Timer = time;
        }

        public void Update(GameTime time)
        {
            if (Timer > 0.0f)
            {
                Timer -= time.ElapsedGameTime.TotalSeconds;
                if (Timer < 0.0f)
                {
                    Next();
                }
            }
        }

        public void ResetNewState()
        {
            mNewState = false;
        }
    }
}
