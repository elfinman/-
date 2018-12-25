using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 超高频会议签到
{
    public class Types
    {
         public const string INITIALVALUE = "ERROR";
         public const string OK = "OK";
         public const string STOP_FLAG = "2";
         public const string NO_DISPLAY = "0";
         public const string TIMING_DISPLAY = "1";
         public const string TRIGGER_DISPLAY = "2";
         public const string TOTAL_DISPLAY = "3";
         public const string CHANNEL_DISPLAY = "4";
         public const byte   LONG_EPC_LEN = 15;

         public const int OPER_OK = 0;
         public const int OPER_SUCCESS = 1;
         public const int INVALID_VALUE = -1;
         public const int PACKET_16 = 16;
         public const int COMMAND_MAX_LEN = 128;
         public const int FREQUENCY_POINT_NUM = 128;
         public const int EPC_LEN = 16;
         public const int PWD_LEN = 4;
         public const int FLITER_LEN = 2;
         public const int ID_LEN = 5;
         public const int MAX_DATABASE_LEN = 14;

         //20180828 乔佳 新增获取以及设置同时读取EPC和TID响应
         public const byte GET_EAT = 0xE3;
         public const byte SET_EAT = 0xE2;
         public const byte Storage_Dev_Send = 0XE1;          //V2.30新增协议，扩展协议，终端上行
         public const byte GET_GPS = 0xE4;



         public const byte SET_POWER = 0x00;
         public const byte SET_GPIO= 0x01;
         public const byte SET_FREQUENCY_POINT = 0x02;
         public const byte SET_CARRIER = 0x05;
         public const byte SET_GEN2 = 0x07;
         public const byte SET_WORK_ANT = 0x08;
         public const byte SET_FREQUENCY_AREA = 0x09;
         public const byte GET_CARRIER = 0x0F;
         public const byte SET_WORK_INTERRUPTED = 0x1D;         //单通道
         public const byte SET_WORK_TIME = 0x1F;
         public const byte SET_FASTID = 0x21;
         public const byte SET_QT = 0x26;
         public const byte SET_TAGFOCUS = 0x29;
         

         public const byte GET_HARD_VERSION = 0x0A;
         public const byte GET_FIRM_VERSION = 0x0B;
         public const byte GET_POWER = 0x0C;
         public const byte GET_FREQUENCY_POINT= 0x0D;
         public const byte GET_WORK_ANT = 0x10;
         public const byte GET_FREQUENCY_AREA = 0x11;
         public const byte GET_TEMPERATURE = 0x12;
         public const byte GET_GPIO = 0x13;
         public const byte GET_GEN2 = 0x14;
         public const byte START_SINGLE_EPC = 0x16;
         public const byte START_MULTI_EPC = 0x17;
         public const byte STOP_MULTI_EPC = 0x18;
         public const byte READ_TAGS = 0x19;
         public const byte WRITE_TAGS = 0x1A;
         public const byte LOCK_TAGS = 0x1B;
         public const byte KILL_TAGS = 0x1C;
         public const byte GET_WORK_INTERRUPTED = 0x1E;
         public const byte GET_WORK_TIME = 0x20;
         public const byte GET_FASTID = 0x22;
         public const byte GET_QT = 0x27;
         public const byte GET_TAGFOCUS = 0x2A;
         public const byte GET_WORK_MODE = 0x34;
         public const byte SET_WORK_MODE = 0x37;

         public const byte SET_POWER_RESPOND = 0x80;
         public const byte SET_GPIO_RESPOND = 0x81;
         public const byte SET_FREQUENCY_POINT_RESPOND = 0x82;
         public const byte SET_RF_LINK_RESPOND = 0x83;
         public const byte SET_CARRIER_RESPOND = 0x85;
         public const byte GET_RF_LINK_RESPOND = 0x8E;
         public const byte GET_CARRIER_RESPOND = 0x8F;
         public const byte SET_GEN2_RESPOND = 0x87;
         public const byte SET_WORK_ANT_RESPOND = 0x88;
         public const byte SET_FREQUENCY_AREA_RESPOND = 0x89;
         public const byte GET_FREQUENCY_POINT_RESPOND = 0x8D;
         public const byte GET_FREQUENCY_AREA_RESPOND = 0x91;
         public const byte GET_TEMPERATURE_RESPOND = 0x92;
         public const byte GET_GPIO_RESPOND = 0x93;
         public const byte GET_GEN2_RESPPOND = 0x94;

         public const byte START_Temp_EPC = 0xF7;      //  模块读取温度响应

         public const byte AntPercentage = 0XAC;  // 天线百分比


         public const byte START_SINGLE_EPC_RESPOND = 0x96;
         public const byte START_MULTI_EPC_RESPOND = 0x97;
         public const byte STOP_MULTI_EPC_RESPOND = 0x98;
         public const byte SET_SINGLE_CHANNEL_RESPOND = 0x9D;
         public const byte GET_HARD_VERSION_RESPOND = 0x8A;
         public const byte GET_FIRM_VERSION_RESPOND = 0x8B;
         public const byte GET_POWER_RESPOND = 0x8C;
         public const byte GET_WORK_ANT_RESPOND = 0x90;
         public const byte READ_TAGS_RESPOND = 0x99;
         public const byte WRITE_TAGS_RESPOND = 0x9A;
         public const byte LOCK_TAGS_RESPOND = 0x9B;
         public const byte KILL_TAGS_RESPOND = 0x9C;
         public const byte GET_WORK_INTERRUPTED_RESPOND = 0x9E;
         public const byte SET_WORK_TIME_RESPOND = 0x9F;
         public const byte GET_WORK_TIME_RESPOND = 0xA0;
         public const byte SET_FASTID_RESPOND = 0xA1;
         public const byte GET_FASTID_RESPOND = 0xA2;
         public const byte SET_QT_RESPOND = 0xA6;
         public const byte GET_QT_RESPOND = 0xA7;
         public const byte SET_TAGFOCUS_RESPOND = 0xA9;
         public const byte GET_TAGFOCUS_RESPOND = 0xAA;
         public const byte GET_COMMUNICATION_INFO_RESPOND = 0xAB;
         public const byte ERROR_RESPOND = 0x0A;
         public const byte SET_COMMUNICATION_INFO_RESPOND = 0xC3;
         public const byte GET_WORK_MODE_RESPOND = 0xC4;
         public const byte SET_WORK_MODE_RESPOND = 0xC7;
         public const byte SET_MAC_ADD_RESPOND = 0xC8;
         public const byte GET_MAC_ADD_RESPOND = 0xC9;
         public const byte GET_SIM_RESPOND = 0xCB;
         public const byte SET_SIM_RESPOND = 0xCC;
         public const byte SET_WIFI_RESPOND = 0xCF;
         public const byte GET_WIFI_RESPOND = 0xD1;                   //获取wifi参数应答
         public const byte SET_EPC_RESPOND = 0xD3;                   //获取wifi参数应答
         public const byte GET_EPC_RESPOND = 0xD4;                   //获取wifi参数应答
         public const byte SET_MQTT_RESPOND = 0xD5;
         public const byte GET_MQTT_RESPOND = 0xD6;                   //获取wifi参数
         public const byte SET_MQTT_THEME_RESPOND = 0xD7;
         public const byte GET_MQTT_THEME_RESPOND = 0xD8;
         public const byte GET_ACCESS_DIR_RESPOND = 0xDB;
         public const byte GET_MULTI_ID = 0xF1;                    //2.4G 查询标签ID
         public const byte START_INFO = 0xF3;                    //2.4G 读卡器模块启动信息
         public const byte DOWN_DATA = 0xFA;                    //2.4G 初始化读卡去全部参数  启动，退出载波测试  模块自动重启 设置过滤
         public const byte DOWN_DATA_RESPOND = 0xFB;                    //2.4G 初始化读卡去全部参数应答 动，退出载波测试应答 模块自动重启响应
         public const byte ERROR_FLAG = 0xFF;

         public const byte SET_MODULE_VERSION_9200_RESPOND = 0xFE;
         public const byte GET_MODULE_VERSION_9200_RESPOND = 0xFD;   
    }

    public struct Respond
    {
        public string HardVersion;
        public string FirmVersion;
        public string Temperature;
        public string Power;
        public string Gerneral;
        public string FrequencyPoint;
        public string FrequencyArea;
        public string Gpio;
        public string Gen2;
        public string singleEPC ;
        public string WorkAnt;
        public string ReadTags;
        public string WriteTags;
        public string LockTags;
        public string Killtags;
        public string WorkTime;
        public string FastID;
        public string QT;
        public string Tagfocus;
        public string Workmode;
        public int Result;
    }

    class tag_mc
    {
        public int ant_id = 0;
        public int count = 0;
        public ushort pc = 0;
        public string epc = "";
        public double rssi = 0.0;
        public string ip = "";
        public string latest_time = "";
    }

    class filesManagement
    {
        public string[] AntEpc;
        public string[] AntEpcNew; 
        public int increment = 0;         //增量
        public int decrement = 0;             //减量
        public int variation = 0;             //变化量
        public int count = 0;             //变化量
    }

    class writeTagComand
    {
        public string bank;
        public string data;
        public string type;
    }
}
