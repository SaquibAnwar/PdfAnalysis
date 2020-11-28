using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfApp1
{
    class PdfSharp
    {
        public static bool RunPdfSharp()
        {
            bool isSuccess = false;
            try
            {
                if(GlobalModule.isReadChecked)
                {
                    isSuccess = Read(GlobalModule.InputFilePath);
                }
                if(GlobalModule.IsWriteChecked)
                {
                    isSuccess = Write();
                }
                if(GlobalModule.IsEncryptChecked)
                {
                    isSuccess = Encrypt();
                }
                if(GlobalModule.IsDecryptChecked)
                {
                    isSuccess = Decrypt();
                }
            }
            catch(Exception ex)
            {

            }
            return isSuccess;
        }

        public static bool Read(string pdfFileName)
        {
            bool isSuccess = false;
            try
            {
                using (var _document = PdfReader.Open(pdfFileName, GlobalModule.Password, PdfDocumentOpenMode.ReadOnly))
                {
                    var result = new StringBuilder();
                    ExtractText(ContentReader.ReadContent(_document.Pages[0]), result);
                    result.AppendLine();
                    //foreach (var page in _document.Pages.OfType<PdfPage>())
                    //{
                    //    ExtractText(ContentReader.ReadContent(page), result);
                    //    result.AppendLine();
                    //}
                    //return result.ToString();
                }
                isSuccess = true;
            }
            catch(Exception ex)
            {
                GlobalModule.ErrorMessage = ex.Message;
            }
            return isSuccess;
        }

        #region CObject Visitor
        private static void ExtractText(CObject obj, StringBuilder target)
        {
            if (obj is CArray)
                ExtractText((CArray)obj, target);
            else if (obj is CComment)
                ExtractText((CComment)obj, target);
            else if (obj is CInteger)
                ExtractText((CInteger)obj, target);
            else if (obj is CName)
                ExtractText((CName)obj, target);
            else if (obj is CNumber)
                ExtractText((CNumber)obj, target);
            else if (obj is COperator)
                ExtractText((COperator)obj, target);
            else if (obj is CReal)
                ExtractText((CReal)obj, target);
            else if (obj is CSequence)
                ExtractText((CSequence)obj, target);
            else if (obj is CString)
                ExtractText((CString)obj, target);
            else
                throw new NotImplementedException(obj.GetType().AssemblyQualifiedName);
        }

        private static void ExtractText(CArray obj, StringBuilder target)
        {
            foreach (var element in obj)
            {
                ExtractText(element, target);
            }
        }
        private static void ExtractText(CComment obj, StringBuilder target) { /* nothing */ }
        private static void ExtractText(CInteger obj, StringBuilder target) { /* nothing */ }
        private static void ExtractText(CName obj, StringBuilder target) { /* nothing */ }
        private static void ExtractText(CNumber obj, StringBuilder target) { /* nothing */ }
        private static void ExtractText(COperator obj, StringBuilder target)
        {
            if (obj.OpCode.OpCodeName == OpCodeName.Tj || obj.OpCode.OpCodeName == OpCodeName.TJ)
            {
                foreach (var element in obj.Operands)
                {
                    ExtractText(element, target);
                }
                target.Append(" ");
            }
        }
        private static void ExtractText(CReal obj, StringBuilder target) { /* nothing */ }
        private static void ExtractText(CSequence obj, StringBuilder target)
        {
            foreach (var element in obj)
            {
                ExtractText(element, target);
            }
        }
        private static void ExtractText(CString obj, StringBuilder target)
        {
            target.Append(obj.Value);
        }
        #endregion

        private static bool Write()
        {
            bool isSuccess = false;
            try
            {
                PdfDocument document = new PdfDocument();

                PdfPage page = document.AddPage();

                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

                gfx.DrawString(GlobalModule.InputText, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

                
                string outputPath = Path.Combine(GlobalModule.OutputFolderPath, GlobalModule.OutputPdftFile);
                document.Save(outputPath);
                Process.Start(outputPath);
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
                string filenameSource = GlobalModule.InputFilePath;
                string filenameDest = "Encrypted Pdf.pdf";
                string outputPath = Path.Combine(GlobalModule.OutputFolderPath, filenameDest);

                File.Copy(filenameSource, outputPath, true);

                PdfDocument document = PdfReader.Open(outputPath, GlobalModule.Password);

                PdfSecuritySettings securitySettings = document.SecuritySettings;

                
                securitySettings.UserPassword = securitySettings.OwnerPassword = GlobalModule.EncryptionPassword;

                
                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = true;
                securitySettings.PermitFullQualityPrint = false;
                securitySettings.PermitModifyDocument = true;
                securitySettings.PermitPrint = false;

                document.Save(outputPath);
                Process.Start(outputPath);
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

                string filenameSource = GlobalModule.InputFilePath;
                string filenameDest = "Decrypted Pdf.pdf";
                string outputPath = Path.Combine(GlobalModule.OutputFolderPath, filenameDest);

                File.Copy(filenameSource, outputPath, true);

                PdfDocument document;

                try
                {
                    document = PdfReader.Open(outputPath, GlobalModule.Password);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                document = PdfReader.Open(outputPath, PdfDocumentOpenMode.Modify, PasswordProvider);

                document = PdfReader.Open(outputPath, GlobalModule.DecryptionPassword, PdfDocumentOpenMode.ReadOnly);

                
                bool hasOwnerAccess = document.SecuritySettings.HasOwnerPermissions;

                document = PdfReader.Open(outputPath, GlobalModule.DecryptionPassword);
                hasOwnerAccess = document.SecuritySettings.HasOwnerPermissions;

                PdfDocumentSecurityLevel level = document.SecuritySettings.DocumentSecurityLevel;
                document.Save(outputPath);
                Process.Start(outputPath);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                GlobalModule.ErrorMessage = ex.Message;
            }
            return isSuccess;
        }

        static void PasswordProvider(PdfPasswordProviderArgs args)
        {
            args.Password = GlobalModule.DecryptionPassword;
        }
    }
}
