using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoAhead.Commands
{
    public class BitstreamCompression : Command
    {
        protected override void DoCommandAction()
        {
            string[] files = Directory.GetFiles(BitstreamRepository);
            List<string> bitstreams = new List<string>();
            foreach (string s in files)
            {
                if (s.EndsWith("bin"))
                {
                    bitstreams.Add(s);
                }
            }

            if (bitstreams.Count == 0)
            {
                throw new ArgumentException("Found no bitstreams");
            }

            string referenceBitstream = bitstreams[0];
            // pick reference bitstream
            byte[] referenceData = File.ReadAllBytes(referenceBitstream);

            uint stepWidth = 2;

            for (int bitstreamIndex = 1; bitstreamIndex < bitstreams.Count; bitstreamIndex++)
            {
                string bitstream = bitstreams[bitstreamIndex];
                CompressedBitstream comp = new CompressedBitstream();
                byte[] otherData = File.ReadAllBytes(bitstream);

                uint positionIndependentData = 0;
                uint positionDependentData = 0;

                uint offset = 0;
                uint length = 0;
                while (offset + length < otherData.Length)
                {
                    length = CommonHeaderLength(referenceData, otherData, offset, stepWidth);
                    if (length > 0)
                    {
                        comp.AddReference(referenceData, offset, length);
                        offset += length * stepWidth;
                        positionIndependentData += length * stepWidth;
                    }
                    else
                    {
                        int data = 0;
                        int incr = 0;
                        for (int i = (int)stepWidth - 1; i >= 0; i--)
                        {
                            data += (otherData[offset + incr] << i * 8);
                            incr++;
                        };

                        positionDependentData += stepWidth;

                        comp.AddWord((uint)data);
                        offset += stepWidth;
                    }
                }

                int sizeInBytes = (comp.NumOfWords + comp.NumOfReferenceEntries) * 8;
                int roundUp = sizeInBytes % 32 == 0 ? sizeInBytes : (sizeInBytes - (sizeInBytes % 32)) + 32;

                double independentShare = ((double)positionIndependentData / (double)(positionIndependentData + positionDependentData) * 100);
                double dependentShare = ((double)positionDependentData / (double)(positionIndependentData + positionDependentData) * 100);
                OutputManager.WriteOutput(Path.GetFileName(bitstream) + " dependent/independent: " + positionDependentData + "<>" + positionIndependentData);
                OutputManager.WriteOutput(Path.GetFileName(bitstream) + " dependent/independent: " + dependentShare + "<>" + independentShare);
                OutputManager.WriteOutput(Path.GetFileName(bitstream) + " compressed with " + comp.NumOfWords + " words and " + comp.NumOfReferenceEntries + " references entries to " + Path.GetFileName(referenceBitstream) + " to " + sizeInBytes + " bytes (command length), rounded up to n*32 " + roundUp);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the number of common bytes in both buffers
        /// </summary>
        /// <param name="buffer1"></param>
        /// <param name="buffer2"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private uint CommonHeaderLength(byte[] buffer1, byte[] buffer2, uint start, uint stepWidth)
        {
            uint length = (uint)buffer2.Length;
            uint offset = start;
            uint commonPrefixLength = 0;
            while (offset + stepWidth < length)
            {
                int v1 = 0;
                int v2 = 0;

                int incr = 0;
                for (int i = (int)stepWidth - 1; i >= 0; i--)
                {
                    v1 += (buffer1[offset + incr] << i * 8);
                    v2 += (buffer2[offset + incr] << i * 8);
                    incr++;

                    /*
                    v1 += (buffer1[offset + 0] << 24);
                    v1 += (buffer1[offset + 1] << 16);
                    v1 += (buffer1[offset + 2] << 8);
                    v1 += (buffer1[offset + 3] << 0);

                    v2 += (buffer2[offset + 0] << 24);
                    v2 += (buffer2[offset + 1] << 16);
                    v2 += (buffer2[offset + 2] << 8);
                    v2 += (buffer2[offset + 3] << 0);
                     * */
                }

                if (v1 != v2)
                {
                    break;
                }
                else
                {
                    commonPrefixLength++;
                    offset += stepWidth;
                }
            }

            return commonPrefixLength;
        }

        private class CompressedBitstream
        {
            public CompressedBitstream()
            {
            }

            public void AddReference(byte[] refernceData, uint offset, uint length)
            {
                ReferenceEntry entry = new ReferenceEntry(offset, length);
                m_entries.Add(entry);
            }

            public void AddWord(uint data)
            {
                WordEntry entry = new WordEntry(data, 0);
                m_entries.Add(entry);
            }

            public int NumOfWords
            {
                get { return m_entries.Count(e => e is WordEntry); }
            }

            public int NumOfReferenceEntries
            {
                get { return m_entries.Count(e => e is ReferenceEntry); }
            }

            private List<DataEntry> m_entries = new List<DataEntry>();

            private abstract class DataEntry
            {
                public DataEntry(uint offset, uint length)
                {
                    m_offset = offset;
                    m_length = length;
                }

                public override string ToString()
                {
                    return "Offset=" + m_offset + " Length=" + m_length;
                }

                protected readonly uint m_offset = 0;
                protected readonly uint m_length = 0;
            }

            private class ReferenceEntry : DataEntry
            {
                public ReferenceEntry(uint offset, uint length)
                    : base(offset, length)
                {
                }
            }

            private class WordEntry : DataEntry
            {
                public WordEntry(uint offset, uint length)
                    : base(offset, length)
                {
                }
            }
        }

        [Parameter(Comment = "The directory in which the bitstreams *.bin are stored")]
        public string BitstreamRepository = "";
    }
}