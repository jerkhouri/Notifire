namespace WindowsFormsApp1
{
    partial class Accueil
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Accueil));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.immoToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.zendeskToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.rentrerCesHeuresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incidentsToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.infosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer_change_type = new System.Windows.Forms.Timer(this.components);
            this.Ping = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Notifire - AramisAuto";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.immoToolStrip,
            this.zendeskToolStrip,
            this.rentrerCesHeuresToolStripMenuItem,
            this.incidentsToolStrip,
            this.infosToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(217, 114);
            // 
            // immoToolStrip
            // 
            this.immoToolStrip.Image = global::WindowsFormsApp1.Properties.Resources.CasqueChantier;
            this.immoToolStrip.Name = "immoToolStrip";
            this.immoToolStrip.Size = new System.Drawing.Size(216, 22);
            this.immoToolStrip.Text = "Créer une demande Immo";
            this.immoToolStrip.Click += new System.EventHandler(this.immoToolStrip_Click);
            // 
            // zendeskToolStrip
            // 
            this.zendeskToolStrip.Image = global::WindowsFormsApp1.Properties.Resources.zendesk;
            this.zendeskToolStrip.Name = "zendeskToolStrip";
            this.zendeskToolStrip.Size = new System.Drawing.Size(216, 22);
            this.zendeskToolStrip.Text = "Contactez l\'équipe support";
            this.zendeskToolStrip.Click += new System.EventHandler(this.ZendeskToolStrip_Click);
            // 
            // rentrerCesHeuresToolStripMenuItem
            // 
            this.rentrerCesHeuresToolStripMenuItem.Image = global::WindowsFormsApp1.Properties.Resources.redmine;
            this.rentrerCesHeuresToolStripMenuItem.Name = "rentrerCesHeuresToolStripMenuItem";
            this.rentrerCesHeuresToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.rentrerCesHeuresToolStripMenuItem.Text = "Rentrer ces heures";
            this.rentrerCesHeuresToolStripMenuItem.Visible = false;
            this.rentrerCesHeuresToolStripMenuItem.Click += new System.EventHandler(this.rentrerCesHeuresToolStripMenuItem_Click);
            // 
            // incidentsToolStrip
            // 
            this.incidentsToolStrip.Image = global::WindowsFormsApp1.Properties.Resources.CachetHQ;
            this.incidentsToolStrip.Name = "incidentsToolStrip";
            this.incidentsToolStrip.Size = new System.Drawing.Size(216, 22);
            this.incidentsToolStrip.Text = "Ouvrir la page d\'incident";
            this.incidentsToolStrip.Click += new System.EventHandler(this.incidentsToolStrip_Click);
            // 
            // infosToolStripMenuItem
            // 
            this.infosToolStripMenuItem.Image = global::WindowsFormsApp1.Properties.Resources.logo_aa;
            this.infosToolStripMenuItem.Name = "infosToolStripMenuItem";
            this.infosToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.infosToolStripMenuItem.Text = "A propos de Notifire";
            this.infosToolStripMenuItem.Click += new System.EventHandler(this.infosToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // timer_change_type
            // 
            this.timer_change_type.Interval = 1000;
            this.timer_change_type.Tick += new System.EventHandler(this.timer_change_type_Tick);
            // 
            // Ping
            // 
            this.Ping.Interval = 6000;
            this.Ping.Tick += new System.EventHandler(this.Ping_Tick);
            // 
            // Accueil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Accueil";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem immoToolStrip;
        private System.Windows.Forms.ToolStripMenuItem zendeskToolStrip;
        private System.Windows.Forms.ToolStripMenuItem incidentsToolStrip;
        private System.Windows.Forms.ToolStripMenuItem infosToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.ToolStripMenuItem rentrerCesHeuresToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Timer timer_change_type;
        private System.Windows.Forms.Timer Ping;
    }
}

