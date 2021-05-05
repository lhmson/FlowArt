using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowArt
{
    /// this class helps calculate built-int functions, within KNOWN VALUE parameters
    static class FunctionHandler
    {    
        static private SortedDictionary< string, Func< List<VarInfo>, VarInfo > > 
                BuiltInFunctions = new SortedDictionary< string, Func< List<VarInfo>, VarInfo>> ();

        static private void AddFunction(string funcName, Func< List<VarInfo>, VarInfo> FAfunction)
        {
            funcName = funcName.ToUpper();
            BuiltInFunctions.Add(funcName, FAfunction);
        }

        static public void InitBuiltInFunctions()
        {
            // -- Lib 001 -- //
            AddFunction( "Sum",                         FlowArtLib.Sum                          );
            AddFunction( "Mul",                         FlowArtLib.Mul                          );
            AddFunction( "Min",                         FlowArtLib.Min                          );
            AddFunction( "Max",                         FlowArtLib.Max                          );
            AddFunction( "Concat",                      FlowArtLib.Concat                       );
            AddFunction( "IsInc",                       FlowArtLib.IsInc                        );
            AddFunction( "IsDec",                       FlowArtLib.IsDec                        );
            AddFunction( "IsIncEq",                     FlowArtLib.IsIncEq                      );
            AddFunction( "IsDecEq",                     FlowArtLib.IsDecEq                      );
            AddFunction( "IsSmaller",                   FlowArtLib.IsInc                        ); // 
            AddFunction( "IsGreater",                   FlowArtLib.IsDec                        ); // 
            AddFunction( "IsSmEq",                      FlowArtLib.IsIncEq                      ); //
            AddFunction( "IsGrEq",                      FlowArtLib.IsDecEq                      ); //
            AddFunction( "Sub",                         FlowArtLib.Sub                          );
            AddFunction( "Div",                         FlowArtLib.Div                          );
            AddFunction( "Mod",                         FlowArtLib.Mod                          );
            AddFunction( "Pow",                         FlowArtLib.Pow                          );
            AddFunction( "Root",                        FlowArtLib.Root                         );
            AddFunction( "Sqrt",                        FlowArtLib.Sqrt                         );
            AddFunction( "Log",                         FlowArtLib.Log                          );
            AddFunction( "Ln",                          FlowArtLib.Ln                           );
            AddFunction( "IsEqual",                     FlowArtLib.IsEqual                      );
            AddFunction( "Floor",                       FlowArtLib.Floor                        );
            AddFunction( "Ceiling",                     FlowArtLib.Ceiling                      );
            AddFunction( "Round",                       FlowArtLib.Round                        );
            AddFunction( "Length",                      FlowArtLib.Length                       );
            AddFunction( "ToString",                    FlowArtLib.ToString                     );
            AddFunction( "ToStr",                       FlowArtLib.ToString                     ); //
            AddFunction( "Negative",                    FlowArtLib.Neg                          );
            AddFunction( "Neg",                         FlowArtLib.Neg                          ); //
            AddFunction( "Inverse",                     FlowArtLib.Inverse                      );
            AddFunction( "GetChar",                     FlowArtLib.GetChar                      );
            AddFunction( "SetChar",                     FlowArtLib.SetChar                      );
            AddFunction( "Replace",                     FlowArtLib.Replace                      );
            AddFunction( "Remove",                      FlowArtLib.Remove                       );
            AddFunction( "Insert",                      FlowArtLib.Insert                       );
            AddFunction( "CountSubstr",                 FlowArtLib.CountSubstr                  );
            AddFunction( "Find",                        FlowArtLib.Find                         );
            AddFunction( "SubStr",                      FlowArtLib.SubStr                       );
            AddFunction( "ToNumber",                    FlowArtLib.ToNumber                     );
            AddFunction( "ToNum",                       FlowArtLib.ToNumber                     ); //
            AddFunction( "And",                         FlowArtLib.And                          );
            AddFunction( "Or",                          FlowArtLib.Or                           );
            AddFunction( "Xor",                         FlowArtLib.Xor                          );
            AddFunction( "Not",                         FlowArtLib.Not                          );
            AddFunction( "Random",                      FlowArtLib.Random                       );
            AddFunction( "Average",                     FlowArtLib.Average                      );
            AddFunction( "AndNum",                      FlowArtLib.AndNum                       );
            AddFunction( "OrNum",                       FlowArtLib.OrNum                        );
            AddFunction( "XorNum",                      FlowArtLib.XorNum                       );
            AddFunction( "RandomFloat",                 FlowArtLib.RandomFloat                  );
            AddFunction( "RandomPick",                  FlowArtLib.RandomPick                   );
            AddFunction( "AsciiOrd",                    FlowArtLib.AsciiOrd                     );
            AddFunction( "AsciiChar",                   FlowArtLib.AsciiChar                    );
            AddFunction( "IsNumber",                    FlowArtLib.IsNumber                     );
            AddFunction( "IsNum",                       FlowArtLib.IsNumber                     ); //
            AddFunction( "IsInteger",                   FlowArtLib.IsInteger                    );
            AddFunction( "IsInt",                       FlowArtLib.IsInteger                    ); //
            AddFunction( "Abs",                         FlowArtLib.Abs                          ); 

            // -- Lib 002 -- //
            AddFunction( "ToUpper",                     FlowArtLib.ToUpper                      ); 
            AddFunction( "ToLower",                     FlowArtLib.ToLower                      ); 
            AddFunction( "Sin",                         FlowArtLib.Sin                          ); 
            AddFunction( "Cos",                         FlowArtLib.Cos                          ); 
            AddFunction( "Tan",                         FlowArtLib.Tan                          ); 
            AddFunction( "Asin",                        FlowArtLib.Asin                         ); 
            AddFunction( "Acos",                        FlowArtLib.Acos                         ); 
            AddFunction( "Atan",                        FlowArtLib.Atan                         );             
            AddFunction( "Sinh",                        FlowArtLib.Sinh                         ); 
            AddFunction( "Cosh",                        FlowArtLib.Cosh                         ); 
            AddFunction( "Tanh",                        FlowArtLib.Tanh                         );              
            AddFunction( "GCD",                         FlowArtLib.GCD                          ); 
            AddFunction( "LCM",                         FlowArtLib.LCM                          ); 
            AddFunction( "Exp",                         FlowArtLib.Exp                          ); 
            AddFunction( "Frac",                        FlowArtLib.Frac                         ); 
            AddFunction( "Freeze",                      FlowArtLib.Freeze                       ); 
            

        }

        static public List<string> GetAllFunctionsName()
        {
            return BuiltInFunctions.Keys.ToList();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        static private bool ExistFunction(string functionName)
        {
            functionName = functionName.ToUpper();
            return BuiltInFunctions.ContainsKey( functionName );
        }

        static public VarInfo Execute(string functionName, List<VarInfo> parameters)
        {
            try
            {
                if ( ! ExistFunction( functionName ) )
                {
                    return VarInfo.NullVal;
                }

                foreach(VarInfo parameter in parameters)
                {
                    if( parameter.isNull )
                        return VarInfo.NullVal;
                }

                return BuiltInFunctions[functionName](parameters);
            }
            catch
            {
                return VarInfo.NullVal;
            }
        }
    }
}
