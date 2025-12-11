namespace FormTest
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TB_Value = new TrackBar();
            L_ValueL = new Label();
            NUD_Value = new NumericUpDown();
            L_ValueR = new Label();
            L_Pointer = new Label();
            B_S = new Button();
            B_P = new Button();
            ((System.ComponentModel.ISupportInitialize)TB_Value).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NUD_Value).BeginInit();
            SuspendLayout();
            // 
            // TB_Value
            // 
            TB_Value.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TB_Value.Location = new Point(12, 12);
            TB_Value.Maximum = 0;
            TB_Value.Name = "TB_Value";
            TB_Value.Size = new Size(1146, 80);
            TB_Value.TabIndex = 0;
            TB_Value.Scroll += TB_Value_Scroll;
            // 
            // L_ValueL
            // 
            L_ValueL.AutoSize = true;
            L_ValueL.Location = new Point(12, 64);
            L_ValueL.Name = "L_ValueL";
            L_ValueL.Size = new Size(72, 28);
            L_ValueL.TabIndex = 1;
            L_ValueL.Text = "00000";
            // 
            // NUD_Value
            // 
            NUD_Value.Location = new Point(12, 98);
            NUD_Value.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            NUD_Value.Name = "NUD_Value";
            NUD_Value.Size = new Size(210, 34);
            NUD_Value.TabIndex = 2;
            NUD_Value.ValueChanged += NUD_Value_ValueChanged;
            // 
            // L_ValueR
            // 
            L_ValueR.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            L_ValueR.AutoSize = true;
            L_ValueR.Location = new Point(1086, 64);
            L_ValueR.Name = "L_ValueR";
            L_ValueR.Size = new Size(72, 28);
            L_ValueR.TabIndex = 3;
            L_ValueR.Text = "00000";
            // 
            // L_Pointer
            // 
            L_Pointer.AutoSize = true;
            L_Pointer.Location = new Point(228, 100);
            L_Pointer.Name = "L_Pointer";
            L_Pointer.Size = new Size(0, 28);
            L_Pointer.TabIndex = 4;
            // 
            // B_S
            // 
            B_S.Location = new Point(1086, 94);
            B_S.Name = "B_S";
            B_S.Size = new Size(72, 40);
            B_S.TabIndex = 5;
            B_S.Text = "S";
            B_S.UseVisualStyleBackColor = true;
            B_S.Click += B_S_Click;
            // 
            // B_P
            // 
            B_P.Location = new Point(1008, 94);
            B_P.Name = "B_P";
            B_P.Size = new Size(72, 40);
            B_P.TabIndex = 6;
            B_P.Text = "P";
            B_P.UseVisualStyleBackColor = true;
            B_P.Click += this.B_P_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1170, 145);
            Controls.Add(B_P);
            Controls.Add(B_S);
            Controls.Add(L_Pointer);
            Controls.Add(L_ValueR);
            Controls.Add(NUD_Value);
            Controls.Add(L_ValueL);
            Controls.Add(TB_Value);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)TB_Value).EndInit();
            ((System.ComponentModel.ISupportInitialize)NUD_Value).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TrackBar TB_Value;
        private Label L_ValueL;
        private NumericUpDown NUD_Value;
        private Label L_ValueR;
        private Label L_Pointer;
        private Button B_S;
        private Button B_P;
    }
}
