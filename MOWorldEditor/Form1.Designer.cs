namespace MOWorldEditor
{
    partial class WorldEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.RaiseGround = new System.Windows.Forms.CheckBox();
            this.AreaSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.worldView = new MOWorldEditor.WorldView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreaSize)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.RaiseGround);
            this.panel1.Controls.Add(this.AreaSize);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(830, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 583);
            this.panel1.TabIndex = 0;
            // 
            // RaiseGround
            // 
            this.RaiseGround.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RaiseGround.AutoSize = true;
            this.RaiseGround.Location = new System.Drawing.Point(58, 29);
            this.RaiseGround.Name = "RaiseGround";
            this.RaiseGround.Size = new System.Drawing.Size(91, 17);
            this.RaiseGround.TabIndex = 3;
            this.RaiseGround.Text = "Raise Ground";
            this.RaiseGround.UseVisualStyleBackColor = true;
            // 
            // AreaSize
            // 
            this.AreaSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AreaSize.Location = new System.Drawing.Point(58, 3);
            this.AreaSize.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.AreaSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AreaSize.Name = "AreaSize";
            this.AreaSize.Size = new System.Drawing.Size(177, 20);
            this.AreaSize.TabIndex = 2;
            this.AreaSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Area";
            // 
            // worldView
            // 
            this.worldView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.worldView.Location = new System.Drawing.Point(13, 12);
            this.worldView.Name = "worldView";
            this.worldView.Size = new System.Drawing.Size(811, 583);
            this.worldView.TabIndex = 1;
            // 
            // WorldEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 607);
            this.Controls.Add(this.worldView);
            this.Controls.Add(this.panel1);
            this.Name = "WorldEditor";
            this.Text = "World Editor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreaSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private WorldView worldView;
        private System.Windows.Forms.NumericUpDown AreaSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox RaiseGround;
    }
}

