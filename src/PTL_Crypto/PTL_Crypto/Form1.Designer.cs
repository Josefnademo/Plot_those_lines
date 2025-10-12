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
            btnImportJson = new Button();
            btn7Days = new Button();
            btn30Days = new Button();
            btn365Day = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            btn1Day = new Button();
            comboBoxCoins = new ComboBox();
            clbCryptos = new CheckedListBox();
            SuspendLayout();
            // 
            // btnImportJson
            // 
            btnImportJson.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            btnImportJson.FlatAppearance.BorderSize = 0;
            btnImportJson.Location = new Point(745, 671);
            btnImportJson.Margin = new Padding(3, 4, 3, 4);
            btnImportJson.Name = "btnImportJson";
            btnImportJson.Size = new Size(357, 96);
            btnImportJson.TabIndex = 0;
            btnImportJson.UseVisualStyleBackColor = true;
            btnImportJson.Click += btnImportJson_Click;
            // 
            // btn7Days
            // 
            btn7Days.Location = new Point(76, 80);
            btn7Days.Margin = new Padding(3, 4, 3, 4);
            btn7Days.Name = "btn7Days";
            btn7Days.Size = new Size(46, 33);
            btn7Days.TabIndex = 1;
            btn7Days.Text = "7j";
            btn7Days.UseVisualStyleBackColor = true;
            btn7Days.Click += btn7Days_Click;
            // 
            // btn30Days
            // 
            btn30Days.Location = new Point(129, 80);
            btn30Days.Margin = new Padding(3, 4, 3, 4);
            btn30Days.Name = "btn30Days";
            btn30Days.Size = new Size(46, 33);
            btn30Days.TabIndex = 2;
            btn30Days.Text = "30j";
            btn30Days.UseVisualStyleBackColor = true;
            btn30Days.Click += btn30Days_Click;
            // 
            // btn365Day
            // 
            btn365Day.Location = new Point(181, 80);
            btn365Day.Margin = new Padding(3, 4, 3, 4);
            btn365Day.Name = "btn365Day";
            btn365Day.Size = new Size(46, 33);
            btn365Day.TabIndex = 3;
            btn365Day.Text = "1a";
            btn365Day.UseVisualStyleBackColor = true;
            this.btn365Day.Click += new System.EventHandler(this.btn365Day_Click);

            // 
            // formsPlot1
            // 
            formsPlot1.DisplayScale = 1F;
            formsPlot1.Location = new Point(22, 122);
            formsPlot1.Margin = new Padding(3, 4, 3, 4);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(851, 517);
            formsPlot1.TabIndex = 4;
            formsPlot1.Load += formsPlot1_Load;
            // 
            // btn1Day
            // 
            btn1Day.Location = new Point(24, 80);
            btn1Day.Margin = new Padding(3, 4, 3, 4);
            btn1Day.Name = "btn1Day";
            btn1Day.Size = new Size(46, 33);
            btn1Day.TabIndex = 5;
            btn1Day.Text = "1j";
            btn1Day.UseVisualStyleBackColor = true;
            btn1Day.Click += btn1Day_Click;
            // 
            // comboBoxCoins
            // 
            comboBoxCoins.FormattingEnabled = true;
            comboBoxCoins.Location = new Point(540, 80);
            comboBoxCoins.Margin = new Padding(3, 4, 3, 4);
            comboBoxCoins.Name = "comboBoxCoins";
            comboBoxCoins.Size = new Size(333, 28);
            comboBoxCoins.TabIndex = 7;
            comboBoxCoins.SelectedIndexChanged += comboBoxCoins_SelectedIndexChanged;
            // 
            // clbCryptos
            // 
            clbCryptos.BackColor = Color.AliceBlue;
            clbCryptos.CheckOnClick = true;
            clbCryptos.Font = new Font("Roboto", 9F, FontStyle.Regular, GraphicsUnit.Point);
            clbCryptos.ForeColor = SystemColors.WindowText;
            clbCryptos.FormattingEnabled = true;
            clbCryptos.Location = new Point(879, 123);
            clbCryptos.Name = "clbCryptos";
            clbCryptos.Size = new Size(223, 508);
            clbCryptos.Sorted = true;
            clbCryptos.TabIndex = 8;
            clbCryptos.ItemCheck += checkedListBoxCryptos_ItemCheck;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1114, 865);
            Controls.Add(clbCryptos);
            Controls.Add(comboBoxCoins);
            Controls.Add(btn1Day);
            Controls.Add(formsPlot1);
            Controls.Add(btn365Day);
            Controls.Add(btn30Days);
            Controls.Add(btn7Days);
            Controls.Add(btnImportJson);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "PTL_Crypto";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnImportJson;
        private Button btn7Days;
        private Button btn30Days;
        private Button btn365Day;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private Button btn1Day;
        private ComboBox comboBoxCoins;
        public CheckedListBox clbCryptos;
    }
}
