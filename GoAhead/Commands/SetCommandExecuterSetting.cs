using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    class SetCommandExecuterSetting : Command
    {
        public override void Do()
        {
            this.m_oldValueMute = CommandExecuter.Instance.Mute;
            this.m_oldValueColor = CommandExecuter.Instance.ColorOutput;
            CommandExecuter.Instance.Mute = this.Mute;
            CommandExecuter.Instance.ColorOutput = this.Color;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.Mute = this.m_oldValueMute;
            CommandExecuter.Instance.ColorOutput = this.m_oldValueColor;
        }

        [Paramter(Comment = "The new value for the setting Mute")]
        public bool Mute = false;

        [Paramter(Comment = "The new value for the setting Colorize outputs")]
        public bool Color = false;

        /// <summary>
        /// for undo
        /// </summary>
        private bool m_oldValueMute = false;
        private bool m_oldValueColor = false;
    }
}
