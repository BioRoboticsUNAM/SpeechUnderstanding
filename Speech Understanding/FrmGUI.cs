using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SpeechUnderstanding
{
	public partial class FrmGUI : Form
	{
		private Phase2 phase2;
		private string grammarFile;

		public FrmGUI()
		{
			InitializeComponent();
			grammarFile = Path.Combine("Grammars", "grammar.xml");
			Phase1.Progress+= (value, max) =>{
				this.BeginInvoke(new Action(() => {
					pbPhase1.Maximum = max;
					pbPhase1.Value = value;
				}));
			};
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

		private void AsyncPhase1(object drive)
		{
			Phase1.Run(grammarFile, (string)drive);
			this.Invoke( new Action( () => {
				btnPhase1.Enabled = true;
				btnPhase2.Enabled = true;
				pbPhase1.Visible = false;
			}));
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
			btnPhase1.Enabled = false;
			btnPhase2.Enabled = false;
			Thread worker = new Thread(new ParameterizedThreadStart(AsyncPhase1));
			worker.IsBackground = true;
			pbPhase1.Visible = true;
			pbPhase1.Value = 0;
			worker.Start(cmbIODrive.SelectedItem);
		}

		private void btnPhase2_Click(object sender, EventArgs e)
		{
			if (phase2 == null)
			{
				btnPhase1.Enabled = false;
				phase2 = Phase2.Run(grammarFile, (string)cmbIODrive.SelectedItem);
				btnPhase2.Text = "Stop Pase 2";
			}
			else
			{
				phase2.Stop();
				phase2 = null;
				btnPhase1.Enabled = true;
				btnPhase2.Text = "Pase 2: Mic input";
			}
		}
	}
}
