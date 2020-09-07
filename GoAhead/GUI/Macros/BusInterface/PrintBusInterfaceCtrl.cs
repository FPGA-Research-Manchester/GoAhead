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
            TextReader reader = new StreamReader(fileName);
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
            int wiresType = Int32.Parse( this.m_drpDwnWires.SelectedItem.ToString());
            int startIndex = Int32.Parse(this.m_drpDwnStartIndex.Text.ToString());
            int signalsPerTile = Int32.Parse(this.m_drpDwnSignals.Text.ToString());

            //Check paramaters are all valid.
            CheckParameters(border, pips, wiresType, startIndex);


            string cmdPart1 = "PrintInterfaceConstraintsForSelection\n"
                            + " FileName=";
            string cmdPart2 = " Append=False\n"
                            + " CreateBackupFile=False\n"
                            + " SignalPrefix=";
            string cmdPart3 = " InstanceName=%InstanceName%\n"
                            + " Border=";
            string cmdPart4 = " NumberOfSignals=";
            string cmdPart5 = " PreventWiresFromBlocking=True\n"
                            + " InterfaceSpecs=In:";
            string cmdPart6 = " StartIndex=";
            string cmdPart7 = " SignalsPerTile=";
            

            foreach (var record in records)
            {
                string signalNamePrefix = record.SignalName.Split(new char[] { '_' })[0];
                string signalNameSuffix = record.SignalName.Split(new char[] { '_' })[1];
    
                string formatString = cmdPart1 + tclFilePath + "\n" +cmdPart2 + signalNamePrefix + "\n"
                                    + cmdPart3 + border + '\n'
                                    + cmdPart4 + record.BusWidth + '\n'
                                    + cmdPart5 + wiresType + ":" + signalNameSuffix +":" + pips + '\n'
                                    + cmdPart6 + startIndex + '\n'+ cmdPart7 + signalsPerTile+ ';';

                buffer.Append(formatString + '\n' + '\n');
            }
        
            //Output in text file format.
            write.Write(buffer.ToString());
            
            reader.Close();
            write.Close();

        }
        private void CheckParameters(string border, string pips, int wireType, int startIndex)
        {
            bool borderIsCorrect = border.Equals("West") ||
                                   border.Equals("East");
            bool startIndexIsCorrect = startIndex >= 0;
            bool pipsIsCorrect = pips.Equals("W") ||
                                   pips.Equals("E");
            bool wireTypeIsCorrect = wireType == 2 || wireType == 4;

            if (!borderIsCorrect || !startIndexIsCorrect || !pipsIsCorrect || !wireTypeIsCorrect)
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
