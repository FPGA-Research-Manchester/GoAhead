using GoAhead.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Objects
{
    struct BELType
    {
        public string VHDLGenericMap;
        public List<string> outputNames;
        public List<string> inputNames;
        public bool inputsConstantValue;
    }

    class VivadoBELManager : IResetable
    {
        public static VivadoBELManager Instance = new VivadoBELManager();

        private Dictionary<string, BELType> BELTypes;

        private VivadoBELManager()
        {
        }

        public void Reset()
        {
            BELTypes = null;
        }

        public void AddBELType(string name, BELType data)
        {
            if(BELTypes == null)
            {
                BELTypes = new Dictionary<string, BELType>();
            }

            BELTypes[name] = data;
        }

        public BELType? GetBELType(string name)
        {
            if (BELTypes == null || !BELTypes.ContainsKey(name)) return null;

            return BELTypes[name];
        }
    }
}
