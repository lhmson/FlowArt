using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowArt
{
    public enum FlowRunState
    {
        Start = 0,
        Run = 1,
        Pause = 2,
    }

    class FlowRunManager
    {
        MainInterface form;

        FlowRunState state;
        public FlowRunState State
        {
            get
            {
                return this.state;
            }
            set
            {
                if( this.state != value )
                {
                    this.state = value;
                    FlowRunStateChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        private bool auto = true;
        public bool Auto
        {
            get
            {
                return auto;
            }
            set
            {
                if (!value)
                {
                    StopTimer();
                    if (State == FlowRunState.Run)
                        State = FlowRunState.Pause;
                }

                auto = value;
            }
        }

        public bool ClearOldResult = true;
        public bool NotifyOnEnd = false;
        public bool PlayFadingEffect = true;
        public bool PlaySoundEffect = true;

        private int timePerBlock = 1;
        public int TimePerBlock // in ms
        {
            get
            {
                return timePerBlock;
            }
            set
            {
                if(value <= 0)
                    return;
                if(timer == null)
                    return;

                timePerBlock = value;
                timer.Interval = value;
            }
        }


        Timer timer = new Timer();


        public EventHandler ReachEndBlock;
        public EventHandler RaiseError;
        public EventHandler TerminatedByUser;
        public EventHandler FlowRunStateChanged;
        public EventHandler EnterNextBlock;

        // this field is used to sync block tick
        bool ABlockIsExecuting = false;

        AudioManager audioManager = new AudioManager();


        // initialization ////////////////////////////////////////////////////////////////////////////////////////////////////    
        public FlowRunManager(MainInterface form, int TimePerBlock, bool Auto, bool ClearOldResult)
        {
            this.form = form;
            this.State = FlowRunState.Start;

            this.TimePerBlock = TimePerBlock;
            this.Auto = Auto;
            this.ClearOldResult = ClearOldResult;

            InitializeEvent();
        }

        private void InitializeEvent()
        {
            // add event handlers here
            // observer pattern I guess :)

            timer.Tick += timer_TickEvent;
            ReachEndBlock += FlowRunManager_ReachEndBlock_TerminateFlow;
            RaiseError += FlowRunManager_RaiseError_TerminateFlow; 
            EnterNextBlock += FlowRunManager_EnterNextBlock_PlayBlockTickSound;
        }


        
        // handle command ////////////////////////////////////////////////////////////////////////////////////////////////////    
        public void HandleControlCommand()
        {
            switch (State)
            {
                case FlowRunState.Start:
                {
                    // Control button is START right now
                    if( ClearOldResult )
                        ClearResult();

                    ClearHighLight();

                    if( Auto )
                    {
                        StartTimer();
                        State = FlowRunState.Run;
                    }
                    else
                    {
                        State = FlowRunState.Pause;
                        ExecuteNextBlock();
                    }
                    
                    break;
                }
                case FlowRunState.Pause:
                {
                    // Control button is RESUME/NEXT right now
                    if( Auto )
                    {
                        StartTimer();
                        State = FlowRunState.Run;
                    }
                    else
                    {
                        State = FlowRunState.Pause;
                        ExecuteNextBlock();
                    }
                    break;
                }
                case FlowRunState.Run:
                {
                    // disabled
                    break;
                }
                default:
                {
                    MessageBox.Show("Not implemented");
                    break;
                }
            }
        }

        public void HandlePauseCommand()
        {
            switch ( State )
            {
                case FlowRunState.Start:
                {
                    // disabled
                    break;
                }
                case FlowRunState.Pause:
                {
                    // disabled
                    break;
                }
                case FlowRunState.Run:
                {
                    StopTimer();
                    State = FlowRunState.Pause;
                    break;
                }
                default:
                {
                    MessageBox.Show("Not implemented");
                    break;
                }
            }
        }

        public void HandleTerminateCommand()
        {
            switch (State)
            {
                case FlowRunState.Start:
                {
                    // disabled
                    break;
                }
                case FlowRunState.Pause:
                {
                    ResetFlow();
                    StopTimer();
                    State = FlowRunState.Start;
                    NotifyTermination();
                    ClearHighLight();
                    TerminatedByUser?.Invoke(this, new EventArgs());
                    break;
                }
                case FlowRunState.Run:
                {
                    ResetFlow();
                    StopTimer();
                    State = FlowRunState.Start;
                    NotifyTermination();
                    ClearHighLight();
                    TerminatedByUser?.Invoke(this, new EventArgs());
                    break;
                }
                default:
                {
                    MessageBox.Show("Not implemented");
                    break;
                }
            }
        }

        public void HandleTimerTick()
        {
            if( ABlockIsExecuting )
                return;

            // synchronization
            ABlockIsExecuting = true;

            if( this.State == FlowRunState.Run )
                ExecuteNextBlock();
            else
                StopTimer();

            ABlockIsExecuting = false;
        }

        public void HandleEndBlockEvent()
        {
            ResetFlow();
            StopTimer();
            
            if( PlaySoundEffect )
                audioManager.PlaySound(FlowArtSound.Success);
            if( NotifyOnEnd )
                ShowSuccessNotification();

            State = FlowRunState.Start;
        }

        public void HandleErrorEvent()
        {
            ResetFlow();
            StopTimer();

            if( PlaySoundEffect )
                audioManager.PlaySound(FlowArtSound.Error);
            if( NotifyOnEnd )
                ShowErrorNotification();

            State = FlowRunState.Start;
        }

        // event delegate here //////////////////////////////////////////////////////////////////////////////////////////////////// 
        public void timer_TickEvent(object sender, EventArgs e)
        {
            HandleTimerTick();
        }

        public void FlowRunManager_ReachEndBlock_TerminateFlow(object sender, EventArgs e)
        {
            HandleEndBlockEvent();
        }

        public void FlowRunManager_RaiseError_TerminateFlow(object sender, EventArgs e)
        {
            HandleErrorEvent();
        }

        public void FlowRunManager_EnterNextBlock_PlayBlockTickSound(object sender, EventArgs e)
        {
            // if the running speed is too fast, disable tick sound
            if( PlaySoundEffect && timePerBlock > 100 )
            {
                audioManager.PlaySound(FlowArtSound.BlockTick);
            }
        }


        // implementation //////////////////////////////////////////////////////////////////////////////////////////////////// 
        FlowDataManager FlowData = new FlowDataManager();
        VarInfo LastBlockResult = null;
        FlowBlock CurrentBlock = null;

        private bool ResetFlow()
        {
            State = FlowRunState.Start;
            LastBlockResult = null;
            CurrentBlock = null;
            FlowData.ClearData();

            return true;
        }
    
        /// return false if the flow is terminated due to error or end-block
        private bool ExecuteNextBlock()
        {
            string ErrorReport = "";
            FlowView view = form.GetCurrentGoView() as FlowView;
            FlowMap map = form.ActiveMdiChild as FlowMap;

            if (view == null)
            {
                MainInterface.Instance.UserLog("Error: No flow is found", LogType.Error);
                MainInterface.Instance.UserLog("Please create a new Flow art or open your old project", LogType.Normal);
                ResetFlow();
                RaiseError?.Invoke(this, new EventArgs());
                return false;
            }
            if (map == null)
                return false;

            FlowBlock OldBlock = CurrentBlock;
            CurrentBlock = view.GetNextBlock(CurrentBlock, LastBlockResult, out ErrorReport);
            // remember to check if this view is the same as the previous view

            // reset flow when there is link error
            if( CurrentBlock == null )
            {
                MainInterface.Instance.UserLog(ErrorReport, LogType.Error);
                ResetFlow();
                RaiseError?.Invoke(this, new EventArgs());
                return false;
            }
            
            // unhighlight old block
            if( PlayFadingEffect )
                OldBlock?.FxPlayer.FadeToNormalColor();
            else
                OldBlock?.FxPlayer.NormalizeColor();

            // highlight new block
            CurrentBlock?.FxPlayer.DarkenColor();

            EnterNextBlock?.Invoke( this, new EventArgs() );            

            // reset flow if there is expression error
            LastBlockResult = HandleBlock(CurrentBlock, out ErrorReport);
            if( ErrorReport != "" )
            {
                MainInterface.Instance.UserLog(ErrorReport, LogType.Error);
                ResetFlow();
                RaiseError?.Invoke(this, new EventArgs());
                return false;
            }

            // reset flow if this is then End-block
            if( CurrentBlock.Kind == BlockType.End)
            {
                ResetFlow();
                ReachEndBlock?.Invoke(this, new EventArgs());
                return false;
            }

            // blank space because it's beautiful :)
            MainInterface.Instance.UserLog("", LogType.Normal);

            return true;
        }

        private bool StartTimer()
        {
            timer.Interval = Math.Max(1, TimePerBlock);
            if( Auto )
            {
                timer.Start();
            }
            return true;
        }

        private bool StopTimer()
        {
            timer.Stop();
            return true;
        }

        private bool ClearResult()
        {
            FlowMap View = form.ActiveMdiChild as FlowMap;
            MainInterface.Instance.ClearUserLog();
            MainInterface.Instance.ClearUserOutput();
            return true;
        }
        

        private bool UpdateWatch()
        {
            FlowMap view = form.ActiveMdiChild as FlowMap;
            MainInterface.Instance.UpdateWatch(FlowData);
            return true;
        }

        private bool ClearHighLight()
        {
            FlowMap View = form.ActiveMdiChild as FlowMap;
            FlowDocument doc = View?.Doc;
            
            if( doc == null )
                return true;

            DarkenBlockEffectPlayer.NormalizeColorDocument(doc);            
            return true;
        }

        public bool ShowSuccessNotification()
        {
            MessageBox.Show("Flow execution has been completed!", "Woo hoo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        public bool ShowErrorNotification()
        {
            MessageBox.Show("Flow execution has been terminated due to an error", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return true;
        }

        private bool NotifyTermination()
        {
            MainInterface.Instance.UserLog("Flow execution has been terminated.", LogType.Error);
            return true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handle blocks ///////////////////////////////////////////////////////////////////////////////////////////////// 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        // handle blocks behavior and return value of blocks if available
        private VarInfo HandleBlock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            VarInfo BlockResult = null;
            ErrorReport = "";

            MainInterface.Instance.UserLog("--> " + CurrentBlock.Kind.ToString() + " Block: " + CurrentBlock.Text, LogType.Normal);

            switch (CurrentBlock.Kind)
            {
                case BlockType.Start:
                {
                    BlockResult = HandleStartBlock(CurrentBlock, out ErrorReport);
                    break;
                }
                case BlockType.End:
                {
                    BlockResult = HandleEndBlock(CurrentBlock, out ErrorReport);
                    break;
                }
                case BlockType.Process:
                {
                    BlockResult = HandleProcessBlock(CurrentBlock, out ErrorReport);
                    break;
                }
                case BlockType.Input:
                {
                    BlockResult = HandleInputBlock(CurrentBlock, out ErrorReport);
                    break;
                }
                case BlockType.Output:
                {
                    BlockResult = HandleOutputblock(CurrentBlock, out ErrorReport);
                    break;
                }
                case BlockType.Condition:
                {
                    BlockResult = HandleConditionBlock(CurrentBlock, out ErrorReport);
                    break;
                }
            }

            UpdateWatch();
            return BlockResult;
        }  

        private VarInfo HandleStartBlock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            ErrorReport = "";
            MainInterface.Instance.UserLog("Flow execution has started", LogType.Normal);
            return null;
        }

        private VarInfo HandleEndBlock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            ErrorReport = "";
            MainInterface.Instance.UserLog("Flow execution has completed!", LogType.Success);
            return null;
        }

        private VarInfo HandleProcessBlock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            ErrorReport = "";

            string expression = CurrentBlock.Text;
            if( CurrentBlock.Text == "" )
            {
                ErrorReport = "Blank expression";
                return null;
            }

            // handle deletion
            if( CurrentBlock.Text[0] == '~' )
            {
                FlowData.HandleDeletion(expression, out ErrorReport);
                if( ErrorReport != "" )
                {
                    return null;
                }

                MainInterface.Instance.UserLog("Deletion: " + expression.Remove(0,1), LogType.Normal);   
                return null;
            }

            VarInfo varRes = FlowData.HandleAssignment(expression, out ErrorReport);
            if( ErrorReport != "" )
            {
                return null;
            }

            if( !varRes.isNull )
                MainInterface.Instance.UserLog("Assignment: " + CurrentBlock.Text + " = " + varRes.value, LogType.Normal);
            else
                MainInterface.Instance.UserLog("Declaration: " + CurrentBlock.Text, LogType.Normal);
            
            return varRes;
        }

        private VarInfo HandleInputBlock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            ErrorReport = "";

            string varAndType = CurrentBlock.Text;
            string varName;

            FlowData.HandleDeclaration(varAndType, out varName, out ErrorReport);
            if (ErrorReport != "")
            {
                return null;
            }

            DataType varType = FlowData.GetVarInfo(varName, out ErrorReport).type;
            if( ErrorReport != "" )
            {
                return null;
            }

            frmScan InputForm = new frmScan(varName, varType);
            DialogResult dialogResult = InputForm.ShowDialog();
            
            if(dialogResult == DialogResult.Cancel)
            {
                ErrorReport = "Error: User input was canceled";
                return null;
            }
            
            VarInfo InputVarInfo = InputForm.VarInfoResult;

            FlowData.AssignVariable(varName, InputVarInfo, out ErrorReport);
            if (ErrorReport != "")
            {
                return null;
            }

            MainInterface.Instance.UserLog("Input: " + varName + " = " + InputVarInfo.value, LogType.Normal);   
            return null;
        }

        private VarInfo HandleOutputblock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            ErrorReport = "";

            string expression = CurrentBlock.Text;
            VarInfo varRes = FlowData.HandleExpression(expression, out ErrorReport);

            if( ErrorReport != "" )
            {
                return null;
            }

            MainInterface.Instance.UserLog("Output: " + CurrentBlock.Text + " = " + varRes.value, LogType.Normal);
            MainInterface.Instance.UserOutput(varRes.value, LogType.Normal);
            return varRes;
        }

        private VarInfo HandleConditionBlock(FlowBlock CurrentBlock, out string ErrorReport)
        {
            FlowMap map = form.ActiveMdiChild as FlowMap;
            ErrorReport = "";

            string expression = CurrentBlock.Text;
            VarInfo varRes = FlowData.HandleExpression(expression, out ErrorReport);

            if( ErrorReport != "" )
            {
                return null;
            }

            MainInterface.Instance.UserLog("Value: " + CurrentBlock.Text + " = " + varRes.value, LogType.Normal);
            return varRes;
        }
        




    }
}
