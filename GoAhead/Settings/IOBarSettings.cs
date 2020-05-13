using System;

namespace GoAhead.Settings
{
    [Serializable]
    public class IOBarSettings
    {
        public string IOBarConfig_SlotWidth = "2";
        public decimal IOBarConfig_SlotCount = 4;
        public decimal IOBarConfig_LinesPerRow = 4;
        public bool IOBarConfig_ConnectModule = true;
        public decimal IOBarConfig_SlotToConnect = 2;
        public bool IOBarConfig_ModInFF = true;
        public bool IOBarConfig_ModOutFF = true;
        public bool IOBarConfig_ModNoneFF = false;
        public string IOBarConfig_StaticIn = "static_in";
        public string IOBarConfig_StaticOut = "static_out";
        public string IOBarConfig_ModuleIn = "module_in";
        public string IOBarConfig_ModuleOut = "module_out";
        public string IOBarConfig_Direction = "east";
    }
}