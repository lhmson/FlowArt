using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowArt
{
    static partial class FlowArtLib
    {
        static public VarInfo Sum( List<VarInfo> parameters )
        {
            double temp = 0;
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp += parameter.toCS_Double();
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }       

        static public VarInfo Mul( List<VarInfo> parameters )
        {
            double temp = 1;
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp *= parameter.toCS_Double();
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }  

        static public VarInfo Min( List<VarInfo> parameters )
        {
            double temp = parameters[0].toCS_Double();
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp = Math.Min( temp, parameter.toCS_Double() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }  

        static public VarInfo Max( List<VarInfo> parameters )
        {
            double temp = parameters[0].toCS_Double();
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp = Math.Max( temp, parameter.toCS_Double() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        } 

        static public VarInfo Concat( List<VarInfo> parameters )
        {
            string temp = "";
            foreach (var parameter in parameters)
            {
                if( !parameter.isNull )
                    temp += parameter.toCS_String();     
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }

        static public VarInfo IsInc( List<VarInfo> parameters )
        {
            bool temp = true;
            for(int i=0; i<parameters.Count-1; i++)
            {
                if( parameters[i].type == DataType.FA_Number && parameters[i+1].type == DataType.FA_Number )
                    temp = temp && ( parameters[i].toCS_Double() < parameters[i+1].toCS_Double() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }

        static public VarInfo IsDec( List<VarInfo> parameters )
        {
            bool temp = true;
            for(int i=0; i<parameters.Count-1; i++)
            {
                if( parameters[i].type == DataType.FA_Number && parameters[i+1].type == DataType.FA_Number )
                    temp = temp && ( parameters[i].toCS_Double() > parameters[i+1].toCS_Double() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }

        static public VarInfo IsIncEq( List<VarInfo> parameters )
        {
            bool temp = true;
            for(int i=0; i<parameters.Count-1; i++)
            {
                if( parameters[i].type == DataType.FA_Number && parameters[i+1].type == DataType.FA_Number )
                    temp = temp && ( parameters[i].toCS_Double() <= parameters[i+1].toCS_Double() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }

        static public VarInfo IsDecEq( List<VarInfo> parameters )
        {
            bool temp = true;
            for(int i=0; i<parameters.Count-1; i++)
            {
                if( parameters[i].type == DataType.FA_Number && parameters[i+1].type == DataType.FA_Number )
                    temp = temp && ( parameters[i].toCS_Double() >= parameters[i+1].toCS_Double() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }

        static public VarInfo Sub( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                double y = parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = x - y;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Div( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                double y = parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = x / y;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Pow( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                double y = parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Pow(x,y);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Mod( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                double y = parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = x % y;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Root( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                double y = parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Pow(y,1f/x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if( CheckParameterTypes(parameters, "N") )
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Pow(x,0.5f);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Sqrt( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "N") )
            {
                return Root( parameters );
            }

            return VarInfo.NullVal;
        }
        
        static public VarInfo Log( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                double y = parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Log(y, x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if( CheckParameterTypes(parameters, "N") )
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Log10(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }
        
        static public VarInfo Ln( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "N") )
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Log(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }
        
        static public VarInfo IsEqual( List<VarInfo> parameters )
        {
            if( parameters.Count == 2 )
            {
                bool temp;
                try
                {
                    temp = VarInfo.IsEqual(parameters[0], parameters[1]);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Floor( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "N") )
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Floor(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Ceiling( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "N") )
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Ceiling(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Round( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "NN") )
            {
                double x = parameters[0].toCS_Double();
                int y = (int)Math.Floor( parameters[1].toCS_Double() );
                double temp;

                try
                {
                    temp = (double)Math.Round(x, y);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if( CheckParameterTypes(parameters, "N") )
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = (double)Math.Round(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Length(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "S"))
            {
                string x = parameters[0].toCS_String();
                double temp;

                try
                {
                    temp = (double)x.Length;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo ToString(List<VarInfo> parameters)
        {
            string x = parameters[0].toCS_String();
            var temp = "";

            try
            {
                temp = x;
            }
            catch
            {
                return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo(temp);
            return result;
        }

        static public VarInfo Neg(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = -(double)(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Inverse(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = 1 / x;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo GetChar(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SN"))
            {
                string x = parameters[0].toCS_String();
                int y = (int)Math.Floor(parameters[1].toCS_Double());
                string temp;

                try
                {
                    temp = x[y].ToString();
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }
            return VarInfo.NullVal;
        }


        static public VarInfo SetChar(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SNC"))
            {
                string x = parameters[0].toCS_String();
                int y = (int)Math.Floor(parameters[1].toCS_Double());
                string c = parameters[2].toCS_String();
                string temp;

                try
                {
                    temp = x.Substring(0, y) + c + x.Substring(y+1);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }
            return VarInfo.NullVal;
        }

        static public VarInfo Replace(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SSS"))
            {
                string x = parameters[0].toCS_String();
                string a = parameters[1].toCS_String();
                string b = parameters[2].toCS_String();
                string temp;

                try
                {
                    temp = x.Replace(a, b);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }
            return VarInfo.NullVal;
        }

        static public VarInfo Remove(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SNN"))
            {
                string x = parameters[0].toCS_String();
                int a = (int)parameters[1].toCS_Double();
                int b = (int)parameters[2].toCS_Double();
                string temp;

                try
                {
                    temp = x.Remove(a, b);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, "SN"))
            {
                string x = parameters[0].toCS_String();
                int a = (int)parameters[1].toCS_Double();
                string temp;

                try
                {
                    temp = x.Remove(a);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, "SS"))
            {
                string x = parameters[0].toCS_String();
                string y = parameters[1].toCS_String();
                string temp;

                try
                {
                    temp = x.Replace(y, "");
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Insert(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SNS"))
            {
                string x = parameters[0].toCS_String();
                string y = parameters[2].toCS_String();
                int a = (int)parameters[1].toCS_Double();
                string temp;

                try
                {
                    temp = x.Insert(a, y);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo CountSubstr(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SS"))
            {
                string x = parameters[0].toCS_String();
                string y = parameters[1].toCS_String();
                double temp;

                try
                {
                    temp = (double)Regex.Matches(x, y).Count; // used for counting substring of str
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Find(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SSNN"))
            {
                string x = parameters[0].toCS_String();
                string y = parameters[1].toCS_String();
                int a = (int)parameters[2].toCS_Double();
                int b = (int)parameters[3].toCS_Double();
                double temp;

                try
                {
                    temp = (double)x.IndexOf(y, a, b);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, "SSN"))
            {
                string x = parameters[0].toCS_String();
                string y = parameters[1].toCS_String();
                int a = (int)parameters[2].toCS_Double();
                double temp;

                try
                {
                    temp = (double)x.IndexOf(y, a);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, "SS"))
            {
                string x = parameters[0].toCS_String();
                string y = parameters[1].toCS_String();
                double temp;

                try
                {
                    temp = (double)x.IndexOf(y);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo SubStr(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "SNN"))
            {
                string x = parameters[0].toCS_String();
                int a = (int)parameters[1].toCS_Double();
                int b = (int)parameters[2].toCS_Double();
                string temp;

                try
                {
                    temp = x.Substring(a, b);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, "SN"))
            {
                string x = parameters[0].toCS_String();
                int a = (int)parameters[1].toCS_Double();
                string temp;

                try
                {
                    temp = x.Substring(a);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo ToNumber(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "S"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = x;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo And( List<VarInfo> parameters )
        {
            bool temp = true;
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Boolean && !parameter.isNull )
                    temp = temp && parameter.toCS_bool();
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }  

        static public VarInfo Or( List<VarInfo> parameters )
        {
            bool temp = false;
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Boolean && !parameter.isNull )
                    temp = temp || parameter.toCS_bool();
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }  

        static public VarInfo Xor( List<VarInfo> parameters )
        {
            bool temp = false;
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Boolean && !parameter.isNull )
                    temp = temp ^ parameter.toCS_bool();
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }  

        static public VarInfo Not(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "B"))
            {
                bool x = parameters[0].toCS_bool();
                bool temp;

                try
                {
                    temp = ! x;
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Random(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "NN"))
            {
                int x = (int)parameters[0].toCS_Double();
                int y = (int)parameters[1].toCS_Double();
                double temp;

                try
                {
                    temp = random.Next(x,y+1);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, "N"))
            {
                int x = (int)parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = random.Next(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            if (CheckParameterTypes(parameters, ""))
            {
                double temp;

                try
                {
                    temp = random.Next();
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo Average( List<VarInfo> parameters )
        {
            double temp = 0;
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp += parameter.toCS_Double();
                else
                    return VarInfo.NullVal;
            }

            if( parameters.Count == 0 )
                return VarInfo.NullVal;

            temp/=parameters.Count;

            VarInfo result = new VarInfo( temp );
            return result;
        }       

        static public VarInfo AndNum( List<VarInfo> parameters )
        {
            if( parameters.Count == 0 )
                return VarInfo.NullVal;
            
            int temp = parameters[0].toCS_Int();
            foreach (var parameter in parameters)
            {
                try
                {
                    if( parameter.type == DataType.FA_Number && !parameter.isNull )
                        temp &= parameter.toCS_Int();
                    else
                        return VarInfo.NullVal;
                }
                catch
                {
                    return VarInfo.NullVal;
                }
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }       
        
        static public VarInfo OrNum( List<VarInfo> parameters )
        {
            if( parameters.Count == 0 )
                return VarInfo.NullVal;
            
            int temp = parameters[0].toCS_Int();
            foreach (var parameter in parameters)
            {
                try
                {
                    if( parameter.type == DataType.FA_Number && !parameter.isNull )
                        temp |= parameter.toCS_Int();
                    else
                        return VarInfo.NullVal;
                }
                catch
                {
                    return VarInfo.NullVal;
                }
            }

            VarInfo result = new VarInfo( temp );
            return result;
        } 

        static public VarInfo XorNum( List<VarInfo> parameters )
        {
            if( parameters.Count == 0 )
                return VarInfo.NullVal;
            
            int temp = parameters[0].toCS_Int();
            foreach (var parameter in parameters)
            {
                try
                {
                    if( parameter.type == DataType.FA_Number && !parameter.isNull )
                        temp ^= parameter.toCS_Int();
                    else
                        return VarInfo.NullVal;
                }
                catch
                {
                    return VarInfo.NullVal;
                }
            }

            VarInfo result = new VarInfo( temp );
            return result;
        } 

        static public VarInfo RandomFloat(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, ""))
            {
                double temp;

                try
                {
                    temp = random.NextDouble();
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }
        
        static public VarInfo RandomPick( List<VarInfo> parameters )
        {
            if( parameters.Count == 0 )
                return VarInfo.NullVal;

            int index = random.Next(parameters.Count);

            foreach (var parameter in parameters)
            {
                if( parameter.type != parameters[0].type || parameter.isNull )
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( parameters[index] );
            return result;
        }   

        static public VarInfo AsciiOrd(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "C"))
            {
                string x = parameters[0].toCS_String();
                int temp;

                try
                {
                    temp = (int)x[0];
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo AsciiChar(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                int x = parameters[0].toCS_Int();
                string temp;

                try
                {
                    temp = ((char)x).ToString();
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

        static public VarInfo IsInteger(List<VarInfo> parameters)
        {
            if( parameters.Count != 1 )
                return VarInfo.NullVal;

            if( parameters[0].isNull )
                return VarInfo.NullVal;

            bool temp;            
            string value = parameters[0].value;

            try
            {
                int.Parse(value);
                temp = true;
            }
            catch
            {
                temp = false;
            }

            VarInfo result = new VarInfo(temp);
            return result;
        }

        static public VarInfo IsNumber(List<VarInfo> parameters)
        {
            if( parameters.Count != 1 )
                return VarInfo.NullVal;

            if( parameters[0].isNull )
                return VarInfo.NullVal;

            bool temp;            
            string value = parameters[0].value;

            try
            {
                double.Parse(value);
                temp = true;
            }
            catch
            {
                temp = false;
            }

            VarInfo result = new VarInfo(temp);
            return result;
        }

        static public VarInfo Abs(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Abs(x);
                }
                catch
                {
                    return VarInfo.NullVal;
                }

                VarInfo result = new VarInfo(temp);
                return result;
            }

            return VarInfo.NullVal;
        }

    }
}

// 50 functions