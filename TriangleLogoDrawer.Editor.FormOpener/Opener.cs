using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TriangleLogoDrawer.Editor.FormOpener
{
    public static class Opener
    {
        public enum Options
        {
            Edit,
            Open
        }

        private static readonly Dictionary<Options, Action> OpenActions = new Dictionary<Options, Action>();

        public static void AddOpenAction(Options option, Action providingMethod)
        {
            OpenActions.Add(option, providingMethod);
        }

        public static void Open(Options options)
        {
            if (OpenActions.ContainsKey(options))
            {
                OpenActions[options]();
            }
        }
    }
}
