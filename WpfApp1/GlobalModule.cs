using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApp1
{
    class GlobalModule
    {
        public static string OutputTextFile = "sample.txt";
        public static string OutputPdftFile = "sample.pdf";
        public static bool IsPdfSharp;
        public static bool IsITextSharp;
        public static bool isReadChecked;
        public static bool IsWriteChecked;
        public static bool IsEncryptChecked;
        public static bool IsDecryptChecked;
        public static string InputFilePath;
        public static string OutputFolderPath;
        public static string InputText;
        public static string EncryptionPassword;
        public static string DecryptionPassword;
        public static string Password;
        public static string ErrorMessage;
    }
}
