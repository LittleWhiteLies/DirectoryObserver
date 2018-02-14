using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO
{
    public partial class Form1 : Form
    {
        Dictionary<string, CancellationTokenSource> cancelationTokens = new Dictionary<string, CancellationTokenSource>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancelationToken = new CancellationTokenSource();

            if (cancelationTokens.Where(o => o.Key == "label1").Any())
            {
                cancelationTokens.Remove("label1");
            }

            cancelationTokens.Add("label1", cancelationToken);

            DoWorkAsyncInfiniteLoop(textBox1.Text, "label1", cancelationToken);
        }

        public async Task DoWorkAsyncInfiniteLoop(string directoryPath, string labelName, CancellationTokenSource cancelationToken)
        {
            DateTime timestamp = System.DateTime.Now;

            using (FileSystemWatcher watcher = new FileSystemWatcher(directoryPath, "*"))
            {
                watcher.EnableRaisingEvents = true;
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.Created += delegate (object source, FileSystemEventArgs e)
                {
                    OnChangeCallback(labelName, e.Name, e.ChangeType.ToString());
                };
                watcher.Deleted += delegate (object source, FileSystemEventArgs e)
                {
                    OnChangeCallback(labelName, e.Name, e.ChangeType.ToString());
                };

                watcher.Changed += delegate (object source, FileSystemEventArgs e)
                {
                    OnChangeCallback(labelName, e.Name, e.ChangeType.ToString());
                };
                
                watcher.Renamed += delegate (object source, RenamedEventArgs e)
                {
                    OnRenameCallback(labelName, e.OldName, e.Name, e.ChangeType.ToString());
                };


                while (!cancelationToken.Token.IsCancellationRequested)
                {
                      await Task.Delay(100);
                };
            }
        }
        
        private void OnChangeCallback(string labelName, string fileName, string changeType)
        {
            if (this.Controls[labelName].InvokeRequired)
            {
                this.Controls[labelName].Invoke(new Action(() => this.Controls[labelName].Text = fileName + " " + changeType));
            }
            else
            {
                this.Controls[labelName].Text = fileName + " " + changeType;
            }
        }

        private void OnRenameCallback(string labelName, string oldName, string newName, string changeType)
        {
            if (this.Controls[labelName].InvokeRequired)
            {
                this.Controls[labelName].Invoke(new Action(() => this.Controls[labelName].Text = oldName + " renamed to " + newName + " " + changeType));
            }
            else
            {
                this.Controls[labelName].Text = oldName + " renamed to " + newName + " " + changeType;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancelationToken = new CancellationTokenSource();

            if (cancelationTokens.Where(o => o.Key == "label2").Any())
            {
                cancelationTokens.Remove("label2");
            }

            cancelationTokens.Add("label2", cancelationToken);

            DoWorkAsyncInfiniteLoop(textBox2.Text, "label2", cancelationToken);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cancelationTokens.Where(o => o.Key == "label1").Any())
            {
                CancellationTokenSource cancelationToken = cancelationTokens["label1"];
                cancelationToken.Cancel();

                cancelationTokens.Remove("label1");
                this.label1.Text = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cancelationTokens.Where(o => o.Key == "label2").Any())
            {
                CancellationTokenSource cancelationToken = cancelationTokens["label2"];

                cancelationToken.Cancel();

                cancelationTokens.Remove("label2");

                this.label2.Text = null;
            }
        }
    }
}
