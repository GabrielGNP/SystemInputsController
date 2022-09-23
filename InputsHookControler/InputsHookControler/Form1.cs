using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



//nesesario para el drag and drop de la ventana del programa al no tener el marco predeterminado
using System.Runtime.InteropServices;

using Microsoft.Win32; // Para poder configurar el programa y se ejecute al iniciar el sistema

using System.IO; // Necesario para crear y manipular archivos
using System.Runtime.Serialization.Formatters;
using System.Threading;
using FontAwesome.Sharp;
using KeyDetectorSIC;
using ClickDetectorSIC;
using System.Reflection;

namespace InputsHookControler
{
    


    public partial class Form1 : Form
    {

        string ProgramDirectory;
        Color colorAccion = Color.FromArgb(255, 9, 70, 255);

        public struct ClassColor
        {
            public struct BlueOrigin
            {
                public static Color Basic = Color.FromArgb(255, 34, 89, 255);//Define una Key para este color
                public static Color Power = Color.FromArgb(255, 9, 70, 255);//Define una Key para este color
                public static Color Light = Color.FromArgb(255, 34, 117, 255);
            }
            public struct Red
            {
                //public static Color Power = Color.FromArgb(255, 9, 70, 255);//Define una Key para este color
                public static Color Basic = Color.Firebrick;//Define una Key para este color
                public static Color Power = Color.FromArgb(192, 0, 0);//Define una Key para este color
                public static Color Light = Color.FromArgb(255, 255, 70, 70);
            }
            public struct Orange
            {
                public static Color Basic = Color.FromArgb(255, 255, 128, 0);//Define una Key para este color
                public static Color Power = Color.Orange;
                public static Color Dark = Color.DarkOrange;//Define una Key para este color
                public static Color Light = Color.FromArgb(255, 255, 203, 115);
            }
            public struct YellowGreen
            {
                //public static Color Power = Color.FromArgb(255, 9, 70, 255);//Define una Key para este color
                public static Color Basic = Color.YellowGreen;
                public static Color Power = Color.LawnGreen;//Define una Key para este color
                public static Color Light = Color.LightGreen;
            }
            public struct Turquoise
            {
                public static Color Power = Color.Turquoise;
                public static Color Basic = Color.LightSeaGreen;
                public static Color Light = Color.Aquamarine;
            }
            public struct CornflowerBlue
            {
                public static Color Power = Color.DeepSkyBlue;
                public static Color Basic = Color.CornflowerBlue; 
                public static Color Light = Color.LightSkyBlue;
            }
            public struct MediumPurple
            {
                public static Color Power = Color.MediumOrchid;//Define una Key para este color
                public static Color Basic = Color.MediumPurple;
                public static Color Light = Color.FromArgb(255, 190, 109, 239); 
            }

            public struct TypeFormBlack
            {
                public static Color T1 = Color.FromArgb(255, 25, 25, 25);//Más oscuro
                public static Color T2 = Color.FromArgb(255, 36, 36, 36);
                public static Color T3 = Color.FromArgb(255, 52, 52, 52);
                public static Color T4 = Color.FromArgb(255, 60, 60, 60);//Menos oscuro
            }

            public struct TypeFormWhite
            {
                public static Color T1 = Color.Gray;//Más oscuro
                public static Color T2 = Color.DarkGray;
                public static Color T3 = Color.FromArgb(224, 224, 224);
                public static Color T4 = Color.White;//Menos oscuro
            }



            public static Color ColorFloralWhite = Color.FloralWhite;
            public static Color ColorBlack = Color.FromArgb(15, 15, 15);
            public static Color ColorLightSlateGray = Color.LightSlateGray;
            public static Color ColorDarkGray = Color.DarkGray;
            /*
            public override string Nombre()
            {
                if (this.Blue)
            }*/
        }

        public class TipsNewConf
        {
            public Color color = Color.Black;
            public string text = "";
            public bool booleano = false;
            public int entero = 0;

            public void asignar(Color colorp)
            {
                this.color = colorp;
            }
            public void asignar(string stringp)
            {
                this.text = stringp;
            }
            public void asignar(bool boolp)
            {
                this.booleano = boolp;
            }
            public void asignar(int enterop)
            {
                this.entero = enterop;
            }
        }
        KeysConverter Key = new KeysConverter();

        ClassKeyDetectorSIC KeyDetect = new ClassKeyDetectorSIC();
        ClassClickDetectorSIC ClickDetec = new ClassClickDetectorSIC();

        public Form1() //  Se ejecuta al iniciar el programa
        {
            
            // Para el autoclicker
            KeyDetect.KeyDown += TeclaPulsadaClick;
            KeyDetect.KeyUp += TeclaSoltadaClick;
            
            // Para los contadores
            //KeyDect.KeyDown += TeclaPulsada;
            KeyDetect.KeyUp += TeclaSoltada;
            ClickDetec.ClickUp += ClickSoltado;
            

            InitializeComponent();
            BGWorkerAutoclicker.CancelAsync();

            //ContPrincipalContadores.Visible = true;
            //ContPrincipalContadores.Dock = DockStyle.Fill;

            /*
            string sourceFileA = @"C:\Users\USUARIO\Desktop\InputsHookControler.exe";
            string destinationFileA = @"C:\Users\USUARIO\Desktop\Evi\InputsHookControler.exe";

            string sourceFileB = @"C:\Users\USUARIO\Desktop\FontAwesome.Sharp.dll";
            string destinationFileB = @"C:\Users\USUARIO\Desktop\Evi\FontAwesome.Sharp.dll";

            // To move a file or folder to a new location:
            System.IO.File.Move(sourceFileA, destinationFileA);
            System.IO.File.Move(sourceFileB, destinationFileB);
            */
           
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //ProgramDirectory = Directory.GetCurrentDirectory(); // Esta manera de obtener el directorio no es muy fiable. En el caso de este programa, cuando se ejecuta al iniciar el sistema, obtiene el directorio en WIN32 porque es desde donde se está ejecutando y eso da problemas
            ProgramDirectory = Application.StartupPath;
                        
            ProgramInit();
        }

        private void ProgramInit()
        {
            
            if (!Directory.Exists(ProgramDirectory + "\\Data"))
            {
                Directory.CreateDirectory(ProgramDirectory + "\\Data");
            }

            if (File.Exists(ProgramDirectory+ "\\Data\\Config.txt"))
            {
                var Configs = CargarConfigs();
                var i = 0;
                /*
                 * 0º linea de config | Color del sistema
                 * 1º linea de config | Paleta de color
                 * 2º linea de config | Color de texto
                 * 3º linea de config | Bold
                 * 4º linea de config | Iniciar con el sistema
                 * 5º linea de config | ContKeyIndividual
                 * 6º linea de config | CantCPSAutoclick
                 * 7º linea de config | KeysClick
                 * 8º linea de config | Activar o mantener autoclick
                 * 9º linea de config | Click izquierdo o derecho autoclick
                */
                foreach (var item in Configs)
                {
                    if (i == 0) //Color del sistema
                    {
                        ColorProgram(item);
                    }
                    if (i == 1) //Paleta de color
                    {
                        switch (item)
                        {
                            case "ColorOrigin":
                                PaletaColorPrograma("ColorOrigin");
                                break;
                            case "ColorRed":
                                PaletaColorPrograma("ColorRed");
                                break;
                            case "ColorOrange":
                                PaletaColorPrograma("ColorOrange");
                                break;
                            case "ColorGreen":
                                PaletaColorPrograma("ColorGreen");
                                break;
                            case "ColorTurquoise":
                                PaletaColorPrograma("ColorTurquoise");
                                break;
                            case "ColorCeleste":
                                PaletaColorPrograma("ColorCeleste");
                                break;
                            case "ColorViolet":
                                PaletaColorPrograma("ColorViolet");
                                break;
                        }

                    }
                    if (i == 2) //Color de texto
                    {
                        if (item == "ColorFloralWhite")
                            ColorTexto(Color.FloralWhite);
                        if (item == "ColorBlack")
                            ColorTexto(ClassColor.ColorBlack);
                        if (item == "ColorLightSlateGray")
                            ColorTexto(ClassColor.ColorLightSlateGray);
                        if (item == "ColorDarkGray")
                            ColorTexto(ClassColor.ColorDarkGray);
                    }
                    if (i == 3) //Bold
                    {
                        if (item == "true")
                        {
                            ChangeBold.Checked = true;
                        }
                        else
                        {
                            ChangeBold.Checked = false;
                        }
                    }
                    if (i == 4) //Iniciar con el sistema
                    {
                        if (item == "true")
                        {
                            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                            if (regkey.GetValueNames().Contains("SystemInputsController"))
                            {
                                regkey.DeleteValue("SystemInputsController");
                                regkey.SetValue("SystemInputsController", ProgramDirectory + "\\System Inputs Controler-SIC.exe");
                            }
                            else
                            {
                                regkey.SetValue("SystemInputsController", ProgramDirectory + "\\System Inputs Controler-SIC.exe");
                            }
                            CheckIniciarEnSistema.Checked = true;
                        }
                        if (item == "false")
                        {
                            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                            if (regkey.GetValueNames().Contains("SystemInputsController"))
                            {
                                regkey.DeleteValue("SystemInputsController");
                            }
                            CheckIniciarEnSistema.Checked = false;
                        }
                    }
                    if (i == 5) //ContKeyIndividual
                    {
                        if (item == "true")
                        {
                            //CheckContKeyIndividual.Checked = true;
                        }
                        if (item == "false")
                        {
                            //CheckContKeyIndividual.Checked = false;
                        }
                    }
                    if (i == 6) //CantCPSAutoclick
                    {
                        CPS = Int32.Parse(item);
                        ClickPerSecond.Text = CPS.ToString();
                    }
                    if (i == 7) //CantCPSAutoclick
                    {
                        teclasplus = item;
                        InfTeclaAsignada.Text = "Tecla Asignada:\n" + teclasplus;
                        if (teclasplus.Contains("LControl"))
                        {
                            LControlPress = true;
                            teclasplus = teclasplus.Replace("LControl", "");

                        }
                        if (teclasplus.Contains("RControl"))
                        {
                            RControlPress = true;
                            teclasplus = teclasplus.Replace("RControl", "");

                        }
                        if (teclasplus.Contains("LShift"))
                        {
                            LShifPress = true;
                            teclasplus = teclasplus.Replace("LShift", "");

                        }
                        if (teclasplus.Contains("RShift"))
                        {
                            RShifPress = true;
                            teclasplus = teclasplus.Replace("RShift", "");

                        }
                        if (teclasplus.Contains("LMenu"))
                        {
                            LMenuPress = true;
                            teclasplus = teclasplus.Replace("LMenu", "");

                        }
                        if (teclasplus.Contains("RMenu"))
                        {
                            RMenuPress = true;
                            teclasplus = teclasplus.Replace("RMenu", "");

                        }
                        teclasplus = teclasplus.Replace("+", "");
                        teclasplus = teclasplus.Replace("Ñ", "Oemtilde");
                        
                            NewKeyPress = (Keys)Key.ConvertFromString(teclasplus);
                    }
                    if (i == 8) //CantCPSAutoclick
                    {
                        if (item == "Activar")
                        {
                            RadioAutoActivar.Checked = true;
                        }
                        if (item == "Mantener")
                        {
                            RadioAutoMantener.Checked = true;
                        }
                        
                    }
                    if (i == 9) //CantCPSAutoclick
                    {
                        if (item == "ClickIzq")
                        {
                            RadioClickIzq.Checked = true;
                        }
                        if (item == "ClickDer")
                        {
                            RadioClickDer.Checked = true;
                        }

                    }
                    i++;
                }

            }
            else
            {
                /*
                 * i=0 linea de config | Color del sistema
                 * i=1 linea de config | Paleta de color
                 * i=2 linea de config | Color de texto
                 * i=3 linea de config | Bold
                 * i=4 linea de config | Iniciar con el sistema
                 * i=5 linea de config | ContKeyIndividual
                 * i=6 linea de config | CantCPSAutoclick
                 * i=7 linea de config | KeysClick
                 * i=8 linea de config | Activar
                 * i=9 linea de config | ClickIzq
                 */
                StreamWriter ArchivoConfigEsc = new StreamWriter(ProgramDirectory + "\\Data\\Config.txt");
               
               // MessageBox.Show("Se a creado un nuevo archivo Config.txt");
                for (int i = 0; i < 10; i++)
                {
                    switch (i)
                    {
                        case 0:
                            ArchivoConfigEsc.WriteLine("AplicationBlack");
                            break;
                        case 1:
                            ArchivoConfigEsc.WriteLine("ColorOrigin");
                            break;
                        case 2:
                            ArchivoConfigEsc.WriteLine("ColorFloralWhite");

                            break;
                        case 3:
                            ArchivoConfigEsc.WriteLine("false"); //Bold

                            break;
                        case 4:
                            ArchivoConfigEsc.WriteLine("true"); //Iniciar con el sistema
                           
                            break;
                        case 5:
                            ArchivoConfigEsc.WriteLine("false"); //ContKeyIndividual

                            break;
                        case 6:
                            ArchivoConfigEsc.WriteLine("10");

                            break;
                        case 7:
                            ArchivoConfigEsc.WriteLine("A + LControl");

                            break;
                        case 8:
                            ArchivoConfigEsc.WriteLine("Activar");

                            break;
                        case 9:
                            ArchivoConfigEsc.WriteLine("ClickIzq");

                            break;
                    }
                    
                }
                ArchivoConfigEsc.Close();
                ColorProgram("AplicationBlack");
                PaletaColorPrograma("ColorOrigin");
                ColorTexto(Color.FloralWhite);
                ChangeBold.Checked = false;
                CheckIniciarEnSistema.Checked = true;
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (regkey.GetValueNames().Contains("SystemInputsController"))
                {
                    regkey.DeleteValue("SystemInputsController");
                    regkey.SetValue("SystemInputsController", ProgramDirectory + "\\System Inputs Controler-SIC.exe");
                }
                else
                {
                    regkey.SetValue("SystemInputsController", ProgramDirectory + "\\System Inputs Controler-SIC.exe");
                }
                ClickPerSecond.Text = "10";
                LControlPress = true;
                NewKeyPress = (Keys)Key.ConvertFromString("A");
                RadioAutoActivar.Checked = true;
                RadioClickIzq.Checked = true;

            }

            if (File.Exists(ProgramDirectory + "\\Data\\RegisterCounters.txt"))
            {
                var Counters = LoadConts(ProgramDirectory + "\\Data\\RegisterCounters.txt");
                if (Counters.Count < 9)
                {
                    System.Console.WriteLine("No se ha podido cargar el archivo RegisterCounters. Se intenta cargar el archivo STRC");
                    Counters = LoadConts(ProgramDirectory + "\\Data\\STRC.txt");
                    System.Console.WriteLine("Se ha cargado el archivo STRC en lugar de RegisterCounters.");
                }
                int i = 0;
                foreach (var Counter in Counters)
                {
                    switch (i)
                    {
                        case 0: //Days
                            Days = Int32.Parse(Counter.Substring(6));
                            break;
                        case 1: //Timer
                            string H = "";
                            string M = "";
                            string S = "";
                            int c = 0;
                            for (int j = 7; j < Counter.Length; j++)
                            {
                                switch (c)
                                {
                                    case 0:
                                        if (!Counter[j].Equals(':'))
                                        {
                                            H = H + Counter[j].ToString();
                                        }
                                        else
                                        {
                                            c = 1;
                                        }
                                        break;
                                    case 1:
                                        if (!Counter[j].Equals(':'))
                                        {
                                            M = M + Counter[j].ToString();
                                        }
                                        else
                                        {
                                            c = 2;
                                        }
                                        break;
                                    case 2:
                                        if (!Counter[j].Equals(':'))
                                        {
                                            S = S + Counter[j].ToString();
                                        }
                                        else
                                        {
                                            c = 3;
                                        }
                                        break;
                                }
                            }
                            hor = Int32.Parse(H);
                            min = Int32.Parse(M);
                            seg = Int32.Parse(S);
                            break;
                        case 2:
                            LeftCLick = Int32.Parse(Counter.Substring(10));
                            break;
                        case 3:
                            RigthClick = Int32.Parse(Counter.ToString().Substring(10));
                            break;
                        case 4:
                            MidleClick = Int32.Parse(Counter.Substring(11));
                            break;
                        case 5:
                            XButton1Click = Int32.Parse(Counter.Substring(9));
                            break;
                        case 6:
                            XBUtton2Click = Int32.Parse(Counter.Substring(9));
                            break;
                        case 7:
                            ClickDetec.RdR = Int32.Parse(Counter.Substring(8));
                            break;
                        case 8:
                            PulsTec = Int32.Parse(Counter.Substring(14));
                            break;
                    }
                    i++;
                }               
            }
            else
            {
                StreamWriter ArchivoRegisterEsc = new StreamWriter(ProgramDirectory + "\\Data\\RegisterCounters.txt");

                //MessageBox.Show("Se a creado un nuevo archivo RegisterCounters.txt");
                for (int i = 0; i < 9; i++)
                {
                    switch (i)
                    {
                        case 0:
                            ArchivoRegisterEsc.WriteLine("Days: 0");
                            break;
                        case 1:
                            ArchivoRegisterEsc.WriteLine("Timer: 00:00:00");
                            break;
                        case 2:
                            ArchivoRegisterEsc.WriteLine("ClickIzq: 0");
                            break;
                        case 3:
                            ArchivoRegisterEsc.WriteLine("ClickDer: 0");
                            break;
                        case 4:
                            ArchivoRegisterEsc.WriteLine("ClickCent: 0");
                            break;
                        case 5:
                            ArchivoRegisterEsc.WriteLine("ClickX1: 0");
                            break;
                        case 6:
                            ArchivoRegisterEsc.WriteLine("ClickX2: 0");
                            break;
                        case 7:
                            ArchivoRegisterEsc.WriteLine("TicsRR: 0");
                            break;
                        case 8:
                            ArchivoRegisterEsc.WriteLine("PulsKeyBoard: 0");
                            break;
                    }
                }
                ArchivoRegisterEsc.Close();
            }

            this.WindowState = FormWindowState.Normal;
        }


        // Funcion que permite que no queden seleccionados los objectos de Form
        private void NoFocus_Click(object sender, EventArgs e)
        {
            BarraSuperior.Focus();
        }


        private void ActualizadorContadores_Tick(object sender, EventArgs e)
        {

            AutoClicksInActivation.Text = CountClick.ToString();

            TotalAutoClicks.Text = CountClickTotal.ToString();

            ContPulsTec.Text = PulsTec.ToString();

            ContClIzq.Text = LeftCLick.ToString();

            ContClDer.Text = RigthClick.ToString();

            ContClCent.Text = MidleClick.ToString();

            ContClEx1.Text = XButton1Click.ToString();

            ContClEx2.Text = XBUtton2Click.ToString();

            RuedaRatonTics = ClickDetec.RdR;
            ContTicRR.Text = RuedaRatonTics.ToString();
        }

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                                     ||//
        //||                        MANEJO DE ARCHIVOS                           ||//
        //||                                                                     ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        private List<string> CargarConfigs()
        {
            /*
             * 0º linea de config | Color del sistema
             * 1º linea de config | Paleta de color
             * 2º linea de config | Color de texto
             * 3º linea de config | Bold
             * 4º linea de config | Iniciar con el sistema
             * 5º linea de config | ContKeyIndividual
             * 6º linea de config | CantCPSAutoclick
             * 7º linea de config | KeysClick
             * 8º linea de config | Activar o mantener autoclick
             * 9º linea de config | Click izquierdo o derecho autoclick
            */

            StreamReader ArchivoConfigLec = new StreamReader(ProgramDirectory + "\\Data\\Config.txt");
            var linea = ArchivoConfigLec.ReadLine();
            List<string> Configs = new List<string>();
            for (int i = 0; linea != null; i++)
            {
                //MessageBox.Show("siclo "+i);
                //MessageBox.Show("linea " + linea);
                Configs.Add(linea);
                //MessageBox.Show("Configs " + Configs[i]);
                linea = ArchivoConfigLec.ReadLine();
            }
            ArchivoConfigLec.Close();
            return Configs;
        }

        private void EscribirConfig(int NumLin, string NewConfig)
        {
            /*
             * 0º linea de config | Color del sistema
             * 1º linea de config | Paleta de color
             * 2º linea de config | Color de texto
             * 3º linea de config | Bold
             * 4º linea de config | Iniciar con el sistema
             * 5º linea de config | ContKeyIndividual
             * 6º linea de config | CantCPSAutoclick
             * 7º linea de config | KeysClick
             * 8º linea de config | Activar o mantener autoclick
             * 9º linea de config | Click izquierdo o derecho autoclick
            */
            var ContConfigs = CargarConfigs();
            //var i = 0;
            StreamWriter ArchivoConfigEsc = new StreamWriter(ProgramDirectory + "\\Data\\Config.txt");
            //foreach (var config in ContConfigs)
            for (int i = 0; i < 10; i++)
            {

                if (i == NumLin)
                {
                    if (i == 0) //Color del sistema
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig);
                    }
                    if (i == 1)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //Paleta de color
                    }
                    if (i == 2)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig);//Color de texto
                    }
                    if (i == 3)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //Bold
                    }
                    if (i == 4)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //Iniciar con el sistema
                    }
                    if (i == 5)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //ContKeyIndividual
                    }
                    if (i == 6)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //CantCPSAutoclick
                    }
                    if (i == 7)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //Keys asignadas
                    }
                    if (i == 8)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //Keys asignadas
                    }
                    if (i == 9)
                    {
                        ArchivoConfigEsc.WriteLine(NewConfig); //Keys asignadas
                    }
                }
                else
                {
                    ArchivoConfigEsc.WriteLine(ContConfigs[i]);
                }
            }

            ArchivoConfigEsc.Close();
        }

        private List<string> LoadConts(string file)
        {
            /*
             * 0º linea de RegisterCOnters | Days
             * 1º linea de RegisterConters | Timer
             * 2º linea de RegisterConters | ClickIzq
             * 3º linea de RegisterConters | ClickDer
             * 4º linea de RegisterConters | ClickCent
             * 5º linea de RegisterConters | ClickX1
             * 6º linea de RegisterConters | ClickX2
             * 7º linea de RegisterConters | TicsRR
             * 8º linea de RegisterConters | PulsKeyBoard
            */

            StreamReader ArchivoConfigLec = new StreamReader(file);
            var linea = ArchivoConfigLec.ReadLine();

            List<string> CountersData = new List<string>();
            CountersData.Clear();
            for (int i = 0; linea != null; i++)
            {
                CountersData.Add(linea);
                linea = ArchivoConfigLec.ReadLine();
            }
            ArchivoConfigLec.Close();

            ArchivoConfigLec = new StreamReader(ProgramDirectory + "\\Data\\RegisterCounters.txt");
            linea = ArchivoConfigLec.ReadLine();
            int CantLine = 0;
            while (linea != null)
            {
                CantLine++;
                linea = ArchivoConfigLec.ReadLine();
            }
            ArchivoConfigLec.Close();
            //Console.WriteLine("CantLine" + CantLine);

            if (CantLine < 9)
            {
                while(CantLine < 9)
                {
                    switch (CantLine)
                    {
                        case 0:
                            CountersData.Add("Days: 0");
                            break;
                        case 1:
                            CountersData.Add("Timer: 00:00:00");
                            break;
                        case 2:
                            CountersData.Add("ClickIzq: 0"); 
                            break;
                        case 3:
                            CountersData.Add("ClickDer: 0"); 
                            break;
                        case 4:
                            CountersData.Add("ClickCent: 0"); 
                            break;
                        case 5:
                            CountersData.Add("ClickX1: 0");
                            break;
                        case 6:
                            CountersData.Add("ClickX2: 0");
                            break;
                        case 7:
                            CountersData.Add("TicsRR: 0"); 
                            break;
                        case 8:
                            CountersData.Add("PulsKeyBoard: 0");
                            break;
                    }
                    CantLine++;

                }
            }

            return CountersData;
        }

        
        private void SaveConts()
        {
            /*
             * 1º linea de RegisterCOnters | Days
             * 2º linea de RegisterConters | Timer
             * 3º linea de RegisterConters | ClickIzq
             * 4º linea de RegisterConters | ClickDer
             * 5º linea de RegisterConters | ClickCent
             * 6º linea de RegisterConters | ClickX1
             * 7º linea de RegisterConters | ClickX2
             * 8º linea de RegisterConters | TicsRR
             * 9º linea de RegisterConters | PulsKeyBoard
           */
            if (savedFiles)
            {
                var CountersData = LoadConts(ProgramDirectory + "\\Data\\RegisterCounters.txt");
                var i = 0;
                StreamWriter ArchivoCounters = new StreamWriter(ProgramDirectory + "\\Data\\TRC.txt");
                foreach (var Counter in CountersData)
                {
                    switch (i)
                    {
                        case 0:
                            ArchivoCounters.WriteLine("Days: " + Days);
                            break;
                        case 1:
                            ArchivoCounters.WriteLine("Timer: " + time);
                            break;
                        case 2:
                            ArchivoCounters.WriteLine("ClickIzq: " + LeftCLick.ToString());
                            break;
                        case 3:
                            ArchivoCounters.WriteLine("ClickDer: " + RigthClick.ToString());
                            break;
                        case 4:
                            ArchivoCounters.WriteLine("ClickCent: " + MidleClick.ToString());
                            break;
                        case 5:
                            ArchivoCounters.WriteLine("ClickX1: " + XButton1Click.ToString());
                            break;
                        case 6:
                            ArchivoCounters.WriteLine("ClickX2: " + XBUtton2Click.ToString());
                            break;
                        case 7:
                            ArchivoCounters.WriteLine("TicsRR: " + RuedaRatonTics.ToString());
                            break;
                        case 8:
                            ArchivoCounters.WriteLine("PulsKeyBoard: " + PulsTec.ToString());
                            break;
                    }
                    i++;
                }

                ArchivoCounters.Close();
            }
            
        }

        bool savedFiles = true;
        private void Saved_Tick(object sender, EventArgs e)
        {
            //Se guarda la información en un nuevo archivo
            savedFiles = false;
            File.Replace(ProgramDirectory + "\\Data\\TRC.txt", ProgramDirectory + "\\Data\\RegisterCounters.txt", ProgramDirectory + "\\Data\\STRC.txt");
            savedFiles = true;
        }


        //_______________________________________________________________






        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                                     ||//
        //||         CÓDIGO USADO PARA EL DRAG AND DROP DEL FORMULARIO           ||//
        //||                                                                     ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void BarraSuperior_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        //_______________________________________________________________



        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                                     ||//
        //||        FUNCIONAMIENTO DE LAS UTILIDADES DE LA BARRA SUPERIOR        ||//
        //||                                                                     ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        private void CloseProgram_Click(object sender, EventArgs e)
        {
            
            SaveConts();
            //this.Close();
            this.Hide();
            
        }
        
        private void MinimizeProgram_Click(object sender, EventArgs e)
        {
           
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void OpenMasterOptions_Click(object sender, EventArgs e)
        {
           
            if (OptionsMaster.Visible)
            {
                OptionsMaster.Visible = false;
            }
            else
            {
                OptionsMaster.Visible = true;
            }
            OptionsMaster.Dock = DockStyle.Fill;
           
        }

        private void MaximizeProgram_Click(object sender, EventArgs e)
        {
           
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            NoFocus_Click(sender, e);


        }
        //_______________________________________________________________




        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                             ||//
        //||        OPCIONES Y FUNCIONAMIENTOS DE MasterOptions          ||//
        //||                                                             ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        private void CambiarColorAplication(object sender, EventArgs e)
        {
            NoFocus_Click(sender, e);
            var btn = (Button)sender;
            ColorProgram(btn.Name.ToString());
        }

        private void CambiarPaletaColor(object sender, EventArgs e)
        {
            
            var btn = (Button)sender;

            PaletaColorPrograma(btn.Name);
            EscribirConfig(1, btn.Name);
            NoFocus_Click(sender, e);
        }

        private void CambiarColorTexto(object sender, EventArgs e)
        {
            NoFocus_Click(sender, e);
            var btn = (Button)sender;
            ColorTexto(btn.BackColor);
            if (btn.BackColor.ToArgb() == ClassColor.ColorFloralWhite.ToArgb())
            {
                EscribirConfig(2, "ColorFloralWhite");
            }
            if (btn.BackColor.ToArgb() == ClassColor.ColorBlack.ToArgb())
            {
                EscribirConfig(2, "ColorBlack");
            }
            if (btn.BackColor.ToArgb() == ClassColor.ColorLightSlateGray.ToArgb())
            {
                EscribirConfig(2, "ColorLightSlateGray");
            }
            if (btn.BackColor.ToArgb() == ClassColor.ColorDarkGray.ToArgb())
            {
                EscribirConfig(2, "ColorDarkGray");
            }
    }

        private void ChangeBold_CheckedChanged(object sender, EventArgs e)
        {
            NoFocus_Click(sender, e);
            var checkBold = (CheckBox)sender;

            if (checkBold.Checked)
            {
                float currentSize;

                OptionsMaster.Font = new Font(this.Font, FontStyle.Bold);
                currentSize = OptionsMaster.Font.Size;
                OptionsMaster.Font = new Font(OptionsMaster.Font.Name, currentSize,
                    OptionsMaster.Font.Style, OptionsMaster.Font.Unit);

                ContainerAutoClick.Font = new Font(this.Font, FontStyle.Bold);
                currentSize = ContainerAutoClick.Font.Size;
                ContainerAutoClick.Font = new Font(ContainerAutoClick.Font.Name, currentSize,
                    ContainerAutoClick.Font.Style, ContainerAutoClick.Font.Unit);

                panelContadores.Font = new Font(this.Font, FontStyle.Bold);
                currentSize = panelContadores.Font.Size;
                panelContadores.Font = new Font(panelContadores.Font.Name, currentSize,
                    panelContadores.Font.Style, panelContadores.Font.Unit);

                
                EscribirConfig(3, "true");
            }
            else
            {
                float currentSize;
                OptionsMaster.Font = new Font(this.Font, FontStyle.Regular);
                currentSize = OptionsMaster.Font.Size;
                OptionsMaster.Font = new Font(OptionsMaster.Font.Name, currentSize,
                    OptionsMaster.Font.Style, OptionsMaster.Font.Unit);

                ContainerAutoClick.Font = new Font(this.Font, FontStyle.Regular);
                currentSize = ContainerAutoClick.Font.Size;
                ContainerAutoClick.Font = new Font(ContainerAutoClick.Font.Name, currentSize,
                    ContainerAutoClick.Font.Style, ContainerAutoClick.Font.Unit);

                panelContadores.Font = new Font(this.Font, FontStyle.Regular);
                currentSize = panelContadores.Font.Size;
                panelContadores.Font = new Font(panelContadores.Font.Name, currentSize,
                    panelContadores.Font.Style, panelContadores.Font.Unit);



                // Form1.ActiveForm.Font = new Font(Form1.ActiveForm.Font.Name, currentSize, Form1.ActiveForm.Font.Style);
                EscribirConfig(3, "false");
            }
        }

        //Funcion para configurar el programa y se inicie al iniciar windows
        private void InitOrNotInitInSistem(object sender, EventArgs e)
        {
            NoFocus_Click(sender, e);
            /* 
            * Va a configurar que este programa se inicie o no lo haga junto con el sistema operativo
            * Si el CheckBox que tenga esta funcion asignada cambia su estado de checked a unchecked y viseversa, ejecutará esta funcion
            * Se evaluará si el estado checked del checkBox está en true o false
           */


            var CheckInitSistem = (CheckBox)sender;

            /* 
             * Accede a la direccion del sistema donde se guardará la configuración para que el programa se inicie automaticamente
             * Esta direccion se la guarda en regkey
            */
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            // Se comprueba si El checkBox está checked o no
            if (CheckInitSistem.Checked)
            { // Si Checked es true
                /* Se creará una nueva key con: 
                 * 1-"un nombre al cual se hará referencia", 
                 * 2-"El link o direccion del ejecutable del programa a iniciar"
                 * La direccion del ejecutable debe estar completa y debe tener tambien el nombre del ejecutable junto con la extención
                 * Esto se ará una sola vez 
                */
                if (regkey.GetValueNames().Contains("SystemInputsController"))
                {
                    regkey.DeleteValue("SystemInputsController");
                    regkey.SetValue("SystemInputsController", ProgramDirectory + "\\System Inputs Controler-SIC.exe");
                    EscribirConfig(4, "true");
                }
                else
                {
                    regkey.SetValue("SystemInputsController", ProgramDirectory + "\\System Inputs Controler-SIC.exe");
                    EscribirConfig(4, "true");
                }
               
            }
            else
            { // Si Checked es false
                /* Elimina el key que coincida con el nombre.
                 * Esto permite que el programa configurado para que se inicie automaticamente con el sistema
                 * deje de estar configurado para iniciarse junto con el sistema. 
                 * Elimina el key.
                */
                if (regkey.GetValueNames().Contains("SystemInputsController"))
                {
                    regkey.DeleteValue("SystemInputsController");
                    EscribirConfig(4, "false");
                }
            }
        }


        //_______________________________________________________________

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                             ||//
        //||                  CAMBIOS EN EL FORMULARIO                   ||//
        //||                                                             ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        private void ColorProgram(string color)
        {
            var ContConfigs = CargarConfigs();
            var i = 0;
            if (color == "AplicationBlack")
            {
                foreach (var config in ContConfigs)
                {
                    i++;
                    if (i == 3)
                    {
                        if (config == "ColorBlack")
                        {
                            ColorTexto(Color.FloralWhite);
                            EscribirConfig(2, "ColorFloralWhite");
                        }
                    }
                }
                
                //Centro Y Bordes
                gradientPanelLeft2.ColorBottom = ClassColor.TypeFormBlack.T1;
                gradientPanelRight2.ColorBottom = ClassColor.TypeFormBlack.T1;
                gradientPanelBottom.ColorBottom = ClassColor.TypeFormBlack.T1;
                ContainerAutoClick.BackColor = ClassColor.TypeFormBlack.T1;
                Separator.BackColor = ClassColor.TypeFormBlack.T1;


                //Panel de opciones
                OptionsMaster.BackColor = ClassColor.TypeFormBlack.T3;

                //Panel Contadores
                ContClIzq.BackColor = ClassColor.TypeFormBlack.T2;
                ContClDer.BackColor = ClassColor.TypeFormBlack.T2;
                ContClCent.BackColor = ClassColor.TypeFormBlack.T2;
                ContClEx1.BackColor = ClassColor.TypeFormBlack.T2;
                ContClEx2.BackColor = ClassColor.TypeFormBlack.T2;
                ContTicRR.BackColor = ClassColor.TypeFormBlack.T2;
                ContPulsTec.BackColor = ClassColor.TypeFormBlack.T2;
                panelContadores.BackColor = ClassColor.TypeFormBlack.T3;

                //Panel autoclicker
                ClickPerSecond.BackColor = ClassColor.TypeFormBlack.T2;
                AutoClicksInActivation.BackColor = ClassColor.TypeFormBlack.T2;
                TotalAutoClicks.BackColor = ClassColor.TypeFormBlack.T2;
                
                
                EscribirConfig(0, "AplicationBlack");
            }
            if (color == "AplicationWhite")
            { 
                foreach (var config in ContConfigs)
                {
                    i++;
                    if (i == 3)
                    {
                        if (config == "ColorFloralWhite")
                        {
                            ColorTexto(Color.Black);
                            EscribirConfig(2, "ColorBlack");
                        }
                    }
                }
                
                //Centro
                ContainerAutoClick.BackColor = ClassColor.TypeFormWhite.T2;
                Separator.BackColor = ClassColor.TypeFormWhite.T2;

                //Panel de opciones
                OptionsMaster.BackColor = ClassColor.TypeFormWhite.T3;

                //Panel Contadores
                ContClIzq.BackColor = ClassColor.TypeFormWhite.T4;
                ContClDer.BackColor = ClassColor.TypeFormWhite.T4;
                ContClCent.BackColor = ClassColor.TypeFormWhite.T4;
                ContClEx1.BackColor = ClassColor.TypeFormWhite.T4;
                ContClEx2.BackColor = ClassColor.TypeFormWhite.T4;
                ContTicRR.BackColor = ClassColor.TypeFormWhite.T4;
                ContPulsTec.BackColor = ClassColor.TypeFormWhite.T4;
                panelContadores.BackColor = ClassColor.TypeFormWhite.T3;

                //Panel autoclicker
                ClickPerSecond.BackColor = ClassColor.TypeFormWhite.T4;
                AutoClicksInActivation.BackColor = ClassColor.TypeFormWhite.T4;
                TotalAutoClicks.BackColor = ClassColor.TypeFormWhite.T4;


                EscribirConfig(0, "AplicationWhite");
            }
            //Para actualizar los colores de los GradientPanels
            //this.WindowState = FormWindowState.Minimized;
            //this.WindowState = FormWindowState.Normal;
            //Update();
        }
        private void PaletaColorPrograma(String TransColor)
        {
            Color color = ClassColor.BlueOrigin.Basic;
            Color ligthColor = ClassColor.BlueOrigin.Power;
            Color shadowColor = ClassColor.BlueOrigin.Light;
            
            switch (TransColor)
            {
                case "ColorOrigin":
                    color = ClassColor.BlueOrigin.Basic;
                    shadowColor = ClassColor.BlueOrigin.Power;
                    ligthColor = ClassColor.BlueOrigin.Light;
                    colorAccion = ClassColor.BlueOrigin.Power;
                   // SICimagen.Image = InputsHookControler.Properties.Resources.SIC;
                    break;
                case "ColorRed":
                    color = ClassColor.Red.Basic;
                    shadowColor = ClassColor.Red.Power;
                    ligthColor = ClassColor.Red.Light;
                    colorAccion = ClassColor.Red.Power;
                   // SICimagen.Image = InputsHookControler.Properties.Resources.SICRojo;
                    break;
                case "ColorOrange":
                    color = ClassColor.Orange.Basic;
                    shadowColor = ClassColor.Orange.Power;
                    ligthColor = ClassColor.Orange.Light;
                    colorAccion = ClassColor.Orange.Power;
                   // SICimagen.Image = InputsHookControler.Properties.Resources.SICAmarillo;
                    break;
                case "ColorGreen":
                    color = ClassColor.YellowGreen.Basic;
                    shadowColor = ClassColor.YellowGreen.Power;
                    ligthColor = ClassColor.YellowGreen.Light;
                    colorAccion = ClassColor.YellowGreen.Power;
                   // SICimagen.Image = InputsHookControler.Properties.Resources.SICVerde;
                    break;
                case "ColorTurquoise":
                    color = ClassColor.Turquoise.Basic;
                    shadowColor = ClassColor.Turquoise.Power;
                    ligthColor = ClassColor.Turquoise.Light;
                    colorAccion = ClassColor.Turquoise.Power;
                   // SICimagen.Image = InputsHookControler.Properties.Resources.SICAquoso;
                    break;
                case "ColorCeleste":
                    color = ClassColor.CornflowerBlue.Basic;
                    shadowColor = ClassColor.CornflowerBlue.Power;
                    ligthColor = ClassColor.CornflowerBlue.Light;
                    colorAccion = ClassColor.CornflowerBlue.Power;
                    //SICimagen.Image = InputsHookControler.Properties.Resources.SICCeleste;
                    break;
                case "ColorViolet":
                    color = ClassColor.MediumPurple.Basic;
                    shadowColor = ClassColor.MediumPurple.Power;
                    ligthColor = ClassColor.MediumPurple.Light;
                    colorAccion = ClassColor.MediumPurple.Power;
                    //SICimagen.Image = InputsHookControler.Properties.Resources.SICVioleta;
                    break;
            }

            // Barra superior
            gradientPanelUP.ColorTop = shadowColor;
            Separador1.ColorTop = shadowColor;
            gradientPanelLeft1.ColorTop = shadowColor;
            gradientPanelLeft2.ColorTop = shadowColor;
            gradientPanelRight2.ColorTop = shadowColor;
            gradientPanelRight1.ColorTop = shadowColor;
            gradientPanelBottom.ColorTop = shadowColor;
            NameProgram.ForeColor = color;

            OpenMasterOptions.FlatAppearance.MouseDownBackColor = color; //ligthColor;
            CloseProgram.FlatAppearance.MouseDownBackColor = color; //ligthColor;
            MaximizeProgram.FlatAppearance.MouseDownBackColor = color; //ligthColor;
            MinimizeProgram.FlatAppearance.MouseDownBackColor = color; //ligthColor;

            TitleAutoClick.ForeColor = color;




            //Panel Opciones
            SubTitulo1OptionsMaster.ForeColor = color;
            SubTitulo2OptionsMaster.ForeColor = color;

            //SeparadorCentro.BackColor = shadowColor;
            //SeparadorIzquierdo.BackColor = shadowColor;
            //BarraSuperiorHerramientas.BackColor = color;
            //SeparadorInfBtn1.BackColor = ligthColor;
            //SeparadorInfBtn2.BackColor = ligthColor;
            //SeparadorInfBtn3.BackColor = ligthColor;

            // Panel Contadores
            DaysActiv.ForeColor = ligthColor;
            TiempoAct.ForeColor = ligthColor;
            TitleContadores.ForeColor = color;
            ContClIzq.ForeColor = /*color;*/ ligthColor;
            ContClDer.ForeColor = /*color;*/ ligthColor;
            ContClCent.ForeColor = /*color;*/ ligthColor;
            ContClEx1.ForeColor = /*color;*/ ligthColor;
            ContClEx2.ForeColor = /*color;*/ ligthColor;
            ContTicRR.ForeColor = /*color;*/ ligthColor;
            ContPulsTec.ForeColor = /*color;*/ ligthColor;

            // Panel autoclick
            ClickPerSecond.ForeColor = /*color;*/ ligthColor;
            AutoClicksInActivation.ForeColor = /*color;*/ ligthColor;
            TotalAutoClicks.ForeColor = /*color;*/ ligthColor;
            AsignarBtnAutoClick.BackColor = color;
            DownCPS.BackColor = shadowColor;
            DownCPS.FlatAppearance.BorderColor = shadowColor;
            UpCPS.BackColor = ligthColor;
            UpCPS.FlatAppearance.BorderColor = shadowColor;



            //Para actualizar los colores de los GradientPanels
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
        }
        private void ColorTexto(Color color)
        {
            Color newColor = color;
            OptionsMaster.ForeColor = color;
            Form1.ActiveForm.ForeColor = color;
            panelContadores.ForeColor = color;
        }
        //_______________________________________________________________


        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                             ||//
        //||            FUNCIONAMIENTO DE LOS CONTADORES                 ||//
        //||                                                             ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//

        int seg = 0;
        int min = 0;
        int hor = 0;
        int Days = 0;
        string time = "";
        private void Temporizador_Tick(object sender, EventArgs e)
        {
            if (seg < 59)
            {
                seg++;
            }
            else
            {
                seg = 0;
                if (min < 59)
                {
                    min++;
                }
                else
                {
                    min = 0;
                    if (hor < 23)
                    {
                        hor++;
                    }
                    else
                    {
                        hor=0;
                        Days++;
                    }
                }
            }
            if (hor < 10)
            {
                time = "0" + hor.ToString() + ":";
            }
            else
            {
                time = hor.ToString() + ":";
            }
            if (min < 10)
            {
                time = time + "0" + min.ToString() + ":";
            }
            else
            {
                time = time + min.ToString() + ":";
            }
            if (seg < 10)
            {
                time = time + "0" + seg.ToString();
            }
            else
            {
                time = time + seg.ToString();
            }
            TiempoAct.Text = time;
            DaysActiv.Text = Days.ToString();

            SaveConts();
        }

        int LeftCLick = 0;
        int RigthClick = 0;
        int MidleClick = 0;
        int XButton1Click = 0;
        int XBUtton2Click = 0;
        int RuedaRatonTics = 0;
        private void ClickSoltado(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftCLick++;
            }
            if (e.Button == MouseButtons.Right)
            {
                RigthClick++;
            }
            if (e.Button == MouseButtons.Middle)
            {
                MidleClick++;
            }
            if (e.Button == MouseButtons.XButton1)
            {
                XButton1Click++;
                //MessageBox.Show(e.Button.ToString()+" "+ContadorClickXB1.ToString());
                //ClicksExtra1Cont.Text = ContadorClickXB1.ToString();
            }
            if (e.Button == MouseButtons.XButton2)
            {
                XBUtton2Click++;
                //MessageBox.Show(e.Button.ToString() + " " + ContadorClickXB2.ToString());
                //ClicksExtra2Cont.Text = ContadorClickXB2.ToString();
            }
        }

        int PulsTec = 0;

        private void TeclaSoltada(object sender, KeyEventArgs e)
        {
            PulsTec++;
            //MessageBox.Show(e.KeyValue.ToString());
            //MessageBox.Show(e.KeyCode.ToString());
            //MessageBox.Show(e.KeyData.ToString());
            /*
            if (CheckContKeyIndividual.Checked)
            {
                RegistroTotalTecPuls(e);
            }*/


        }



        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                             ||//
        //||            FUNCIONAMIENTO DEL AUTOCLICKER                   ||//
        //||                                                             ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//

        
        bool NewKey = false; //Booleano para controlar la activación de la detección de tecla en el Form1_KeyUp1
        Keys NewKeyPress;

        // Cambia el NewKey al presionar el botón
        // Cambia el NewKey al presionar el botón
        // El NewKey permite controlar cuando se debe asignar una nueva tecla y cuando no
        private void AsignarBtnAutoClick_Click(object sender, EventArgs e)
        {
            NoFocus_Click(sender, e);
            if (NewKey == false)
            {
                NewKey = true;
                InfTeclaAsignada.Text = "*Presione una tecla*";
                // MessageBox.Show("NewKey ahora es true");
                teclasplus = "";
                NewKeyPress = Keys.None;
                LControlPress = false;
                RControlPress = false;
                LShifPress = false;
                RShifPress = false;
                LMenuPress = false;
                RMenuPress = false;
            }
        }

        string teclasplus = "";
        string teclas = "";
        // Asignar tecla al autoclicker
        private void SIC_KeyUp(object sender, KeyEventArgs e)
        {
           
            
            if (NewKey == true)
            {
                NewKeyPress = e.KeyCode;
                System.Diagnostics.Debug.WriteLine(NewKeyPress.ToString());
                NewKey = false;
                teclas = e.KeyData.ToString();
                teclas=teclas.Replace(",", "");
                teclas = teclas.Replace("Control", "");
                teclas = teclas.Replace("Shift", "");
                teclas = teclas.Replace("Alt", "");
                teclas = teclas.Replace("Oemtilde", "Ñ");

                if (GetAsyncKeyState(Keys.LControlKey) < 0)
                {
                    System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.LControlKey));
                    LControlPress = true;
                    teclas = teclas + "+ LControl ";
                }
                if (GetAsyncKeyState(Keys.RControlKey) < 0)
                {
                    System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.RControlKey));
                    RControlPress = true;
                    teclas = teclas + "+ RControl ";
                }
                if (GetAsyncKeyState(Keys.LShiftKey) < 0)
                {
                    System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.LShiftKey));
                    LShifPress = true;
                    teclas = teclas + "+ LShift ";
                }
                if (GetAsyncKeyState(Keys.RShiftKey) < 0)
                {
                    System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.RShiftKey));
                    RShifPress = true;
                    teclas = teclas + "+ RShift ";
                }
                if (GetAsyncKeyState(Keys.LMenu) < 0)
                {
                    System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.LMenu));
                    LMenuPress = true;
                    teclas = teclas + "+ LMenu ";
                }
                if (GetAsyncKeyState(Keys.RMenu) < 0)
                {
                    System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.RMenu));
                    RMenuPress = true;
                    teclas = teclas + "+ RMenu ";
                }



                System.Diagnostics.Debug.WriteLine(teclasplus.Contains("Control"));
                InfTeclaAsignada.Text = "Tecla Asignada:\n" + teclas;
                EscribirConfig(7, teclas);
            }
        }

        // Detecta las teclas pulsadas
        // ¡¡ IMPORTANTE !!
        // Para que esto funcione se necesita que el KeyPreview en el Form1 esté en True


        bool ControlPress = false;
        bool ShifPress = false;
        bool MenuPress = false;

        bool LControlPress = false;
        bool RControlPress = false;
        bool LShifPress = false;
        bool RShifPress = false;
        bool LMenuPress = false;
        bool RMenuPress = false;

        //_____________________________________________________________________
        // Necesario para detectar las convinaciones de teclas fuera del programa
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        // Deteccion de teclas en el autoclicker
        // TeclaPulsadaClick solo aactua cuando la configuración del autoclicker está en mantener
        // sirbe para deneter el autoclicker al soltar
        // La ejecución del autoclicker inicia en la funcion TeclaSoltadaClick
        private void TeclaPulsadaClick(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (RadioAutoMantener.Checked)
            {
                if (NewKey == false)
                {
                    if (LControlPress)
                    {
                        if (RControlPress)
                        {
                            if (LShifPress)
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (LShifPress)
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                && GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (RControlPress)
                        {
                            if (LShifPress)
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                && GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (LShifPress)
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                && GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (RShifPress)
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                && GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMenuPress)
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                && GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                && GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                        else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                        {
                                            if (GetAsyncKeyState(NewKeyPress) < 0
                                                //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                                //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                                //&& GetAsyncKeyState(Keys.LMenu) < 0
                                                //&& GetAsyncKeyState(Keys.RMenu) < 0
                                                )
                                            {
                                                InitAutoClickMantener();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            

        }
        
        private void InitAutoClickMantener()
        {
            if (RadioAutoMantener.Checked)
            {
                if (!BGWorkerAutoclicker.IsBusy)
                {
                    //CountClick = 0;
                    AutoClickStart = true;
                    BGWorkerAutoclicker.RunWorkerAsync();
                    
                    //ActualizadorContadores.Start();
                }
            }
        }

        private void TeclaSoltadaClick(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(NewKeyPress));
            //System.Diagnostics.Debug.WriteLine(GetAsyncKeyState(Keys.LShiftKey));
            //MessageBox.Show(NewKey.ToString());
            if (NewKey == true)
            {

            }
            else
            {
                if (LControlPress)
                {
                    if (RControlPress)
                    {
                        if (LShifPress)
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0 
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0 
                                            && GetAsyncKeyState(Keys.RControlKey) < 0 
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0 
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0 
                                            && GetAsyncKeyState(Keys.LMenu) < 0 
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (LShifPress)
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            && GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (RControlPress)
                    {
                        if (LShifPress)
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            && GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (LShifPress)
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            && GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (RShifPress)
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            && GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (LMenuPress)
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            && GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                                else
                                {
                                    if (RMenuPress) //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress && RMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            && GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                    else //caso: LControlPress && RControlPress && LShifPress && RShifPress && LMenuPress
                                    {
                                        if (GetAsyncKeyState(NewKeyPress) < 0
                                            //&& GetAsyncKeyState(Keys.LControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.RControlKey) < 0
                                            //&& GetAsyncKeyState(Keys.LShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.RShiftKey) < 0
                                            //&& GetAsyncKeyState(Keys.LMenu) < 0
                                            //&& GetAsyncKeyState(Keys.RMenu) < 0
                                            )
                                        {
                                            ActivarAutoCLick();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
          
        }

        private void ActivarAutoCLick()
        {
            if (RadioAutoActivar.Checked)
            {
                if (!BGWorkerAutoclicker.IsBusy)
                {
                    Console.WriteLine("enciende");
                    AutoClickStart = true;
                    BGWorkerAutoclicker.RunWorkerAsync();
                    
                    //ActualizadorContadores.Start();
                }
                else
                {
                    Console.WriteLine("apaga");
                    AutoClickStart = false;
                    BGWorkerAutoclicker.CancelAsync();
                    
                    //ActualizadorContadores.Stop();
                }
            }
            if (RadioAutoMantener.Checked)
            {
                if (BGWorkerAutoclicker.IsBusy)
                {
                    AutoClickStart = false;
                    BGWorkerAutoclicker.CancelAsync();
                    //ActualizadorContadores.Stop();
                }
            }
        }

        //Necesarios para la function mouse_event
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //comandos de mouse_event para levantar y pulsar click 
        private const int LEFTUP = 0x0004;  //Comando levantar click izquierdo
        private const int LEFTDOWN = 0x0002; //Comando pulsar click izquierdo
        private const int RIGHTDOWN = 0x0008; //Comando pulsar click derecho
        private const int RIGHTUP = 0x00010; //Comando levantar click derecho

        int CountClick = 0; //Contador de clicks por activación
        int CountClickTotal = 0; //Contador de clicks totales

        bool AutoClickStart = false; //Es necesario para poder frenar el Worker, si no frena, entonces nunca se va a poder detener la ejecución de dicho worker

        int CPS = 10; // Variable que contiene la cantidad de clicks por segundo que se van a realizar

        // Ejecutador de clicks
        private void BGWorkerAutoclicker_DoWork(object sender, DoWorkEventArgs e)
        {
            CountClick = 0;
            while (AutoClickStart)
            {
                if (RadioClickIzq.Checked)
                {
                    mouse_event(dwFlags: LEFTDOWN, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                    Thread.Sleep(1);
                    mouse_event(dwFlags: LEFTUP, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                    LeftCLick--;

                }
                if (RadioClickDer.Checked)
                {
                    mouse_event(dwFlags: RIGHTDOWN, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                    Thread.Sleep(1);
                    mouse_event(dwFlags: RIGHTUP, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                    RigthClick--;
                }
                CountClick++;
                CountClickTotal++;

                Thread.Sleep(499 / CPS);
            }


            //(1000 / (int.Parse(intervals.ToString()))).ToString();
        }

        private void CambioDeCPS(object sender, EventArgs e)
        {
            NoFocus_Click(sender, e);
            var btn = (Button)sender;
            if (btn.Name == "UpCPS")
            {
                CPS++;
            }
            if (btn.Name == "DownCPS")
            {
                CPS--;
            }
            ClickPerSecond.Text = CPS.ToString();
            EscribirConfig(6, CPS.ToString());
        }

        private void ChangeModoAutoclick(object sender, MouseEventArgs e)
        {
            var objCheck = (RadioButton)sender;
            if (objCheck.Name == "RadioAutoActivar")
            {
                EscribirConfig(8, "Activar");
            }
            if (objCheck.Name == "RadioAutoMantener")
            {
                EscribirConfig(8, "Mantener");
            }
            if (objCheck.Name == "RadioClickIzq")
            {
                EscribirConfig(9, "ClickIzq");
            }
            if (objCheck.Name == "RadioClickDer")
            {
                EscribirConfig(9, "ClickDer");
            }
        }
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //||                                                             ||//
        //||                            OTROS                            ||//
        //||                                                             ||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//


        private new void Paint(object sender, EventArgs e)
        {
            
            var element = (IconButton)sender;
            if (element.Name == "CloseProgram")
            {
                element.IconColor = Color.FromArgb(255, 200, 00, 0);
            }
            else
            {
                element.IconColor = colorAccion;
            }
        }

        private void Cleen(object sender, EventArgs e)
        {
            
            var element = (IconButton)sender;
            element.IconColor = Color.FloralWhite;
        }

  
        private void PrintFont(object sender, MouseEventArgs e)
        {
          
            var element = (IconButton)sender;
            element.IconColor = Color.FloralWhite;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task save = Task.Factory.StartNew(() => SaveConts());
            Task.WaitAll(save);
            this.Close();
        }

       









        //_______________________________________________________________
    }
}



/*
 * Configurar para que el programa se ejecute al iniciar el sistema
 * https://www.codeproject.com/Questions/750418/Adding-Registry-entry-in-SOFTWARE-Microsoft-Window
 * https://docs.microsoft.com/en-us/windows/win32/setupapi/run-and-runonce-registry-keys
 * https://www.youtube.com/watch?v=9ZBDu8Zs40Q

 * Customizar botones (Para botones redondeados)
 * https://rjcodeadvance.com/rounded-button-custom-controls-winform-c/
 
 * Arrastrar formulario y Utilizar FontAwesome
 * https://rjcodeadvance.com/disenar-interfaz-grafico-de-usuario-moderno-con-c-y-windows-form/
 
 * Para mover archivos de una carpeta a otra
 * https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-copy-delete-and-move-files-and-folders
 * 
 * Crear Instalador
 * https://www.youtube.com/watch?v=B2v8U0HtzzA
 * 
 * Uso de listas en C#
 * https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-6.0
 * 
 */