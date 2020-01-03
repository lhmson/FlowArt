using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowArt
{
    static class SpecialConstHandler
    {
        static private Dictionary<string, VarInfo> BuiltInConst = new Dictionary<string, VarInfo>();

        static private void AddConst(string constName, object value)
        {
            constName = constName.ToUpper();
            if( value is double )
            {
                double temp = (double)value;
                BuiltInConst.Add( constName, new VarInfo(temp) );
                return;
            }
            if( value is bool )
            {
                bool temp = (bool)value;
                BuiltInConst.Add( constName, new VarInfo(temp) );
                return;
            }
            if( value is string )
            {
                string temp = (string)value;
                BuiltInConst.Add( constName, new VarInfo(temp) );
                return;
            }
            if( value is char )
            {
                char temp = (char)value;
                BuiltInConst.Add( constName, new VarInfo(temp) );
                return;
            }
            throw new Exception("unhandled datatype");
        }

        static public void InitBuiltInConst()
        {
            AddConst("True",                                    true                                    );
            AddConst("False",                                   false                                   );
            AddConst("Yes",                                     true                                    );
            AddConst("No",                                      false                                   );
            AddConst("Pi",                                      Math.PI                                 );
            AddConst("E",                                       Math.E                                  );
            AddConst("NewLine",                                 Environment.NewLine                     );
            AddConst("Love",                                    "❤ Nguyễn Thị Hạnh ❤"                 );
        }

        static bool ExistConst(string constName)
        {
            constName = constName.ToUpper();
            return BuiltInConst.ContainsKey(constName);
        }

        static public List<String> GetAllConstName()
        {
            return BuiltInConst.Keys.ToList();
        }

        static public VarInfo GetConst(string constName)
        {
            constName = constName.ToUpper();
            if(! ExistConst(constName) )
            {
                return VarInfo.NullVal;
            }

            return BuiltInConst[constName];
        }

    }
}
