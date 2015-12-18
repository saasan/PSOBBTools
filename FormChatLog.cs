using System;
using System.IO;
using System.Collections;           // ICollection
using System.Collections.Generic;   // Queue<>
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PSOBBTools
{
    public partial class FormChatLog : Form
    {
        private Settings settings;

        // Tail
        private Tail logTail = new Tail();
        private Tail teamLogTail = new Tail();

        // ���X�g�r���[�ɒǉ�����A�C�e��
        private Queue<ListViewItem> logItems = new Queue<ListViewItem>();
        private Queue<ListViewItem> teamLogItems = new Queue<ListViewItem>();

        public FormChatLog(Settings settings)
        {
            InitializeComponent();

            this.settings = settings;

            // �t�H�[���̏�Ԃ𕜌�
            this.Size = settings.ChatLogSize;
            this.Location = settings.ChatLogLocation;
            splitContainer.SplitterDistance = settings.ChatLogSplitterDistance;
            listViewChatLog.Sorting = settings.ChatLogSortOrder;
            listViewTeamChatLog.Sorting = settings.ChatLogTeamSortOrder;

            // Tail
            logTail.Encoding = Encoding.Unicode;
            logTail.Filter = Settings.chatFilePrefix + "*" + Settings.chatFileExtension;
            logTail.Changed += new TailEventArgsHandler(LogFile_Changed);
            teamLogTail.Encoding = Encoding.Unicode;
            teamLogTail.Filter = Settings.teamChatFilePrefix + "*" + Settings.teamChatFileExtension;
            teamLogTail.Changed += new TailEventArgsHandler(TeamLogFile_Changed);

            // �ݒ�̓K�p
            ApplySettings();

            // �t�@�C����ǂݍ���
            ReadFile();

            // �^�C�}�[���J�n
            timerUpdateListView.Enabled = true;

            // ���X�g�r���[���\�[�g
            listViewChatLog.ListViewItemSorter = new ListViewItemComparer(0, listViewChatLog.Sorting);
            listViewTeamChatLog.ListViewItemSorter = new ListViewItemComparer(0, listViewTeamChatLog.Sorting);

            // �Ď����J�n
            logTail.EnableRaisingEvents = teamLogTail.EnableRaisingEvents = true;

            // �C�x���g�n���h����ǉ�
            settings.Changed += new EventHandler(settings_Changed);
        }

        /// <summary>
        /// �g�p���̃��\�[�X�����ׂăN���[���A�b�v���܂��B
        /// </summary>
        /// <param name="disposing">�}�l�[�W ���\�[�X���j�������ꍇ true�A�j������Ȃ��ꍇ�� false �ł��B</param>
        protected override void Dispose(bool disposing)
        {
            // �C�x���g�n���h�����폜
            settings.Changed -= new EventHandler(settings_Changed);

            // �Ď����~
            logTail.EnableRaisingEvents = teamLogTail.EnableRaisingEvents = false;

            // �t�H�[���̏�Ԃ�ۑ�
            settings.ChatLogLocation = this.Location;
            settings.ChatLogSize = this.Size;
            settings.ChatLogSplitterDistance = splitContainer.SplitterDistance;
            settings.ChatLogSortOrder = listViewChatLog.Sorting;
            settings.ChatLogTeamSortOrder = listViewTeamChatLog.Sorting;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// log�t�H���_���J��
        /// </summary>
        private void menuFileOpenFolderLog_Click(object sender, EventArgs e)
        {
            OpenFolder(settings.PSOBBFolder + '\\' + Settings.logFolder);
        }

        /// <summary>
        /// ����
        /// </summary>
        private void menuFileClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// �ҏW���j���[�̃h���b�v�_�E���C�x���g
        /// </summary>
        private void menuEdit_DropDownOpened(object sender, EventArgs e)
        {
            menuEditCopyGuildCardId.Enabled = splitContainer.ActiveControl.Equals(listViewChatLog);
        }

        /// <summary>
        /// ���ׂĂ̍��ڂ��R�s�[
        /// </summary>
        private void menuEditCopyAll_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                string text = "";

                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    if (i != 0)
                    {
                        text += '\n';
                    }

                    for (int j = 0; j < listView.SelectedItems[i].SubItems.Count; j++)
                    {
                        if (j != 0)
                        {
                            text += '\t';
                        }

                        text += listView.SelectedItems[i].SubItems[j].Text;
                    }
                }

                if (!String.IsNullOrEmpty(text))
                {
                    Clipboard.SetText(text);
                }
            }
        }

        /// <summary>
        /// �M���h�J�[�hID���R�s�[
        /// </summary>
        private void menuEditCopyGuildCardId_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl.Equals(listViewChatLog))
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                CopyColumn(listView, 1);
            }
        }

        /// <summary>
        /// ���O���R�s�[
        /// </summary>
        private void menuEditCopyName_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                int index;

                if (listView.Equals(listViewChatLog))
                {
                    index = 2;
                }
                else
                {
                    index = 1;
                }

                CopyColumn(listView, index);
            }
        }

        /// <summary>
        /// �������R�s�[
        /// </summary>
        private void menuEditCopyWord_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                int index;

                if (listView.Equals(listViewChatLog))
                {
                    index = 3;
                }
                else
                {
                    index = 2;
                }

                CopyColumn(listView, index);
            }
        }

        /// <summary>
        /// �J�������R�s�[
        /// </summary>
        private static void CopyColumn(ListView listView, int index)
        {
            string text = "";

            for (int i = 0; i < listView.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    text += '\n';
                }

                text += listView.SelectedItems[i].SubItems[index].Text;
            }

            if (!String.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
            }
        }

        /// <summary>
        /// �I���̐؂�ւ�
        /// </summary>
        private void menuEditInvertSelection_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                listView.BeginUpdate();

                foreach (ListViewItem listViewItem in listView.Items)
                {
                    listViewItem.Selected = !listViewItem.Selected;
                }

                listView.EndUpdate();
            }
        }

        /// <summary>
        /// ���ׂđI��
        /// </summary>
        private void menuEditSelectAll_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                listView.BeginUpdate();

                foreach (ListViewItem listViewItem in listView.Items)
                {
                    listViewItem.Selected = true;
                }

                listView.EndUpdate();
            }
        }

        /// <summary>
        /// �N���A
        /// </summary>
        private void menuEditClear_Click(object sender, EventArgs e)
        {
            if (splitContainer.ActiveControl is ListView)
            {
                ListView listView = (ListView)splitContainer.ActiveControl;

                listView.BeginUpdate();

                listView.Items.Clear();

                listView.EndUpdate();
            }
        }

        /// <summary>
        /// �\�����j���[�̃h���b�v�_�E���C�x���g
        /// </summary>
        private void menuView_DropDownOpened(object sender, EventArgs e)
        {
            menuViewTeamChat.Checked = settings.ChatLogTeamVisible;
            menuViewAutoScroll.Checked = settings.ChatLogAutoScroll;
        }

        /// <summary>
        /// �`�[���`���b�g��\��
        /// </summary>
        private void menuViewTeamChat_Click(object sender, EventArgs e)
        {
            settings.ChatLogTeamVisible = !settings.ChatLogTeamVisible;
        }

        /// <summary>
        /// �����X�N���[��
        /// </summary>
        private void menuViewAutoScroll_Click(object sender, EventArgs e)
        {
            settings.ChatLogAutoScroll = !settings.ChatLogAutoScroll;
        }

        /// <summary>
        /// �t�@�C����ǂݍ���
        /// </summary>
        private void ReadFile()
        {
            DateTime dt = DateTime.Today;

            // ���t
            string date = dt.Year.ToString("d4") + '/' + dt.Month.ToString("d2") + '/' + dt.Day.ToString("d2");
            // �t�@�C�����̓��t����
            string dateString = dt.Year.ToString("d4") + dt.Month.ToString("d2") + dt.Day.ToString("d2");

            // ���O�t�H���_
            string logFolder = settings.PSOBBFolder + @"\" + Settings.logFolder + @"\";

            // �`���b�g���O
            {
                logTail.FullPath = logFolder + Settings.chatFilePrefix + dateString + Settings.chatFileExtension;

                string content = "";

                try
                {
                    content = logTail.GetDifference();
                }
                catch (Exception) {}

                AddLog(false, date, content);
            }

            // �`�[���`���b�g���O
            {
                teamLogTail.FullPath = logFolder + Settings.teamChatFilePrefix + dateString + Settings.teamChatFileExtension;

                string content = "";

                try
                {
                    content = teamLogTail.GetDifference();
                }
                catch (Exception) {}

                AddLog(true, date, content);
            }
        }
        
        /// <summary>
        /// ���O���L���[�ɒǉ�����
        /// </summary>
        private void AddLog(bool isTeamLog, string date, string content)
        {
            string[] lines = content.Split(new char[] { '\r', '\n' });

            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');

                if (isTeamLog)
                {
                    // �`�[���`���b�g�̃��O

                    if (parts.Length == 3)
                    {
                        parts[0] = date + ' ' + parts[0];

                        lock (((ICollection)teamLogItems).SyncRoot)
                        {
                            teamLogItems.Enqueue(new ListViewItem(parts));
                        }
                    }
                }
                else
                {
                    // �`���b�g�̃��O

                    if (parts.Length == 4)
                    {
                        // 4�Ȃ�M���h�J�[�hID�L��
                        parts[0] = date + ' ' + parts[0];

                        lock (((ICollection)logItems).SyncRoot)
                        {
                            logItems.Enqueue(new ListViewItem(parts));
                        }
                    }
                    else if (parts.Length == 3)
                    {
                        // 3�Ȃ�M���h�J�[�hID����
                        parts[0] = date + ' ' + parts[0];

                        lock (((ICollection)logItems).SyncRoot)
                        {
                            logItems.Enqueue(new ListViewItem(new string[] { parts[0], "", parts[1], parts[2] }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ���X�g�r���[�ɃA�C�e����ǉ�����
        /// </summary>
        private void AddListViewItems(ListView listView, Queue<ListViewItem> items)
        {
            lock (((ICollection)items).SyncRoot)
            {
                if (items.Count == 0)
                {
                    return;
                }
            }

            listView.BeginUpdate();

            lock (((ICollection)items).SyncRoot)
            {
                // �A�C�e����ǉ�
                while (items.Count > 0)
                {
                    ListViewItem item = items.Dequeue();

                    listView.Items.Add(item);
                }
            }

            // �J�����̕�����������
            foreach (ColumnHeader ch in listView.Columns)
            {
                ch.Width = -2;
            }

            // ���O���X�N���[��
            if (settings.ChatLogAutoScroll)
            {
                if (listView.Items.Count > 0)
                {
                    if (listView.Sorting != SortOrder.Descending)
                    {
                        listView.TopItem = listView.Items[listView.Items.Count - 1];
                    }
                    else
                    {
                        listView.TopItem = listView.Items[0];
                    }
                }
            }

            listView.EndUpdate();
        }

        /// <summary>
        /// �`���b�g���O�t�@�C���ύX�C�x���g
        /// </summary>
        private void LogFile_Changed(object sender, TailEventArgs e)
        {
            Regex regex = new Regex(Settings.chatFilePrefix + @"(?<1>\d{4})(?<2>\d{2})(?<3>\d{2})\" + Settings.chatFileExtension);
            Match match = regex.Match(e.Name);
            string date = match.Groups[1].Value + '/' + match.Groups[2].Value + '/' + match.Groups[3].Value;

            this.AddLog(false, date, e.Difference);
        }

        /// <summary>
        /// �`�[���`���b�g���O�t�@�C���ύX�C�x���g
        /// </summary>
        private void TeamLogFile_Changed(object sender, TailEventArgs e)
        {
            Regex regex = new Regex(Settings.teamChatFilePrefix + @"(?<1>\d{4})(?<2>\d{2})(?<3>\d{2})\" + Settings.teamChatFileExtension);
            Match match = regex.Match(e.Name);
            string date = match.Groups[1].Value + '/' + match.Groups[2].Value + '/' + match.Groups[3].Value;

            this.AddLog(true, date, e.Difference);
        }

        /// <summary>
        /// �ݒ肪�ύX���ꂽ�C�x���g
        /// </summary>
        private void settings_Changed(object sender, EventArgs e)
        {
            ApplySettings();
        }

        /// <summary>
        /// �ݒ�̓K�p
        /// </summary>
        private void ApplySettings()
        {
            splitContainer.Panel2Collapsed = !settings.ChatLogTeamVisible;

            // ��U�Ď����~
            bool logTailEnable = logTail.EnableRaisingEvents;
            bool teamLogTailEnable = logTail.EnableRaisingEvents;
            logTail.EnableRaisingEvents = teamLogTail.EnableRaisingEvents = false;

            if (!String.IsNullOrEmpty(settings.PSOBBFolder) && Directory.Exists(settings.PSOBBFolder))
            {
                logTail.Path = teamLogTail.Path = settings.PSOBBFolder + @"\" + Settings.logFolder;

                // �Ď������ɖ߂�
                logTail.EnableRaisingEvents = logTailEnable;
                teamLogTail.EnableRaisingEvents = teamLogTailEnable;
            }
        }

        private void listViewChatLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewChatLog, e.Column);
        }

        private void listViewTeamChatLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortListView(listViewTeamChatLog, e.Column);
        }

        /// <summary>
        /// ���X�g�r���[���\�[�g����
        /// </summary>
        private void SortListView(ListView listView, int column)
        {
            listView.Sorting = (listView.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);

            listView.BeginUpdate();
            listView.ListViewItemSorter = new ListViewItemComparer(column, listView.Sorting);
            listView.EndUpdate();
        }

        /// <summary>
        /// ���X�g�r���[�X�V�p�^�C�}�[�C�x���g
        /// </summary>
        private void timerUpdateListView_Tick(object sender, EventArgs e)
        {
            AddListViewItems(listViewChatLog, logItems);
            AddListViewItems(listViewTeamChatLog, teamLogItems);
        }

        /// <summary>
        /// �t�H���_���J��
        /// </summary>
        private void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/n," + path);
        }
    }
}