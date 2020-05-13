using System;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.XDL.ResourceDescription
{
    public class XDLSliceParser
    {
        /// <summary>
        ///  Parse the next slice, add it to containingTile and return the added slice
        /// </summary>
        /// <param name="containingTile"></param>
        /// <param name="mapping"></param>
        /// <param name="line"></param>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static Slice Parse(Tile containingTile, InPortOutPortMapping mapping, string line, XDLStreamReaderWithUndo sr)
        {
            // (primitive_site SLICE_X0Y79 SLICEM public 50
            char[] pipSeparator = { ' ', '(', ')' };
            string[] primitiveSiteAtoms = line.Split(pipSeparator);
            //five elements expected, see above
            if (primitiveSiteAtoms.Length != 6)
                throw new ArgumentException("Unexpected primitive_site found: " + line);

            // get S6, V5, ... slice
            Slice slice = Slice.GetSlice(containingTile, primitiveSiteAtoms[2], primitiveSiteAtoms[3]);
            containingTile.Slices.Add(slice);

            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex4))
            {
                // Slice in Virtex4 come in order 0 2 1 3 -> reorder to 0 1 2 3
                if (containingTile.Slices.Count == 4 && IdentifierManager.Instance.IsMatch(containingTile.Location, IdentifierManager.RegexTypes.CLB))
                {
                    Slice temp = containingTile.Slices[2];
                    containingTile.Slices[2] = containingTile.Slices[1];
                    containingTile.Slices[1] = temp;
                }
            }

            //read until end of tile
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("pinwire"))
                {
                    string[] atoms = line.Split(' ', ')');
                    if (atoms.Length != 5)
                        throw new ArgumentException("Unexpected pinwire line: " + line);

                    Port p = new Port(atoms[3]);
                    if (mapping.Contains(p))
                    {
                        Console.WriteLine("Warning: Port " + p.Name + " already exists in " + containingTile.Location + ". This port will be skipped." + p.GetHashCode());
                    }

                    if (atoms[2].Equals("input"))
                    {
                        mapping.AddSlicePort(p, FPGATypes.PortDirection.In);
                    }
                    else if (atoms[2].Equals("output"))
                    {
                        mapping.AddSlicePort(p, FPGATypes.PortDirection.Out);
                    }
                    else if (atoms[2].Equals("bidir"))
                    {
                        mapping.AddSlicePort(p, FPGATypes.PortDirection.In);
                        mapping.AddSlicePort(p, FPGATypes.PortDirection.Out);
                    }
                    else
                    {
                        throw new ArgumentException("Unexpected slice port " + line);
                    }
                }
                //reached end of tile
                else if (m_endOfFile.IsMatch(line))
                {
                    return slice;
                }
                else
                {
                    continue;
                }
            }

            // should never be reached
            throw new ArgumentException("Could not parse slice starting with " + line);
        }

        //private static Regex m_pinwireRegexp = new Regex("pinwire", RegexOptions.Compiled);
        private static Regex m_endOfFile = new Regex(@"\t+\)$", RegexOptions.Compiled);
    }
}