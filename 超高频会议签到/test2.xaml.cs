using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using NetFrame.Net.TCP.Sock.Asynchronous;
using 超高频会议签到.Business;
using 超高频会议签到.Log;

namespace 超高频会议签到
{
    /// <summary>
    /// test2.xaml 的交互逻辑
    /// </summary>
    public partial class test2 : Window
    {
        public Reader ReaderControllor = new Reader();
        List<AsyncSocketState> clients;
        private string portname = "com2";
        private int baudRate = 115200;
        private int dataBits = 8;
        private Parity parity = Parity.None;
        private StopBits stopbits = StopBits.One;
        private string flag = "0";
        private string COM_DevID = "";
        private static DispatcherTimer readDataTimer = new DispatcherTimer();
        private bool isop = true;
        private int isstate = 0;
        private string read_rfid = "";
        private BackgroundWorker backgroundWorker;
        public test2()
        {
            InitializeComponent();
            ReaderControllor.cmd.MultiEPC_Event += ShowData;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            //可以返回工作进度
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            //允许取消
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string Devname = "";
            while (!backgroundWorker.CancellationPending)
            {
                Thread.Sleep(500);
                clients = ReaderControllor.GetClientInfo();
                if (clients.Count == 1)                          //只有一台连接的时候直接默认选择这一台
                {
                    var  currentclient = clients[0];
                    if (currentclient.types == connect.net)
                    {
                        ReaderControllor.GetMACDev(clients[0]);
                        Devname = "Device:" + PrivateStringFormat.shortTolongNum(currentclient.dev); // + currentclient.dev;
                        backgroundWorker.CancelAsync();
                    }
                    else
                    {
                        // currentDev_l.Text = "Device:" + currentclient.com;
                        Devname = "Device:" + COM_DevID;
                    }

                }
                e.Result = Devname;
            }
        }

        private void ShowData(object sender, Command.ShowEPCEventArgs e)
        {
            string[] result = (e.CommandRespond + "," + flag).Split(',');           //每条命令后面加一个flag  用于循环盘存EPC显示
            byte type = Convert.ToByte(result[1], 16);
            readDataTimer.Tick += new EventHandler(timeCycle);
            readDataTimer.Interval = new TimeSpan(0, 0, 0, 1);
            switch (type)
            {
                case Types.START_MULTI_EPC_RESPOND:
                case Types.START_SINGLE_EPC_RESPOND:

                     string ants="天线:"+result[4].ToString() +"/",
                           srfid="卡号:"+result[5].ToString().Replace("-","")+"/",
                             times="相对时间:"+result[8].ToString()+"/",
                              ips="端口:"+result[10].ToString()+"/",
                             counts="次数:"+result[11].ToString()+"/",
                              stimes="接收时间:"+result[12].ToString();
                    read_rfid = ants + srfid + counts + stimes + "\n";
                    isstate = 2;
                    //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    //    (System.Threading.ThreadStart) delegate()
                    //    {
                    //        listinfo.DataContext = "";
                    //      Thread.Sleep(TimeSpan.FromSeconds(1));
                         
                    //        listinfo.AppendText(read_rfid ); 
                    //    });
                    //readDataTimer.Start();
                    //if (isSourceDataSave)
                    //{
                    //    GetSourceData(result);
                    //    UpdateMultiEPC(result); // 开始保存原始数据
                    //    NNNum = NNNum + 1;     // 记录每秒读标速度
                    //    if (IsTagDoor == true)
                    //    {
                    //        updatedoor();
                    //    }
                    //}
                    //else
                    //{
                    //    UpdateMultiEPC(result);
                    //    NNNum = NNNum + 1;
                    //    if (IsTagDoor == true)
                    //    {
                    //        updatedoor();
                    //    }
                    //}
                   // dev_version = "97";
                    break;
                case Types.GET_MAC_ADD_RESPOND:

                    COM_DevID = PrivateStringFormat.shortTolongNum(result[4]);

                    string bz_str = "";
                    if (COM_DevID.Length < 15)
                    {
                        int bz_len = 15 - COM_DevID.Length;

                        for (int i = 0; i < bz_len; i++)
                        {
                            bz_str += "0";
                        }

                        COM_DevID = COM_DevID.Substring(0, 2) + bz_str + COM_DevID.Substring(2, 11);
                    }
                    isstate = 1;
                   
                    readDataTimer.Start();

                    //  UpdateLBM(result); // 乔佳 2018-7-27 网络参数页面获取长编码
                    //  UpdateMac(result);

                    break;
                default:
                    break;
            }
        }
        private void timeCycle(object sender, EventArgs e)
        {    
            readDataTimer.Stop();
            switch (isstate)
            {
                case 1:
                    listinfo.AppendText(COM_DevID + "\n\r");
                    break;
                case 2:
                   // listinfo.AppendText(read_rfid );
                    break;
                default:
                    break;
            }
           
      
       
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isop==true)
            {
                bool isok = ReaderControllor.ComStart(portname, baudRate, dataBits, parity, stopbits);
                // COM   115200   8  None   One
                clients = ReaderControllor.GetClientInfo();
                var temps = ReaderControllor.GetMACDev(clients[0]);
                bu1.Content = "以打开端口";
                isop = false;
            }
            else
            {
                ReaderControllor.SerialPortClose();
                bu1.Content = "以关闭端口";
                isop = false;
            }
    
        }

        private void bu2_Click(object sender, RoutedEventArgs e)
        {
            clients = ReaderControllor.GetClientInfo();
            var temps = ReaderControllor.StartMultiEPC(clients[0]);
        }

        private void bu3_Click(object sender, RoutedEventArgs e)
        {
            ReaderControllor.StopMultiEPC();
        }

        private void bu1_opsre_Click(object sender, RoutedEventArgs e)
        {
            string dev_ip = txt_sreip.Text.ToString();
            int port = int.Parse(txt_port.Text);
            IPAddress ipaddress = IPAddress.Parse(dev_ip);
            if (ReaderControllor.ServerStart(ipaddress, port))
            {
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                ReaderControllor.ServerClose();    
            }
        }
    }
}
