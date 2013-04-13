﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace WorldEditor.Dialogs
{
    public abstract class GameDeviceControl : GraphicsDeviceControl
    {
        private Stopwatch mTimer;
        private TimeSpan mPreviousFrameTime;

        protected override void Initialize()
        {
            mTimer = Stopwatch.StartNew();
            mPreviousFrameTime = mTimer.Elapsed;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {
                LoadContent();
            }
        }

        protected override void Dispose(bool disposing)
        {
            UnloadContent();
            base.Dispose(disposing);
        }

        protected abstract void LoadContent();

        protected abstract void UnloadContent();

        protected override void Draw()
        {
            TimeSpan elapsed = mTimer.Elapsed - mPreviousFrameTime;
            Draw(new GameTime(mTimer.Elapsed, elapsed));
            Update(new GameTime(mTimer.Elapsed, elapsed));
            Invalidate();
        }

        protected abstract void Draw(GameTime gameTime);

        protected abstract void Update(GameTime gameTime);
    }
}
