namespace KursNetworks
{
    partial class ConnectionSettings
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
            this.SpeedBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BitBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StopBitBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EvenBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.PortBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SpeedBox
            // 
            this.SpeedBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpeedBox.FormattingEnabled = true;
            this.SpeedBox.Items.AddRange(new object[] {
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600"});
            this.SpeedBox.Location = new System.Drawing.Point(91, 77);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(185, 21);
            this.SpeedBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Скорость";
            // 
            // BitBox
            // 
            this.BitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BitBox.FormattingEnabled = true;
            this.BitBox.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.BitBox.Location = new System.Drawing.Point(91, 126);
            this.BitBox.Name = "BitBox";
            this.BitBox.Size = new System.Drawing.Size(185, 21);
            this.BitBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Биты данных";
            // 
            // StopBitBox
            // 
            this.StopBitBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StopBitBox.FormattingEnabled = true;
            this.StopBitBox.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.StopBitBox.Location = new System.Drawing.Point(91, 179);
            this.StopBitBox.Name = "StopBitBox";
            this.StopBitBox.Size = new System.Drawing.Size(185, 21);
            this.StopBitBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Стоп-биты";
            // 
            // EvenBox
            // 
            this.EvenBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EvenBox.FormattingEnabled = true;
            this.EvenBox.Items.AddRange(new object[] {
            "Нет",
            "Четные",
            "Нечетные"});
            this.EvenBox.Location = new System.Drawing.Point(91, 227);
            this.EvenBox.Name = "EvenBox";
            this.EvenBox.Size = new System.Drawing.Size(185, 21);
            this.EvenBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Четность";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 48);
            this.button1.TabIndex = 8;
            this.button1.Text = "Открыть порт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PortBox
            // 
            this.PortBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortBox.FormattingEnabled = true;
            this.PortBox.Location = new System.Drawing.Point(91, 34);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(185, 21);
            this.PortBox.TabIndex = 9;
            this.PortBox.SelectedValueChanged += new System.EventHandler(this.PortBox_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Порт";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(152, 283);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 48);
            this.button2.TabIndex = 11;
            this.button2.Text = "Закрыть порт";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 343);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EvenBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StopBitBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BitBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SpeedBox);
            this.Name = "ConnectionSettings";
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.ConnectionSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SpeedBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox BitBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox StopBitBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox EvenBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox PortBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
    }
}