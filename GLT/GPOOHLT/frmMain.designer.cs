namespace GPOOHLT
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkCallTaker = new System.Windows.Forms.LinkLabel();
            this.lnkNurse = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkDoctor = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "To process the CallTaker and Patient data,";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "To process the Nurse consultation data,";
            // 
            // lnkCallTaker
            // 
            this.lnkCallTaker.AutoSize = true;
            this.lnkCallTaker.Location = new System.Drawing.Point(224, 148);
            this.lnkCallTaker.Name = "lnkCallTaker";
            this.lnkCallTaker.Size = new System.Drawing.Size(53, 13);
            this.lnkCallTaker.TabIndex = 4;
            this.lnkCallTaker.TabStop = true;
            this.lnkCallTaker.Text = "click here";
            this.lnkCallTaker.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCallTaker_LinkClicked);
            // 
            // lnkNurse
            // 
            this.lnkNurse.AutoSize = true;
            this.lnkNurse.Location = new System.Drawing.Point(207, 175);
            this.lnkNurse.Name = "lnkNurse";
            this.lnkNurse.Size = new System.Drawing.Size(53, 13);
            this.lnkNurse.TabIndex = 6;
            this.lnkNurse.TabStop = true;
            this.lnkNurse.Text = "click here";
            this.lnkNurse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNurse_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(129, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "GPOOH Load Testing";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 59);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(412, 58);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "GPOOH Load Testing tool process the multiple calls simultaneously";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(354, 274);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "To process the Doctor consultation data,";
            // 
            // lnkDoctor
            // 
            this.lnkDoctor.AutoSize = true;
            this.lnkDoctor.Location = new System.Drawing.Point(218, 201);
            this.lnkDoctor.Name = "lnkDoctor";
            this.lnkDoctor.Size = new System.Drawing.Size(53, 13);
            this.lnkDoctor.TabIndex = 13;
            this.lnkDoctor.TabStop = true;
            this.lnkDoctor.Text = "click here";
            this.lnkDoctor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDoctor_LinkClicked);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 310);
            this.Controls.Add(this.lnkDoctor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lnkNurse);
            this.Controls.Add(this.lnkCallTaker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GPOOHLT";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkCallTaker;
        private System.Windows.Forms.LinkLabel lnkNurse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lnkDoctor;
    }
}