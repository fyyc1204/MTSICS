using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace toolkit
{
    /// <summary>
    /// INI 配置文件操作类
    /// 完成对配置文件的读写操作
    /// </summary>
    public class IniFileHelper
    {
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileSectionNamesW", CharSet=  CharSet.Unicode )]          
        private extern static int getSectionNames([MarshalAs(UnmanagedType.LPWStr)] string szBuffer, int nlen, string filename);
   
        [DllImport("kernel32",EntryPoint="GetPrivateProfileSectionW", CharSet = CharSet.Unicode)]          
        private extern static int getSectionValues(string Section,[MarshalAs(UnmanagedType.LPWStr)] string szBuffer, int nlen, string filename);  
   
        [DllImport("kernel32",EntryPoint="GetPrivateProfileIntW" , CharSet = CharSet.Unicode)]  
        private static extern int getKeyIntValue(string Section,string Key,int nDefault,string FileName);    
          
        [DllImport("kernel32",EntryPoint="GetPrivateProfileStringW" , CharSet = CharSet.Unicode)]  
        private extern static int getKeyValue(string section,string key,int lpDefault, [MarshalAs(UnmanagedType.LPWStr)] string szValue, int nlen, string filename);
    
        [DllImport("kernel32",EntryPoint="WritePrivateProfileStringW" , CharSet =  CharSet.Unicode)]  
        private static extern bool setKeyValue(string Section,string key,string szValue,string FileName);         
  
        [DllImport("kernel32",EntryPoint ="WritePrivateProfileSectionW" , CharSet =  CharSet.Unicode)]   
        private static extern bool setSectionValue(string section,string szvalue,string filename);  

    }
}
