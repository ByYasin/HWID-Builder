/******************************\
| Memoryhackers.org / ByYasin  |
\******************************/
using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using Figgle;

namespace HWID_Builder
{
    class Program
    {
        static void println(String str) 
            => Console.WriteLine(str);

        static void Main(string[] args)
        {
            Console.BufferHeight = 513;
            Console.BufferWidth = 1300;
            Console.Title = "Hwid Builder - 7 hardware features | Memoryhackers.org | ByYasin";
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(FiggleFonts.Standard.Render("Memoryhackers . org"));
            //println(null);
            var hwid = new HWID();
            Console.ForegroundColor = ConsoleColor.White;
            println(hwid.ToString());
            hwid.Copy();
            Console.ForegroundColor = ConsoleColor.Green;
            println("(ENG) Generated Hardware ID has been copied to clipboard! | (TR) Oluşturulan Donanım Kimliği panoya kopyalandı!");
            println(null);
            Console.ForegroundColor = ConsoleColor.Red;
            println("(ENG) Press any key to exit... | (TR) Çıkmak için herhangi bir tuşa basın...");
            println(null);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(FiggleFonts.Standard.Render("ByYasin"));
            Console.ReadLine();
        }
    }

    class HWID
    {
        Boolean IsServer { get; set; }
        String BIOS { get; set; }
        String CPU { get; set; }
        String HDD { get; set; }
        String GPU { get; set; }
        String MAC { get; set; }
        String HardwareID { get; set; }
        String OS { get; set; }
        String SCSI { get; set; }

        public HWID()
        {
            BIOS = GetWMIIdent("Win32_BIOS", "Manufacturer", "SMBIOSBIOSVersion", "IdentificationCode");
            CPU  = GetWMIIdent("Win32_Processor", "ProcessorId", "UniqueId", "Name");
            HDD  = GetWMIIdent("Win32_DiskDrive", "Model", "TotalHeads");
            GPU  = GetWMIIdent("Win32_VideoController", "DriverVersion", "Name");
            MAC  = GetWMIIdent("Win32_NetworkAdapterConfiguration", "MACAddress");
            OS   = GetWMIIdent("Win32_OperatingSystem", "SerialNumber", "Name");
            SCSI = GetWMIIdent("Win32_SCSIController", "DeviceID", "Name");

            // checking if system is a server. scsi indicates a server system
            // sistemin bir sunucu olup olmadığını kontrol etmek için. scsi bir sunucu sistemini belirtir
            IsServer = HDD.Contains("SCSI");

            HardwareID = Build();
        }

        private String Build()
        {
            var tmp = String.Concat(BIOS, CPU, HDD, GPU, MAC, SCSI);

            if (tmp == null)
                Console.WriteLine("(ENG) Could not resolve hardware informations... | (TR) Donanım bilgileri çözülemedi...");

            return Convert.ToBase64String(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(tmp)));
        }

        private Boolean IsWinServer()
            => OS.Contains("Microsoft Windows Server");

        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        // Ghetto SetClipboard function using imports
       // İçe aktarımları kullanan Ghetto SetClipboard işlevi
        public void Copy()
        {
            OpenClipboard(IntPtr.Zero);
            var ptr = Marshal.StringToHGlobalUni(HardwareID == null ? Build() : HardwareID);
            SetClipboardData(13, ptr);
            CloseClipboard();
            Marshal.FreeHGlobal(ptr);
        }

        public override String ToString()
            => String.Format("BIOS\t\t - \t{0}\nCPU\t\t - \t{1}\nGPU\t\t - \t{2}\nMAC\t\t - \t{3}\nOS\t\t - \t{4}\n" + (IsServer ? "SCSI\t\t - \t{5}\n" : "") + "\nGenerated Hardware ID:\n{6}\n", BIOS, CPU, GPU, MAC, OS, SCSI, HardwareID);

        private static String GetWMIIdent(String Class, String Property)
        {
            var ident = "";
            var objCol = new ManagementClass(Class).GetInstances();
            foreach(var obj in objCol)
            {
                if((ident = obj.GetPropertyValue(Property) as String) != "")
                    break;
            }
            return ident;
        }

        private static String GetWMIIdent(String Class, params String[] Propertys)
        {
            var ident = "";
            Array.ForEach(Propertys, prop => ident += GetWMIIdent(Class, prop) + " ");
            return ident;
        }

        public static String Get()
            => new HWID().HardwareID;
    }
}
/******************************\
| Memoryhackers.org / ByYasin  |
\******************************/