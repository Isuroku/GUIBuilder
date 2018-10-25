namespace GUIBuilderForm
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNewFile = new System.Windows.Forms.Button();
            this.tbNewFileName = new System.Windows.Forms.TextBox();
            this.tvTree = new System.Windows.Forms.TreeView();
            this.rtLog = new System.Windows.Forms.RichTextBox();
            this.lbSourceFiles = new System.Windows.Forms.ListBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.tbSourceText = new System.Windows.Forms.TextBox();
            this.pnlWindow = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnNewFile
            // 
            this.btnNewFile.Location = new System.Drawing.Point(1176, 194);
            this.btnNewFile.Name = "btnNewFile";
            this.btnNewFile.Size = new System.Drawing.Size(106, 23);
            this.btnNewFile.TabIndex = 40;
            this.btnNewFile.Text = "CreateNewFile";
            this.btnNewFile.UseVisualStyleBackColor = true;
            this.btnNewFile.Click += new System.EventHandler(this.btnNewFile_Click);
            // 
            // tbNewFileName
            // 
            this.tbNewFileName.Location = new System.Drawing.Point(867, 194);
            this.tbNewFileName.Name = "tbNewFileName";
            this.tbNewFileName.Size = new System.Drawing.Size(303, 20);
            this.tbNewFileName.TabIndex = 39;
            // 
            // tvTree
            // 
            this.tvTree.Location = new System.Drawing.Point(446, 2);
            this.tvTree.Name = "tvTree";
            this.tvTree.Size = new System.Drawing.Size(415, 614);
            this.tvTree.TabIndex = 38;
            // 
            // rtLog
            // 
            this.rtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtLog.HideSelection = false;
            this.rtLog.Location = new System.Drawing.Point(2, 622);
            this.rtLog.Name = "rtLog";
            this.rtLog.Size = new System.Drawing.Size(859, 187);
            this.rtLog.TabIndex = 37;
            this.rtLog.Text = "";
            // 
            // lbSourceFiles
            // 
            this.lbSourceFiles.FormattingEnabled = true;
            this.lbSourceFiles.Location = new System.Drawing.Point(867, 2);
            this.lbSourceFiles.Name = "lbSourceFiles";
            this.lbSourceFiles.Size = new System.Drawing.Size(415, 186);
            this.lbSourceFiles.TabIndex = 36;
            this.lbSourceFiles.SelectedIndexChanged += new System.EventHandler(this.lbSourceFiles_SelectedIndexChanged);
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(867, 622);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResult.Size = new System.Drawing.Size(415, 187);
            this.tbResult.TabIndex = 34;
            // 
            // tbSourceText
            // 
            this.tbSourceText.AcceptsReturn = true;
            this.tbSourceText.AcceptsTab = true;
            this.tbSourceText.Location = new System.Drawing.Point(2, 2);
            this.tbSourceText.Multiline = true;
            this.tbSourceText.Name = "tbSourceText";
            this.tbSourceText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSourceText.Size = new System.Drawing.Size(438, 614);
            this.tbSourceText.TabIndex = 35;
            this.tbSourceText.Leave += new System.EventHandler(this.tbSourceText_Leave);
            // 
            // pnlWindow
            // 
            this.pnlWindow.Location = new System.Drawing.Point(867, 220);
            this.pnlWindow.Name = "pnlWindow";
            this.pnlWindow.Size = new System.Drawing.Size(415, 396);
            this.pnlWindow.TabIndex = 41;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 817);
            this.Controls.Add(this.pnlWindow);
            this.Controls.Add(this.btnNewFile);
            this.Controls.Add(this.tbNewFileName);
            this.Controls.Add(this.tvTree);
            this.Controls.Add(this.rtLog);
            this.Controls.Add(this.lbSourceFiles);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.tbSourceText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNewFile;
        private System.Windows.Forms.TextBox tbNewFileName;
        private System.Windows.Forms.TreeView tvTree;
        private System.Windows.Forms.RichTextBox rtLog;
        private System.Windows.Forms.ListBox lbSourceFiles;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.TextBox tbSourceText;
        private System.Windows.Forms.Panel pnlWindow;
    }
}

