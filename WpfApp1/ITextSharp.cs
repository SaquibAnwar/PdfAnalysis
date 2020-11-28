using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApp1
{
    public static class ITextSharp
    {
        public static bool RunITextSharp()
        {
            bool isSuccess = false;
            try
            {
                if (GlobalModule.isReadChecked)
                {
                    isSuccess = Read();
                }
                if (GlobalModule.IsWriteChecked)
                {
                    isSuccess = Write();
                }
                if (GlobalModule.IsEncryptChecked)
                {
                    isSuccess = Encrypt();
                }
                if (GlobalModule.IsDecryptChecked)
                {
                    isSuccess = Decrypt();
                }
            }
            catch (Exception ex)
            {

            }
            return isSuccess;
        }

        private static bool Read()
        {
            bool isSuccess = false;
            try
            {

                isSuccess = true;
            }
            catch (Exception ex)
            {
                GlobalModule.ErrorMessage = ex.Message;
            }
            return isSuccess;
        }

        private static bool Write()
        {
            bool isSuccess = false;
            try
            {

                isSuccess = true;
            }
            catch (Exception ex)
            {
                GlobalModule.ErrorMessage = ex.Message;
            }
            return isSuccess;
        }

        private static bool Encrypt()
        {
            bool isSuccess = false;
            try
            {

                isSuccess = true;
            }
            catch (Exception ex)
            {
                GlobalModule.ErrorMessage = ex.Message;
            }
            return isSuccess;
        }

        private static bool Decrypt()
        {
            bool isSuccess = false;
            try
            {

                isSuccess = true;
            }
            catch (Exception ex)
            {
                GlobalModule.ErrorMessage = ex.Message;
            }
            return isSuccess;
        }
    }
}
