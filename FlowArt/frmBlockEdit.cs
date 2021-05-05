using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; // include this to do the round corner trick
using Northwoods.Go;
using System.Text.RegularExpressions;

namespace FlowArt
{
    public partial class frmBlockEdit : Form
    {
        public string ExpressionResult = "";
        private FlowBlock Block;

        
        private bool UseSmartAutoCompleteSorter = true;
        private List<string> SuggestionList;

        public frmBlockEdit()
        {
            InitializeComponent();
        }

        public frmBlockEdit(FlowBlock block)
        {
            InitializeComponent();

            // round the corner of form
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.Block = block;
            tbType.Text = block.Kind.ToString();
            tbExpression.Text = block.Text;

            InitMoveFormHandlers();
            InitializeAutoComplete();

            // Additional event handler
            tbExpression.TextChanged += tbExpression_UpdateCheckValidValue;
        }


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")] // this one rounds the corner
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );


        // Event Handlers //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private void btnOK_Click(object sender, EventArgs e)
        {
            if( tbExpression.Text != "" )
            {
                this.ExpressionResult = tbExpression.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                tbExpression.Focus();
            }
            
        }

        private void tbExpression_UpdateCheckValidValue(object sender, EventArgs e)
        {
            if( tbExpression.Text == "" )
            {
                tbExpression.ForeColor = Color.OrangeRed;
                tbExpression.BackColor = Color.FromArgb(255, 217, 215); // pastel red color
            }
            else
            {
                tbExpression.ForeColor = Color.Black;
                tbExpression.BackColor = Color.White;
            }
        }


        // User drag to move form /////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        private bool MouseIsDown = false;
        private Point MouseDownMouseLocation;
        private Point MouseDownFormLocation;

        private void frmBlockEdit_MouseDownMoveForm(object sender, MouseEventArgs e)
        {
            if( e.Button == MouseButtons.Left )
            {
                MouseIsDown = true;
                MouseDownMouseLocation = Cursor.Position;
                MouseDownFormLocation = this.Location;
            }
        }

        private void frmBlockEdit_MouseMoveMoveForm(object sender, MouseEventArgs e)
        {
            if( MouseIsDown && e.Button == MouseButtons.Left )
            {
                this.Location = new Point
                (
                    MouseDownFormLocation.X + Cursor.Position.X - MouseDownMouseLocation.X,  
                    MouseDownFormLocation.Y + Cursor.Position.Y - MouseDownMouseLocation.Y
                );
            }
        }

        private void frmBlockEdit_MouseUpMoveForm(object sender, MouseEventArgs e)
        {
            if( e.Button == MouseButtons.Left )
                MouseIsDown = false;
        }

        private void InitMoveFormHandlers()
        {
            this.MouseDown += frmBlockEdit_MouseDownMoveForm;
            this.MouseUp += frmBlockEdit_MouseUpMoveForm;
            this.MouseMove += frmBlockEdit_MouseMoveMoveForm;

            foreach(Control control in Controls)
                if( control is Label )
                {
                    control.MouseDown += frmBlockEdit_MouseDownMoveForm;
                    control.MouseUp += frmBlockEdit_MouseUpMoveForm;
                    control.MouseMove += frmBlockEdit_MouseMoveMoveForm;
                }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        // Auto completion ////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        
        // collection to store varname and function name
        private List<string> NameList = new List<string>();        

        
        /// function to get var name from a declaration
        private string TakeVarNameFromDeclaration(string expression)
        {
            string temp = FlowDataManager.RemoveSpacing(expression + "=");
            string result = "";
            bool StartedVarName = false;

            for(int i=0; i<temp.Length; i++)
            {
                if(temp[i] == ':')
                {
                    StartedVarName = true;
                    continue;
                }

                if(temp[i] == '=')
                {
                    break;
                }

                if(StartedVarName)
                {
                    result += temp[i];
                }
            }

            if( FlowDataManager.ValidName(result) )
                return result;
            else
                return null;
        }

        private List<string> FindAllVarName()
        {
            FlowDocument doc = Block.Document as FlowDocument;

            List<string> result = new List<string>();
            result.Clear();

            foreach(GoObject obj in doc)
                if( obj is FlowBlock )
                    if( (obj as FlowBlock).Kind == BlockType.Process || (obj as FlowBlock).Kind == BlockType.Input )
                    {
                        string varName = TakeVarNameFromDeclaration( (obj as FlowBlock).Text );
                        if( varName != null )
                            result.Add(varName);
                    }
                
            result = result.Distinct().ToList(); // BE AWARE OF THIS :)
            return result;
        }

        private List<string> FindAllFuncName()
        {
            List<string> result = FunctionHandler.GetAllFunctionsName();
            for(int i=0; i<result.Count; i++)
            {
                result[i] += "(";
            }
            return result;
        }

        private List<string> FindAllConstName()
        {
            List<string> result = SpecialConstHandler.GetAllConstName();
            for(int i=0; i<result.Count; i++)
            {
                result[i] = "$" + result[i];
            }
            return result;
        }

        private List<string> SpecialNames()
        {
            List<string> result = new List<string>();
            result.Clear();

            result.Add("Number:");
            result.Add("String:");
            result.Add("Boolean:");

            return result;
        }

        // do not show suggestions when declaring new variable
        private bool IsDeclaringNewVariableName()
        {
            string temp = tbExpression.Text;

            int colonIndex = temp.IndexOf(':');
            if( colonIndex < 0 )
                return false;

            int equalsgnIndex = temp.IndexOf('=', colonIndex);
            if( equalsgnIndex < 0 )
                return true;

            int curIndex = tbExpression.SelectionStart;
            if( colonIndex <= curIndex && curIndex <= equalsgnIndex )
                return true;
            else
                return false;
        }
        
        // do not show suggestions when writing a string
        private bool IsWritingString()
        {
            string temp = tbExpression.Text.Substring(0, tbExpression.SelectionStart);

            // count double primes in prefix
            int countDP = Regex.Matches( temp, "\"").Count;

            if( countDP % 2 == 1 )
                return true;
            
            return false;
        }

        private void HideFromSuggestionList(string name)
        {
            if( lstbxAutoComplete.Items.Contains(name) )
                lstbxAutoComplete.Items.Remove(name);
            if( SuggestionList.Contains(name) )
                SuggestionList.Remove(name);
        }

        private void HideSecretNames()
        {
            HideFromSuggestionList("$LOVE");
            HideFromSuggestionList("FREEZE(");
        }


        private void InitializeAutoComplete()
        {
            SuggestionList = new List<string>();
            lstbxAutoComplete.Items.Clear();
            
            List<string> tempList = new List<string>();
            tempList.AddRange( FindAllVarName() );
            tempList.AddRange( FindAllFuncName() );
            tempList.AddRange( FindAllConstName() );
            tempList.AddRange( SpecialNames() );
            tempList.Sort();

            lstbxAutoComplete.Items.AddRange( tempList.ToArray() );

            if(UseSmartAutoCompleteSorter)
            {
                SuggestionList.AddRange(tempList);
            }

            HideSecretNames();
        }

        private int CurrentWordStartPos()
        {
            if( tbExpression.Text == "" )
                return 0;

            int index = tbExpression.SelectionStart - 1;

            while( true )
            {
                if( index < 0 || index > tbExpression.Text.Length - 1 )
                    break;

                char c = tbExpression.Text[ index ];
                if( FlowDataManager.ValidNameChar(c) || c == '$' )
                    index--;
                else
                {
                    break;
                }
            }
            index++;
            return index;
        }

        private int CurrentWordEndPos()
        {
            if( tbExpression.Text == "" )
                return 0;

            int index = tbExpression.SelectionStart - 1;

            while( true )
            {
                if( index < 0 || index > tbExpression.Text.Length - 1 )
                    break;

                char c = tbExpression.Text[ index ];
                if( FlowDataManager.ValidNameChar(c) || c == '$' )
                    index++;
                else
                {
                    break;
                }
            }
            index--;
            return index;
        }

        private string CurrentWord()
        {
            int startIndex = CurrentWordStartPos();
            int endIndex = CurrentWordEndPos();

            if( startIndex < 0 )
                return "";
            if( endIndex > tbExpression.Text.Length - 1)
                return "";
            if( startIndex > endIndex )
                return "";
            if( tbExpression.Text == "" )
                return "";

            return tbExpression.Text.Substring(startIndex, endIndex - startIndex + 1);
        }

        private void ShowAutoCompleteList()
        {
            // do not show suggestions when declaring new variable
            if( IsDeclaringNewVariableName() )
            {
                lstbxAutoComplete.Hide();
                return;
            }
            
            // do not show suggestions when writing a string
            if( IsWritingString() )
            {
                lstbxAutoComplete.Hide();
                return;
            }

            string word = CurrentWord();

            if( word != "" )
            {
                // do not show suggestion when the first character of the word is a number
                if(word[0] >= '0' && word[0] <= '9')
                {
                    lstbxAutoComplete.Hide();
                    return;
                }

                if( UseSmartAutoCompleteSorter )
                {
                    SmartAutoCompletionSorter.Sort(ref SuggestionList, word);
                    
                    lstbxAutoComplete.Items.Clear();
                    lstbxAutoComplete.Items.AddRange(SuggestionList.ToArray());
                    lstbxAutoComplete.SelectedIndex = 0;

                    lstbxAutoComplete.Show();
                }
                else
                {
                    int tempIndex = lstbxAutoComplete.FindString( word );
                    if( tempIndex >= 0 )
                    {
                        lstbxAutoComplete.SelectedIndex = tempIndex;
                        lstbxAutoComplete.Show();
                    }
                    else
                    {
                        lstbxAutoComplete.Hide();
                    }
                }
            }
            else
            {
                lstbxAutoComplete.Hide();
            }
        }

        private void ApplyAutoComplete()
        {
            int startIndex = CurrentWordStartPos();
            int endIndex = CurrentWordEndPos();
            tbExpression.Text = tbExpression.Text.Remove( startIndex, endIndex - startIndex + 1 );
            tbExpression.Text = tbExpression.Text.Insert(startIndex, lstbxAutoComplete.SelectedItem.ToString() );

            tbExpression.SelectionStart = startIndex + lstbxAutoComplete.SelectedItem.ToString().Length;

            // if this is a function, move pointer back
            // if( tbExpression.Text[ tbExpression.SelectionStart - 1 ] == ')' )
            //     tbExpression.SelectionStart --;
        
            lstbxAutoComplete.Hide();
        }

        private void tbExpression_ShowLstBxAutoComplete(object sender, EventArgs e)
        {
            ShowAutoCompleteList();
        }
        private void lstbxAutoComplete_ApplyAutoCompleteByClick(object sender, MouseEventArgs e)
        {
            if( e.Button == MouseButtons.Left )
                ApplyAutoComplete();
        }
                

        /// <summary>
        /// Override ProcessCmdKey to have full control of autocompletelistbox
        /// And catch keys down to control Undo Action
        /// </summary>
        /// <param name="m"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if( lstbxAutoComplete.Visible )
            {
                switch (keyData)
                {
                    case Keys.Up:
                    {
                        if( lstbxAutoComplete.SelectedIndex > 0 )
                            lstbxAutoComplete.SelectedIndex --;
                        return true;
                    }
                    case Keys.Down:
                    {
                        if( lstbxAutoComplete.SelectedIndex < lstbxAutoComplete.Items.Count - 1 )
                            lstbxAutoComplete.SelectedIndex ++;
                        return true;
                    }

                    case Keys.Right:
                    case Keys.Left:
                    {
                        lstbxAutoComplete.Hide();
                        break;
                    }

                    case Keys.Enter:
                    case Keys.Tab:
                    {
                        ApplyAutoComplete();
                        return true;
                    }

                    case Keys.Escape:
                    {
                        lstbxAutoComplete.Hide();
                        return true;
                    }
                }
            }
            else
            {
                switch (keyData)
                {
                    case Keys.Down:
                    {
                        ShowAutoCompleteList();
                        return true;
                    }

                    case Keys.Escape:
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();   
                        return true;
                    }

                    case Keys.Enter:
                    {
                        if( tbExpression.Text != "" )
                        {
                            this.ExpressionResult = tbExpression.Text;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            tbExpression.Focus();
                        }
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref m, keyData);
        }









    }
}
