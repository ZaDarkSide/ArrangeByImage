using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArrangeByImage
{
    public partial class frmMain : Form
    {

        const uint MEM_COMMIT = 0x1000;
        const uint PAGE_READWRITE = 4;
        const uint LVM_GETITEMCOUNT = 4100;
        const uint LVM_GETITEMPOSITION = 4112;
        const uint LVM_SETITEMPOSITION = 4111;
        const uint LVM_GETITEMTEXT = 4141;
        const ushort LVIF_TEXT = 1;

        const uint LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;
        const uint LVM_GETEXTENDEDLISTVIEWSTYLE = 4151;
        const uint LVS_EX_SNAPTOGRID = 524288;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        /// <summary>
        /// Returns a list of child windows
        /// </summary>
        /// <param name="parent">Parent of the windows to return</param>
        /// <returns>List of child windows</returns>
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static List<IntPtr> GetWindows()
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(IntPtr.Zero , childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// Callback method to be used when enumerating windows.
        /// </summary>
        /// <param name="handle">Handle of the next window</param>
        /// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
        /// <returns>True to continue the enumeration, false to bail</returns>
        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        public frmMain()
        {
            InitializeComponent();
        }

        private IntPtr CalculateCoordinates(int x, int y)
        {
            return (IntPtr)(x + (y << 16));
        }

        private void Arrange()
        {

        }

        private void TurnOffSnapToGrid(HandleRef desktopReference)
        {
            try
            {
                int CurrentSettings = (int)SendMessage(desktopReference, LVM_GETEXTENDEDLISTVIEWSTYLE, IntPtr.Zero, IntPtr.Zero);
                IntPtr ptr = new IntPtr(CurrentSettings & ~LVS_EX_SNAPTOGRID);
                SendMessage(desktopReference, LVM_SETEXTENDEDLISTVIEWSTYLE, IntPtr.Zero, ptr);
            }
            catch
            {
            }
        }

        private void TurnOnSnapToGrid(HandleRef desktopReference)
        {
            try
            {
            int CurrentSettings = (int)SendMessage(desktopReference, LVM_GETEXTENDEDLISTVIEWSTYLE, IntPtr.Zero, IntPtr.Zero);
            IntPtr ptr = new IntPtr(CurrentSettings | LVS_EX_SNAPTOGRID);
            SendMessage(desktopReference, LVM_SETEXTENDEDLISTVIEWSTYLE, IntPtr.Zero, ptr);
            }
            catch
            {
            }
        }

        private IntPtr listviewWindow;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Check whether we're running under Vista or XP
            Version version = Environment.OSVersion.Version;
            if (version.Major != 6 && version.Major != 5)
            {
                MessageBox.Show("This application only works under Windows Vista or XP, and not " + version.ToString());
                Application.Exit();
            }

            // Find a handle to the listview that makes up the desktop
            IntPtr programmanagerWindow = FindWindow(null, "Program Manager");
            IntPtr desktopWindow = FindWindowEx(programmanagerWindow, IntPtr.Zero, "SHELLDLL_DefView", null);
            listviewWindow = FindWindowEx(desktopWindow, IntPtr.Zero, "SysListView32", null);
            if (desktopWindow == IntPtr.Zero )
            {
                List<IntPtr> l = GetWindows();
                foreach (IntPtr p in l)
                {
                    desktopWindow = FindWindowEx(p, IntPtr.Zero, "SHELLDLL_DefView", null);
                    if (desktopWindow != IntPtr.Zero)
                        break;
                }
                
                
                listviewWindow = FindWindowEx(desktopWindow, IntPtr.Zero, "SysListView32", null);
            }
            HandleRef desktopReference = new HandleRef(null, listviewWindow);
            int iconCount = (int)SendMessage(desktopReference, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
            txtNumberOfIcons.Text = iconCount.ToString();
            //displayUsage();
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                runCommandPrompt(args);
                return;
            }

        }

        bool silent = false;
        private void runCommandPrompt(string[] args)
        {
            
            bool nosave = false;
            
            for (int i = 1; i <= args.Length - 1; i++)
            {
                if ((args[i] == "-silent"))
                {
                    silent = true;
                }
                else
                {
                    if ((args[i] == "-icons"))
                    {
                        if ((i + 1 < args.Length))
                        {
                            txtNumberOfIcons.Text = args[i + 1];
                        }
                    }
                    else if ((args[i] == "-bmp"))
                    {
                        if ((i + 1 < args.Length))
                        {
                            txtFileName.Text = args[i + 1];
                        }
                    }
                    else if ((args[i] == "-help"))
                    {
                        displayUsage();
                        this.Close();
                    }
                    else if ((args[i] == "-nosave"))
                    {
                        nosave = true;
                    }
                    else if ((args[i] == "-restore"))
                    {
                        RestoreIconPositions(listviewWindow);
                        this.Close();
                    }
                    else if ((args[i] == "-heart"))
                    {
                        chkEnableDrawMode.Checked = true;
                        pbGraphicsImg = ArrangeByImage.Properties.Resources.heart;
                        pbDisplay.Image = pbGraphicsImg;
                    }
                    else if ((args[i] == "-smiley"))
                    {
                        chkEnableDrawMode.Checked = true;
                        pbGraphicsImg = ArrangeByImage.Properties.Resources.smiley;
                        pbDisplay.Image = pbGraphicsImg;
                    }
                    else if ((args[i] == "-star"))
                    {
                        chkEnableDrawMode.Checked = true;
                        pbGraphicsImg = ArrangeByImage.Properties.Resources.star;
                        pbDisplay.Image = pbGraphicsImg;
                    }
                    else if ((args[i] == "-penis"))
                    {
                        chkEnableDrawMode.Checked = true;
                        pbGraphicsImg = ArrangeByImage.Properties.Resources.penis;
                        pbDisplay.Image = pbGraphicsImg;
                    }
                }
            }

            if (silent == true)
            {
                if (!nosave) StoreIconPositions(listviewWindow);
                this.Visible = false;
                arrangeByImage(listviewWindow);
                this.Close();
            }


        }

        void displayUsage()
        {
            string message = "";
            message += "ArrangeByImage v-1.2 USAGE" + "\r\n";
            message += "ArrangeByImage -bmp pathToImage [-icons XX] [ -silent ] [ -help ]" + "\r\n";
            message += "               [ -restore ] [ -nosave ]" + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-bmp the path to the image to be used to arrange the icons" + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-icons a number, the desired number of icons to sample for " + "\r\n";
            message += "\t" + "       if left blank the number of icons on the desktop " + "\r\n";
            message += "\t" + "       will be used." + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-silent program will automatically arrange icons and close" + "\r\n";
            message += "\t" + "        itself displaying nothing to the user. " + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-nosave by default the current icon layout is saved, this flag" + "\r\n";
            message += "\t" + "        will stop that behavior. " + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-restore restores a saved version of the layout (undoes what you" + "\r\n";
            message += "\t" + "         have wrought. " + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-help display this menu" + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-smiley arange icons in the shape of a smiley (built in image)" + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-heart arange icons in the shape of a heart (built in image)" + "\r\n";
            message += "" + "\r\n";
            message += "\t" + "-star arange icons in the shape of a star (built in image)" + "\r\n";
            message += "" + "\r\n";
            message += "" + "\r\n";
            message += "download the current version at officewarfare.net" + "\r\n";
            message += "" + "\r\n";
            Console.Write(message);
            MessageBox.Show(message);
        }

        //private void ArrangeByImage(IntPtr listviewWindow)
        //{
        //    Size monitorSize = SystemInformation.PrimaryMonitorSize;

        //    Graph g = new Graph();

        //    // Convert the desktop listview handle to a handle reference so we can later use it in unsafe code
        //    HandleRef desktopReference = new HandleRef(null, listviewWindow);

        //    // Get the number of icons that's currently on the desktop
        //    int iconCount = (int)SendMessage(desktopReference, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);

        //    double step = g.End / (double)iconCount;

        //    double v = 0.0;
        //    for (int i = 0; i < iconCount; i++)
        //    {
        //        Point p = g.getPoint(v, monitorSize.Width, monitorSize.Height);
        //        SendMessage(desktopReference, LVM_SETITEMPOSITION, (IntPtr)i, CalculateCoordinates(p.x, p.y));
        //        v += step;
        //    }
        //}

        struct LVITEM
        {
            public UInt32 mask;
            public int iItem;
            public int iSubItem;
            public UInt32 state;
            public UInt32 stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public UInt32 lParam;
            int iIndent;
            int iGroupId;
            uint cColumns;
            IntPtr puColumns;
            IntPtr piColFmt;
            int iGroup;
        };

        private void StoreIconPositions(IntPtr listviewWindow)
        {
            // Get the process handle to the currently running explorer
            Process[] processes = Process.GetProcessesByName("explorer");

            LinkedList<IconInfo> iconInfos = new LinkedList<IconInfo>();

            foreach (Process process in processes)
            {
                // Convert the desktop listview handle to a handle reference so we can later use it in unsafe code
                HandleRef desktopReference = new HandleRef(null, listviewWindow);

                // Allocate some memory in explorer's space so we can use that as temporary storage
                IntPtr ptr = VirtualAllocEx(process.Handle, IntPtr.Zero, 8, MEM_COMMIT, PAGE_READWRITE);
                IntPtr ipc_iconlabel = VirtualAllocEx(process.Handle, IntPtr.Zero, 100, MEM_COMMIT, PAGE_READWRITE);
                IntPtr ipc_buffer = VirtualAllocEx(process.Handle, IntPtr.Zero, 300, MEM_COMMIT, PAGE_READWRITE);

                // Get the number of icons that's currently on the desktop
                int iconCount = (int)SendMessage(desktopReference, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);

                for (int i = 0; i < iconCount; i++)
                {
                    // Send a window message to the desktop listview to get it to store the icon's position in our borrowed memory space
                    IntPtr x2 = SendMessage(desktopReference, LVM_GETITEMPOSITION, (IntPtr)i, (IntPtr)ptr);

                    // Copy the contents of the remote allocated array into our own memory space
                    byte[] b = new byte[8];
                    int f;
                    unsafe
                    {
                        fixed (byte* bp = b)
                        {
                            ReadProcessMemory(process.Handle, ptr, (IntPtr)bp, 8, out f);
                        }
                    }

                    int xpos = b[0] + (b[1] << 8) + (b[2] << 16) + (b[3] << 24);
                    int ypos = b[4] + (b[5] << 8) + (b[6] << 16) + (b[7] << 24);


                    LVITEM iconlabel = new LVITEM();
                    iconlabel.iSubItem = 0;
                    iconlabel.cchTextMax = 256;
                    iconlabel.mask = LVIF_TEXT;
                    iconlabel.pszText = ipc_buffer;

                    // Test code
                    /*string s1 = listView1.Items[0].Text;
                    unsafe
                    {
                        HandleRef xReference = new HandleRef(null, listView1.Handle);
                        LVITEM* bp = &iconlabel;
                        byte[] bx = new byte[300];
                        fixed (byte* bxp = bx)
                        {
                            iconlabel.pszText = (IntPtr)bxp;
                            IntPtr x23 = SendMessage(xReference, LVM_GETITEMTEXT, (IntPtr)0, (IntPtr)bp);
                        }
                    }*/

                    unsafe
                    {
                        LVITEM* bp = &iconlabel;
                        WriteProcessMemory(process.Handle, ipc_iconlabel, (IntPtr)bp, 100, out f);
                    }

                    int iconNameLength = (int)SendMessage(desktopReference, LVM_GETITEMTEXT, (IntPtr)i, ipc_iconlabel);

                    // Copy the contents of the remote allocated array into our own memory space
                    byte[] b2 = new byte[300];
                    unsafe
                    {
                        fixed (byte* bp = b2)
                        {
                            ReadProcessMemory(process.Handle, ipc_buffer, (IntPtr)bp, 300, out f);
                        }
                    }
                    char[] c2 = new char[iconNameLength];
                    for (int j = 0; j < iconNameLength; j++)
                        c2[j] = (char)b2[j];
                    String iconName = new String(c2);

                    IconInfo ii = new IconInfo(iconName, xpos, ypos);
                    iconInfos.AddLast(ii);
                }

                string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Arrange\\";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                Stream stream = File.Open(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Arrange\\backup.dat", FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, iconInfos);
                stream.Close();
            }
        }

        private void RestoreIconPositions(IntPtr listviewWindow)
        {
            Stream stream = File.Open(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Arrange\\backup.dat", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            LinkedList<IconInfo> iconInfos = (LinkedList<IconInfo>)bformatter.Deserialize(stream);
            stream.Close();

            // Get the process handle to the currently running explorer
            Process[] processes = Process.GetProcessesByName("explorer");

            foreach (Process process in processes)
            {
                // Convert the desktop listview handle to a handle reference so we can later use it in unsafe code
                HandleRef desktopReference = new HandleRef(null, listviewWindow);

                // Allocate some memory in explorer's space so we can use that as temporary storage
                IntPtr ptr = VirtualAllocEx(process.Handle, IntPtr.Zero, 8, MEM_COMMIT, PAGE_READWRITE);
                IntPtr ipc_iconlabel = VirtualAllocEx(process.Handle, IntPtr.Zero, 100, MEM_COMMIT, PAGE_READWRITE);
                IntPtr ipc_buffer = VirtualAllocEx(process.Handle, IntPtr.Zero, 300, MEM_COMMIT, PAGE_READWRITE);

                // Get the number of icons that's currently on the desktop
                int iconCount = (int)SendMessage(desktopReference, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);

                for (int i = 0; i < iconCount; i++)
                {
                    int f;
                    LVITEM iconlabel = new LVITEM();
                    iconlabel.iSubItem = 0;
                    iconlabel.cchTextMax = 256;
                    iconlabel.mask = LVIF_TEXT;
                    iconlabel.pszText = ipc_buffer;

                    unsafe
                    {
                        LVITEM* bp = &iconlabel;
                        WriteProcessMemory(process.Handle, ipc_iconlabel, (IntPtr)bp, 100, out f);
                    }

                    int iconNameLength = (int)SendMessage(desktopReference, LVM_GETITEMTEXT, (IntPtr)i, ipc_iconlabel);

                    // Copy the contents of the remote allocated array into our own memory space
                    byte[] b2 = new byte[300];
                    unsafe
                    {
                        fixed (byte* bp = b2)
                        {
                            ReadProcessMemory(process.Handle, ipc_buffer, (IntPtr)bp, 300, out f);
                        }
                    }
                    char[] c2 = new char[iconNameLength];
                    for (int j = 0; j < iconNameLength; j++)
                        c2[j] = (char)b2[j];
                    String iconName = new String(c2);

                    foreach (IconInfo ii in iconInfos)
                    {
                        if (ii.name == iconName)
                        {
                            SendMessage(desktopReference, LVM_SETITEMPOSITION, (IntPtr)i, CalculateCoordinates(ii.x, ii.y));
                            break;
                        }
                    }
                }

            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            StoreIconPositions(listviewWindow);
        }

        private void arrangeButton_Click(object sender, EventArgs e)
        {
            arrangeByImage(listviewWindow);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RestoreIconPositions(listviewWindow);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            DialogResult res = OpenFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
                txtFileName.Text = OpenFileDialog1.FileName;

        }

        Graph grph;
        private void arrangeByImage(IntPtr listviewWindow)
        {
            try
            {
                Size monitorSize = SystemInformation.PrimaryMonitorSize;

                grph = new Graph();
                generatePointsFromImage();

                // Convert the desktop listview handle to a handle reference so we can later use it in unsafe code
                HandleRef desktopReference = new HandleRef(null, listviewWindow);

                // Get the number of cions that's currently on the desktop
                int iconCount = (int)SendMessage(desktopReference, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
                int err = Marshal.GetLastWin32Error();
                TurnOffSnapToGrid(desktopReference);
                err = Marshal.GetLastWin32Error();
                double step = grph.End / (double)iconCount;

                double v = 0.0;
                for (int i = 0; i < iconCount; i++)
                {
                    Point p = grph.getPoint(v, monitorSize.Width, monitorSize.Height);
                    SendMessage(desktopReference, LVM_SETITEMPOSITION, (IntPtr)i, CalculateCoordinates(p.x, p.y));
                    v += step;
                }
            }
            catch
            {
                if(silent)
                    this.Close();

                MessageBox.Show("Error Arranging Icons");
            }
        }

        private void generatePointsFromImage()
        {
            try
            {
                Bitmap img;
                if (chkEnableDrawMode.Checked)
                    img = pbGraphicsImg;
                else
                    img = (Bitmap)Bitmap.FromFile(txtFileName.Text);

                int width = img.Width;
                int height = img.Height;

                int countOfBlack = 0;

                for (int i = 0; i <= width - 1; i++)
                {
                    for (int j = 0; j <= height - 1; j++)
                    {
                        Color color = img.GetPixel(i, j);
                        if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
                        {
                            countOfBlack += 1;
                        }
                    }
                }


                double steps = countOfBlack / int.Parse(txtNumberOfIcons.Text);

                //fileWriter = new IO.StreamWriter(txtFileName.Text + "-formatted.txt", false);

                Console.WriteLine("Black Pixels: " + countOfBlack.ToString());
                Console.WriteLine("Steps: " + steps.ToString());

                Graphics g = Graphics.FromImage(img);

                int countUpToStep = 0;
                int count = 0;
                for (int i = 0; i <= width - 1; i++)
                {
                    for (int j = 0; j <= height - 1; j++)
                    {
                        Color color = img.GetPixel(i, j);

                        if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
                        {
                            Point p = new Point(i, j);
                            do
                            {
                                p = traceFromPoint(p.x, p.y, ref count, ref countUpToStep, ref steps, ref img, ref g);
                            }
                            while (!(p.x == 0));
                            countUpToStep = 0;
                        }
                    }
                }
                //fileWriter.Close();
                //MessageBox.Show("done");
                //Process.Start("notepad.exe", txtFileName.Text + "-formatted.txt");
            }
            catch
            {
                if (silent)
                    this.Close();

                MessageBox.Show("Error Analyzing Image");
            }
        }

        private Point traceFromPoint(int i, int j, ref int count, ref int countUpToStep, ref double steps, ref Bitmap img, ref Graphics g)
        {

        
            g.FillRectangle(Brushes.Fuchsia, new Rectangle(i, j, 1, 1));
            pbDisplay.Image = img;
            if ((countUpToStep == 0))
            {
                int width = img.Width;
                int height = img.Height;
                double x = (double) i / width;
                double y = (double) j / height;

                grph.addPoint(count, x, y);
                //fileWriter.WriteLine("points.AddLast(new Node(" + count.ToString() + ".0, " + x.ToString("##.##") + ", " + y.ToString("##.##") + "));");
                count += 1;
            }

            if ((countUpToStep >= steps))
            {
                countUpToStep = 0;
            }
            else
            {
                countUpToStep += 1;
            }


            //'Find the next point
            //'Right
            int nextI = 0;
            int nextJ = 0;
            Color color = img.GetPixel(i + 1, j);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i + 1, j, count, countUpToStep, steps, img, g)
                nextI = i + 1;
                nextJ = j;
            }

            //'Left
            color = img.GetPixel(i - 1, j);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i - 1, j, count, countUpToStep, steps, img, g)
                nextI = i - 1;
                nextJ = j;
            }

            //'RightTop
            color = img.GetPixel(i + 1, j - 1);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i + 1, j - 1, count, countUpToStep, steps, img, g)
                nextI = i + 1;
                nextJ = j - 1;
            }

            //'LeftTop
            color = img.GetPixel(i - 1, j - 1);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i - 1, j - 1, count, countUpToStep, steps, img, g)
                nextI = i - 1;
                nextJ = j - 1;
            }

            //'Middle Top
            color = img.GetPixel(i, j - 1);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i, j - 1, count, countUpToStep, steps, img, g)
                nextI = i;
                nextJ = j - 1;
            }


            //'RightBottom
            color = img.GetPixel(i + 1, j + 1);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i + 1, j + 1, count, countUpToStep, steps, img, g)
                nextI = i + 1;
                nextJ = j + 1;
            }

            //'LeftBottom
            color = img.GetPixel(i - 1, j + 1);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i - 1, j + 1, count, countUpToStep, steps, img, g)
                nextI = i - 1;
                nextJ = j + 1;
            }

            //'MiddleBottom
            color = img.GetPixel(i, j + 1);
            if (color == Color.FromArgb(255, 0, 0, 0) | color == Color.Black)
            {
                //traceFromPoint(i, j + 1, count, countUpToStep, steps, img, g)
                nextI = i;
                nextJ = j + 1;
            }


            return new Point(nextI, nextJ);
        }

        Graphics pbGraphics;
        Bitmap pbGraphicsImg;
        private void chkEnableDrawMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableDrawMode.Checked)
            {
                txtFileName.Visible = false;
                cmdBrowse.Visible = false;
                lblFileName.Visible = false;
                cmdClear.Visible = true;
                pbGraphicsImg = new Bitmap(pbDisplay.Width, pbDisplay.Height);
                pbGraphics = Graphics.FromImage(pbGraphicsImg);
                pbDisplay.Image = pbGraphicsImg;
                
            }
            else
            {
                cmdClear.Visible = false;
                txtFileName.Visible = true;
                cmdBrowse.Visible = true;
                lblFileName.Visible = true;
                pbGraphics = null;
            }
        }

        private void pbDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (pbGraphics == null) return;

            if ((e.Button & MouseButtons.Left)  == MouseButtons.Left)
            {
                pbGraphics.FillRectangle(Brushes.Black, e.X, e.Y, 1, 1);
                pbDisplay.Image = pbGraphicsImg;
            }
            //else
            //    Console.WriteLine(e.Button.ToString());
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            pbGraphicsImg = new Bitmap(pbDisplay.Width, pbDisplay.Height);
            pbGraphics = Graphics.FromImage(pbGraphicsImg);
            pbDisplay.Image = pbGraphicsImg;
        }
    }

}

