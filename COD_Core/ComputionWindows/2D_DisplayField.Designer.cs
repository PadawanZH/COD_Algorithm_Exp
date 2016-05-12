namespace ComputionWindows
{
    partial class _2D_DisplayField
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pl_Canvas = new System.Windows.Forms.Panel();
            this.RefreshDataPointsTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pl_Canvas
            // 
            this.pl_Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pl_Canvas.BackColor = System.Drawing.SystemColors.Control;
            this.pl_Canvas.Location = new System.Drawing.Point(10, 91);
            this.pl_Canvas.Name = "pl_Canvas";
            this.pl_Canvas.Size = new System.Drawing.Size(851, 535);
            this.pl_Canvas.TabIndex = 0;
            this.pl_Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pl_Canvas_Paint);
            // 
            // RefreshDataPointsTimer
            // 
            this.RefreshDataPointsTimer.Interval = 1000;
            this.RefreshDataPointsTimer.Tick += new System.EventHandler(this.RefreshDataPointsTimer_Tick);
            // 
            // _2D_DisplayField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 640);
            this.Controls.Add(this.pl_Canvas);
            this.Name = "_2D_DisplayField";
            this.Text = "_2D_DisplayField";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pl_Canvas;
        private System.Windows.Forms.Timer RefreshDataPointsTimer;
    }
}