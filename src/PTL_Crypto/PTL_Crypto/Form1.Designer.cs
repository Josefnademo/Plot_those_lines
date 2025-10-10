namespace PTL_Crypto
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            button5 = new Button();
            comboBoxCoins = new ComboBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.FlatAppearance.BorderSize = 0;
            button1.Location = new Point(626, 680);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(357, 96);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(185, 93);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(46, 33);
            button2.TabIndex = 1;
            button2.Text = "7j";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(238, 93);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(46, 33);
            button3.TabIndex = 2;
            button3.Text = "30j";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(290, 93);
            button4.Margin = new Padding(3, 4, 3, 4);
            button4.Name = "button4";
            button4.Size = new Size(46, 33);
            button4.TabIndex = 3;
            button4.Text = "1a";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1F;
            formsPlot1.Location = new Point(131, 135);
            formsPlot1.Margin = new Padding(3, 4, 3, 4);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(851, 517);
            formsPlot1.TabIndex = 4;
            formsPlot1.Load += formsPlot1_Load;
            // 
            // button5
            // 
            button5.Location = new Point(133, 93);
            button5.Margin = new Padding(3, 4, 3, 4);
            button5.Name = "button5";
            button5.Size = new Size(46, 33);
            button5.TabIndex = 5;
            button5.Text = "1j";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // comboBoxCoins
            // 
            comboBoxCoins.FormattingEnabled = true;
            comboBoxCoins.Location = new Point(649, 93);
            comboBoxCoins.Margin = new Padding(3, 4, 3, 4);
            comboBoxCoins.Name = "comboBoxCoins";
            comboBoxCoins.Size = new Size(333, 28);
            comboBoxCoins.TabIndex = 7;
            comboBoxCoins.SelectedIndexChanged += comboBoxCoins_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1114, 865);
            Controls.Add(comboBoxCoins);
            Controls.Add(button5);
            Controls.Add(formsPlot1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "PTL_Crypto";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button button5;
        private ComboBox comboBoxCoins;
    }
}
