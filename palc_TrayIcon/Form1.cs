using System.Diagnostics;

namespace palc_TrayIcon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = Properties.Settings.Default.MaxTasks.ToString();
            toolStripTextBox1.LostFocus += ToolStripTextBox1_LostFocus;
            toolStripComboBox2.SelectedIndex = Properties.Settings.Default.DeleteOnConvert ? 0 : 1;
            toolStripComboBox2.LostFocus += ToolStripComboBox2_LostFocus;
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            Hide();
        }

        private void ToolStripComboBox2_LostFocus(object? sender, EventArgs e)
        {
            bool previous = Properties.Settings.Default.DeleteOnConvert;

            bool _new = toolStripComboBox2.SelectedIndex == 0 ? true : false;

            if(previous != _new)
            {
                Properties.Settings.Default.DeleteOnConvert = _new;
                Properties.Settings.Default.Save();
            }
        }

        private void ToolStripTextBox1_LostFocus(object? sender, EventArgs e)
        {
            var previous = Properties.Settings.Default.MaxTasks;
            int parsed = 0;

            int.TryParse(toolStripTextBox1.Text, out parsed);
            if(parsed != 0)
            {
                if(previous != parsed)
                {
                    Properties.Settings.Default.MaxTasks = parsed;
                    Properties.Settings.Default.Save();
                }
            } 
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if (File.Exists(Path.Combine(AppContext.BaseDirectory, "palc.exe")))
                    {
                        using (Process process = new Process())
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo(@"cmd.exe", $"/C {Path.Combine(AppContext.BaseDirectory, "palc.exe")} -d {fbd.SelectedPath} -t {Properties.Settings.Default.MaxTasks} {(Properties.Settings.Default.DeleteOnConvert ? "-dl" : "")}");
                            process.StartInfo = startInfo;
                            process.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("couldn't find palc!");
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
