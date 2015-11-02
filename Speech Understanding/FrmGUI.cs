using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpeechUnderstanding
{
	public partial class FrmGUI : Form
	{
		private string grammarFile;

		public FrmGUI()
		{
			InitializeComponent();
			grammarFile = Path.Combine("Grammars", "grammar.xml");
		}

		private void PopulateRemovableDriveCombo()
		{
			cmbIODrive.Items.Clear();
			cmbIODrive.Items.AddRange(Helper.GetRemvableDriveList().ToArray());

			btnPhase1.Enabled = cmbIODrive.Items.Count > 0;
			btnPhase2.Enabled = cmbIODrive.Items.Count > 0;
			cmbIODrive.SelectedIndex = cmbIODrive.Items.Count > 0 ? 0 : -1;
			Console.WriteLine("Selected I/O Drive: {0}", cmbIODrive.SelectedItem ?? "none");
		}

		private void cmbIODrive_Click(object sender, EventArgs e)
		{
			PopulateRemovableDriveCombo();
		}

		private void FrmGUI_Load(object sender, EventArgs e)
		{
			PopulateRemovableDriveCombo();
		}

		private void btnPhase1_Click(object sender, EventArgs e)
		{
			 Phase1.Run(grammarFile, (string)cmbIODrive.SelectedItem);
		}
	}
}
