namespace PSOBBTools
{
    partial class FormChatLog
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChatLog));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyGuildCardId = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyWord = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorEdit1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemEditClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorEdit2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInvertSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewTeamChat = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.listViewChatLog = new System.Windows.Forms.ListView();
            this.columnHeaderTime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderID = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderWord = new System.Windows.Forms.ColumnHeader();
            this.listViewTeamChatLog = new System.Windows.Forms.ListView();
            this.columnHeaderTeamTime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTeamName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderTeamWord = new System.Windows.Forms.ColumnHeader();
            this.timerUpdateListView = new System.Windows.Forms.Timer(this.components);
            this.toolStripSeparatorView1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEdit,
            this.menuItemView});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCopyAll,
            this.menuItemCopyGuildCardId,
            this.menuItemCopyName,
            this.menuItemCopyWord,
            this.toolStripSeparatorEdit1,
            this.MenuItemEditClear,
            this.toolStripSeparatorEdit2,
            this.menuItemSelectAll,
            this.menuItemInvertSelection});
            this.menuItemEdit.Name = "menuItemEdit";
            this.menuItemEdit.Size = new System.Drawing.Size(57, 20);
            this.menuItemEdit.Text = "編集(&E)";
            this.menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
            // 
            // menuItemCopyAll
            // 
            this.menuItemCopyAll.Name = "menuItemCopyAll";
            this.menuItemCopyAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuItemCopyAll.Size = new System.Drawing.Size(229, 22);
            this.menuItemCopyAll.Text = "すべての項目をコピー(&C)";
            this.menuItemCopyAll.Click += new System.EventHandler(this.menuItemCopyAll_Click);
            // 
            // menuItemCopyGuildCardId
            // 
            this.menuItemCopyGuildCardId.Name = "menuItemCopyGuildCardId";
            this.menuItemCopyGuildCardId.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.menuItemCopyGuildCardId.Size = new System.Drawing.Size(229, 22);
            this.menuItemCopyGuildCardId.Text = "ギルドカードIDをコピー(&G)";
            this.menuItemCopyGuildCardId.Click += new System.EventHandler(this.menuItemCopyGuildCardId_Click);
            // 
            // menuItemCopyName
            // 
            this.menuItemCopyName.Name = "menuItemCopyName";
            this.menuItemCopyName.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuItemCopyName.Size = new System.Drawing.Size(229, 22);
            this.menuItemCopyName.Text = "名前をコピー(&N)";
            this.menuItemCopyName.Click += new System.EventHandler(this.menuItemCopyName_Click);
            // 
            // menuItemCopyWord
            // 
            this.menuItemCopyWord.Name = "menuItemCopyWord";
            this.menuItemCopyWord.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.menuItemCopyWord.Size = new System.Drawing.Size(229, 22);
            this.menuItemCopyWord.Text = "発言をコピー(&W)";
            this.menuItemCopyWord.Click += new System.EventHandler(this.menuItemCopyWord_Click);
            // 
            // toolStripSeparatorEdit1
            // 
            this.toolStripSeparatorEdit1.Name = "toolStripSeparatorEdit1";
            this.toolStripSeparatorEdit1.Size = new System.Drawing.Size(226, 6);
            // 
            // MenuItemEditClear
            // 
            this.MenuItemEditClear.Name = "MenuItemEditClear";
            this.MenuItemEditClear.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.MenuItemEditClear.Size = new System.Drawing.Size(229, 22);
            this.MenuItemEditClear.Text = "クリア(&L)";
            this.MenuItemEditClear.Click += new System.EventHandler(this.MenuItemEditClear_Click);
            // 
            // toolStripSeparatorEdit2
            // 
            this.toolStripSeparatorEdit2.Name = "toolStripSeparatorEdit2";
            this.toolStripSeparatorEdit2.Size = new System.Drawing.Size(226, 6);
            // 
            // menuItemSelectAll
            // 
            this.menuItemSelectAll.Name = "menuItemSelectAll";
            this.menuItemSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuItemSelectAll.Size = new System.Drawing.Size(229, 22);
            this.menuItemSelectAll.Text = "すべて選択(&A)";
            this.menuItemSelectAll.Click += new System.EventHandler(this.menuItemSelectAll_Click);
            // 
            // menuItemInvertSelection
            // 
            this.menuItemInvertSelection.Name = "menuItemInvertSelection";
            this.menuItemInvertSelection.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.I)));
            this.menuItemInvertSelection.Size = new System.Drawing.Size(229, 22);
            this.menuItemInvertSelection.Text = "選択の切り替え(&I)";
            this.menuItemInvertSelection.Click += new System.EventHandler(this.menuItemInvertSelection_Click);
            // 
            // menuItemView
            // 
            this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewTeamChat,
            this.toolStripSeparatorView1,
            this.menuItemAutoScroll});
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(57, 20);
            this.menuItemView.Text = "表示(&V)";
            this.menuItemView.DropDownOpened += new System.EventHandler(this.menuItemView_DropDownOpened);
            // 
            // menuItemViewTeamChat
            // 
            this.menuItemViewTeamChat.Name = "menuItemViewTeamChat";
            this.menuItemViewTeamChat.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuItemViewTeamChat.Size = new System.Drawing.Size(228, 22);
            this.menuItemViewTeamChat.Text = "チームチャット(&T)";
            this.menuItemViewTeamChat.Click += new System.EventHandler(this.menuItemViewTeamChat_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.listViewChatLog);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listViewTeamChatLog);
            this.splitContainer.Size = new System.Drawing.Size(632, 424);
            this.splitContainer.SplitterDistance = 212;
            this.splitContainer.TabIndex = 2;
            // 
            // listViewChatLog
            // 
            this.listViewChatLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderID,
            this.columnHeaderName,
            this.columnHeaderWord});
            this.listViewChatLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewChatLog.FullRowSelect = true;
            this.listViewChatLog.GridLines = true;
            this.listViewChatLog.Location = new System.Drawing.Point(0, 0);
            this.listViewChatLog.Name = "listViewChatLog";
            this.listViewChatLog.Size = new System.Drawing.Size(632, 212);
            this.listViewChatLog.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewChatLog.TabIndex = 1;
            this.listViewChatLog.UseCompatibleStateImageBehavior = false;
            this.listViewChatLog.View = System.Windows.Forms.View.Details;
            this.listViewChatLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewChatLog_ColumnClick);
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "日時";
            this.columnHeaderTime.Width = 85;
            // 
            // columnHeaderID
            // 
            this.columnHeaderID.Text = "ギルドカードID";
            this.columnHeaderID.Width = 85;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "名前";
            this.columnHeaderName.Width = 85;
            // 
            // columnHeaderWord
            // 
            this.columnHeaderWord.Text = "発言";
            this.columnHeaderWord.Width = 300;
            // 
            // listViewTeamChatLog
            // 
            this.listViewTeamChatLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTeamTime,
            this.columnHeaderTeamName,
            this.columnHeaderTeamWord});
            this.listViewTeamChatLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTeamChatLog.FullRowSelect = true;
            this.listViewTeamChatLog.GridLines = true;
            this.listViewTeamChatLog.Location = new System.Drawing.Point(0, 0);
            this.listViewTeamChatLog.Name = "listViewTeamChatLog";
            this.listViewTeamChatLog.Size = new System.Drawing.Size(632, 208);
            this.listViewTeamChatLog.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewTeamChatLog.TabIndex = 2;
            this.listViewTeamChatLog.UseCompatibleStateImageBehavior = false;
            this.listViewTeamChatLog.View = System.Windows.Forms.View.Details;
            this.listViewTeamChatLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewTeamChatLog_ColumnClick);
            // 
            // columnHeaderTeamTime
            // 
            this.columnHeaderTeamTime.Text = "日時";
            this.columnHeaderTeamTime.Width = 85;
            // 
            // columnHeaderTeamName
            // 
            this.columnHeaderTeamName.Text = "名前";
            this.columnHeaderTeamName.Width = 85;
            // 
            // columnHeaderTeamWord
            // 
            this.columnHeaderTeamWord.Text = "発言";
            this.columnHeaderTeamWord.Width = 400;
            // 
            // timerUpdateListView
            // 
            this.timerUpdateListView.Interval = 500;
            this.timerUpdateListView.Tick += new System.EventHandler(this.timerUpdateListView_Tick);
            // 
            // toolStripSeparatorView1
            // 
            this.toolStripSeparatorView1.Name = "toolStripSeparatorView1";
            this.toolStripSeparatorView1.Size = new System.Drawing.Size(225, 6);
            // 
            // menuItemAutoScroll
            // 
            this.menuItemAutoScroll.Name = "menuItemAutoScroll";
            this.menuItemAutoScroll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.A)));
            this.menuItemAutoScroll.Size = new System.Drawing.Size(228, 22);
            this.menuItemAutoScroll.Text = "自動スクロール(&A)";
            this.menuItemAutoScroll.Click += new System.EventHandler(this.menuItemAutoScroll_Click);
            // 
            // FormChatLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 448);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormChatLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "チャットログ";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyGuildCardId;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyWord;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewTeamChat;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListView listViewChatLog;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderID;
        private System.Windows.Forms.ColumnHeader columnHeaderWord;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ListView listViewTeamChatLog;
        private System.Windows.Forms.ColumnHeader columnHeaderTeamTime;
        private System.Windows.Forms.ColumnHeader columnHeaderTeamWord;
        private System.Windows.Forms.ColumnHeader columnHeaderTeamName;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorEdit1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemInvertSelection;
        private System.Windows.Forms.ToolStripMenuItem MenuItemEditClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorEdit2;
        private System.Windows.Forms.Timer timerUpdateListView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorView1;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoScroll;
    }
}