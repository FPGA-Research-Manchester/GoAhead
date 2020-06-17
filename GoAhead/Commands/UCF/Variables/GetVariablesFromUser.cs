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
    
        public String VariableName;
        public DomainType Domain;        
        public String Value;
        public List<String> ExplicitRange = new List<String>();
    }

    class GetVariablesFromUser : Command
    {
        protected override void DoCommandAction()
        {
            List<Set> setCommands = new List<Set>();
            
            if (this.Mode.ToUpper().Equals("GUI"))
            {
                SetVariablesForm dlg = new SetVariablesForm(this.GetInputs().ToList());
                dlg.Show();

                setCommands = dlg.GetSetCommands();
            }
            else if (this.Mode.ToUpper().Equals("CONSOLE"))
            {
                foreach (Input input in this.GetInputs())
                {
                    Console.WriteLine("# Type value for variable " + input.VariableName + " (Domain is " + input.Domain + ")");
                    if (input.Domain == Input.DomainType.Range)
                    {
                        Console.WriteLine("# Possible values are " + string.Join(" ", input.ExplicitRange));
                    }
                    String value = "";
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

            foreach (Set cmd in setCommands.Where(c => String.IsNullOrEmpty(c.Variable) && !String.IsNullOrEmpty(c.Value)))
            {
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        private IEnumerable<Input> GetInputs()
        {
            foreach (String tupel in this.Variables)
            {
                String[] varAtoms = tupel.Split(':');
                String variableName = varAtoms[0];
                String variableDomain = varAtoms[1];

                Input input = new Input();
                input.VariableName = variableName;

                if (Regex.IsMatch(variableDomain, @"^\(.*\)$"))
                {
                    input.Domain = Input.DomainType.Range;
                    String[] possibleValues = variableDomain.Split(')', '(', ' ');
                    foreach (String possibleValue in possibleValues.Where(s => !String.IsNullOrEmpty(s)))
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
                    String[] varNames = variableName.Split(' ');
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
        public String Mode = "GUI";

        [Parameter(Comment = "The list of the variable and their domain")]
        public List<String> Variables = new List<String>();
    }
}
