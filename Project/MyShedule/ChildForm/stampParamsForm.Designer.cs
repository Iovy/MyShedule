namespace MyShedule
{
    partial class stampParamsForm
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
            this.prorectName = new System.Windows.Forms.TextBox();
            this.courseNum = new System.Windows.Forms.ComboBox();
            this.facultyName = new System.Windows.Forms.TextBox();
            this.semNum = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // prorectName
            // 
            this.prorectName.Location = new System.Drawing.Point(135, 12);
            this.prorectName.Name = "prorectName";
            this.prorectName.Size = new System.Drawing.Size(137, 20);
            this.prorectName.TabIndex = 0;
            // 
            // courseNum
            // 
            this.courseNum.FormattingEnabled = true;
            this.courseNum.Location = new System.Drawing.Point(218, 64);
            this.courseNum.Name = "courseNum";
            this.courseNum.Size = new System.Drawing.Size(54, 21);
            this.courseNum.TabIndex = 1;
            // 
            // facultyName
            // 
            this.facultyName.Location = new System.Drawing.Point(135, 38);
            this.facultyName.Name = "facultyName";
            this.facultyName.Size = new System.Drawing.Size(137, 20);
            this.facultyName.TabIndex = 2;
            // 
            // semNum
            // 
            this.semNum.FormattingEnabled = true;
            this.semNum.Location = new System.Drawing.Point(69, 64);
            this.semNum.Name = "semNum";
            this.semNum.Size = new System.Drawing.Size(54, 21);
            this.semNum.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Проректор по уч. раб.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Факультет";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Семестр";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(181, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Курс";
            // 
            // btnReject
            // 
            this.btnReject.Image = global::MyShedule.Properties.Resources.cancel;
            this.btnReject.Location = new System.Drawing.Point(197, 101);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 33);
            this.btnReject.TabIndex = 11;
            this.btnReject.Text = "Отмена";
            this.btnReject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Image = global::MyShedule.Properties.Resources.accept;
            this.btnAccept.Location = new System.Drawing.Point(109, 101);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(80, 33);
            this.btnAccept.TabIndex = 10;
            this.btnAccept.Text = "Принять";
            this.btnAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // stampParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 145);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.semNum);
            this.Controls.Add(this.facultyName);
            this.Controls.Add(this.courseNum);
            this.Controls.Add(this.prorectName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "stampParamsForm";
            this.Text = "Параметры штампа";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox prorectName;
        private System.Windows.Forms.ComboBox courseNum;
        private System.Windows.Forms.TextBox facultyName;
        private System.Windows.Forms.ComboBox semNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnAccept;
    }
}