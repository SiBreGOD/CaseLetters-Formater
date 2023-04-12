using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CaseLetters_Formater
{
    public partial class Form1 : Form
    {
        const int CONST_update_intervall = 1000;
        string text;
        bool is_changed = false;
        private BackgroundWorker bw_manager;
        private readonly List<BackgroundWorker> bw_tasks = new List<BackgroundWorker>();
        internal delegate void thisforms_void();
        internal delegate void thisforms_string(string text);

        public Form1()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.font;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            text = "";
            bw_manager = new BackgroundWorker();
            bw_manager.DoWork += new DoWorkEventHandler(_BW_Manage);
            bw_manager.RunWorkerAsync();
        }

        private void _richtextboxInput_update ()
        {
            if (!is_changed)
                if (text != richTextBoxInput.Text)
                {
                    is_changed = true;
                    text = richTextBoxInput.Text;
                }
        }
        private void _richtextboxOutput_update(string text)
        {
            richTextBoxOutput.Text = text;
        }
        private void _BW_Manage(object sender, DoWorkEventArgs e)
        {
            bw_tasks.Add(new BackgroundWorker());
            bw_tasks[bw_tasks.Count - 1].DoWork += _BW_update_output;
            bw_tasks[bw_tasks.Count - 1].RunWorkerCompleted += _BW_update_output_richtextbox;

            while (true)
            {
                for (int i = 0; i < bw_tasks.Count; i++)
                {
                    if (!bw_tasks[i].IsBusy)
                        bw_tasks[i].RunWorkerAsync();
                }

                Thread.Sleep(CONST_update_intervall);
            }
        }
        private void _BW_update_output(object sender, DoWorkEventArgs e)
        {
            thisforms_void update = new thisforms_void(_richtextboxInput_update);
            this.BeginInvoke(update);
            if (is_changed)
            {
                e.Result = Formater.UpperFirstLetter(text);
            }
        }
        private void _BW_update_output_richtextbox (object sender, RunWorkerCompletedEventArgs e)
        {
            if (is_changed)
            {
                is_changed = false;
                thisforms_string update = new thisforms_string(_richtextboxOutput_update);
                this.BeginInvoke(update, e.Result);
            }
        }
    }
}
