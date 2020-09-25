using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper;
using System.Runtime.CompilerServices;
using GoAhead.Commands.Debug;
using GoAhead.FPGA;
using GoAhead.Commands.GridStyle;

namespace GoAhead.GUI.Macros.BusInterface
{
    public partial class PrintBusInterfaceCtrl : UserControl
    {
        public PrintBusInterfaceCtrl()
        {
            InitializeComponent();
        }

        private void m_btnPrintBusInterface_Click(object sender, EventArgs e)
        {
            string fileName;

            //If the file name is empty, throw error.
            if (this.m_fileSelect.FileName.Length == 0)
            {
                MessageBox.Show("Please provide a valid CSV input file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                fileName = this.m_fileSelect.FileName;
                ParseCSV(fileName);
            }        
        }

        private void ParseCSV(string fileName)
        {

            try
            {
                

                using (TextReader reader = new StreamReader(fileName))
                {
                    TextWriter write;
                    var csvReader = new CsvReader(reader);

                    csvReader.Configuration.RegisterClassMap<InterfaceConstraintMap>();
                    var records = csvReader.GetRecords<InterfaceConstraint>();

                    //If the output file name is empty, throw error.
                    if (this.m_fileSelectOut.FileName.Length == 0)
                    {
                        MessageBox.Show("Please provide a valid output file path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string outputFilePath = this.m_fileSelectOut.FileName;

                    //Check if the append option is selected and append to the outputfile or fully overwrite it.
                    if (m_fileSelectOut.Append)
                        write = new StreamWriter(outputFilePath, true);
                    else
                        write = new StreamWriter(outputFilePath, false);

                    StringBuilder buffer = new StringBuilder();

                    string tclFilePath = this.m_txtBoxTCLPath.Text.ToString();
                    string border = this.m_drpDwnBorder.SelectedItem.ToString();
                    string pips = this.m_drpDwnPips.SelectedItem.ToString();
                    int wiresType = Int32.Parse(this.m_drpDwnWires.SelectedItem.ToString());
                    int signalsPerTile = Int32.Parse(this.m_drpDwnSignals.Text.ToString());

                    //Check paramaters are all valid.
                    CheckParameters(border, pips, wiresType);

                    if(File.Exists(tclFilePath))
                        File.Delete(tclFilePath);

                    List<Tile> tilesInFinalOrder = PrintPartitionPinConstraintsForSelection.GetTilesInFinalOrder(this.m_mode, this.m_vertical, this.m_horizontal);
                    string cmdPart0 = "ClearSelection;";
                    string cmdPart1 = "PrintInterfaceConstraintsForSelection\n"
                                    + " FileName=";
                    string cmdPart2 = " Append=True\n"
                                    + " CreateBackupFile=False\n"
                                    + " SignalPrefix=";
                    string cmdPart3 = " InstanceName=%InstanceName%\n"
                                    + " Border=";
                    string cmdPart4 = " NumberOfSignals=";
                    string cmdPart5 = " PreventWiresFromBlocking=True\n"
                                    + " InterfaceSpecs=In:";
                    string cmdPart6 = " StartIndex=";
                    string cmdPart7 = " SignalsPerTile=";
                    string selectCmd;

                    
                    foreach (var record in records)
                    {
                        string signalNamePrefix = record.SignalName.Split(new char[] { '_' })[0];
                        string signalNameSuffix = record.SignalName.Split(new char[] { '_' })[1];

                        int numberOfTylesReq = (int)Math.Ceiling((((double)record.BusWidth) / signalsPerTile));

                        //selectCmd = GenerateSelectCommand(numberOfTylesReq, ref tilesInFinalOrder);
                        selectCmd = GenerateSelectCommand(numberOfTylesReq + 1, ref tilesInFinalOrder);

                        if (selectCmd == "")
                            return;

                        string formatString = cmdPart0 + "\n" + selectCmd + "\n" 
                                            + cmdPart1 + tclFilePath + "\n" + cmdPart2 + signalNamePrefix + "\n"
                                            + cmdPart3 + border + '\n'
                                            + cmdPart4 + record.BusWidth + '\n'
                                            + cmdPart5 + wiresType + ":" + signalNameSuffix + ":" + pips + '\n'
                                            + cmdPart6 + record.StartIndex + '\n' + cmdPart7 + signalsPerTile + ';';

                        buffer.Append(formatString + '\n' + '\n');
                    }

                    //Output in text file format.
                    write.Write(buffer.ToString());

                    reader.Close();
                    write.Close();
                }
            }
            catch (IOException exp)
            {
                MessageBox.Show("Your CSV seems to be used by another process. Make sure you close it and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private string GenerateSelectCommand(int noOfTilesReq, ref List<Tile> tilesInFinalOrder)
        {
            
            if (noOfTilesReq > tilesInFinalOrder.Count())
            {
                MessageBox.Show("Selection does not contain enough physical wires to generate the interface constraints.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            // BOTTOM_UP tile assignment
            //Tile topTile = tilesInFinalOrder[0];
            //Tile bottomTile = tilesInFinalOrder[noOfTilesReq - 1];

            ////Remove the range from the selected tiles.
            //tilesInFinalOrder.RemoveRange(0, noOfTilesReq);
            // TOP_DOWN tile assignment
            Tile topTile = tilesInFinalOrder[tilesInFinalOrder.Count() - 1];
            Tile bottomTile = tilesInFinalOrder[tilesInFinalOrder.Count() - noOfTilesReq];

            //Remove the range from the selected tiles.
            tilesInFinalOrder.RemoveRange(tilesInFinalOrder.Count() - noOfTilesReq, noOfTilesReq);

            return "AddBlockToSelection UpperLeftTile=" + topTile.Location + " LowerRightTile=" + bottomTile.Location + ";";

        }

        private void CheckParameters(string border, string pips, int wireType)
        {
            bool borderIsCorrect = border.Equals("West") ||
                                   border.Equals("East");
         
            bool pipsIsCorrect = pips.Equals("W") ||
                                   pips.Equals("E");
            bool wireTypeIsCorrect = wireType == 2 || wireType == 4;

            if (!borderIsCorrect || !pipsIsCorrect || !wireTypeIsCorrect)
            {
                throw new ArgumentException("Unexpected format in one of the parameters.");
            }
        }
        private void m_txtBoxPips_TextChanged(object sender, EventArgs e)
        {

        }

        private void m_grpBox_Enter(object sender, EventArgs e)
        {

        }

        private void m_labelWiresType_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged_1(object sender, EventArgs e)
        {

        }

        private void m_labelBorder_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
