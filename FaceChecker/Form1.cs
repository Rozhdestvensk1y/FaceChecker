using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Structure;
using FaceRecognition;
using DirectShowLib;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Net.Mime;

namespace FaceChecker
{
    public partial class Form1 : Form
    {
        public Capture capture = null;
        FaceRec faceRec = new FaceRec();
        public Form1()
        {
            InitializeComponent();
        }

        private void StreamVideo_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Камера";
            faceRec.openCamera(Camera, pictureBox1);
        }
      

        private void MakeFoto_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = new Bitmap(Camera.Image);
            bmp1.Save("123.jpeg");
        }


        private void button2_Click(object sender, EventArgs e)//добавить лицо в распознаватель
        {
            faceRec.Save_IMAGE(textBox1.Text);
            MessageBox.Show("Успешно!");
        }

        private void button1_Click(object sender, EventArgs e)//детект лица
        {
            faceRec.isTrained = true;
            faceRec.getPersonName(label1);
        }

        private void button3_Click(object sender, EventArgs e)//отправить сообщение
        {
            SendMessage(label1.Text);
        }
        public void SendMessage(string mailAddress)
        {
            if (mailAddress != "")
            {
                try
                {
                    MailAddress from = new MailAddress("roketa1337@gmail.com", "Никита");
                    MailAddress to = new MailAddress(mailAddress);
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Фотография";
                    // письмо представляет код html
                    m.IsBodyHtml = true;
                    m.AlternateViews.Add(getEmbeddedImage("123.jpeg"));
                    // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    // логин и пароль
                    smtp.Credentials = new NetworkCredential("roketa1337@gmail.com", "mjavtykzbayomoag");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex);
                }
            }
            else MessageBox.Show("Почта не найдена");
        }

        private AlternateView getEmbeddedImage(String filePath)
        {
                LinkedResource res = new LinkedResource(filePath);
                res.ContentId = Guid.NewGuid().ToString();
                string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                try
                {
                    alternateView.LinkedResources.Add(res);
                return alternateView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex);
                return alternateView;
            }
        }
    }
}
