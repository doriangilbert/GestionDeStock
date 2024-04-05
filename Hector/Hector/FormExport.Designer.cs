namespace Hector
{
    partial class FormExport
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
            this.LabelStatus = new System.Windows.Forms.Label();
            this.BarreDeProgression = new System.Windows.Forms.ProgressBar();
            this.ExportationDonnees = new System.Windows.Forms.Button();
            this.FichierChoisi = new System.Windows.Forms.TextBox();
            this.ChoisirFichier = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.Location = new System.Drawing.Point(6, 59);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(99, 13);
            this.LabelStatus.TabIndex = 16;
            this.LabelStatus.Text = "Aucun fichier choisi";
            // 
            // BarreDeProgression
            // 
            this.BarreDeProgression.Location = new System.Drawing.Point(9, 74);
            this.BarreDeProgression.Margin = new System.Windows.Forms.Padding(2);
            this.BarreDeProgression.Name = "BarreDeProgression";
            this.BarreDeProgression.Size = new System.Drawing.Size(164, 19);
            this.BarreDeProgression.TabIndex = 15;
            // 
            // ExportationDonnees
            // 
            this.ExportationDonnees.Location = new System.Drawing.Point(204, 74);
            this.ExportationDonnees.Margin = new System.Windows.Forms.Padding(2);
            this.ExportationDonnees.Name = "ExportationDonnees";
            this.ExportationDonnees.Size = new System.Drawing.Size(118, 19);
            this.ExportationDonnees.TabIndex = 14;
            this.ExportationDonnees.Text = "Exporter les données";
            this.ExportationDonnees.UseVisualStyleBackColor = true;
            this.ExportationDonnees.Click += new System.EventHandler(this.ExportationDonnees_Click);
            // 
            // FichierChoisi
            // 
            this.FichierChoisi.Location = new System.Drawing.Point(9, 10);
            this.FichierChoisi.Margin = new System.Windows.Forms.Padding(2);
            this.FichierChoisi.Name = "FichierChoisi";
            this.FichierChoisi.ReadOnly = true;
            this.FichierChoisi.Size = new System.Drawing.Size(218, 20);
            this.FichierChoisi.TabIndex = 12;
            // 
            // ChoisirFichier
            // 
            this.ChoisirFichier.Location = new System.Drawing.Point(230, 10);
            this.ChoisirFichier.Margin = new System.Windows.Forms.Padding(2);
            this.ChoisirFichier.Name = "ChoisirFichier";
            this.ChoisirFichier.Size = new System.Drawing.Size(92, 19);
            this.ChoisirFichier.TabIndex = 11;
            this.ChoisirFichier.Text = "Choisir le fichier";
            this.ChoisirFichier.UseVisualStyleBackColor = true;
            this.ChoisirFichier.Click += new System.EventHandler(this.ChoisirFichier_Click);
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 104);
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.BarreDeProgression);
            this.Controls.Add(this.ExportationDonnees);
            this.Controls.Add(this.FichierChoisi);
            this.Controls.Add(this.ChoisirFichier);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormExport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelStatus;
        private System.Windows.Forms.ProgressBar BarreDeProgression;
        private System.Windows.Forms.Button ExportationDonnees;
        private System.Windows.Forms.TextBox FichierChoisi;
        private System.Windows.Forms.Button ChoisirFichier;
    }
}