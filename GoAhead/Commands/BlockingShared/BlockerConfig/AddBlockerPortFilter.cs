﻿using System;

namespace GoAhead.Commands.BlockingShared.BlockerConfig
{
    public class AddBlockerPortFilter : AddBlockerConfigCommand
    {
        protected override void DoCommandAction()
        {
            Objects.BlockerSettings.Instance.AddPortFilter(FamilyRegexp, Regexp);
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The regular expression which identifies ports that will not not be blocked")]
        public string Regexp = "";
    }
}