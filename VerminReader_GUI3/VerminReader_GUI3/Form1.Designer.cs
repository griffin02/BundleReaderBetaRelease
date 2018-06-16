namespace VerminReader_GUI3
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vT2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vT1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cancelSearchBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.searchDisplaybox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.searchInputbox = new System.Windows.Forms.TextBox();
            this.searchBundlesBtn = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.hashLookupBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hashCalcBtn = new System.Windows.Forms.Button();
            this.hashOutputbox = new System.Windows.Forms.TextBox();
            this.hashInputbox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modeToolStripMenuItem,
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(866, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsStripMenuItem,
            this.vT2ToolStripMenuItem,
            this.vT1ToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.modeToolStripMenuItem.Text = "Mode";
            // 
            // settingsStripMenuItem
            // 
            this.settingsStripMenuItem.Name = "settingsStripMenuItem";
            this.settingsStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.settingsStripMenuItem.Text = "Configure Settings";
            this.settingsStripMenuItem.Click += new System.EventHandler(this.settingsStripMenuItem_Click);
            // 
            // vT2ToolStripMenuItem
            // 
            this.vT2ToolStripMenuItem.CheckOnClick = true;
            this.vT2ToolStripMenuItem.Name = "vT2ToolStripMenuItem";
            this.vT2ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.vT2ToolStripMenuItem.Text = "VT2";
            this.vT2ToolStripMenuItem.Click += new System.EventHandler(this.vT2ToolStripMenuItem_Click);
            // 
            // vT1ToolStripMenuItem
            // 
            this.vT1ToolStripMenuItem.CheckOnClick = true;
            this.vT1ToolStripMenuItem.Name = "vT1ToolStripMenuItem";
            this.vT1ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.vT1ToolStripMenuItem.Text = "VT1";
            this.vT1ToolStripMenuItem.Click += new System.EventHandler(this.vT1ToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.fileToolStripMenuItem.Text = "Bundle";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(12, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(586, 405);
            this.treeView1.TabIndex = 1;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(678, 435);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Credits: IamLupo, Aussiemon, Griffin02";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.textBox1.Location = new System.Drawing.Point(56, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 379);
            this.textBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(604, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(250, 415);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(242, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cancelSearchBtn);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.searchDisplaybox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.searchInputbox);
            this.tabPage2.Controls.Add(this.searchBundlesBtn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(242, 389);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Search";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cancelSearchBtn
            // 
            this.cancelSearchBtn.Location = new System.Drawing.Point(116, 47);
            this.cancelSearchBtn.Name = "cancelSearchBtn";
            this.cancelSearchBtn.Size = new System.Drawing.Size(89, 23);
            this.cancelSearchBtn.TabIndex = 5;
            this.cancelSearchBtn.Text = "Cancel Search";
            this.cancelSearchBtn.UseVisualStyleBackColor = true;
            this.cancelSearchBtn.Click += new System.EventHandler(this.cancelSearchBtn_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 369);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Search";
            // 
            // searchDisplaybox
            // 
            this.searchDisplaybox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchDisplaybox.FormattingEnabled = true;
            this.searchDisplaybox.Location = new System.Drawing.Point(10, 76);
            this.searchDisplaybox.Name = "searchDisplaybox";
            this.searchDisplaybox.Size = new System.Drawing.Size(226, 290);
            this.searchDisplaybox.TabIndex = 3;
            this.searchDisplaybox.DoubleClick += new System.EventHandler(this.searchDisplaybox_DoubleClick);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Search For FileType";
            // 
            // searchInputbox
            // 
            this.searchInputbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchInputbox.Location = new System.Drawing.Point(6, 21);
            this.searchInputbox.Name = "searchInputbox";
            this.searchInputbox.Size = new System.Drawing.Size(230, 20);
            this.searchInputbox.TabIndex = 1;
            // 
            // searchBundlesBtn
            // 
            this.searchBundlesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBundlesBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.searchBundlesBtn.Location = new System.Drawing.Point(10, 47);
            this.searchBundlesBtn.Name = "searchBundlesBtn";
            this.searchBundlesBtn.Size = new System.Drawing.Size(100, 23);
            this.searchBundlesBtn.TabIndex = 0;
            this.searchBundlesBtn.Text = "Search Bundles";
            this.searchBundlesBtn.UseVisualStyleBackColor = true;
            this.searchBundlesBtn.Click += new System.EventHandler(this.searchBundlesBtn_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.hashLookupBtn);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.hashCalcBtn);
            this.tabPage3.Controls.Add(this.hashOutputbox);
            this.tabPage3.Controls.Add(this.hashInputbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(242, 389);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "HashCalculator";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // hashLookupBtn
            // 
            this.hashLookupBtn.Location = new System.Drawing.Point(107, 102);
            this.hashLookupBtn.Name = "hashLookupBtn";
            this.hashLookupBtn.Size = new System.Drawing.Size(118, 23);
            this.hashLookupBtn.TabIndex = 5;
            this.hashLookupBtn.Text = "Hash Lookup";
            this.hashLookupBtn.UseVisualStyleBackColor = true;
            this.hashLookupBtn.Click += new System.EventHandler(this.hashLookupBtn_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Input";
            // 
            // hashCalcBtn
            // 
            this.hashCalcBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashCalcBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hashCalcBtn.Location = new System.Drawing.Point(7, 102);
            this.hashCalcBtn.Name = "hashCalcBtn";
            this.hashCalcBtn.Size = new System.Drawing.Size(94, 23);
            this.hashCalcBtn.TabIndex = 2;
            this.hashCalcBtn.Text = "Calculate Hash";
            this.hashCalcBtn.UseVisualStyleBackColor = true;
            this.hashCalcBtn.Click += new System.EventHandler(this.hashCalcBtn_Click);
            // 
            // hashOutputbox
            // 
            this.hashOutputbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashOutputbox.Location = new System.Drawing.Point(3, 62);
            this.hashOutputbox.Multiline = true;
            this.hashOutputbox.Name = "hashOutputbox";
            this.hashOutputbox.ReadOnly = true;
            this.hashOutputbox.Size = new System.Drawing.Size(223, 34);
            this.hashOutputbox.TabIndex = 1;
            // 
            // hashInputbox
            // 
            this.hashInputbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashInputbox.Location = new System.Drawing.Point(3, 20);
            this.hashInputbox.Name = "hashInputbox";
            this.hashInputbox.Size = new System.Drawing.Size(223, 20);
            this.hashInputbox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Vermintide Bundle Reader v0.99 BETA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vT2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vT1ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button searchBundlesBtn;
        private System.Windows.Forms.Button hashCalcBtn;
        private System.Windows.Forms.TextBox hashOutputbox;
        private System.Windows.Forms.TextBox hashInputbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox searchInputbox;
        public System.Windows.Forms.ListBox searchDisplaybox;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cancelSearchBtn;
        private System.Windows.Forms.Button hashLookupBtn;
    }
}

