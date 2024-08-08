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

namespace file_pro
{
    

    public partial class Form1 : Form
    {
        private string currentPath;
        
        private Dictionary<string, List<string>> DIC_categorizedFiles;
        public Form1()
        {
            InitializeComponent();
            
            DIC_categorizedFiles = new Dictionary<string, List<string>>();
           
        }


        //Refresh After any event
        
        private void LoadFiles()
        {
            string catogry="";
            if (lstFiles.SelectedIndex!=-1) 
            {
                catogry = lstFiles.SelectedItem.ToString();
            }
           
            lstFiles.Items.Clear();
            DIC_categorizedFiles.Clear();

            
            DirectoryInfo dir = new DirectoryInfo(currentPath);

            foreach (var file in dir.GetFiles())
            {
                string category = GetCategory(file.Name);
                if (!DIC_categorizedFiles.ContainsKey(category))
                {
                    DIC_categorizedFiles[category] = new List<string>();
                }

                DIC_categorizedFiles[category].Add(file.Name);
            }

           var sortedKeys = DIC_categorizedFiles.Keys.ToList();
            sortedKeys.Sort();

            if (sortedKeys.Contains("Other")) 
            {
                sortedKeys.Remove("Other");
                sortedKeys.Add("Other");
            
            }

            foreach (var category in sortedKeys)
            {

               
               
                lstFiles.Items.Add(category);
            }


            if (DIC_categorizedFiles.Keys.Contains(catogry))
            {

                lstFiles.SelectedItem = catogry;
            }
            
        }


      
        //Get Catogry of the Files with Getting Thier Extension
        private string GetCategory(string fileName)
        {
          
            string extension = Path.GetExtension(fileName)?.ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".png":
                case ".gif":
                    return "Images";
                case ".doc":
                case ".docx":
                case ".txt":
                case ".pdf":
                    return "Documents";
                case ".zip":
                case ".rar":
                case ".7z":
                case ".tar":
                case ".tar.gz":
                    return "Archive and Compressed";
                case ".mp3":
                case ".wav":
                case ".flac":
                    return "Auido and Music";
                case ".mp4":
                case ".avi":
                case ".mkv":
                case ".mov":
                    return "video";
                case ".cs":
                case ".py":
                case ".java":
                case ".html":
                case ".css":
                    return "Programming and Script";
                case ".exe":
                    return "APP";
                case ".db":
                case ".csv":
                case ".json":
                case ".xml":
                    return "Database";
                default:
                    return "Other";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
           
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;

                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    currentPath = folderDlg.SelectedPath;
                    LoadFiles();
                    button3.Enabled = true; button4.Enabled = true;
                    button1.Enabled = true;
                    button5.Enabled = true; button6.Enabled = true;
                }
            }
           
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex != -1)
            {
                
                string category = lstFiles.SelectedItem.ToString();
                if (DIC_categorizedFiles.ContainsKey(category))
                {
                    listBoxFiles.Items.Clear();
                    foreach (var file in DIC_categorizedFiles[category])
                    {
                        listBoxFiles.Items.Add(file);
                    }
                }
            }

        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string destPath = Path.Combine(currentPath, Path.GetFileName(fileName));
                    File.Copy(fileName, destPath, true);
                }
                LoadFiles();
                
            }
        }

    
    private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedIndex != -1)
            {
                string fileName = listBoxFiles.SelectedItem.ToString();
                string filePath = Path.Combine(currentPath, fileName);
                File.Delete(filePath);
                LoadFiles();
                
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.ToLower();
            foreach (var category in DIC_categorizedFiles.Keys)
            {
                foreach (var file in DIC_categorizedFiles[category])
                {
                    if (file.ToLower().Contains(searchTerm))
                    {
                        lstFiles.SelectedItem = category;
                        listBoxFiles.SelectedItem = file;
                        textBox1.Clear();
                        textBox1.Refresh();
                        textBox1.Text = null;
                        return;
                    }
                }
            }
            MessageBox.Show("File not found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        //Ba

   private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;

           

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
              

                string newFolderPath = Path.Combine(folderDlg.SelectedPath, "Back-Up"+Path.GetFileName(currentPath));
                Directory.CreateDirectory(newFolderPath);

                string[] files = Directory.GetFiles(currentPath);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destinationPath = Path.Combine(newFolderPath, fileName);
                    File.Copy(file, destinationPath, true);
                }

                MessageBox.Show("Backup successful. Files copied to: " + newFolderPath, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        //Back-UP

     

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.SuppressKeyPress = true;  // Prevents the beep sound when Enter is pressed
            }
        }
    }
}
