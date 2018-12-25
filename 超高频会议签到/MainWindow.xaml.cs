using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetFrame.Net.TCP.Sock.Asynchronous;
using 超高频会议签到.Business;
using 超高频会议签到.Log;

namespace 超高频会议签到
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 基本设定

        //将控件属性通过控制器Controllor与model绑定起来
        //当model的值改变时，控件对应的属性会发生改变，当控件的值改变时，对应的model属性值发生改变
        //同时出发属性值改变事件  PropertyChanged

        public Reader ReaderControllor = new Reader();
        //    private Clients Clients;
        private const int OPER_OK = 0;          // 表示sr api函数是否成功执行
        private int m_connect_type = 0;
        private const int READ_FLAG = 1;
        private const int WRITE_FLAG = 2;

        //  private volatile List<tag> tags_list = new List<tag>(1000);
        //   private volatile List<tag_mc> md_epc_list = new List<tag_mc>(1000);
        private volatile List<_epc_t> epcs_list = new List<_epc_t>(1000);
        private volatile List<string[]> epcstr_list = new List<string[]>(1000);
        private volatile List<string[]> IDtr_list = new List<string[]>(1000);
        private volatile List<string[]> sourceEpcStrList = new List<string[]>(1000);
        private int tags_count_persecond = 0;
        private volatile List<string[]> checkEpcList = new List<string[]>(1000);

        private volatile List<string[]> lastEpcStrList = new List<string[]>(1000);

        private volatile List<string[]> QJCS = new List<string[]>(1000);

        List<AsyncSocketState> clients;           //客户端信息

        private const int listView_label_Num = 0;
        private const int listView_label_AntID = 1;
        private const int listView_label_EPC = 2;
        private const int listView_label_TID = 3;
        private const int listView_label_PC = 4;
        private const int listView_label_RSSI = 5;
        private const int listView_label_Count = 6;
        private const int listView_label_Last_Time = 7;

        SynchronizationContext m_SyncContext = null;

        public static string WorkState = "正常状态";

        private const int listView_md_State = 3;


        //多设备标签显示项
        private const int listView_md_epc_Num = 0;
        private const int listView_md_epc_AntID = 1;
        private const int listView_md_epc_EPC = 2;
        private const int listView_md_epc_PC = 3;
        private const int listView_md_epc_Rssi = 4;
        private const int listView_md_epc_Count = 5;
        private const int listView_md_epc_DevID = 6;
        private const int listView_md_epc_Last_Time = 7;
        private const int listView_md_epc_Direction = 8;
        private const int listView_md_epc_Temp = 9;
        private const int listView_md_epc_TID = 10;
        // 网络模块显示项
        private const int listView_net_Num = 0;
        private const int listView_net_MAC = 1;
        private const int listView_net_IP = 2;



        //多设备标签显示项
        private const int listView_Num = 0;
        private const int listView_ID = 1;
        private const int listView_dev = 2;
        private const int listView_tem = 3;
        private const int listView_remove = 4;
        private const int listView_RSSI = 5;
        private const int listView_power = 6;
        private const int listView_battery = 7;
        private const int listView_md_id_Count = 8;
        private const int listView_md_id_Last_Time = 9;

        public static string path;
      //  PrivateFontCollection pfc = new PrivateFontCollection();  // LED字体

       // MySorter mySorter = new MySorter();   //ListView排序
        // ListView虚拟模式缓冲 [无源]
        protected List<ListViewItem> ItemsSource
        {
            get;
            private set;
        }
        protected List<ListViewItem> CurrentCacheItemsSource
        {
            get;
            private set;
        }

        // ListView虚拟模式缓冲  [有源]
        protected List<ListViewItem> ItemsSource_ID
        {
            get;
            private set;
        }

        protected List<ListViewItem> CurrentCacheItemsSource_ID
        {
            get;
            private set;
        }

        //设置listview属性和虚拟模式事件   [无源]
    




        //void listView_RetrieveVirtualItem_ID(object sender, RetrieveVirtualItemEventArgs e)
        //{
        //    if (this.CurrentCacheItemsSource_ID == null || this.CurrentCacheItemsSource_ID.Count == 0)
        //    {
        //        return;
        //    }

        //    e.Item = this.CurrentCacheItemsSource_ID[e.ItemIndex];
        //    if (e.ItemIndex == this.CurrentCacheItemsSource_ID.Count)
        //    {
        //        this.CurrentCacheItemsSource_ID = null;
        //    }
        //}



        #endregion

        public MainWindow()
        {
            InitializeComponent();
        //    InintManagement();

             

            string HostName = Dns.GetHostName(); //得到主机名
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    this.com_SerIp.Items.Add(IpEntry.AddressList[i].ToString());
                }
            }
            if (this.com_SerIp.Items.Count == 0)
            {
                this.com_SerIp.Items.Clear();
                this.com_SerIp.Text = "192.168.1.168";
            }
            else
            {
                this.com_SerIp.SelectedIndex = 0;
            }
            string[] names = SerialPort.GetPortNames();
            foreach (string name in names)
            {
                com_ports.Items.Add(name);
                com_ports.SelectedIndex = 0;
            }



            //============================
              ReaderControllor.cmd.MultiEPC_Event += ShowEPC;



        }


        //读取配置信息

        private void InintManagement()
        {
             
            
            
            if (ConfigurationManager.AppSettings["InventoryTime"] != "null")               //获取数据显示时间间隔
            {
               // Text_time_fliecabinet.Text = ConfigurationManager.AppSettings["InventoryTime"];
               // timerTest.Interval = TimerShowID.Interval = show_EPC_t.Interval = int.Parse(Text_time_fliecabinet.Text);
            }
            if (ConfigurationManager.AppSettings["IsRfidDataBaseWork"] != "null")               //数据库是否启用
            {
                string IsDataBaseWork = ConfigurationManager.AppSettings["IsRfidDataBaseWork"];
                if (IsDataBaseWork == "yes")
                {
                    _IsRfidDatabaseWork = true;
                }
                else
                {
                    _IsRfidDatabaseWork = false;
                }
            }
            if (ConfigurationManager.AppSettings["IsAvtiveDataBaseWork"] != "null")               //数据库是否启用
            {
                string IsDataBaseWork = ConfigurationManager.AppSettings["IsAvtiveDataBaseWork"];
                if (IsDataBaseWork == "yes")
                {
                    _IsActiveDatabaseWork = true;
                }
                else
                {
                    _IsActiveDatabaseWork = false;
                }
            }
            if (ConfigurationManager.AppSettings["IsCommandDataBaseWork"] != "null")               //数据库是否启用
            {
                string IsDataBaseWork = ConfigurationManager.AppSettings["IsCommandDataBaseWork"];
                if (IsDataBaseWork == "yes")
                {
                    _IsCommandDatabaseWork = true;
                }
                else
                {
                    _IsCommandDatabaseWork = false;
                }
            }
            datasource = ConfigurationManager.AppSettings["database"];
            if (ConfigurationManager.AppSettings["CommandType"] != "null")               //数据库是否启用
            {
                byte type = byte.Parse(ConfigurationManager.AppSettings["CommandType"]);
                ReaderControllor.SetCommandType(type);
            }




        }

        private string portname = "";
        private int baudRate = 115200;
        private int dataBits = 8;
        private Parity parity = Parity.None;
        private StopBits stopbits = StopBits.One;

        private void bu_opport_Click(object sender, RoutedEventArgs e)
        {
            portname = com_ports.Text;
            string s;        
            try
            {
                ReaderControllor.ComStart(portname, baudRate, dataBits, parity, stopbits);
                // COM   115200   8  None   One
                clients = ReaderControllor.GetClientInfo();
                foreach (AsyncSocketState client in clients)
                {
                    ReaderControllor.GetMACDev(client);
                }
                if (clients.Count == 1)                          //只有一台连接的时候直接默认选择这一台
                {
                    currentclient =  clients[0];
                    if (currentclient.types == connect.net)
                    {
                         s = "Device:" + PrivateStringFormat.shortTolongNum(currentclient.dev); // + currentclient.dev;
                    }
                    else
                    {
                        // currentDev_l.Text = "Device:" + currentclient.com;
                        s = "Device:" + COM_DevID;
                    }

                }
            }
            catch (Exception ex)
            {
                UpdateLog("Error:" + ex.ToString());
                ErrorLog.WriteError(ex.ToString());
                return;
            }
            serialisstart = true;
            if ((serialisstart || serverisstart) && ((_IsActiveDatabaseWork) || (_IsRfidDatabaseWork) || (_IsCommandDatabaseWork)))
            {
              //  timer_md_query_Tick.Enabled = true;
            }
           
           // TimerShowID.Enabled = true;
           // show_EPC_t.Enabled = true;

         //   PortOpen_b.Text = stop;
          //  UpdateLog(openserial + success);
         //   EventLog.WriteEvent(openserial + success, null);

        }

        #region  信息定义
        private AsyncSocketState currentclient;
        public AsyncSocketState CurrentClient
        {
            get { return currentclient; }
            set { }
        }

        string dev_version = "";

        byte[] testproto;
        int DEVIDCount = 0;
        string COM_DevID = "";
        public static bool DataMark = false; // 接到响应数据标志

        public static int SetMode = 0; // 设置工作模式清除变量
        public static bool IsAutoMode = false; // 自动模式变量

        long HaveYuanSpeed = 0;
        string start = "";
        string stop = "";
        string set = "";
        string get = "";
        string success = "";
        string failed = "";
        string creatserver = "";
        string closeserver = "";
        string openserial = "";
        string closeserial = "";
        string startmonitor = "";
        string stopmonitor = "";
        string multiepc = "";
        string cleardata = "";
        string readtags = "";
        string writetags = "";
        string locktags = "";
        string killtags = "";
        string Workant = "";
        string Worktime = "";
        string GEN2 = "";
        string Gpio = "";
        string fRequency = "";
        string fRequencypoint = "";
        string Version = "";
        string Tempreture = "";
        string Power = "";
        string Workmode = "";
        string communication = "";
        string Invalid = "";
        string openmoniterfirst = "";
        string pwdlength = "";
        string fresh = "";
        string Area = "";
        string MacAnddev = "";
        string moudleparameters = "";
        string SIMConfig = "";
        string CommunicationInfo = "";
        string Init = "";
        string SpecialDisplay = "";
        string TimingDisplay = "";
        string TriggerDisplay = "";
        string CumulativeDisplay = "";
        string ChannelGate = "";
        string ModuleHardVersion = "";
        string ModuleFirmVersion = "";

        // 2.4G测试及多功能处新增语言切换变量
        string NoAnt_EN = "";
        string SourceDataSav = "";
        string GModule = "";
        string GTest = "";
        string LaunchPower = "";
        string Reduce = "";
        string ChannerPower = "";
        string SetAnt_EN = "";
        string StartSaveExcel_EN = "";
        string Type = "";
        string BootAutomaticcycle = "";
        string PoweroutagesSave = "";
        string Exit = "";
        private  bool serverisstart = false;
        private string datasource = string.Empty;
        private bool _IsRfidDatabaseWork = true;
        private bool _IsActiveDatabaseWork = true;
        private bool _IsCommandDatabaseWork = true;
        bool serialisstart = false;
        private string flag = "0";
        long NNNum = 0;
        public int CSSS = 0;



        public void ShowEPC(object sender, Command.ShowEPCEventArgs e)
        {
            //  UpdateLog(e.CommandRespond);
            //  string[] result = e.CommandRespond.Split(',');  
            string[] result = (e.CommandRespond + "," + flag).Split(',');           //每条命令后面加一个flag  用于循环盘存EPC显示
            byte type = Convert.ToByte(result[1], 16);
            switch (type)
            {
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

               
                  //  UpdateLBM(result); // 乔佳 2018-7-27 网络参数页面获取长编码
                  //  UpdateMac(result);
                
                    break;
                default:
                    break;
            }
        }









        public void UpdateLog(string strLog)

        {
            try
            {
                lock (InfoView)
                {
                    string strDateTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                    //ListViewItem item = new ListViewItem((this.listView_oper_log.Items.Count + 1).ToString());
                    //item.SubItems.Add(strDateTime);
                    //item.SubItems.Add(strLog);
                    //this.listView_oper_log.Items.Add(item);
                    //this.listView_oper_log.Items[this.listView_oper_log.Items.Count - 1].EnsureVisible();
                    //this.listView_oper_log.Items[this.listView_oper_log.Items.Count - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {

                
                ErrorLog.WriteError(ex.ToString());
            }


        }


        #endregion 
    }
}
