using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace testOpencv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CookieContainer cookieContainer = new CookieContainer();
            string targetUrl = "http://railway.hinet.net/ImageOut.jsp?pageRandom=0.960527706690981";
            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "GET";
            request.KeepAlive = true;
            request.CookieContainer = new CookieContainer(); //暂存到新实例
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Accept = "image/webp,image/*,*/*;q=0.8";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Timeout = 30000;

            WebResponse response = request.GetResponse();
            cookieContainer = request.CookieContainer; //保存cookies
            Stream stream = response.GetResponseStream();
            Image img = Image.FromStream(stream);
            stream.Close();
            response.Close();
            pictureBox1.Image = img;

            string mynewpath = @"D:\OneDrive - Microsoft\公司\緯創\TEST\pythonTest\pic.png";
            img.Save(mynewpath);

            string[] files = Directory.GetFiles(@"D:\OneDrive - Microsoft\公司\緯創\TEST\pythonTest\tmp", "*.png"); //找出 c:\ *.xml 的檔案.
            foreach (string file in files)
            {
                File.Delete(file);
            }

            //驗證碼破解
            #region 驗證碼破解
            System.Diagnostics.Process ProcessRun = new Process();
            ProcessRun.StartInfo.FileName = @"C:\Python27\python.exe";
            ProcessRun.StartInfo.Arguments
            = @"D:\OneDrive - Microsoft\公司\緯創\TEST\pythonTest\test.py";
            ProcessRun.StartInfo.WorkingDirectory = @"D:\OneDrive - Microsoft\公司\緯創\TEST\pythonTest";
            ProcessRun.StartInfo.UseShellExecute = false;
            ProcessRun.StartInfo.RedirectStandardInput = true;
            ProcessRun.StartInfo.RedirectStandardOutput = true;
            ProcessRun.StartInfo.CreateNoWindow = true;

            ProcessRun.Start();
            StreamReader s = ProcessRun.StandardOutput;
            string output = s.ReadToEnd();
            ProcessRun.WaitForExit();
            testOCR.Text = output;
            ProcessRun.Close();
            #endregion

            string parame = @"http://railway.hinet.net/order_kind1.jsp?person_id=F130572828&from_station=100&to_station=051&getin_date=2017/03/21-09&order_qty_str=1&train_type=*1&getin_start_dtime=12:00&getin_end_dtime=18:00&returnTicket=1&randInput="+ output;
            request = HttpWebRequest.Create(parame) as HttpWebRequest;
            request.Method = "GET";
            request.KeepAlive = true;
            request.CookieContainer = cookieContainer; //暂存到新实例
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Timeout = 30000;

            string result = "";
            // 取得回應資料
            response = request.GetResponse();
            stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream,Encoding.Default);
            result = reader.ReadToEnd();


            //string parame = @"person_id=F130572828&from_station=100&to_station=051&getin_date=2017/03/07-01
            //                    &getin_date2=2017/03/08-02&order_qty_str=4&order_qty_str2=4
            //                    &train_type=*1&train_type2=*1&getin_start_dtime=07:00&getin_start_dtime2=16:00
            //                    &getin_end_dtime=14:00&getin_end_dtime2=20:00";
            //byte[] postData = Encoding.UTF8.GetBytes(parame);

            //HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.Timeout = 30000;
            //request.ContentLength = postData.Length;
            //// 寫入 Post Body Message 資料流
            //using (Stream st = request.GetRequestStream())
            //{
            //    st.Write(postData, 0, postData.Length);
            //}

            //string result = "";
            //// 取得回應資料
            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            //    {
            //        result = sr.ReadToEnd();
            //    }
            //}

            //Response.Write(result);
        }
    }
}
