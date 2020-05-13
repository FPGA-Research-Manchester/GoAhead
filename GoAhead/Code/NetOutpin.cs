using System;

namespace GoAhead.Code
{
    [Serializable]
    public class NetOutpin : NetPin
    {
        public override string GetDirection()
        {
            return "outpin";
        }
    }
}