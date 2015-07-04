using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Timers;
namespace MT_SICS
{
    public class MT_SICS
    {

        private SerialPort mySrialPort;

        public SerialPort MySrialPort
        {
            get { return mySrialPort; }
            set { mySrialPort = value; }
        }
        
        private String portName;//串口号

        public String PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        private int baudRate;//波特率

        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }
        private Parity portParity;//奇偶校验位

        public Parity PortParity
        {
            get { return portParity; }
            set { portParity = value; }
        }
        private int dataBits;//数据位

        public int DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }
        private StopBits portStopBits;//停止位

        public StopBits PortStopBits
        {
            get { return portStopBits; }
            set { portStopBits = value; }
        }

        public delegate void DataStableEventHandler(object sender,DataStableEventArgs args);
        public event DataStableEventHandler OnDataStable;

        /// <summary>
        /// 构造函数 初始化串口组件
        /// </summary>
        public MT_SICS()
        {
            if (mySrialPort == null)
            {
                //mySrialPort = new SerialPort();
                mySrialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
                mySrialPort.DataReceived+=new SerialDataReceivedEventHandler(mySrialPort_DataReceived);
            }
        }

        /// <summary>
        /// 构造函数 初始化串口组件
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="portParity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="portStopBits">停止位</param>
        public MT_SICS(String portName, int baudRate, Parity portParity, int dataBits, StopBits portStopBits)
        {
            if (mySrialPort == null)

            {

                mySrialPort = new SerialPort(portName,baudRate, portParity, dataBits, portStopBits);
               // mySrialPort = new SerialPort("COM3",2400,Parity.None, 8, StopBits.Two);
                mySrialPort.DataReceived += new SerialDataReceivedEventHandler(mySrialPort_DataReceived);
            }
        }

        /// <summary>
        /// 打开串口连接
        /// </summary>
        public void Open()
        {
            if (mySrialPort == null) throw new ArgumentNullException("mySrialPort");
            if (mySrialPort.IsOpen) throw new InvalidOperationException("串口已经处于打开状态，无法再次进行打开");
            
            //开启消息队列扫描
            //workThread.Start();
            mySrialPort.Open();
        }

        /// <summary>
        /// 关闭串口连接
        /// </summary>
        public void Close()
        {
            if (mySrialPort == null) throw new ArgumentNullException("mySrialPort");
            if (!mySrialPort.IsOpen) throw new InvalidOperationException("串口已经处于关闭状态，无法再次进行关闭");

            //关闭消息队列扫描
            //workThread.Abort();

            mySrialPort.DataReceived -= new SerialDataReceivedEventHandler(mySrialPort_DataReceived);
            mySrialPort.Close();
        }

        /// <summary>
        /// 字符串转ASCII
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        private byte[] Str2Ascii(String org)
        {
            if (org == null) throw new ArgumentNullException("org");
            return System.Text.Encoding.ASCII.GetBytes(org);
        }


        private static string  HEXToAscii(String hex)

        {
            byte[] buff = new byte[hex.Length / 2];

            string Mes="";
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i]=byte.Parse(hex.Substring(i*2,2),
                System.Globalization.NumberStyles.HexNumber);
            }

            System.Text.Encoding chs = System.Text.Encoding.ASCII;
            Mes = chs.GetString(buff);
            return Mes;



           
        }

        /// <summary>
        /// ASCII转字符串
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        private String Bytes2Str(byte[] org)
        {
            if (org.Length == 0) throw new ArgumentNullException("org");
            return System.Text.Encoding.ASCII.GetString(org);
        }

        /// <summary>
        /// 发送取得稳定称量值请求
        /// </summary>
        public void SendStableWeightRequest()
        {
            String command = "S" + Environment.NewLine; //大写S+回车换行
            byte[] asciiByte = Str2Ascii(command);
            mySrialPort.Write(asciiByte, 0, asciiByte.Length);
        }




        public void SendSeriesWeightRequest_E3000Y(string cmd)
        {
            
            String command = HEXToAscii(cmd) + Environment.NewLine;
            byte[] asciiByte = Str2Ascii(command);
            mySrialPort.Write(asciiByte, 0, asciiByte.Length);
           // mySrialPort.Write(command);
        }

        public void SendSeriesWeightRequest()
        {
            String command = "SIR" + Environment.NewLine;
            byte[] asciiByte = Str2Ascii(command);
            mySrialPort.Write(asciiByte, 0, asciiByte.Length);
        }


        private void mySrialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            byte[] orgByte = new byte[mySrialPort.BytesToRead];
            Thread.Sleep(80);
            //mySrialPort.Read(orgByte, 0, orgByte.Length);
            String respone = mySrialPort.ReadLine();
            //string respone = Bytes2Str(orgByte);

            if (OnDataStable != null)
            {
                OnDataStable(this, new DataStableEventArgs(respone));
            }
        }

            
    }

    public class DataStableEventArgs : EventArgs
    {
        public DataStableEventArgs(String orgData)
        {
            this.orgData = orgData;
        }
        private String orgData;

        public String OrgData
        {
            get { return orgData; }
            set { orgData = value; }
        }


    }
}

