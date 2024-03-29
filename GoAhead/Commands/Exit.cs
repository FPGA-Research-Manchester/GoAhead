﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description="Saves all setting and exits GoAhead", Wrapper=false)]
    class Exit : Command
    {
        protected override void DoCommandAction()
        {
            // save settings
            Settings.StoredPreferences.SavePrefernces();
            if(!string.IsNullOrEmpty(Text))
            {
                Console.WriteLine(Text);
            }
            Environment.Exit(ReturnValue);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The return value for the shell")]
        public int ReturnValue = 0;

        [Parameter(Comment = "The message is printed to the console out before exiting", PrintParameter = false)]
        public string Text = "";
    }
}
