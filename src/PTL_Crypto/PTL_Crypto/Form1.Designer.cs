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
            textBoxCoin = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.FlatAppearance.BorderSize = 0;
            button1.Location = new Point(548, 510);
            button1.Name = "button1";
            button1.Size = new Size(312, 72);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(162, 70);
            button2.Name = "button2";
            button2.Size = new Size(40, 25);
            button2.TabIndex = 1;
            button2.Text = "7j";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(208, 70);
            button3.Name = "button3";
            button3.Size = new Size(40, 25);
            button3.TabIndex = 2;
            button3.Text = "30j";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(254, 70);
            button4.Name = "button4";
            button4.Size = new Size(40, 25);
            button4.TabIndex = 3;
            button4.Text = "1a";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1F;
            formsPlot1.Location = new Point(115, 101);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(745, 388);
            formsPlot1.TabIndex = 4;
            formsPlot1.Load += formsPlot1_Load;
            // 
            // button5
            // 
            button5.Location = new Point(116, 70);
            button5.Name = "button5";
            button5.Size = new Size(40, 25);
            button5.TabIndex = 5;
            button5.Text = "1j";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // textBoxCoin
            // 
            textBoxCoin.BackColor = Color.SteelBlue;
            textBoxCoin.Location = new Point(568, 70);
            textBoxCoin.Name = "textBoxCoin";
            textBoxCoin.Size = new Size(292, 23);
            textBoxCoin.TabIndex = 6;
            textBoxCoin.TextChanged += textBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(975, 649);
            Controls.Add(textBoxCoin);
            Controls.Add(button5);
            Controls.Add(formsPlot1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "PTL_Crypto";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button button5;
        private TextBox textBoxCoin;
    }
}
