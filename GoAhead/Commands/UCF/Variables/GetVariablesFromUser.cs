using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.GUI.SetVariables;

namespace GoAhead.Commands.Variables
{        
    public class Input
    {        
        /// <summary>
        /// Identifier
        /// </summary>
        public enum DomainType { AnyValue = 0, TileIdentifier = 1, TileSelection = 2, Range = 3 }
    
        public string VariableName;
        public DomainType Domain;        
        public string Value;
        public List<string> ExplicitRange = new List<string>();
    }

    class GetVariablesFromUser : Command
    {
        protected override void DoCommandAction()
        {
            List<Set> setCommands = new List<Set>();
            
            if (Mode.ToUpper().Equals("GUI"))
            {
                SetVariablesForm dlg = new SetVariablesForm(GetInputs().ToList());
                dlg.Show();

                setCommands = dlg.GetSetCommands();
            }
            else if (Mode.ToUpper().Equals("CONSOLE"))
            {
                foreach (Input input in GetInputs())
                {
                    Console.WriteLine("# Type value for variable " + input.VariableName + " (Domain is " + input.Domain + ")");
                    if (input.Domain == Input.DomainType.Range)
                    {
                        Console.WriteLine("# Possible values are " + string.Join(" ", input.ExplicitRange));
                    }
                    string value = "";
                    while (true)
                    {
                        value = Console.ReadLine();
                        if (input.Domain == Input.DomainType.Range && !input.ExplicitRange.Contains(value))
                        {
                            Console.WriteLine("# Value " + value + " not contained in " + string.Join(" ", input.ExplicitRange));
                        }
                        if (input.Domain == Input.DomainType.TileIdentifier && !FPGA.FPGA.Instance.Contains(value))                            
                        {
                            Console.WriteLine("# Value " + value + " is not a valid tile identifier");
                        }
                        else
                        {
                            break;
                        }
                    }
                    Set setCmd = new Set();
                    setCmd.Variable = input.VariableName;
                    setCmd.Value = value;
                    setCommands.Add(setCmd);
                }
            }
            else
            {
                throw new ArgumentException("Use either Console or GUI for paramter Mode");
            }

            foreach (Set cmd in setCommands.Where(c => string.IsNullOrEmpty(c.Variable) && !string.IsNullOrEmpty(c.Value)))
            {
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        private IEnumerable<Input> GetInputs()
        {
            foreach (string tupel in Variables)
            {
                string[] varAtoms = tupel.Split(':');
                string variableName = varAtoms[0];
                string variableDomain = varAtoms[1];

                Input input = new Input();
                input.VariableName = variableName;

                if (Regex.IsMatch(variableDomain, @"^\(.*\)$"))
                {
                    input.Domain = Input.DomainType.Range;
                    string[] possibleValues = variableDomain.Split(')', '(', ' ');
                    foreach (string possibleValue in possibleValues.Where(s => !string.IsNullOrEmpty(s)))
                    {
                        input.ExplicitRange.Add(possibleValue);
                    }
                }
                else
                {
                    input.Domain = (Input.DomainType)Enum.Parse(typeof(Input.DomainType), variableDomain);
                }

                // expect four values for TileSelection
                if (input.Domain == Input.DomainType.TileSelection)
                {
                    string[] varNames = variableName.Split(' ');
                    if (varNames.Length != 4)
                    {
                        throw new ArgumentException("Ranges of type TileSelection required four variables, e.g. (X1, Y1, X2, Y2)");
                    }
                }

                yield return input;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Whether to get the values from Console or GUI (use either Console or GUI)")]
        public string Mode = "GUI";

        [Parameter(Comment = "The list of the variable and their domain")]
        public List<string> Variables = new List<string>();
    }
}
