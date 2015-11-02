namespace SpeechUnderstanding
{
	partial class FrmGUI
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.cmbIODrive = new System.Windows.Forms.ComboBox();
			this.btnPhase1 = new System.Windows.Forms.Button();
			this.btnPhase2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "I/O Drive:";
			// 
			// cmbIODrive
			// 
			this.cmbIODrive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbIODrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbIODrive.FormattingEnabled = true;
			this.cmbIODrive.Location = new System.Drawing.Point(72, 12);
			this.cmbIODrive.Name = "cmbIODrive";
			this.cmbIODrive.Size = new System.Drawing.Size(200, 21);
			this.cmbIODrive.TabIndex = 1;
			this.cmbIODrive.Click += new System.EventHandler(this.cmbIODrive_Click);
			// 
			// btnPhase1
			// 
			this.btnPhase1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnPhase1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPhase1.Location = new System.Drawing.Point(12, 39);
			this.btnPhase1.Name = "btnPhase1";
			this.btnPhase1.Size = new System.Drawing.Size(260, 75);
			this.btnPhase1.TabIndex = 2;
			this.btnPhase1.Text = "Phase 1: Wav files";
			this.btnPhase1.UseVisualStyleBackColor = true;
			this.btnPhase1.Click += new System.EventHandler(this.btnPhase1_Click);
			// 
			// btnPhase2
			// 
			this.btnPhase2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnPhase2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPhase2.Location = new System.Drawing.Point(12, 120);
			this.btnPhase2.Name = "btnPhase2";
			this.btnPhase2.Size = new System.Drawing.Size(260, 75);
			this.btnPhase2.TabIndex = 3;
			this.btnPhase2.Text = "Pase 2: Mic input";
			this.btnPhase2.UseVisualStyleBackColor = true;
			// 
			// FrmGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 204);
			this.Controls.Add(this.btnPhase2);
			this.Controls.Add(this.btnPhase1);
			this.Controls.Add(this.cmbIODrive);
			this.Controls.Add(this.label1);
			this.Name = "FrmGUI";
			this.Text = "Speech Understanding Functionality";
			this.Load += new System.EventHandler(this.FrmGUI_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbIODrive;
		private System.Windows.Forms.Button btnPhase1;
		private System.Windows.Forms.Button btnPhase2;
	}
}

