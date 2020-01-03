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
        static private Random random = new Random();

        static private bool CheckParameterTypes( List< VarInfo > parameters, string typesCode )
        {
            typesCode = typesCode.ToUpper();
            if( parameters.Count !=  typesCode.Length )
            {
                return false;
            }

            for(int i = 0 ; i < parameters.Count; i++)
            {
                switch (typesCode[i])
                {
                    case 'B':
                    {
                        if( parameters[i].isNull == true || parameters[i].type != DataType.FA_Boolean )
                            return false;
                        break;
                    }

                    case 'N':
                    {
                       if( parameters[i].isNull == true || parameters[i].type != DataType.FA_Number )
                            return false; 
                        break;
                    }

                    case 'S':
                    {
                       if( parameters[i].isNull == true || parameters[i].type != DataType.FA_String )
                            return false; 
                        break;
                    }

                    case 'C':
                    {
                       if( parameters[i].isNull == true || parameters[i].type != DataType.FA_String || parameters[i].value.Length != 1 )
                            return false; 
                        break;
                    }

                    default:
                        return false;
                }
            }
            
            return true;
        } 

        private static int additional_GCD(int x, int y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            while (y != 0)
            {
                int temp = x % y;
                x = y;
                y = temp;
            }

            return x;
        }

        private static int additional_LCM(int x, int y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            try
            {
                return x*y/additional_GCD(x,y);
            }
            catch
            {
                return 0;
            }
        }


    }
}