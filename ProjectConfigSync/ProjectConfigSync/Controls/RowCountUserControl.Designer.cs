namespace ProjectConfigSync.Controls
{
    partial class RowCountUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblVisibleCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlVisibleContainer = new System.Windows.Forms.Panel();
            this.pnlHiddenContainer = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTotalContainer = new System.Windows.Forms.Panel();
            this.pnlVisibleContainer.SuspendLayout();
            this.pnlHiddenContainer.SuspendLayout();
            this.pnlTotalContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Visible:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Total rows:";
            // 
            // lblVisibleCount
            // 
            this.lblVisibleCount.BackColor = System.Drawing.Color.White;
            this.lblVisibleCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVisibleCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisibleCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblVisibleCount.Location = new System.Drawing.Point(133, 5);
            this.lblVisibleCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblVisibleCount.Name = "lblVisibleCount";
            this.lblVisibleCount.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblVisibleCount.Size = new System.Drawing.Size(43, 24);
            this.lblVisibleCount.TabIndex = 6;
            this.lblVisibleCount.Text = "0";
            this.lblVisibleCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(133, 5);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label11.Size = new System.Drawing.Size(43, 24);
            this.label11.TabIndex = 11;
            this.label11.Text = "0";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlVisibleContainer
            // 
            this.pnlVisibleContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlVisibleContainer.BackColor = System.Drawing.Color.White;
            this.pnlVisibleContainer.Controls.Add(this.label4);
            this.pnlVisibleContainer.Controls.Add(this.lblVisibleCount);
            this.pnlVisibleContainer.Location = new System.Drawing.Point(8, 9);
            this.pnlVisibleContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlVisibleContainer.Name = "pnlVisibleContainer";
            this.pnlVisibleContainer.Size = new System.Drawing.Size(182, 34);
            this.pnlVisibleContainer.TabIndex = 12;
            // 
            // pnlHiddenContainer
            // 
            this.pnlHiddenContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHiddenContainer.BackColor = System.Drawing.Color.White;
            this.pnlHiddenContainer.Controls.Add(this.label7);
            this.pnlHiddenContainer.Controls.Add(this.label10);
            this.pnlHiddenContainer.Controls.Add(this.label9);
            this.pnlHiddenContainer.Controls.Add(this.label8);
            this.pnlHiddenContainer.Controls.Add(this.label6);
            this.pnlHiddenContainer.Controls.Add(this.label3);
            this.pnlHiddenContainer.Controls.Add(this.label2);
            this.pnlHiddenContainer.Controls.Add(this.label1);
            this.pnlHiddenContainer.Location = new System.Drawing.Point(8, 45);
            this.pnlHiddenContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHiddenContainer.Name = "pnlHiddenContainer";
            this.pnlHiddenContainer.Size = new System.Drawing.Size(182, 104);
            this.pnlHiddenContainer.TabIndex = 13;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DimGray;
            this.label10.Location = new System.Drawing.Point(133, 75);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label10.Size = new System.Drawing.Size(43, 19);
            this.label10.TabIndex = 18;
            this.label10.Text = "0";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(133, 53);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label9.Size = new System.Drawing.Size(43, 19);
            this.label9.TabIndex = 17;
            this.label9.Text = "0";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(133, 31);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label8.Size = new System.Drawing.Size(43, 19);
            this.label8.TabIndex = 16;
            this.label8.Text = "0";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(133, 6);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label7.Size = new System.Drawing.Size(43, 24);
            this.label7.TabIndex = 15;
            this.label7.Text = "0";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(20, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Delete Pending:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(20, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Hidden Explicitly:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(20, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Filtered:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Hidden:";
            // 
            // pnlTotalContainer
            // 
            this.pnlTotalContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTotalContainer.BackColor = System.Drawing.Color.White;
            this.pnlTotalContainer.Controls.Add(this.label5);
            this.pnlTotalContainer.Controls.Add(this.label11);
            this.pnlTotalContainer.Location = new System.Drawing.Point(8, 151);
            this.pnlTotalContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTotalContainer.Name = "pnlTotalContainer";
            this.pnlTotalContainer.Size = new System.Drawing.Size(182, 34);
            this.pnlTotalContainer.TabIndex = 14;
            // 
            // RowCountUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(this.pnlHiddenContainer);
            this.Controls.Add(this.pnlVisibleContainer);
            this.Controls.Add(this.pnlTotalContainer);
            this.Name = "RowCountUserControl";
            this.Size = new System.Drawing.Size(198, 193);
            this.pnlVisibleContainer.ResumeLayout(false);
            this.pnlVisibleContainer.PerformLayout();
            this.pnlHiddenContainer.ResumeLayout(false);
            this.pnlHiddenContainer.PerformLayout();
            this.pnlTotalContainer.ResumeLayout(false);
            this.pnlTotalContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblVisibleCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel pnlVisibleContainer;
        private System.Windows.Forms.Panel pnlHiddenContainer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlTotalContainer;
    }
}
