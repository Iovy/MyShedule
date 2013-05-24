namespace MyShedule
{
    partial class ChooseGroupForm
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
            this.ListGroups = new System.Windows.Forms.CheckedListBox();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ListGroups
            // 
            this.ListGroups.FormattingEnabled = true;
            this.ListGroups.Location = new System.Drawing.Point(3, 26);
            this.ListGroups.Name = "ListGroups";
            this.ListGroups.Size = new System.Drawing.Size(276, 184);
            this.ListGroups.TabIndex = 0;
            // 
            // btnReject
            // 
            this.btnReject.Image = global::MyShedule.Properties.Resources.cancel;
            this.btnReject.Location = new System.Drawing.Point(198, 217);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 33);
            this.btnReject.TabIndex = 9;
            this.btnReject.Text = "Отмена";
            this.btnReject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Image = global::MyShedule.Properties.Resources.accept;
            this.btnAccept.Location = new System.Drawing.Point(110, 217);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(80, 33);
            this.btnAccept.TabIndex = 8;
            this.btnAccept.Text = "Принять";
            this.btnAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Выберете элементы";
            // 
            // ChooseGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.ListGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChooseGroupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор групп";
            this.Load += new System.EventHandler(this.ChooseGroupsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox ListGroups;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnAccept;
      //  private System.Windows.Forms.ComboBox cmbProjection;
        private System.Windows.Forms.Label label2;
    }
}