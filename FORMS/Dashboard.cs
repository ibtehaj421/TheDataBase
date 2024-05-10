using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainerInterface
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // New Selection
            button1.BackColor = Color.Red;
            pictureBox2.BackColor = Color.Red;

            // Potential Previous Selection
            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;
            
            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;
            
            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screen
            feedbackText.Hide();
            feedbackCol1.Hide();
            feedbackCol2.Hide();
            feedbackCol3.Hide();
            feedbackCol4.Hide();
            feedbackTable.Hide();
            gymRatings.Hide();
            appointmentText.Show();
            appointmentCol1.Show();
            appointmentCol2.Show();
            appointmentCol3.Show();
            appointmentCol4.Show();
            appointmentCol5.Show();
            appointmentTable.Show();
            label2.Text = "2";
            label3.Text = "Abdullah Saqib";
            label4.Text = "Gym 1";
            labelx5.Text = "17/05/24";
            labelx5.Show();
            wcPanel.Hide();
            dcPanel.Hide();
            panelWP.Hide();
            dpPanel.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // New Selection
            button2.BackColor = Color.Red;
            pictureBox3.BackColor = Color.Red;

            // Potential Previous Selection
            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;

            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screens
            appointmentText.Hide();
            appointmentCol1.Hide();
            appointmentCol2.Hide();
            appointmentCol3.Hide();
            appointmentCol4.Hide();
            appointmentCol5.Hide();
            appointmentTable.Hide();
            feedbackText.Hide();
            feedbackCol1.Hide();
            feedbackCol2.Hide();
            feedbackCol3.Hide();
            feedbackCol4.Hide();
            feedbackTable.Hide();
            gymRatings.Hide();
            wcPanel.Show();
            dcPanel.Show();
            panelWP.Hide();
            dpPanel.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // New Selection
            button3.BackColor = Color.Red;
            pictureBox4.BackColor = Color.Red;

            // Potential Previous Selection
            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;

            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screens
            feedbackText.Hide();
            feedbackCol1.Hide();
            feedbackCol2.Hide();
            feedbackCol3.Hide();
            feedbackCol4.Hide();
            feedbackTable.Hide();
            gymRatings.Hide();
            wcPanel.Hide();
            dcPanel.Hide();
            appointmentText.Hide();
            appointmentCol1.Hide();
            appointmentCol2.Hide();
            appointmentCol3.Hide();
            appointmentCol4.Hide();
            appointmentCol5.Hide();
            appointmentTable.Hide();
            panelWP.Show();
            dpPanel.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // New Selection
            button4.BackColor = Color.Red;
            pictureBox5.BackColor = Color.Red;

            // Potential Previous Selection
            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            button5.BackColor = Color.Black;
            pictureBox6.BackColor = Color.Black;

            // Show Screens
            feedbackText.Hide();
            feedbackCol1.Hide();
            feedbackCol2.Hide();
            feedbackCol3.Hide();
            feedbackCol4.Hide();
            feedbackTable.Hide();
            gymRatings.Hide();
            wcPanel.Hide();
            dcPanel.Hide();
            appointmentText.Hide();
            appointmentCol1.Hide();
            appointmentCol2.Hide();
            appointmentCol3.Hide();
            appointmentCol4.Hide();
            appointmentCol5.Hide();
            appointmentTable.Hide();
            panelWP.Hide();
            dpPanel.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // New Selection
            button5.BackColor = Color.Red;
            pictureBox6.BackColor = Color.Red;

            // Potential Previous Selection
            button2.BackColor = Color.Black;
            pictureBox3.BackColor = Color.Black;

            button3.BackColor = Color.Black;
            pictureBox4.BackColor = Color.Black;

            button4.BackColor = Color.Black;
            pictureBox5.BackColor = Color.Black;

            button1.BackColor = Color.Black;
            pictureBox2.BackColor = Color.Black;

            // Show Screen
            feedbackText.Show();
            feedbackCol1.Show();
            feedbackCol2.Show();
            feedbackCol3.Show();
            feedbackCol4.Show();
            feedbackTable.Show();
            gymRatings.Show();
            appointmentText.Hide();
            appointmentCol1.Hide();
            appointmentCol2.Hide();
            appointmentCol3.Hide();
            appointmentCol4.Hide();
            appointmentCol5.Hide();
            appointmentTable.Hide();
            label2.Text = "Abdullah Saqib";
            label3.Text = "5.0";
            label4.Text = "Knowledgable Coach.";
            wcPanel.Hide();
            dcPanel.Hide();
            panelWP.Hide();
            dpPanel.Hide();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void wcTB2_TextChanged(object sender, EventArgs e)
        {
            wcTB2.ForeColor = Color.Red;
            panel9.BackColor = Color.Red;
        }

        private void wcTB3_TextChanged(object sender, EventArgs e)
        {
            wcTB3.ForeColor = Color.Red;
            panel7.BackColor = Color.Red;
        }

        private void wcTB1_TextChanged(object sender, EventArgs e)
        {
            wcTB1.ForeColor = Color.Red;
            panel6.BackColor = Color.Red;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            textBox16.ForeColor = Color.Red;
            panel10.BackColor = Color.Red;
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            textBox15.ForeColor = Color.Red;
            panel8.BackColor = Color.Red;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            textBox14.ForeColor = Color.Red;
            panel5.BackColor = Color.Red;
        }
    }
}
