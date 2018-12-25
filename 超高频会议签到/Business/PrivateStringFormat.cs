using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace 超高频会议签到.Business
{
    class PrivateStringFormat
    {

        //2.4G  十进制字符串转16进制byte[]  
        /// <summary>
        /// 十进制字符串转16进制byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] DecStrToHexByte(string hexString)
        {

            hexString = hexString.Replace("\0", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            byte[] numby = new byte[3];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                {
                    if (i > 2)
                    {
                        int num = int.Parse(hexString.Substring(i * 2, 6));
                        numby[0] = (byte)(num >> 16);
                        numby[1] = (byte)(num >> 8);
                        numby[2] = (byte)num;
                        break;
                    }
                    else
                    {
                        returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 10);
                    }
                }
                numby.CopyTo(returnBytes, 3);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnBytes;
        }

        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace("\0", "");
            if ((hexString.Length % 2) != 0) hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return returnBytes;
        }


        public static byte[] LongByteToShortByte(byte[] longbyte)
        {
            byte[] shortbyte = new byte[Types.ID_LEN];
            if (longbyte.Length != (Types.ID_LEN + 1))
            {
                return shortbyte;
            }
            else
            {
                shortbyte[0] = longbyte[0];
                shortbyte[1] = (byte)((longbyte[1] << 2) | ((longbyte[2] & 0x30) >> 4));
                shortbyte[2] = (byte)(((longbyte[2] & 0x0F) << 4) | (longbyte[3] >> 4));
                shortbyte[3] = longbyte[4];
                shortbyte[4] = longbyte[5];
            }
            return shortbyte;
        }

        public static string shortTolongNum(string shortStringNum)
        {
            string longStringNum = string.Empty;
            try
            {
                if (shortStringNum == null) return null;
                byte[] hexNum = PrivateStringFormat.StrToHexByte(shortStringNum);
                if (hexNum.Length==0) return null;
                string type = (hexNum[0] >> 2).ToString("D2");
                string year = ((((hexNum[0] & 0x03) << 4) | (hexNum[1] >> 4)) + 2016).ToString();
                string month = (hexNum[1] & 0x0F).ToString("D2");
                string num = ((hexNum[2] << 8) | hexNum[3]).ToString("D5");
                longStringNum = type + year + month + num;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return longStringNum;
        }
    }
}
