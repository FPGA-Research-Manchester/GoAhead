using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLInstance : Instance
    {
        public TCLInstance()
        {
        }

        public TCLInstance(TCLInstance other)
        {
            BELType = other.BELType;
            InstanceIndex = other.InstanceIndex;
            Location = other.Location;
            LocationX = other.LocationX;
            LocationY = other.LocationY;
            m_code.Append(other.m_code.ToString());
            m_sliceNumber = other.m_sliceNumber;
            m_tileKey = new FPGA.TileKey(other.m_tileKey.X, other.m_tileKey.Y);
            Name = other.Name;
            OmitPlaceCommand = other.OmitPlaceCommand;
            Properties = new TCLProperties(other.Properties);
            SliceName = other.SliceName;
            SliceNumber = other.SliceNumber;
            SliceType = other.SliceType;
        }

        [DefaultValue("undefined bel type"), DataMember]
        public string BELType { get; set; }

        [DefaultValue(false), DataMember]
        public bool OmitPlaceCommand { get; set; }

        public TCLProperties Properties = new TCLProperties();
    }
}