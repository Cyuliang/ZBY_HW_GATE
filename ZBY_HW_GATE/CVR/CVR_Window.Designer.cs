namespace ZBY_HW_GATE.CVR
{
    partial class CVR_Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CVR_Window));
            this.buttonReadCard = new System.Windows.Forms.Button();
            this.pictureBoxPhoto = new System.Windows.Forms.PictureBox();
            this.labelValidDate = new System.Windows.Forms.Label();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelIdCardNo = new System.Windows.Forms.Label();
            this.labelBirthday = new System.Windows.Forms.Label();
            this.labelNation = new System.Windows.Forms.Label();
            this.labelSex = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.InitButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonReadCard
            // 
            this.buttonReadCard.Location = new System.Drawing.Point(177, 385);
            this.buttonReadCard.Name = "buttonReadCard";
            this.buttonReadCard.Size = new System.Drawing.Size(75, 23);
            this.buttonReadCard.TabIndex = 26;
            this.buttonReadCard.Text = "Read";
            this.buttonReadCard.UseVisualStyleBackColor = true;
            this.buttonReadCard.Click += new System.EventHandler(this.ButtonReadCard_Click);
            // 
            // pictureBoxPhoto
            // 
            this.pictureBoxPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPhoto.Location = new System.Drawing.Point(470, 15);
            this.pictureBoxPhoto.Name = "pictureBoxPhoto";
            this.pictureBoxPhoto.Size = new System.Drawing.Size(112, 137);
            this.pictureBoxPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPhoto.TabIndex = 23;
            this.pictureBoxPhoto.TabStop = false;
            // 
            // labelValidDate
            // 
            this.labelValidDate.AutoSize = true;
            this.labelValidDate.Location = new System.Drawing.Point(25, 177);
            this.labelValidDate.Name = "labelValidDate";
            this.labelValidDate.Size = new System.Drawing.Size(65, 12);
            this.labelValidDate.TabIndex = 21;
            this.labelValidDate.Text = "有效期限：";
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Location = new System.Drawing.Point(25, 151);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(65, 12);
            this.labelDepartment.TabIndex = 20;
            this.labelDepartment.Text = "签发机关：";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(49, 125);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(41, 12);
            this.labelAddress.TabIndex = 19;
            this.labelAddress.Text = "地址：";
            // 
            // labelIdCardNo
            // 
            this.labelIdCardNo.AutoSize = true;
            this.labelIdCardNo.Location = new System.Drawing.Point(25, 99);
            this.labelIdCardNo.Name = "labelIdCardNo";
            this.labelIdCardNo.Size = new System.Drawing.Size(65, 12);
            this.labelIdCardNo.TabIndex = 18;
            this.labelIdCardNo.Text = "身份证号：";
            // 
            // labelBirthday
            // 
            this.labelBirthday.AutoSize = true;
            this.labelBirthday.Location = new System.Drawing.Point(25, 73);
            this.labelBirthday.Name = "labelBirthday";
            this.labelBirthday.Size = new System.Drawing.Size(65, 12);
            this.labelBirthday.TabIndex = 17;
            this.labelBirthday.Text = "出生日期：";
            // 
            // labelNation
            // 
            this.labelNation.AutoSize = true;
            this.labelNation.Location = new System.Drawing.Point(49, 203);
            this.labelNation.Name = "labelNation";
            this.labelNation.Size = new System.Drawing.Size(41, 12);
            this.labelNation.TabIndex = 16;
            this.labelNation.Text = "民族：";
            // 
            // labelSex
            // 
            this.labelSex.AutoSize = true;
            this.labelSex.Location = new System.Drawing.Point(49, 47);
            this.labelSex.Name = "labelSex";
            this.labelSex.Size = new System.Drawing.Size(41, 12);
            this.labelSex.TabIndex = 15;
            this.labelSex.Text = "性别：";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(49, 21);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 14;
            this.labelName.Text = "姓名：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(96, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(351, 21);
            this.textBox1.TabIndex = 27;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(96, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(351, 21);
            this.textBox2.TabIndex = 28;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(96, 69);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(351, 21);
            this.textBox3.TabIndex = 29;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(96, 96);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(351, 21);
            this.textBox4.TabIndex = 30;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(96, 122);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(351, 21);
            this.textBox5.TabIndex = 31;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(96, 148);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(351, 21);
            this.textBox6.TabIndex = 32;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(96, 174);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(351, 21);
            this.textBox7.TabIndex = 33;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(96, 203);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(351, 21);
            this.textBox8.TabIndex = 34;
            // 
            // InitButton
            // 
            this.InitButton.Location = new System.Drawing.Point(96, 385);
            this.InitButton.Name = "InitButton";
            this.InitButton.Size = new System.Drawing.Size(75, 23);
            this.InitButton.TabIndex = 35;
            this.InitButton.Text = "Init";
            this.InitButton.UseVisualStyleBackColor = true;
            this.InitButton.Click += new System.EventHandler(this.InitButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(259, 385);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 36;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(96, 247);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(351, 112);
            this.LogTextBox.TabIndex = 37;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(340, 389);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 38;
            this.checkBox1.Text = "Auto";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // CVR_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(618, 436);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.InitButton);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonReadCard);
            this.Controls.Add(this.pictureBoxPhoto);
            this.Controls.Add(this.labelValidDate);
            this.Controls.Add(this.labelDepartment);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.labelIdCardNo);
            this.Controls.Add(this.labelBirthday);
            this.Controls.Add(this.labelNation);
            this.Controls.Add(this.labelSex);
            this.Controls.Add(this.labelName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CVR_Window";
            this.Text = "CVR_Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CVR_Window_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonReadCard;
        private System.Windows.Forms.PictureBox pictureBoxPhoto;
        private System.Windows.Forms.Label labelValidDate;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelIdCardNo;
        private System.Windows.Forms.Label labelBirthday;
        private System.Windows.Forms.Label labelNation;
        private System.Windows.Forms.Label labelSex;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button InitButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}