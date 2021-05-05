using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;

namespace FlowArt
{
    public class DarkenBlockEffectPlayer
    {
        private FlowBlock Block;
        private Color OriginalBrushColor = Color.FromArgb(0,0,0,0);
        private Color OriginalBrushMidColor;
        private Color OriginalBrushForeColor;
        private Color OriginalPenColor;

        static private int FadingTime = 8;
        
        static private int FadingFrame = 8;

        private int curFadingFrame = FadingFrame;
        private int CurFadingFrame
        {
            get
            {
                return this.curFadingFrame;
            }
            set
            {
                this.curFadingFrame = value;
                CurDarkPercentage = MaxDarkPercentage*(FadingFrame - curFadingFrame)/FadingFrame;
                ApplyDarkPercentage();
            }
        }

        static private float MaxDarkPercentage = 0.3f;
        private float CurDarkPercentage = 0f;

        System.Windows.Forms.Timer FadingTimer = new System.Windows.Forms.Timer();
        
        //////////////////////////////////////////////////////////////////////////////////////////////////// 
        public DarkenBlockEffectPlayer(FlowBlock block)
        {
            this.Block = block;

            OriginalBrushColor = (Block.Background as GoShape).BrushColor;
            OriginalBrushMidColor = (Block.Background as GoShape).BrushMidColor;
            OriginalBrushForeColor = (Block.Background as GoShape).BrushForeColor;
            OriginalPenColor = (Block.Background as GoShape).PenColor;

            CurFadingFrame = FadingFrame;
            FadingTimer.Tick += this.FadingTimer_TickUpdate;
        }

        private Color ApplyDarkPercentage(Color color)
        {
            Color darkencolor = ControlPaint.Dark(color, MaxDarkPercentage);
            int dr = (int)(color.R - (color.R - darkencolor.R)*CurDarkPercentage/MaxDarkPercentage);
            int dg = (int)(color.G - (color.G - darkencolor.G)*CurDarkPercentage/MaxDarkPercentage);
            int db = (int)(color.B - (color.B - darkencolor.B)*CurDarkPercentage/MaxDarkPercentage);
            return Color.FromArgb(dr,dg,db);
        }

        private void ApplyDarkPercentage()
        {
            (Block.Background as GoShape).BrushColor = ApplyDarkPercentage(OriginalBrushColor); 
            (Block.Background as GoShape).BrushMidColor = ApplyDarkPercentage(OriginalBrushMidColor); 
            (Block.Background as GoShape).BrushForeColor = ApplyDarkPercentage(OriginalBrushForeColor);  
            (Block.Background as GoShape).PenColor = ApplyDarkPercentage(OriginalPenColor); 
        }

        public void DarkenColor()
        {
            CurFadingFrame = 0;
            FadingTimer.Stop();
        }

        public void NormalizeColor()
        {
            CurFadingFrame = FadingFrame;
        }

        public void FadeToNormalColor()
        {
            if(! FadingTimer.Enabled )
            {
                FadingTimer.Interval = FadingTime/FadingFrame;
                FadingTimer.Start();
            }
        }

        public void UpdateFadingInFrame()
        {
            if(CurFadingFrame >= FadingFrame)
                return; 
            CurFadingFrame++;
        }

        public void FadingTimer_TickUpdate(Object sender, EventArgs e)
        {
            UpdateFadingInFrame();
            if(CurFadingFrame >= FadingFrame)
            {
                FadingTimer.Stop();
            }
        }

        // Normalize color all blocks ////////////////////////////////////////////////////////////////////////////////////////////////// 
        static public void NormalizeColorDocument(FlowDocument doc)
        {
            foreach(GoObject obj in doc)
                if(obj is FlowBlock)
                {
                    (obj as FlowBlock).FxPlayer.NormalizeColor();
                }
        }

    }
}
