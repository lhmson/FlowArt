using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FlowArt
{
    

    class FlowDataManager
    {
        /** UserVariables
            Each elements in this container contains:
                < variable name, keyValuePair< data type, value >
        */
        SortedDictionary< string, VarInfo > UserVariables = new SortedDictionary< string, VarInfo >();


        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // initialize ------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public FlowDataManager()
        {
            UserVariables.Clear();
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // get all var info ------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public List<KeyValuePair<string, VarInfo>> GetVarList()
        {
            List<KeyValuePair<string, VarInfo>> result = new List<KeyValuePair<string, VarInfo>>();

            foreach(var element in UserVariables)
            {
                result.Add( new KeyValuePair<string, VarInfo>(element.Key, element.Value) );
            }

            return result;
        }



        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // variables logic checking ----------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------
    
        /// this function return true if and only if a variable is already declare before
        private bool Exist(string varName)
        {
            return UserVariables.ContainsKey(varName);
        }

        /// return true when this character is available in the variable's name
        /// valid characters include: 'a' -> 'z', 'A' -> 'Z', '0' -> '9', '_'
        static public bool ValidNameChar(char character)
        {
            if(character >= 'a' && character <= 'z')
                return true;
            if(character >= 'A' && character <= 'Z')
                return true;
            if(character >= '0' && character <= '9')
                return true;
            if(character == '_')
                return true;

            return false;
        }

        static public bool ValidName(string varName)
        {
            if( varName == "" )
                return false;

            foreach(char character in varName)
            {
                if(! ValidNameChar(character) )
                    return false;
            }

            return ( varName[0] < '0' || varName[0] > '9' );
        }        


        //------------------------------------------------------------------------------------------------------------------------------------------------------ 
        // declare variables -----------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------ 
        public void DeclareVariable(string varName, DataType dataType, out string ErrorReport)
        {            
            if( ! ValidName(varName) )
            {
                ErrorReport = "Error: Invalid variable name: " + varName;
                return;
            }
            
            if( Exist(varName) )
            {
                ErrorReport = "Error: Duplicate variable name: " + varName;
                return;
            }

            ErrorReport = "";
            VarInfo temp = new VarInfo(dataType, "", true);

            UserVariables.Add(varName, temp);
        }  


        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // assign variables ------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void AssignVariable(string varName, DataType dataType, string value, out string ErrorReport)
        {
            if (! Exist(varName) )
            {
                ErrorReport = "Error: Not declared variable name: " + varName;
                return;
            }

            VarInfo temp = UserVariables[varName];

            if( ! VarInfo.ValidValue(value, temp.type) )
            {
                ErrorReport = "Error: Wrong expected data type: " + varName;
                return;
            }

            ErrorReport = "";
            temp.value = value;
            temp.isNull = false;
            UserVariables[varName] = temp;
        }

        public void AssignVariable(string varName, VarInfo varInfo, out string ErrorReport)
        {
            if (! Exist(varName) )
            {
                ErrorReport = "Error: Not declared variable name: " + varName;
                return;
            }

            VarInfo temp = UserVariables[varName];

            if( varInfo.type != temp.type ) 
            {
                ErrorReport = "Error: Wrong expected data type: " + varName;
                return;
            }
            
            ErrorReport = "";
            temp = new VarInfo(varInfo);
            UserVariables[varName] = temp;
        }

        public VarInfo GetVarInfo(string varName, out string ErrorReport)
        {
            ErrorReport = "";

            if( !Exist(varName) )
            {
                ErrorReport = "Error: Not declared variable name: " + varName;
                return VarInfo.NullVal;
            }

            return UserVariables[varName];
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------ 
        // clear data ------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------ 
        public void ClearData()
        {
            UserVariables.Clear();
        }

        private void DeleteVariable(string varName, out string ErrorReport)
        {
            ErrorReport = "";

            if(! ValidName(varName) )
                ErrorReport = "Invalid variable name: " + varName;

            UserVariables.Remove(varName);            
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // calculate expression --------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------

        static public VarInfo HandleConst_Number(string expression)
        {
            expression = RemoveSpacing(expression);
            return new VarInfo(DataType.FA_Number, expression, false);
        }

        static public VarInfo HandleConst_Special(string expression)
        {
            expression = RemoveSpacing(expression);

            // the first character of bool value must be "$"
            string value = expression.Remove(0, 1).ToUpper();

            return new VarInfo( SpecialConstHandler.GetConst(value) );
        }

        static public VarInfo HandleConst_String(string expression)
        {
            expression = RemoveSpacing(expression);

            string value = expression;
            value = value.Remove(0,1);
            value = value.Remove(value.Length - 1, 1);

            // counting double primes
            int countDP = Regex.Matches(value, "\"").Count;
            value = value.Replace("\"\"", "\"");
            // counting paired double primes
            int countPDP = Regex.Matches(value, "\"").Count;
            
            if( 2*countPDP != countDP )
                return VarInfo.NullVal;

            return new VarInfo(DataType.FA_String, value, false);
        }

        static public VarInfo HandleConst(string expression)
        {
            expression = RemoveSpacing(expression);

            if( expression[0] == '.' || (expression[0] >= '0' && expression[0] <= '9') || expression[0] == '-')
                return HandleConst_Number(expression);

            if( expression[0] == '$' )
                return HandleConst_Special(expression);

            if( expression[0] == '"' && expression[expression.Length - 1] == '"')
                return HandleConst_String(expression);
                            
            return VarInfo.NullVal;
        } 

        private VarInfo HandleVariable(string expression)
        {
            expression = RemoveSpacing(expression);

            if( ! ValidName(expression) )
                return VarInfo.NullVal;

            if( ! Exist(expression) )
                return VarInfo.NullVal;

            return UserVariables[expression];
        }

        private VarInfo HandleFunction(string expression)
        {
            expression = RemoveSpacing(expression);
            List<string> splittedElements = SplitFunctionParameters( expression );
            
            if ( splittedElements[0] == "#ERROR_CODE" )
                return VarInfo.NullVal;

            string functionName = splittedElements[0].ToUpper();
            List<VarInfo> parameters = new List<VarInfo>();

            for( int i=1; i < splittedElements.Count; i++)
            {
                string ErrorReportTemp = "";
                parameters.Add( HandleExpression( splittedElements[i], out ErrorReportTemp) );
            }

            return FunctionHandler.Execute(functionName, parameters);
        }

        public VarInfo HandleExpression(string expression, out string ErrorReport)
        {
            VarInfo temp = VarInfo.NullVal;
            expression = RemoveSpacing( expression );
            ErrorReport = "";

            if( expression == "" )
                return VarInfo.NullVal;

            temp = HandleConst(expression);
            if( ! temp.isNull )
                return temp;
            
            temp = HandleVariable(expression);
            if( ! temp.isNull )
                return temp;

            temp = HandleFunction(expression);
            if( ! temp.isNull )
                return temp;

            ErrorReport = "Error: Illegal Expression";
            return VarInfo.NullVal;
        }



        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // handling assignment ---------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void HandleDeclaration(string declaration, out string varName, out string ErrorReport)
        {
            string temp = RemoveSpacing(declaration);
            ErrorReport = "";

            if( temp == "" )
            {
                ErrorReport = "Illegal expression";
                varName = "";
                return;
            }

            string typeName = "";
            varName = "";

            // slpitting 2 sides
            bool leftSide = true;
            for(int i = 0 ; i < temp.Length; i++)
            {
                if( temp[i] == ':' && leftSide )
                {
                    leftSide = false;
                }
                else
                {
                    if( leftSide )
                    {
                        typeName += temp[i];
                    }
                    else
                    {
                        varName += temp[i];
                    }
                }
            }

            if( leftSide )
            {
                varName = declaration;
                return;
            }
            typeName = typeName.ToUpper();
            DataType dataType;

            switch (typeName)
            {
                case "NUMBER": case "NUM":
                {
                    dataType = DataType.FA_Number;
                    break;
                }
                case "BOOLEAN": case "BOOL":
                {
                    dataType = DataType.FA_Boolean;
                    break;
                }
                case "STRING": case "STR":
                {
                    dataType = DataType.FA_String;
                    break;
                }
                default:
                {
                    ErrorReport = "Error: Not defined datatype: " + typeName;
                    return;
                }
            }

            DeclareVariable(varName, dataType, out ErrorReport);
            if( ErrorReport != "" )
            {
                return;
            }

        }

        public VarInfo HandleAssignment(string assignment, out string ErrorReport)
        {
            string temp = RemoveSpacing(assignment);
            string varName = "";
            string NameAndType = "";
            string expression = "";
            ErrorReport = "";

            if( temp == "" )
            {
                ErrorReport = "Illegal expression";
                return VarInfo.NullVal;
            }

            // splitting 2 sides
            bool leftSide = true;
            for(int i = 0 ; i < temp.Length ; i++)
            {
                if ( temp[i] == '=' && leftSide )
                {
                    leftSide = false;
                }
                else
                {
                    if( leftSide )
                    {
                        NameAndType += temp[i];
                    }
                    else
                    {
                        expression += temp[i];
                    }
                }
            }

            // handle declaration
            // also
            // split varName from NameAndType
            HandleDeclaration(NameAndType, out varName, out ErrorReport);                    
            // if there is error
            if(ErrorReport != "")
                return VarInfo.NullVal;

            // if there is no equal sign
            if ( leftSide )
            {
                if( Exist(varName) )
                {
                    return UserVariables[varName];
                }
                else
                {
                    AssignVariable(varName, VarInfo.NullVal, out ErrorReport);

                    return VarInfo.NullVal;
                }
            }

            VarInfo rightSideResult = HandleExpression(expression, out ErrorReport);
            // if there is error
            if(ErrorReport != "")
                return VarInfo.NullVal;
            
            // if there is error
            if(ErrorReport != "")
                return VarInfo.NullVal;

            AssignVariable(varName, rightSideResult, out ErrorReport);
            // if there is error
            if(ErrorReport != "")
                return VarInfo.NullVal;

            return UserVariables[varName];
        }       

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        // handling deletion ---------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void HandleDeletion(string expression, out string ErrorReport)
        {
            ErrorReport = "";

            if( expression == "" )
                return;            
            if( expression[0] != '~' )
                return;

            string varName = RemoveSpacing( expression.Remove(0,1) );
            DeleteVariable(varName, out ErrorReport);
            if(ErrorReport != "")
                return;
        }










        /// splitting parameters for function handling
        /// output[0] contains function name
        /// also, out[0] MAY returns "#ERROR_CODE" if user violates some fatal errors
        private List<string> SplitFunctionParameters(string expression)
        {
            expression = RemoveSpacing( expression );
            List<string> result = new List<string>();
            string temp = "";
            bool isInString = false;

            int i = 0;
            while ( i < expression.Length - 1 && expression[i] != '(' )
            {
                if( expression [i] == ')' )
                {
                    temp = "#ERROR_CODE";
                    result.Clear();
                    result.Add(temp);
                    return result;
                }
                
                temp += expression[i];
                i++;
            }
            
            if( i == expression.Length )
            {
                temp = "#ERROR_CODE";
                result.Clear();
                result.Add(temp);
                return result;
            }

            // function name
            result.Add(temp);

            // if there is no parameter
            if( i==expression.Length-2 && expression[i+1] == ')')
                return result;

            temp = "";
            int brackets = 1;
            i++;
            for(; i < expression.Length; i++)
            {
                if( expression[i] == '"' )
                    isInString = !isInString;

                if ( ! isInString )
                {
                    if( expression[i] == '(' )
                    {
                        brackets++;
                    }
                    else if( expression[i] == ')' )
                    {
                        brackets--;
                        
                        if( brackets == 0  && i != expression.Length-1 )
                        {
                            temp = "#ERROR_CODE";
                            result.Clear();
                            result.Add(temp);
                            return result;
                        }
                    }
                
                
                    if( expression[i] == ',' && brackets == 1 )
                    {
                        result.Add(temp);
                        temp = "";
                    }
                    else if( i < expression.Length - 1 )
                    {
                        temp += expression[i];
                    }
                }
                else
                {
                    temp += expression[i];
                }

            }

            result.Add(temp);

            if( brackets != 0 )
            {
                temp = "#ERROR_CODE";
                result.Clear();
                result.Add(temp);
                return result;
            }
            
            return result;
        }

        static public string RemoveSpacing(string expression)
        {
            string temp = " " + expression + " ";
            int Length = temp.Length;
            
            bool isInString = false;

            for(int i = Length - 2; i > 0; i-- )
            {
                if( temp[i] == '"' )
                    isInString = ! isInString;

                if( temp[i] == ' ' || temp[i] == '\n' || temp[i] == '\r' )
                    if ( (! isInString ) && ( !ValidNameChar(temp[i-1]) || !ValidNameChar(temp[i+1]) ) )
                        temp = temp.Remove(i, 1);
            }

            temp = temp.Remove( temp.Length-1, 1);
            temp = temp.Remove( 0            , 1);

            return temp;
        }


    }
}