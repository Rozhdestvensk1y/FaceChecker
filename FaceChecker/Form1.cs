using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Structure;

using DirectShowLib;

namespace FaceChecker
{
    public partial class Form1 : Form
    {
        private DsDevice[] webCams = null;
        private int selectedCamId = 0;
        public Capture capture = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void StreamVideo_Click(object sender, EventArgs e)
        {
            groupBox1.Text = "Камера";
            StreamVideo();
        }

        private void StreamVideo()
        {
            try
            {
                if (webCams.Length == 0 || Cameras.SelectedItem == null)
                    throw new Exception("Нет камеры!");
                else if (capture != null)
                    capture.Start();
                else
                {
                    capture = new Capture(selectedCamId);
                    capture.ImageGrabbed += Capture_ImageGrabbed;
                    capture.Start();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void StopVideoCapture()
        {
            try
            {
                if (capture != null)
                {
                    capture.Pause();
                    capture.Dispose();
                    capture = null;

                    Camera.Image.Dispose();
                    //pictureBox1.Image = null;

                    selectedCamId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat mat = new Mat();
                capture.Retrieve(mat);
                Camera.Image = mat.ToImage<Bgr, byte>().Flip(Emgu.CV.CvEnum.FlipType.Horizontal).ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MakeFoto_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = new Bitmap(Camera.Image);
            bmp1.Save("123.jpeg");
            StopVideoCapture();
            groupBox1.Text = "Фото с камеры";
            Camera.Image = bmp1;
        }

        private void Cameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCamId = Cameras.SelectedIndex;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webCams = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            foreach (var cam in webCams)
                Cameras.Items.Add(cam.Name);
        }

    }
}
