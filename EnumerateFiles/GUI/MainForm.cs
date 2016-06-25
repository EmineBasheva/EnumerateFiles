using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form
    {
        private string directoryPath = "";
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                progressBarEnumeration.Visible = false;
                this.textBoxFilesDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonEnumerate_Click(object sender, EventArgs e)
        {
            if (!ExistsDirectory()) return;
            var files = Directory.GetFiles(directoryPath);
            var numFiles = files.Length;
            SetProgressBar(numFiles);

            for (int i = 1; i <= numFiles; i++)
            {
                var file = files[i - 1];
                Rename(file, numFiles, i);
                progressBarEnumeration.PerformStep();
            }
            MessageBox.Show("Rename successful.");
        }

        private void SetProgressBar(int length)
        {
            progressBarEnumeration.Visible = true;
            progressBarEnumeration.Minimum = 1;
            progressBarEnumeration.Maximum = length;
            progressBarEnumeration.Value = 1;
            progressBarEnumeration.Step = 1;
        }

        private bool ExistsDirectory()
        {
            if (Directory.Exists(this.textBoxFilesDirectory.Text))
            {
                directoryPath = this.textBoxFilesDirectory.Text;
                return true;
            }
            MessageBox.Show("The directory path doesn't exists.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void Rename(string filePath, int numberOfFiles, int index)
        {
            var fileName = Path.GetFileName(filePath);
            var prefix = GetNumber(numberOfFiles, index);
            File.Move(filePath, string.Format("{0}/{1}{2}", directoryPath, prefix, fileName));
        }

        private string GetNumber(int numberOfFiles, int index)
        {
            var result = "";

            if (numberOfFiles < 10)
            {
                result = string.Format("{0}_", index);
            }
            else if (numberOfFiles < 100)
            {
                result = string.Format("{0}{1}_", index / 10, index % 10);
            }
            else if (numberOfFiles < 1000)
            {
                result = string.Format("{0}{1}{2}_", index / 100, (index / 10) % 10, index % 10);
            }
            else if (numberOfFiles < 10000)
            {
                result = string.Format("{0}{1}{2}{3}_",
                    index / 1000, (index / 100) % 10, (index / 10) % 10, index % 10);
            }

            return result;
        }
    }
}
