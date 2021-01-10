using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using IWshRuntimeLibrary;
using System.Reflection;

namespace CGPMFinal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // 
            // Required for Windows Form Designer support. 
            //
            InitializeComponent();
            // Initialize the user-defined button, 
            // including defining handler for Click message, 
            // location and size.
            myButtonObject myButton = new myButtonObject();
            EventHandler myHandler = new EventHandler(myButton_Click);
            myButton.Click += myHandler;
            myButton.Location = new System.Drawing.Point(50, 20);
            myButton.Size = new System.Drawing.Size(228,228);
            myButton.BackgroundImage = Image.FromFile(@"C:\Users\vivekpandit\Downloads\download.png");
            this.Controls.Add(myButton);
        }

        public class myButtonObject : UserControl
        {
            // Draw the new button. 
            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics graphics = e.Graphics;
                Pen myPen = new Pen(Color.Black);
                // Draw the button in the form of a circle
                graphics.DrawEllipse(myPen, 12, 12, 200, 200);
                myPen.Dispose();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateShortcut("Speech To Text", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Assembly.GetExecutingAssembly().Location);
        }

        private void myButton_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(@"E:/_Acad_7th_Sem/ME735 Computer Graphics and Product Modeling/CGPMFinal1/CGPMFinal/CGPMFinal/bin/Debug/Instructions.txt");
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.IconLocation = @"C:\Users\vivekpandit\Downloads\myicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 465;
            bool enableSSL = true;

            string emailFrom = "akashkishoreak@gmail.com";
            string password = "kishore1993";
            string emailTo = "kishore19a@gmail.com";
            string subject = "Hello";
            string body = "Hello, I'm just writing this to say Hi!";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
