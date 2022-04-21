namespace Chat_Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.usernameField = new System.Windows.Forms.TextBox();
            this.typedText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ipField = new System.Windows.Forms.TextBox();
            this.portField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearChatHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.showPasswordButton = new System.Windows.Forms.Button();
            this.chatHistory = new System.Windows.Forms.RichTextBox();
            this.censorTextButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // usernameField
            // 
            this.usernameField.Location = new System.Drawing.Point(12, 51);
            this.usernameField.Multiline = true;
            this.usernameField.Name = "usernameField";
            this.usernameField.Size = new System.Drawing.Size(100, 20);
            this.usernameField.TabIndex = 0;
            this.usernameField.Text = "Username";
            // 
            // typedText
            // 
            this.typedText.Location = new System.Drawing.Point(12, 326);
            this.typedText.Multiline = true;
            this.typedText.Name = "typedText";
            this.typedText.Size = new System.Drawing.Size(336, 20);
            this.typedText.TabIndex = 5;
            this.typedText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.typedText_KeyPress);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ipField
            // 
            this.ipField.Location = new System.Drawing.Point(118, 51);
            this.ipField.Multiline = true;
            this.ipField.Name = "ipField";
            this.ipField.Size = new System.Drawing.Size(100, 20);
            this.ipField.TabIndex = 1;
            this.ipField.Text = "127.0.0.1";
            // 
            // portField
            // 
            this.portField.Location = new System.Drawing.Point(224, 51);
            this.portField.Multiline = true;
            this.portField.Name = "portField";
            this.portField.Size = new System.Drawing.Size(100, 20);
            this.portField.TabIndex = 2;
            this.portField.Text = "3400";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(118, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(224, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Port";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(330, 51);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(116, 20);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(354, 326);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(64, 20);
            this.sendButton.TabIndex = 6;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(458, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearChatHistoryToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // clearChatHistoryToolStripMenuItem
            // 
            this.clearChatHistoryToolStripMenuItem.Name = "clearChatHistoryToolStripMenuItem";
            this.clearChatHistoryToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.clearChatHistoryToolStripMenuItem.Text = "Clear Chat History";
            this.clearChatHistoryToolStripMenuItem.Click += new System.EventHandler(this.clearChatHistoryToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // passwordField
            // 
            this.passwordField.Location = new System.Drawing.Point(190, 77);
            this.passwordField.Multiline = true;
            this.passwordField.Name = "passwordField";
            this.passwordField.Size = new System.Drawing.Size(228, 20);
            this.passwordField.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label4.Location = new System.Drawing.Point(12, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Password (blank ONLY if none)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showPasswordButton
            // 
            this.showPasswordButton.Location = new System.Drawing.Point(424, 77);
            this.showPasswordButton.Name = "showPasswordButton";
            this.showPasswordButton.Size = new System.Drawing.Size(22, 20);
            this.showPasswordButton.TabIndex = 8;
            this.showPasswordButton.Text = "~";
            this.showPasswordButton.UseVisualStyleBackColor = true;
            this.showPasswordButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.showPasswordButton_MouseDown);
            this.showPasswordButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.showPasswordButton_MouseUp);
            // 
            // chatHistory
            // 
            this.chatHistory.BackColor = System.Drawing.SystemColors.Control;
            this.chatHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chatHistory.Cursor = System.Windows.Forms.Cursors.Default;
            this.chatHistory.Location = new System.Drawing.Point(12, 103);
            this.chatHistory.Name = "chatHistory";
            this.chatHistory.ReadOnly = true;
            this.chatHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.chatHistory.Size = new System.Drawing.Size(434, 217);
            this.chatHistory.TabIndex = 20;
            this.chatHistory.Text = "";
            this.chatHistory.Enter += new System.EventHandler(this.chatHistory_Enter);
            // 
            // censorTextButton
            // 
            this.censorTextButton.Location = new System.Drawing.Point(424, 326);
            this.censorTextButton.Name = "censorTextButton";
            this.censorTextButton.Size = new System.Drawing.Size(22, 20);
            this.censorTextButton.TabIndex = 7;
            this.censorTextButton.Text = "-";
            this.censorTextButton.UseVisualStyleBackColor = true;
            this.censorTextButton.Click += new System.EventHandler(this.censorTextButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 354);
            this.Controls.Add(this.censorTextButton);
            this.Controls.Add(this.chatHistory);
            this.Controls.Add(this.showPasswordButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.passwordField);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.portField);
            this.Controls.Add(this.ipField);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.typedText);
            this.Controls.Add(this.usernameField);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Client";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameField;
        private System.Windows.Forms.TextBox typedText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipField;
        private System.Windows.Forms.TextBox portField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearChatHistoryToolStripMenuItem;
        private System.Windows.Forms.TextBox passwordField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button showPasswordButton;
        private System.Windows.Forms.RichTextBox chatHistory;
        private System.Windows.Forms.Button censorTextButton;
    }
}

