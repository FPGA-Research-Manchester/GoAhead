using System;

namespace GoAhead.Code
{
    [Serializable]
    public class NetInpin : NetPin
    {
        public override string GetDirection()
        {
            return "inpin";
        }
    }
}