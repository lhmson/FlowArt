using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowArt
{
    public enum DataType
    {
        FA_Number,
        FA_String,
        FA_Boolean,
    }

    class VarInfo
    {
        public DataType type;
        public string value;
        public bool isNull;
        

        static readonly public VarInfo NullVal = new VarInfo(DataType.FA_String, "", true);


        public VarInfo(DataType type, string value, bool isNull = false)
        {
            this.type = type;
            this.value = value;
            this.value = StandardizeValue(this.value, this.type);

            this.isNull = isNull || ! ValidValue(this.value, type);
        }

        public VarInfo(double value)
        {
            this.type = DataType.FA_Number;
            this.value = value.ToString("0." + new string('#', 339));
            this.isNull = false;
        }

        public VarInfo(int value)
        {
            this.type = DataType.FA_Number;
            this.value = value.ToString("0." + new string('#', 339));
            this.isNull = false;
        }

        public VarInfo(string value)
        {
            this.type = DataType.FA_String;
            this.value = value;
            this.isNull = false;
        }

        public VarInfo(bool value)
        {
            this.type = DataType.FA_Boolean;
            this.value = value? "TRUE":"FALSE";
            this.isNull = false;
        }

        public VarInfo(char value)
        {
            this.type = DataType.FA_Boolean;
            this.value = value.ToString();
            this.isNull = false;
        }

        public VarInfo(VarInfo instance)
        {
            this.type = instance.type;
            this.value = instance.value;
            this.value = StandardizeValue(this.value, this.type);
            this.isNull = instance.isNull;
        }

        public double toCS_Double()
        {
            try
            {
                return double.Parse(this.value);
            }
            catch
            {
                return double.NaN;
            }
        }

        public int toCS_Int()
        {
            try
            {
                return int.Parse(this.value);
            }
            catch
            {
                return int.MinValue;
            }
        }

        public string toCS_String()
        {
            return this.value;
        }

        public bool toCS_bool()
        {
            return this.value == "TRUE" || this.value == "YES";
        }

    // CHECK VALID DATA TYPE //////////////////////////////////////////////////////////////////////////////////////////////////// 
        static private bool ValidNumberValue(string value)
        {
            try
            {
                double temp = double.Parse(value);
                if( double.IsInfinity(temp) )
                    return false;
                if( double.IsNaN(temp) )
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        static private bool ValidBooleanValue(string value)
        {
            value = value.ToUpper();
            return ( value == "TRUE" || value == "FALSE" || value == "YES" || value == "NO");
        }

        static private bool ValidStringValue(string value)
        {
            return true;
        }

        static public bool ValidValue(string value, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.FA_Boolean:
                {
                    return ValidBooleanValue(value);
                }
                case DataType.FA_Number:
                {
                    return ValidNumberValue(value);
                }
                case DataType.FA_String:
                {
                    return ValidStringValue(value);
                }
                default:
                {
                    return false;
                }
            }
        }
    // standardize value ////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        static public string StandardizeValue(string value, DataType dataType)
        {
            if( !ValidValue(value, dataType) )
                return value;

            string result = value;
            if( dataType == DataType.FA_Boolean )
            {
                result = value.ToUpper();   
            }
            else
            if( dataType == DataType.FA_Number )
            {
                double doubleVal = double.Parse(value);
                result = doubleVal.ToString("0." + new string('#', 339));
            }
            
            return result;
        }

    // check 2 value is equal //////////////////////////////////////////////////////////////////////////////////////////////////// 
        static public bool IsEqual(VarInfo var1, VarInfo var2)
        {
            if(var1.isNull || var2.isNull)
                return false;

            if(var1.type != var2.type)
                return false;

            switch(var1.type)
            {
                case DataType.FA_Boolean:
                {
                    return var1.toCS_bool() == var2.toCS_bool();
                }
                case DataType.FA_String:
                {
                    return var1.toCS_String() == var2.toCS_String();
                }
                case DataType.FA_Number:
                {
                    const double epsilon = 1e-9;
                    return Math.Abs(var1.toCS_Double() - var2.toCS_Double()) <= epsilon;
                }
            }
            return false;
        }

    }
}
