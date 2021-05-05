using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowArt
{
    static partial class FlowArtLib
    {
        static public VarInfo ToUpper( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "S") )
            {
                string x = parameters[0].toCS_String();
                string temp;

                try
                {
                    temp = x.ToUpper();
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

        static public VarInfo ToLower( List<VarInfo> parameters )
        {
            if( CheckParameterTypes(parameters, "S") )
            {
                string x = parameters[0].toCS_String();
                string temp;

                try
                {
                    temp = x.ToLower();
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

        static public VarInfo Sin(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Sin(x);
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

        static public VarInfo Cos(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Cos(x);
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

        static public VarInfo Tan(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Tan(x);
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

        static public VarInfo Asin(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Asin(x);
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

        static public VarInfo Acos(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Acos(x);
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

        static public VarInfo Atan(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Atan(x);
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

        static public VarInfo Sinh(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Sinh(x);
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

        static public VarInfo Cosh(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Cosh(x);
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

        static public VarInfo Tanh(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Tanh(x);
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

        static public VarInfo GCD( List<VarInfo> parameters )
        {
            int temp = parameters[0].toCS_Int();
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp = additional_GCD( temp, parameter.toCS_Int() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }       

        static public VarInfo LCM( List<VarInfo> parameters )
        {
            int temp = parameters[0].toCS_Int();
            foreach (var parameter in parameters)
            {
                if( parameter.type == DataType.FA_Number && !parameter.isNull )
                    temp = additional_LCM( temp, parameter.toCS_Int() );
                else
                    return VarInfo.NullVal;
            }

            VarInfo result = new VarInfo( temp );
            return result;
        }     

        static public VarInfo Exp(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = Math.Exp(x);
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

        static public VarInfo Frac(List<VarInfo> parameters)
        {
            if (CheckParameterTypes(parameters, "N"))
            {
                double x = parameters[0].toCS_Double();
                double temp;

                try
                {
                    temp = x - (double)Math.Truncate(x);
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

        static public VarInfo Freeze(List<VarInfo> parameters)
        {
            // this is a trollllllll
            while(true){};
        }

    }
}

// 16 functions