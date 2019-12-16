using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FoxPro_Med
{
    public partial class FormSettings : Form
    {
        public FoxPro_MedClass parent;

        public FormSettings()
        {
            InitializeComponent();
        }

		private void updateCertsUI()
		{
			tbCertForeign.Text = parent.getCertificateForeignName();
			tbUserCert.Text = parent.getUserCertificateName();
		}

		private void buSave_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.urlHttp = tbUrlHttp.Text;
			Properties.Settings.Default.urlHttps = tbUrlHttps.Text;
			Properties.Settings.Default.client_id = tbClientId.Text;
			Properties.Settings.Default.client_secret = tbClientSecret.Text;
			Properties.Settings.Default.logEnabled = cbLogs.Checked;
			Properties.Settings.Default.tempDir = tbTempDir.Text;
			Properties.Settings.Default.logDir = tbLogDir.Text;
			Properties.Settings.Default.Save();
			buSave.FlatStyle = FlatStyle.Standard;
		}

		private void buCertForeign_Click(object sender, EventArgs e)
		{
			parent.selectCertificateForeign();
			updateCertsUI();
		}

		private void buCertOur_Click(object sender, EventArgs e)
		{
			parent.selectUserCertificate();
			updateCertsUI();
		}

		private void buUrlWork_Click(object sender, EventArgs e)
		{
			tbUrlHttp.Text = "http://api.mdlp.crpt.ru";
			tbUrlHttps.Text = "https://api.mdlp.crpt.ru";
		}

		private void buUrlSandbox_Click(object sender, EventArgs e)
		{
			tbUrlHttp.Text = "http://api.sb.mdlp.crpt.ru";
			tbUrlHttps.Text = "https://api.sb.mdlp.crpt.ru";
		}

		private void buUrlTest_Click(object sender, EventArgs e)
		{
			tbUrlHttp.Text = "http://api.stage.mdlp.crpt.ru";
			tbUrlHttps.Text = "https://api.stage.mdlp.crpt.ru";
		}

		void buttonBlink()
		{
			buSave.FlatStyle = FlatStyle.Flat;
		}

		private void tbUrlHttp_TextChanged(object sender, EventArgs e)
		{
			buttonBlink();
		}

		private void tbUrlHttps_TextChanged(object sender, EventArgs e)
		{
			buttonBlink();
		}

		private void tbClientId_TextChanged(object sender, EventArgs e)
		{
			buttonBlink();
		}

		private void tbClientSecret_TextChanged(object sender, EventArgs e)
		{
			buttonBlink();
		}

		private void FormSettings_Shown(object sender, EventArgs e)
		{
			tbUrlHttp.Text = Properties.Settings.Default.urlHttp;
			tbUrlHttps.Text = Properties.Settings.Default.urlHttps;
			tbClientId.Text = Properties.Settings.Default.client_id;
			tbClientSecret.Text = Properties.Settings.Default.client_secret;
			cbLogs.Checked = Properties.Settings.Default.logEnabled;
			tbTempDir.Text = Properties.Settings.Default.tempDir;
			tbLogDir.Text = Properties.Settings.Default.logDir;
			updateCertsUI();
		}

		private void buLogDir_Click(object sender, EventArgs e)
		{
			bool test = false;
			while (!test)
			{
					FolderBrowserDialog fbd = new FolderBrowserDialog();
				if (String.IsNullOrEmpty(Properties.Settings.Default.logDir))
					fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				else
					fbd.SelectedPath = Properties.Settings.Default.logDir;
				fbd.ShowNewFolderButton = true;
				if (fbd.ShowDialog() == DialogResult.OK)
					tbLogDir.Text = fbd.SelectedPath;
				try
				{
					File.AppendAllText(tbTempDir.Text + "\\logFoxProMed.txt", "\r\nTESTED\r\n");
					test = true;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					test = false;
				}
			}

		}

		private void buTempDir_Click(object sender, EventArgs e)
		{
			bool test = false;
			while (!test)
			{
				FolderBrowserDialog fbd = new FolderBrowserDialog();
				if (String.IsNullOrEmpty(Properties.Settings.Default.tempDir))
					fbd.SelectedPath = System.IO.Path.GetTempPath();
				else
					fbd.SelectedPath = Properties.Settings.Default.tempDir;
				fbd.ShowNewFolderButton = true;
				if (fbd.ShowDialog() == DialogResult.OK)
					tbTempDir.Text = fbd.SelectedPath;
				try
				{
					File.WriteAllText(tbTempDir.Text + "\\test.txt", "TESTED");
					test = true;
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
					test = false;
				}
			}
		}

		private void tbLogDir_TextChanged(object sender, EventArgs e)
		{
			buttonBlink();
		}

		private void tbTempDir_TextChanged(object sender, EventArgs e)
		{
			buttonBlink();
		}
	}
}
