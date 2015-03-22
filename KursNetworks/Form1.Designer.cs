namespace KursNetworks
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.DBUTTON1 = new System.Windows.Forms.Button();
            this.DBUTTON2 = new System.Windows.Forms.Button();
            this.FileChoice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.ForeColor = System.Drawing.Color.Chartreuse;
            this.textBox1.Location = new System.Drawing.Point(337, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(355, 105);
            this.textBox1.TabIndex = 0;
            this.textBox1.DoubleClick += new System.EventHandler(this.textBox1_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Dev Console";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(26, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(222, 29);
            this.button2.TabIndex = 5;
            this.button2.Text = "Настройки соединения";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label1.Location = new System.Drawing.Point(22, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Отсутствует подключение к COM-порту!";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // DBUTTON1
            // 
            this.DBUTTON1.Location = new System.Drawing.Point(388, 218);
            this.DBUTTON1.Name = "DBUTTON1";
            this.DBUTTON1.Size = new System.Drawing.Size(132, 55);
            this.DBUTTON1.TabIndex = 7;
            this.DBUTTON1.Text = "LISTEN";
            this.DBUTTON1.UseVisualStyleBackColor = true;
            this.DBUTTON1.Click += new System.EventHandler(this.DBUTTON1_Click);
            // 
            // DBUTTON2
            // 
            this.DBUTTON2.Location = new System.Drawing.Point(542, 218);
            this.DBUTTON2.Name = "DBUTTON2";
            this.DBUTTON2.Size = new System.Drawing.Size(150, 55);
            this.DBUTTON2.TabIndex = 8;
            this.DBUTTON2.Text = "SEND BITS";
            this.DBUTTON2.UseVisualStyleBackColor = true;
            this.DBUTTON2.Click += new System.EventHandler(this.DBUTTON2_Click);
            // FileChoice
            // 
            this.FileChoice.Location = new System.Drawing.Point(26, 130);
            this.FileChoice.Name = "FileChoice";
            this.FileChoice.Size = new System.Drawing.Size(222, 25);
            this.FileChoice.TabIndex = 7;
            this.FileChoice.Text = "Выбрать файл";
            this.FileChoice.UseVisualStyleBackColor = true;
            this.FileChoice.Click += new System.EventHandler(this.FileChoice_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(704, 353);
            this.Controls.Add(this.DBUTTON2);
            this.Controls.Add(this.DBUTTON1);
            this.ClientSize = new System.Drawing.Size(704, 313);
            this.Controls.Add(this.FileChoice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.Text = "Skype";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button DBUTTON1;
        private System.Windows.Forms.Button DBUTTON2;

        private System.Windows.Forms.Button FileChoice;
    }
}

