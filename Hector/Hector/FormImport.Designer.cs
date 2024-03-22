namespace Hector
{
    partial class FormImport
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
            this.BarreDeProgression = new System.Windows.Forms.ProgressBar();
            this.AjoutDonnees = new System.Windows.Forms.Button();
            this.EcraserDonnees = new System.Windows.Forms.Button();
            this.FichierChoisi = new System.Windows.Forms.TextBox();
            this.ChoisirFichier = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BarreDeProgression
            // 
            this.BarreDeProgression.Location = new System.Drawing.Point(113, 84);
            this.BarreDeProgression.Name = "BarreDeProgression";
            this.BarreDeProgression.Size = new System.Drawing.Size(218, 23);
            this.BarreDeProgression.TabIndex = 9;
            // 
            // AjoutDonnees
            // 
            this.AjoutDonnees.Location = new System.Drawing.Point(237, 55);
            this.AjoutDonnees.Name = "AjoutDonnees";
            this.AjoutDonnees.Size = new System.Drawing.Size(158, 23);
            this.AjoutDonnees.TabIndex = 8;
            this.AjoutDonnees.Text = "Ajouter aux données";
            this.AjoutDonnees.UseVisualStyleBackColor = true;
            this.AjoutDonnees.Click += new System.EventHandler(this.AjoutDonnees_Click);
            // 
            // EcraserDonnees
            // 
            this.EcraserDonnees.BackColor = System.Drawing.SystemColors.Control;
            this.EcraserDonnees.Location = new System.Drawing.Point(56, 55);
            this.EcraserDonnees.Name = "EcraserDonnees";
            this.EcraserDonnees.Size = new System.Drawing.Size(158, 23);
            this.EcraserDonnees.TabIndex = 7;
            this.EcraserDonnees.Text = "Ecraser les données";
            this.EcraserDonnees.UseVisualStyleBackColor = false;
            this.EcraserDonnees.Click += new System.EventHandler(this.EcraserDonnees_Click);
            // 
            // FichierChoisi
            // 
            this.FichierChoisi.Location = new System.Drawing.Point(12, 12);
            this.FichierChoisi.Name = "FichierChoisi";
            this.FichierChoisi.ReadOnly = true;
            this.FichierChoisi.Size = new System.Drawing.Size(289, 22);
            this.FichierChoisi.TabIndex = 6;
            // 
            // ChoisirFichier
            // 
            this.ChoisirFichier.Location = new System.Drawing.Point(307, 12);
            this.ChoisirFichier.Name = "ChoisirFichier";
            this.ChoisirFichier.Size = new System.Drawing.Size(122, 23);
            this.ChoisirFichier.TabIndex = 5;
            this.ChoisirFichier.Text = "Choisir le fichier";
            this.ChoisirFichier.UseVisualStyleBackColor = true;
            this.ChoisirFichier.Click += new System.EventHandler(this.ChoisirFichier_Click);
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 121);
            this.Controls.Add(this.BarreDeProgression);
            this.Controls.Add(this.AjoutDonnees);
            this.Controls.Add(this.EcraserDonnees);
            this.Controls.Add(this.FichierChoisi);
            this.Controls.Add(this.ChoisirFichier);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormImport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar BarreDeProgression;
        private System.Windows.Forms.Button AjoutDonnees;
        private System.Windows.Forms.Button EcraserDonnees;
        private System.Windows.Forms.TextBox FichierChoisi;
        private System.Windows.Forms.Button ChoisirFichier;
    }
}