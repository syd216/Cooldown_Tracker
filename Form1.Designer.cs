namespace Cooldown_Tracker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            label1 = new Label();
            iconTrayCheck = new CheckBox();
            tabControl1 = new TabControl();
            SaveButton = new Button();
            AddCharacterButton = new Button();
            DeleteCharacterButton = new Button();
            AddSkillButton = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(iconTrayCheck);
            panel1.Location = new Point(575, 320);
            panel1.Name = "panel1";
            panel1.Size = new Size(217, 118);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(165, -1);
            label1.Name = "label1";
            label1.Size = new Size(51, 17);
            label1.TabIndex = 1;
            label1.Text = "Options";
            // 
            // iconTrayCheck
            // 
            iconTrayCheck.AutoSize = true;
            iconTrayCheck.Location = new Point(3, 3);
            iconTrayCheck.Name = "iconTrayCheck";
            iconTrayCheck.Size = new Size(139, 19);
            iconTrayCheck.TabIndex = 0;
            iconTrayCheck.Text = "Minimize to Icon Tray";
            iconTrayCheck.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(553, 426);
            tabControl1.TabIndex = 1;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(575, 291);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(217, 23);
            SaveButton.TabIndex = 3;
            SaveButton.Text = "Save All to File";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // AddCharacterButton
            // 
            AddCharacterButton.Location = new Point(575, 44);
            AddCharacterButton.Name = "AddCharacterButton";
            AddCharacterButton.Size = new Size(217, 23);
            AddCharacterButton.TabIndex = 4;
            AddCharacterButton.Text = "Add Character";
            AddCharacterButton.UseVisualStyleBackColor = true;
            AddCharacterButton.Click += AddCharacterButton_Click;
            // 
            // DeleteCharacterButton
            // 
            DeleteCharacterButton.Location = new Point(575, 102);
            DeleteCharacterButton.Name = "DeleteCharacterButton";
            DeleteCharacterButton.Size = new Size(217, 23);
            DeleteCharacterButton.TabIndex = 6;
            DeleteCharacterButton.Text = "Delete This Character";
            DeleteCharacterButton.UseVisualStyleBackColor = true;
            DeleteCharacterButton.Click += DeleteCharacterButton_Click;
            // 
            // AddSkillButton
            // 
            AddSkillButton.Location = new Point(575, 12);
            AddSkillButton.Name = "AddSkillButton";
            AddSkillButton.Size = new Size(217, 23);
            AddSkillButton.TabIndex = 7;
            AddSkillButton.Text = "Add Skill";
            AddSkillButton.UseVisualStyleBackColor = true;
            AddSkillButton.Click += AddSkillButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(806, 450);
            Controls.Add(AddSkillButton);
            Controls.Add(DeleteCharacterButton);
            Controls.Add(AddCharacterButton);
            Controls.Add(SaveButton);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Cooldown Tracker";
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private CheckBox iconTrayCheck;
        private TabControl tabControl1;
        private Button AddButton;
        private Button SaveButton;
        private Button AddCharacterButton;
        private Button DeleteCharacterButton;
        private Button AddSkillButton;
    }
}
