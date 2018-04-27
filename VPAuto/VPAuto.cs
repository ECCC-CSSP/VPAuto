using CSSPModelsDLL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VPAuto
{
    public partial class VPAuto : Form
    {
        #region Variables

        private int CountScenarioRepeat = 0;
        private int OldScenarioID = 0;
        private int CountScenarioDone = 0;
        private int ScenarioID = 0;
        private string ResTxtFileName = @"C:\Plumes\AutoRunVP\Results.txt";
        CultureInfo cultureInfo;
        private const uint GW_CHILD = 5;
        private const uint GW_HWNDNEXT = 2;
        private const uint WM_LBUTTONDOWN = 0x201;
        private const uint WM_LBUTTONUP = 0x202;
        private const uint WM_CHAR = 0x102;
        private const uint WM_KEYDOWN = 0x100;
        private const uint WM_KEYUP = 0x101;
        private const uint WM_SYSKEYDOWN = 0x104;
        private const uint WM_SYSKEYUP = 0x105;
        private const uint VK_SHIFT = 0x10;
        private const uint VK_CONTROL = 0x11;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_CLOSE = 0xF060;
        IntPtr hWndPlumes = IntPtr.Zero;
        IntPtr hWndPlumesToolBar = IntPtr.Zero;
        IntPtr hWndPlumesTab = IntPtr.Zero;
        IntPtr hWndDiffuserTab = IntPtr.Zero;
        IntPtr hWndAmbientTab = IntPtr.Zero;
        IntPtr hWndSpecialSettingTab = IntPtr.Zero;
        IntPtr hWndTextOutputTab = IntPtr.Zero;
        IntPtr hWndGraphicOutputTab = IntPtr.Zero;
        IntPtr hWndDiffuserProjectTextBox = IntPtr.Zero;
        IntPtr hWndDiffuserFlowMixZoneDataGrid = IntPtr.Zero;
        IntPtr hWndDiffuserFlowMixZoneDataGridEdit = IntPtr.Zero;
        IntPtr hWndAmbientTabPanelDataGrid = IntPtr.Zero;
        IntPtr hWndAmbientTabPanelDataGridEdit = IntPtr.Zero;
        IntPtr hWndTextOutputClearButton = IntPtr.Zero;
        IntPtr hWndTextOutputResultTextBox = IntPtr.Zero;
        APIFunc af = new APIFunc() as APIFunc;
        List<WndHandleAndTitle> DesktopChildrenWindowsList;
        List<CloseCaptionAndCommand> DialogToClose;
        private enum DiffuserVariable
        {
            PortDiameter,
            PortElevation,
            VerticalAngle,
            HorizontalAngle,
            NumberOfPorts,
            PortSpacing,
            AcuteMixZone,
            ChronicMixZone,
            PortDepth,
            EffluentFlow,
            EffluentSalinity,
            EffluentTemperature,
            EffluentConcentration,
            FroudeNumber,
            EffluentVelocity
        }
        private enum AmbientVariable
        {
            MeasurementDepth,
            CurrentSpeed,
            CurrentDirection,
            AmbientSalinity,
            AmbientTemperature,
            BackgroundConcentration,
            PollutantDecayRate,
            FarFieldCurrentSpeed,
            FarFieldCurrentDirection,
            FarFieldDiffusionCoefficient
        }
        private float? PortDiameter = null;
        private float? PortElevation = null;
        private float? VerticalAngle = null;
        private float? HorizontalAngle = null;
        private float? NumberOfPorts = null;
        private float? PortSpacing = null;
        private float? AcuteMixZone = null;
        private float? ChronicMixZone = null;
        private float? PortDepth = null;
        private float? EffluentFlow = null;
        private float? EffluentSalinity = null;
        private float? EffluentTemperature = null;
        private float? EffluentConcentration = null;

        private float?[] MeasurementDepth = { null, null, null, null, null, null, null, null };
        private float?[] CurrentSpeed = { null, null, null, null, null, null, null, null };
        private float?[] CurrentDirection = { null, null, null, null, null, null, null, null };
        private float?[] AmbientSalinity = { null, null, null, null, null, null, null, null };
        private float?[] AmbientTemperature = { null, null, null, null, null, null, null, null };
        private float?[] BackgroundConcentration = { null, null, null, null, null, null, null, null };
        private float?[] PollutantDecayRate = { null, null, null, null, null, null, null, null };
        private float?[] FarFieldCurrentSpeed = { null, null, null, null, null, null, null, null };
        private float?[] FarFieldCurrentDirection = { null, null, null, null, null, null, null, null };
        private float?[] FarFieldDiffusionCoefficient = { null, null, null, null, null, null, null, null };

        #endregion Variables

        #region Properties
        public VPAuto _VPAuto { get; set; }
        public TextBox _TextBoxSaveSite { get; set; }
        #endregion Properties

        #region Constructors
        public VPAuto()
        {
            cultureInfo = new CultureInfo("en-CA");

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            InitializeComponent();

            DesktopChildrenWindowsList = new List<WndHandleAndTitle>();
            DialogToClose = new List<CloseCaptionAndCommand>();
            FillDesktopWindowsChildrenList(false);

            _VPAuto = this;
            _TextBoxSaveSite = textBoxSaveSite;
        }
        #endregion Constructors

        #region Events
        private void timerCheckDB_Tick(object sender, EventArgs e)
        {
            timerCheckDB.Enabled = false;
            CheckDB();
            timerCheckDB.Enabled = true;
        }
        private void timerCheckForDialogBoxToClose_Tick(object sender, EventArgs e)
        {
            timerCheckForDialogBoxToClose.Stop();
            timerCheckForDialogBoxToClose.Enabled = false;
            CloseDialogBox();
        }
        private void timerStopExecutionAfterOneSecond_Tick(object sender, EventArgs e)
        {
            timerStopExecutionAfterOneSecond.Stop();
            timerStopExecutionAfterOneSecond.Enabled = false;
            StopVPExecutionOfScenario();
        }
        private void VPAuto_Load(object sender, EventArgs e)
        {
            //StartVP();
            //timerCheckDB.Enabled = true;
        }
        private void butStartVP_Click(object sender, EventArgs e)
        {
            StartVP();
            //timerCheckDB.Enabled = true;
        }
        #endregion Events

        #region Private Functions
        private void AmbientEnterData(AmbientVariable av, string VariableText, int Row)
        {
            SelectAmbientTab();
            if (av == AmbientVariable.MeasurementDepth)
                af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 100, 10);
            else
                af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 20, 10);

            switch (av)
            {
                case AmbientVariable.MeasurementDepth:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 20, 10);
                    break;
                case AmbientVariable.CurrentSpeed:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 100, 10);
                    break;
                case AmbientVariable.CurrentDirection:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 170, 10);
                    break;
                case AmbientVariable.AmbientSalinity:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 250, 10);
                    break;
                case AmbientVariable.AmbientTemperature:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 330, 10);
                    break;
                case AmbientVariable.BackgroundConcentration:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 405, 10);
                    break;
                case AmbientVariable.PollutantDecayRate:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 475, 10);
                    break;
                case AmbientVariable.FarFieldCurrentSpeed:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 550, 10);
                    break;
                case AmbientVariable.FarFieldCurrentDirection:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 630, 10);
                    break;
                case AmbientVariable.FarFieldDiffusionCoefficient:
                    af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 705, 10);
                    break;
                default:
                    MessageBox.Show("Error in AmbientEnterData - Case not found [" + av + "]");
                    return;
            }

            MegaDoEvents();

            for (int i = 1; i < Row; i++)
            {
                af.APISendMessage(hWndAmbientTabPanelDataGrid, WM_KEYDOWN, (int)Keys.Down, 0);
            }

            af.APISendMessage(hWndAmbientTabPanelDataGridEdit, WM_CHAR, (int)Keys.Enter, 0);
            for (int i = 0; i < 10; i++)
            {
                af.APISendMessage(hWndAmbientTabPanelDataGridEdit, WM_CHAR, (int)Keys.Delete, 0);
                af.APISendMessage(hWndAmbientTabPanelDataGridEdit, WM_CHAR, (int)Keys.Back, 0);
            }
            foreach (char k in VariableText)
            {
                af.APISendMessage(hWndAmbientTabPanelDataGridEdit, WM_CHAR, (int)k, 0);
            }
            af.APISendMessage(hWndAmbientTabPanelDataGridEdit, WM_CHAR, (int)Keys.Enter, 0);
        }
        private void AmbientFillValues()
        {
            for (int Row = 1; Row < 9; Row++)
            {
                if (MeasurementDepth[Row - 1] != null)
                {
                    if (MeasurementDepth[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.MeasurementDepth, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.MeasurementDepth, string.Format("{0:F2}", MeasurementDepth[Row - 1]), Row);
                    }
                    if (CurrentSpeed[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.CurrentSpeed, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.CurrentSpeed, string.Format("{0:F6}", CurrentSpeed[Row - 1]), Row);
                    }
                    if (CurrentDirection[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.CurrentDirection, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.CurrentDirection, string.Format("{0:F1}", CurrentDirection[Row - 1]), Row);
                    }
                    if (AmbientSalinity[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.AmbientSalinity, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.AmbientSalinity, string.Format("{0:F1}", AmbientSalinity[Row - 1]), Row);
                    }
                    if (AmbientTemperature[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.AmbientTemperature, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.AmbientTemperature, string.Format("{0:F1}", AmbientTemperature[Row - 1]), Row);
                    }
                    if (BackgroundConcentration[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.BackgroundConcentration, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.BackgroundConcentration, string.Format("{0:F0}", BackgroundConcentration[Row - 1]), Row);
                    }
                    if (PollutantDecayRate[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.PollutantDecayRate, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.PollutantDecayRate, string.Format("{0:F6}", PollutantDecayRate[Row - 1]), Row);
                    }
                    if (FarFieldCurrentSpeed[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.FarFieldCurrentSpeed, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.FarFieldCurrentSpeed, string.Format("{0:F3}", FarFieldCurrentSpeed[Row - 1]), Row);
                    }
                    if (FarFieldCurrentDirection[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.FarFieldCurrentDirection, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.FarFieldCurrentDirection, string.Format("{0:F1}", FarFieldCurrentDirection[Row - 1]), Row);
                    }
                    if (FarFieldDiffusionCoefficient[Row - 1] == null)
                    {
                        AmbientEnterData(AmbientVariable.FarFieldDiffusionCoefficient, "", Row);
                    }
                    else
                    {
                        AmbientEnterData(AmbientVariable.FarFieldDiffusionCoefficient, string.Format("{0:F6}", FarFieldDiffusionCoefficient[Row - 1]), Row);
                    }
                }

            }
        }
        public void CheckDB()
        {
            richTextBoxStatus.Text = "";
            richTextBoxStatus.AppendText("Checking DB at " + DateTime.Now + "\r\n");

            Uri url = new Uri(textBoxFromSite.Text);
            //richTextBoxStatus.AppendText(url + "\r\n");
            WebClient webClient = new WebClient();

            string JsonStr = "";
            try
            {
                JsonStr = webClient.DownloadString(url);
            }
            catch (WebException we)
            {
                richTextBoxStatus.AppendText(we.Message);
                timerCheckDB.Enabled = true;
                return;
            }
            VPFullModel vpScenarioToRun = JsonConvert.DeserializeObject<VPFullModel>(JsonStr);


            if (vpScenarioToRun.VPScenarioID != 0)
            {
                ScenarioID = vpScenarioToRun.VPScenarioID;
                if (OldScenarioID != ScenarioID)
                {
                    OldScenarioID = ScenarioID;
                    CountScenarioRepeat = 0;
                }
                else
                {
                    CountScenarioRepeat += 1;
                }

                if (CountScenarioRepeat > 3)
                {
                    var baseAddress = textBoxSaveSite.Text;

                    VPScenarioIDAndRawResults vpScenarioIDAndRawResults = new VPScenarioIDAndRawResults();
                    vpScenarioIDAndRawResults.VPScenarioID = ScenarioID;
                    vpScenarioIDAndRawResults.RawResults = "Error while trying to run scenario [" + vpScenarioToRun.VPScenarioName + "] ScenarioID [" + vpScenarioToRun.VPScenarioID + "]. Tried 3 times.";

                    string parsedContent = JsonConvert.SerializeObject(vpScenarioIDAndRawResults);

                    webClient = new WebClient();
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=utf-8");

                    string RetFromWebClient = webClient.UploadString(baseAddress, parsedContent);
                    if (!string.IsNullOrEmpty(RetFromWebClient))
                    {
                        timerCheckDB.Enabled = true;
                    }
                    else
                    {
                        richTextBoxStatus.AppendText(RetFromWebClient);
                    }

                    return;
                }
                lblVPScenarioIDValue.Text = ScenarioID.ToString();

                PortDiameter = (float?)vpScenarioToRun.PortDiameter_m;
                lblPortDiameterValue.Text = PortDiameter.ToString();
                PortElevation = (float?)vpScenarioToRun.PortElevation_m;
                lblPortElevationValue.Text = PortElevation.ToString();
                VerticalAngle = (float?)vpScenarioToRun.VerticalAngle_deg;
                lblVerticalAngleValue.Text = VerticalAngle.ToString();
                HorizontalAngle = (float?)vpScenarioToRun.HorizontalAngle_deg;
                lblHorizontalAngleValue.Text = HorizontalAngle.ToString();
                NumberOfPorts = (float?)vpScenarioToRun.NumberOfPorts;
                lblNumberOfPortsValue.Text = NumberOfPorts.ToString();
                PortSpacing = (float?)vpScenarioToRun.PortSpacing_m;
                lblPortSpacingValue.Text = PortSpacing.ToString();
                AcuteMixZone = (float?)vpScenarioToRun.AcuteMixZone_m;
                lblAcuteMixZoneValue.Text = AcuteMixZone.ToString();
                ChronicMixZone = (float?)vpScenarioToRun.ChronicMixZone_m;
                PortDepth = (float?)vpScenarioToRun.PortDepth_m;
                EffluentFlow = (float?)vpScenarioToRun.EffluentFlow_m3_s;
                EffluentSalinity = (float?)vpScenarioToRun.EffluentSalinity_PSU;
                EffluentTemperature = (float?)vpScenarioToRun.EffluentTemperature_C;
                EffluentConcentration = (float?)vpScenarioToRun.EffluentConcentration_MPN_100ml;

                int Row = 0;
                float? OldMeasurmentDepth = (float?)vpScenarioToRun.AmbientList[0].MeasurementDepth_m;
                foreach (VPAmbientModel vpa in vpScenarioToRun.AmbientList)
                {
                    if (vpa.MeasurementDepth_m == null)
                    {
                        MeasurementDepth[Row] = OldMeasurmentDepth;
                    }
                    else
                    {
                        MeasurementDepth[Row] = (float?)vpa.MeasurementDepth_m;
                    }
                    OldMeasurmentDepth = MeasurementDepth[Row];
                    CurrentSpeed[Row] = (float?)vpa.CurrentSpeed_m_s;
                    CurrentDirection[Row] = (float?)vpa.CurrentDirection_deg;
                    AmbientSalinity[Row] = (float?)vpa.AmbientSalinity_PSU;
                    AmbientTemperature[Row] = (float?)vpa.AmbientTemperature_C;
                    BackgroundConcentration[Row] = (float?)vpa.BackgroundConcentration_MPN_100ml;
                    PollutantDecayRate[Row] = (float?)vpa.PollutantDecayRate_per_day;
                    FarFieldCurrentSpeed[Row] = (float?)vpa.FarFieldCurrentSpeed_m_s;
                    FarFieldCurrentDirection[Row] = (float?)vpa.FarFieldCurrentDirection_deg;
                    FarFieldDiffusionCoefficient[Row] = (float?)vpa.FarFieldDiffusionCoefficient;
                    Row += 1;
                }

                DiffuserFillValues();
                AmbientFillValues();
                ExecuteScenario();
                if (CountScenarioDone > 30)
                {
                    timerCheckDB.Enabled = false;
                    CountScenarioDone = 0;
                    processPlumes.CloseMainWindow();
                    processPlumes.Close();
                    for (int i = 0; i < 200; i++)
                    {
                        MegaDoEvents();
                    }
                    StartVP();
                }
            }

        }
        private bool CheckIfVPRunning()
        {
            richTextBoxStatus.AppendText("Checking if Visual Plumes is running.\r\n");

            hWndPlumes = IntPtr.Zero;
            string VPCaption = "  Visual Plumes,   Ver. 1.0;   U.S. Environmental Protection Agency,  ERD-Athens,   ORD,   14 August 2001";

            if (DesktopChildrenWindowsList.Where(h => h.Title == VPCaption).Count() > 1)
            {
                MessageBox.Show("More than one copy of Visual Plumes is running. Only one is allowed.");
                richTextBoxStatus.AppendText("Too many copies of [" + VPCaption + "] are running.\r\n");
                return true;
            }
            WndHandleAndTitle wht = DesktopChildrenWindowsList.Where(h => h.Title == VPCaption).FirstOrDefault();
            if (wht != null)
            {
                hWndPlumes = wht.Handle;
                richTextBoxStatus.AppendText(VPCaption + " is running.\r\n");
                return true;
            }
            else
            {
                richTextBoxStatus.AppendText(VPCaption + " is NOT running.\r\n");
                hWndPlumes = IntPtr.Zero;
                return false;
            }
        }
        private void ClickClearButton()
        {
            af.APISendMouseClick(hWndTextOutputClearButton, 10, 10);
        }
        private void ClickOnVPLoadProject()
        {
            SelectTextOutputTab();
            af.APISetForegroundWindow(hWndPlumes);

            af.APIPostMouseClick(hWndPlumesToolBar, 45, 10);
            MegaDoEvents();
        }
        private void ClickOnVPSaveResults()
        {
            SelectTextOutputTab();
            af.APISetForegroundWindow(hWndPlumes);

            af.APIPostMouseClick(hWndPlumesToolBar, 90, 10);
            MegaDoEvents();
        }
        private bool CloseDialogBox()
        {
            FillDesktopWindowsChildrenList(false);
            MegaDoEvents();

            IntPtr TemphWnd = IntPtr.Zero;
            foreach (CloseCaptionAndCommand dtc in DialogToClose)
            {
                //if (DesktopChildrenWindowsList.Where(h => h.Title == dtc.Caption).Count() > 1)
                //{
                //    MessageBox.Show("More than one copy of " + dtc.Caption + " was found. Automatically closing the window might cause inappropriate action.");
                //    richTextBoxStatus.AppendText("More than one copy of " + dtc.Caption + " was found. Automatically closing the window might cause inappropriate action.\r\n");
                //    return true;
                //}
                foreach (WndHandleAndTitle wht in DesktopChildrenWindowsList.Where(h => h.Title == dtc.Caption))
                {
                    if (wht != null)
                    {
                        TemphWnd = wht.Handle;
                        richTextBoxStatus.AppendText("Trying to automatically close [" + dtc.Caption + "] with the command [" + dtc.Command + "].\r\n");
                        MegaDoEvents();
                        af.APISetForegroundWindow(TemphWnd);
                        int Counting = 0;
                        bool JumpOver = false;
                        while (af.APIGetForegroundWindow() != TemphWnd)
                        {
                            Counting += 1;
                            if (Counting > 2000)
                            {
                                JumpOver = true;
                                break;
                            }
                            MegaDoEvents();
                        }
                        if (!JumpOver)
                        {
                            SendKeys.SendWait(dtc.Command);

                            MegaDoEvents();
                            MegaDoEvents();
                        }
                    }
                }

                return true;
                //WndHandleAndTitle wht = DesktopChildrenWindowsList.Where(h => h.Title == dtc.Caption).FirstOrDefault();
            }
            return false;
        }
        private bool CopyAutoRunVPFiles()
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Plumes\AutoRunVP\");

            if (!di.Exists)
            {
                richTextBoxStatus.AppendText(@"You need to create a subdirectory called [C:\Plumes\AutoRunVP\]");
                return false;
            }

            FileInfo fi = new FileInfo(@"C:\Plumes\AutoRunVP\AutoRun.vpp.db");
            if (!fi.Exists)
            {
                af.APISetForegroundWindow(this.Handle);
                richTextBoxStatus.AppendText(@"File Missing: [C:\Plumes\AutoRunVP\AutoRun.vpp.db]. Please make sure it exist and in the right subdirectory.");
                return false;
            }

            File.Copy(@"C:\Plumes\AutoRunVP\AutoRun.vpp.db", @"C:\Plumes\AutoRun.vpp.db", true);

            fi = new FileInfo(@"C:\Plumes\AutoRunVP\AutoRun.001.db");
            if (!fi.Exists)
            {
                af.APISetForegroundWindow(this.Handle);
                richTextBoxStatus.AppendText(@"File Missing: [C:\Plumes\AutoRunVP\AutoRun.001.db]. Please make sure it exist and in the right subdirectory.");
                return false;
            }

            File.Copy(@"C:\Plumes\AutoRunVP\AutoRun.001.db", @"C:\Plumes\AutoRun.001.db", true);

            fi = new FileInfo(@"C:\Plumes\AutoRunVP\AutoRun.lst");
            if (!fi.Exists)
            {
                af.APISetForegroundWindow(this.Handle);
                richTextBoxStatus.AppendText(@"File Missing: [C:\Plumes\AutoRunVP\AutoRun.lst]. Please make sure it exist and in the right subdirectory.");
                return false;
            }

            File.Copy(@"C:\Plumes\AutoRunVP\AutoRun.lst", @"C:\Plumes\AutoRun.lst", true);

            MegaDoEvents();

            return true;
        }
        private void DiffuserEnterData(DiffuserVariable dv, string VariableText)
        {
            SelectDiffuserTab();
            if (dv == DiffuserVariable.PortDiameter)
                af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 70, 10);
            else
                af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 20, 10);

            switch (dv)
            {
                case DiffuserVariable.PortDiameter:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 20, 10);
                    break;
                case DiffuserVariable.PortElevation:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 125, 10);
                    break;
                case DiffuserVariable.VerticalAngle:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 175, 10);
                    break;
                case DiffuserVariable.HorizontalAngle:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 225, 10);
                    break;
                case DiffuserVariable.NumberOfPorts:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 275, 10);
                    break;
                case DiffuserVariable.PortSpacing:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 330, 10);
                    break;
                case DiffuserVariable.AcuteMixZone:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 535, 10);
                    break;
                case DiffuserVariable.ChronicMixZone:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 585, 10);
                    break;
                case DiffuserVariable.PortDepth:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 635, 10);
                    break;
                case DiffuserVariable.EffluentFlow:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 690, 10);
                    break;
                case DiffuserVariable.EffluentSalinity:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 735, 10);
                    break;
                case DiffuserVariable.EffluentTemperature:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 790, 10);
                    break;
                case DiffuserVariable.EffluentConcentration:
                    af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 835, 10);
                    break;
                default:
                    MessageBox.Show("Error in DiffuserEnterData - Case not found [" + dv + "]");
                    return;
            }

            af.APISendMessage(hWndDiffuserFlowMixZoneDataGridEdit, WM_CHAR, (int)Keys.Enter, 0);
            for (int i = 0; i < 10; i++)
            {
                af.APISendMessage(hWndDiffuserFlowMixZoneDataGridEdit, WM_CHAR, (int)Keys.Delete, 0);
                af.APISendMessage(hWndDiffuserFlowMixZoneDataGridEdit, WM_CHAR, (int)Keys.Back, 0);
            }
            foreach (char k in VariableText)
            {
                af.APISendMessage(hWndDiffuserFlowMixZoneDataGridEdit, WM_CHAR, (int)k, 0);
            }
            af.APISendMessage(hWndDiffuserFlowMixZoneDataGridEdit, WM_CHAR, (int)Keys.Enter, 0);
        }
        private void DiffuserFillValues()
        {
            DiffuserEnterData(DiffuserVariable.PortDiameter, string.Format("{0:F3}", PortDiameter));
            DiffuserEnterData(DiffuserVariable.PortDepth, string.Format("{0:F3}", PortDepth));
            DiffuserEnterData(DiffuserVariable.VerticalAngle, string.Format("{0:F0}", VerticalAngle));
            DiffuserEnterData(DiffuserVariable.HorizontalAngle, string.Format("{0:F0}", HorizontalAngle));
            DiffuserEnterData(DiffuserVariable.NumberOfPorts, string.Format("{0:F0}", NumberOfPorts));
            DiffuserEnterData(DiffuserVariable.PortSpacing, string.Format("{0:F3}", PortSpacing));
            DiffuserEnterData(DiffuserVariable.AcuteMixZone, string.Format("{0:F0}", AcuteMixZone));
            DiffuserEnterData(DiffuserVariable.ChronicMixZone, string.Format("{0:F0}", ChronicMixZone));
            DiffuserEnterData(DiffuserVariable.PortElevation, string.Format("{0:F3}", PortElevation));
            DiffuserEnterData(DiffuserVariable.EffluentFlow, string.Format("{0:F6}", EffluentFlow));
            DiffuserEnterData(DiffuserVariable.EffluentSalinity, string.Format("{0:F1}", EffluentSalinity));
            DiffuserEnterData(DiffuserVariable.EffluentTemperature, string.Format("{0:F1}", EffluentTemperature));
            DiffuserEnterData(DiffuserVariable.EffluentConcentration, string.Format("{0:F0}", EffluentConcentration));
        }
        private void ExecuteScenario()
        {
            CountScenarioDone += 1;
            int CountSearchForSaveDialogBox = 0;
            File.Delete(ResTxtFileName);
            MegaDoEvents();
            MegaDoEvents();
            MegaDoEvents();
            for (int i = 0; i < 3; i++)
            {
                StopVPExecutionOfScenario();
                richTextBoxStatus.AppendText("Doing Click Clear Button ...\r\n");
                ClickClearButton();
                MegaDoEvents();
            }
            richTextBoxStatus.AppendText("Doing Start VP Execute Scenario ...\r\n");
            StartVPExecuteOfScenario();
            MegaDoEvents();
            while (timerStopExecutionAfterOneSecond.Enabled)
            {
                // waiting for the execution to complete
                MegaDoEvents();
            }
            richTextBoxStatus.AppendText("Doing Click On VP Save Results ...\r\n");
            ClickOnVPSaveResults();
            MegaDoEvents();

            FillDesktopWindowsChildrenList(false);
            WndHandleAndTitle wht = DesktopChildrenWindowsList.Where(u => u.Title == "Save Plumes Output").FirstOrDefault();
            while (wht == null)
            {
                MegaDoEvents();
                FillDesktopWindowsChildrenList(false);
                wht = DesktopChildrenWindowsList.Where(u => u.Title == "Save Plumes Output").FirstOrDefault();
                CountSearchForSaveDialogBox += 1;
                if (CountSearchForSaveDialogBox > 100)
                {
                    richTextBoxStatus.AppendText("ERROR - Could not find the [Save Plumes Output] dialog box\r\n");
                    return;
                }
            }

            IntPtr hWndFileNameTextBox = af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(wht.Handle, GW_CHILD)
                , GW_HWNDNEXT)
                , GW_HWNDNEXT)
                , GW_HWNDNEXT)
                , GW_HWNDNEXT)
                , GW_HWNDNEXT)
                , GW_HWNDNEXT)
                , GW_HWNDNEXT);

            richTextBoxStatus.AppendText("hWndFileNameTextBox = [" + hWndFileNameTextBox + "]\r\n");
            if (hWndFileNameTextBox != IntPtr.Zero)
            {
                af.APISetForegroundWindow(wht.Handle);
                MegaDoEvents();
                while (af.APIGetForegroundWindow() != wht.Handle)
                {
                    MegaDoEvents();
                }
                af.APISendMouseClick(hWndFileNameTextBox, 10, 10);
                af.APISendMouseClick(hWndFileNameTextBox, 10, 10);
                for (int i = 0; i < 100; i++)
                {
                    SendKeys.SendWait("{BACKSPACE}{DELETE}");
                }
                SendKeys.SendWait(ResTxtFileName);
            }
            else
            {
                richTextBoxStatus.AppendText("hWndFileNameTextBox != IntPtr.Zero\r\n");
                return;
            }

            IntPtr hWndSaveButton = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(wht.Handle, GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT);
            richTextBoxStatus.AppendText("hWndSaveButton = [" + hWndSaveButton + "]\r\n");

            if (hWndSaveButton != IntPtr.Zero)
            {
                af.APISetForegroundWindow(wht.Handle);
                MegaDoEvents();
                while (af.APIGetForegroundWindow() != wht.Handle)
                {
                    MegaDoEvents();
                }
                af.APIPostMouseClick(hWndSaveButton, 10, 10);
            }
            else
            {
                richTextBoxStatus.AppendText("hWndFileNameTextBox != IntPtr.Zero\r\n");
                return;
            }

            richTextBoxStatus.AppendText("Checking if result file exist ...\r\n");
            while (!File.Exists(ResTxtFileName))
            {
                MegaDoEvents();
            }
            richTextBoxStatus.AppendText("Doing SaveResultsInDB ...\r\n");
            SaveResultsInDB();
            richTextBoxStatus.AppendText("After SaveResultsInDB ...\r\n");


        }
        private void FillDesktopWindowsChildrenList(bool ShowResults)
        {
            IntPtr hWndDesktop = af.APIGetDesktopWindow();
            DesktopChildrenWindowsList.Clear();
            DesktopChildrenWindowsList = af.GetChildrenWindowsHandleAndTitle(hWndDesktop);

            if (ShowResults)
            {
                richTextBoxStatus.Text = "";
                richTextBoxStatus.Text = "DesktopWindow = [" + hWndDesktop + "]\r\n";
                richTextBoxStatus.AppendText("Handle count = [" + DesktopChildrenWindowsList.Count() + "\r\n");

                foreach (WndHandleAndTitle t in DesktopChildrenWindowsList)
                {
                    richTextBoxStatus.AppendText("Handle = [" + t.Handle.ToString("X") + "] Window Title = [" + t.Title + "]\r\n");
                }
            }
        }
        private double GetCorrectedDistance(double DistVal, double TimeVal, double Dist6, double Dist12, double Dist18, double Dist24, double Dist30)
        {
            if (TimeVal >= 6)
            {
                if (TimeVal >= 12)
                {
                    if (TimeVal >= 18)
                    {
                        if (TimeVal >= 24)
                        {
                            if (TimeVal >= 30)
                            {
                                return DistVal - Dist24 + Dist18 - Dist12 + Dist6;
                            }
                            else
                            {
                                return Dist6 + Dist18 - Dist12;
                            }
                        }
                        else
                        {
                            return Dist6 + Dist18 - Dist12;
                        }
                    }
                    else
                    {
                        return DistVal - Dist12 + Dist6;
                    }
                }
                else
                {
                    return Dist6;
                }
            }
            else
            {
                return DistVal;
            }
        }
        public int GetID(string Path)
        {
            int RetVal = -1; // will return -1 if an error occured or if there are no parent i.e. tvItem is root
            if (!string.IsNullOrWhiteSpace(Path))
            {
                if (Path.Contains(@"p"))
                {
                    RetVal = int.Parse(Path.Substring(Path.LastIndexOf(@"p") + 1));
                }
                else
                {
                    RetVal = int.Parse(Path);
                }
            }

            return RetVal;
        }
        private string GetLastValue(string[] TheValue, int Row)
        {
            for (int i = Row; i > 0; i--)
            {
                if (TheValue[i - 1] != "")
                {
                    return TheValue[i - 1];
                }
            }
            return "";
        }
        private void MegaDoEvents()
        {
            for (int i = 0; i < 20000; i++)
            {
                Application.DoEvents();
            }
        }
        private bool LoadAutoRunVPFiles()
        {
            int CountSearchForSaveDialogBox = 0;
            ClickOnVPLoadProject();
            MegaDoEvents();

            FillDesktopWindowsChildrenList(false);
            WndHandleAndTitle wht = DesktopChildrenWindowsList.Where(u => u.Title == "Project File").FirstOrDefault();
            while (wht == null)
            {
                MegaDoEvents();
                FillDesktopWindowsChildrenList(false);
                wht = DesktopChildrenWindowsList.Where(u => u.Title == "Project File").FirstOrDefault();
                CountSearchForSaveDialogBox += 1;
                if (CountSearchForSaveDialogBox > 100)
                {
                    richTextBoxStatus.AppendText("ERROR - Could not find the [Project File] dialog box\r\n");
                    return false;
                }
            }

            IntPtr hWndFileNameTextBox = af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(wht.Handle, GW_CHILD),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT);

            richTextBoxStatus.AppendText("hWndFileNameTextBox = [" + hWndFileNameTextBox + "]\r\n");
            if (hWndFileNameTextBox != IntPtr.Zero)
            {
                af.APISetForegroundWindow(wht.Handle);
                MegaDoEvents();
                while (af.APIGetForegroundWindow() != wht.Handle)
                {
                    MegaDoEvents();
                }
                af.APISendMouseClick(hWndFileNameTextBox, 10, 10);
                af.APISendMouseClick(hWndFileNameTextBox, 10, 10);
                for (int i = 0; i < 100; i++)
                {
                    SendKeys.SendWait("{BACKSPACE}{DELETE}");
                }
                SendKeys.SendWait(@"C:\Plumes\AutoRun.vpp.db");
            }
            else
            {
                richTextBoxStatus.AppendText("hWndFileNameTextBox != IntPtr.Zero\r\n");
                return false;
            }

            IntPtr hWndOpenButton = af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(
                af.APIGetWindow(wht.Handle, GW_CHILD),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT),
                GW_HWNDNEXT);
            richTextBoxStatus.AppendText("hWndOpenButton = [" + hWndOpenButton + "]\r\n");

            if (hWndOpenButton != IntPtr.Zero)
            {
                af.APISetForegroundWindow(wht.Handle);
                MegaDoEvents();
                while (af.APIGetForegroundWindow() != wht.Handle)
                {
                    MegaDoEvents();
                }
                af.APIPostMouseClick(hWndOpenButton, 10, 10);
            }
            else
            {
                richTextBoxStatus.AppendText("hWndFileNameTextBox != IntPtr.Zero\r\n");
                return false;
            }

            return true;
        }
        private void LoadVP()
        {
            RunVisualPlumes();
            MegaDoEvents();

            FillDesktopWindowsChildrenList(false);

            WndHandleAndTitle wht = DesktopChildrenWindowsList.Where(u => u.Title == "Information").FirstOrDefault();
            while (wht != null)
            {
                SetTimerToCloseDialogBox("Information", "{ENTER}", 1000, 3);
                MegaDoEvents();
                FillDesktopWindowsChildrenList(false);
                wht = DesktopChildrenWindowsList.Where(u => u.Title == "Information").FirstOrDefault();
            }
        }
        private bool RunVisualPlumes()
        {
            try
            {
                string VisualPlumesExecutablePath = @"C:\Plumes\Plumes.exe";
                richTextBoxStatus.AppendText("Starting Visual Plumes.\r\n");
                richTextBoxStatus.AppendText("Trying to run [" + VisualPlumesExecutablePath + "].\r\n");

                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = VisualPlumesExecutablePath;
                pInfo.WindowStyle = ProcessWindowStyle.Normal;
                pInfo.UseShellExecute = true;
                processPlumes.StartInfo = pInfo;
                processPlumes.Start();
                processPlumes.WaitForInputIdle(2000);

                richTextBoxStatus.AppendText("Process for Visual Plumes was started.\r\n");
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Error [" + ex.Message + "]";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                richTextBoxStatus.AppendText(ErrorMessage);
                return false;
            }

            return true;
        }
        private void SaveResultsInDB()
        {
            richTextBoxStatus.AppendText("LoadFile " + ResTxtFileName + " ...\r\n");
            FileInfo fi = new FileInfo(ResTxtFileName);
            StreamReader sr = fi.OpenText();

            StringBuilder sb = new StringBuilder(sr.ReadToEnd());
            sr.Close();

            if (sb.Length != 0)
            {
                var baseAddress = textBoxSaveSite.Text;

                VPScenarioIDAndRawResults vpScenarioIDAndRawResults = new VPScenarioIDAndRawResults();
                vpScenarioIDAndRawResults.VPScenarioID = ScenarioID;
                vpScenarioIDAndRawResults.RawResults = sb.ToString();

                string parsedContent = JsonConvert.SerializeObject(vpScenarioIDAndRawResults);

                WebClient webClient = new WebClient();
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=utf-8");

                string RetFromWebClient = webClient.UploadString(baseAddress, parsedContent);
                if (RetFromWebClient != "\"\"")
                {
                    richTextBoxStatus.AppendText(RetFromWebClient + "\r\n");
                    timerCheckDB.Enabled = true;
                    return;
                }

                FillDesktopWindowsChildrenList(false);
                int tryCount = 0;
                if (DesktopChildrenWindowsList.Where(u => u.Title == "Plumes").Count() > 1)
                {
                    WndHandleAndTitle wht = DesktopChildrenWindowsList.Where(u => u.Title == "Plumes").FirstOrDefault();
                    while (wht != null)
                    {
                        SetTimerToCloseDialogBox("Plumes", "{ENTER}", 1000, 3);
                        MegaDoEvents();
                        FillDesktopWindowsChildrenList(false);
                        if (DesktopChildrenWindowsList.Where(u => u.Title == "Plumes").Count() > 1)
                        {
                            wht = DesktopChildrenWindowsList.Where(u => u.Title == "Plumes").FirstOrDefault();
                        }

                        tryCount += 1;
                        if (tryCount > 2)
                        {
                            wht = null;
                        }
                    }

                    processPlumes.CloseMainWindow();
                    processPlumes.Close();
                    for (int i = 0; i < 200; i++)
                    {
                        MegaDoEvents();
                    }
                    StartVP();
                }
            }
            else
            {
                richTextBoxStatus.AppendText("Result file is empty");
                return;
            }

        }
        private void SelectAmbientTab()
        {
            af.APISendMouseClick(hWndPlumesTab, 230, 3);
            MegaDoEvents();
        }
        private void SelectDiffuserTab()
        {
            af.APISendMouseClick(hWndPlumesTab, 33, 3);
            MegaDoEvents();
        }
        private void SelectGraphicOutputTab()
        {
            af.APISendMouseClick(hWndPlumesTab, 533, 3);
            MegaDoEvents();
        }
        private void SelectTextOutputTab()
        {
            af.APISendMouseClick(hWndPlumesTab, 443, 3);
            MegaDoEvents();
        }
        private bool SetHandles()
        {
            if (!SethWndPlumes())
                return false;
            if (!SethWndPlumesToolBar())
                return false;
            if (!SethWndPlumesTab())
                return false;
            if (!SethWndAmbientTabPanelDataGrid())
                return false;
            if (!SethWndAmbientTabPanelDataGridEdit())
                return false;
            if (!SethWndDiffuserFlowMixZoneDataGrid())
                return false;
            if (!SethWndDiffuserFlowMixZoneDataGridEdit())
                return false;
            if (!SethWndDiffuserProjectTextBox())
                return false;
            if (!SethWndTextOutputClearButton())
                return false;
            if (!SethWndTextOutputResultTextBox())
                return false;

            SelectDiffuserTab();
            return true;
        }
        private bool SethWndDiffuserFlowMixZoneDataGrid()
        {
            SelectDiffuserTab();
            MegaDoEvents();
            hWndDiffuserFlowMixZoneDataGrid = IntPtr.Zero;

            hWndDiffuserFlowMixZoneDataGrid = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT);

            if (hWndDiffuserFlowMixZoneDataGrid == IntPtr.Zero)
            {
                SelectDiffuserTab();
                MegaDoEvents();
                hWndDiffuserFlowMixZoneDataGrid = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT);

                if (hWndDiffuserFlowMixZoneDataGrid == IntPtr.Zero)
                {
                    string ErrorMessage = "hWndDiffuserFlowMixZoneDataGrid not found";
                    richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                    return false;
                }
            }

            return true;
        }
        private bool SethWndDiffuserFlowMixZoneDataGridEdit()
        {
            SelectDiffuserTab();
            MegaDoEvents();
            hWndDiffuserFlowMixZoneDataGridEdit = IntPtr.Zero;
            hWndDiffuserFlowMixZoneDataGridEdit = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_CHILD);

            if (hWndDiffuserFlowMixZoneDataGridEdit == IntPtr.Zero)
            {
                // we need to try to set focus to the DataGrid so the Edit portion becomes available
                SelectDiffuserTab();
                af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 20, 10);
                af.APISendMouseClick(hWndDiffuserFlowMixZoneDataGrid, 20, 10);
                af.APISendMessage(hWndDiffuserFlowMixZoneDataGridEdit, WM_CHAR, (int)Keys.Enter, 0);

                MegaDoEvents();

                hWndDiffuserFlowMixZoneDataGridEdit = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_CHILD);

                if (hWndDiffuserFlowMixZoneDataGridEdit == IntPtr.Zero)
                {
                    string ErrorMessage = "hWndDiffuserFlowMixZoneDataGridEdit not found";
                    richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                    return false;
                }
            }

            return true;
        }
        private bool SethWndDiffuserProjectTextBox()
        {
            SelectDiffuserTab();

            hWndDiffuserProjectTextBox = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT);

            if (hWndDiffuserProjectTextBox == IntPtr.Zero)
            {
                string ErrorMessage = "hWndDiffuserProjectTextBox not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            return true;
        }
        private bool SethWndAmbientTabPanelDataGrid()
        {
            SelectAmbientTab();
            MegaDoEvents();
            IntPtr hWndambpanel = af.APIFindWindowEx(hWndAmbientTab, IntPtr.Zero, "TPanel", "ambpanel");
            if (hWndambpanel == IntPtr.Zero)
            {
                string ErrorMessage = "hWndambpanel not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }
            IntPtr hWndTPanel1 = af.APIGetWindow(hWndambpanel, GW_CHILD);
            if (hWndTPanel1 == IntPtr.Zero)
            {
                string ErrorMessage = "hWndTPanel1 not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }
            IntPtr hWndTPanel2 = af.APIGetWindow(af.APIGetWindow(hWndambpanel, GW_CHILD), GW_HWNDNEXT);
            if (hWndTPanel2 == IntPtr.Zero)
            {
                string ErrorMessage = "hWndTPanel2 not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            hWndAmbientTabPanelDataGrid = IntPtr.Zero;
            MegaDoEvents();

            hWndAmbientTabPanelDataGrid = af.APIFindWindowEx(hWndTPanel1, IntPtr.Zero, "TDBGrid", "");
            //af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_CHILD), GW_CHILD);

            if (hWndAmbientTabPanelDataGrid == IntPtr.Zero)
            {
                MegaDoEvents();
                hWndAmbientTabPanelDataGrid = af.APIFindWindowEx(hWndTPanel2, IntPtr.Zero, "TDBGrid", "");

                if (hWndAmbientTabPanelDataGrid == IntPtr.Zero)
                {
                    string ErrorMessage = "hWndAmbientTabPanelDataGrid not found";
                    richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                    return false;
                }
            }

            return true;
        }
        private bool SethWndAmbientTabPanelDataGridEdit()
        {
            SelectAmbientTab();
            MegaDoEvents();
            hWndAmbientTabPanelDataGridEdit = IntPtr.Zero;

            hWndAmbientTabPanelDataGridEdit = af.APIGetWindow(hWndAmbientTabPanelDataGrid, GW_CHILD);
            //af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_CHILD), GW_CHILD), GW_CHILD);

            if (hWndAmbientTabPanelDataGridEdit == IntPtr.Zero)
            {
                SelectAmbientTab();
                MegaDoEvents();
                af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 20, 10);
                af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 20, 10);
                //af.APISendMouseClick(hWndAmbientTabPanelDataGrid, 20, 10);
                //af.APISendMessage(hWndAmbientTabPanelDataGridEdit, WM_CHAR, (int)Keys.Enter, 0);

                MegaDoEvents();

                hWndAmbientTabPanelDataGridEdit = af.APIGetWindow(hWndAmbientTabPanelDataGrid, GW_CHILD);

                if (hWndAmbientTabPanelDataGridEdit == IntPtr.Zero)
                {
                    string ErrorMessage = "hWndAmbientTabPanelDataGridEdit not found";
                    richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                    return false;
                }

            }

            return true;
        }
        private bool SethWndPlumes()
        {
            hWndPlumes = IntPtr.Zero;
            hWndPlumes = af.APIFindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainform", "  Visual Plumes,   Ver. 1.0;   U.S. Environmental Protection Agency,  ERD-Athens,   ORD,   14 August 2001");

            if (hWndPlumes == IntPtr.Zero)
            {
                if (CheckIfVPRunning())
                {
                    // try again.
                    hWndPlumes = af.APIFindWindowEx(IntPtr.Zero, IntPtr.Zero, "TMainform", "  Visual Plumes,   Ver. 1.0;   U.S. Environmental Protection Agency,  ERD-Athens,   ORD,   14 August 2001");

                    if (hWndPlumes == IntPtr.Zero)
                    {
                        string ErrorMessage = "hWndPlumes not found";
                        richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                        return false;
                    }
                }
            }

            return true;
        }
        private bool SethWndPlumesTab()
        {
            MegaDoEvents();
            hWndPlumesTab = IntPtr.Zero;
            hWndPlumesTab = af.APIFindWindowEx(hWndPlumes, IntPtr.Zero, "TPageControl", "");
            //af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT);

            if (hWndPlumesTab == IntPtr.Zero)
            {
                if (CheckIfVPRunning())
                {
                    MegaDoEvents();
                    hWndPlumesTab = af.APIFindWindowEx(hWndPlumes, IntPtr.Zero, "TPageControl", "");
                    //                    hWndPlumesTab = af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT);

                    if (hWndPlumesTab == IntPtr.Zero)
                    {
                        string ErrorMessage = "hWndPlumesTab not found";
                        richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                        return false;
                    }
                }
            }

            for (int i = 2; i < 500; i += 20)
            {
                af.APISetForegroundWindow(hWndPlumes);
                while (af.APIGetForegroundWindow() != hWndPlumes)
                {
                    MegaDoEvents();
                }
                af.APISendMouseClick(hWndPlumesTab, i, 2);
                af.APISendMouseClick(hWndPlumesTab, i, 2);
                MegaDoEvents();
            }

            hWndTextOutputTab = af.APIFindWindowEx(hWndPlumesTab, IntPtr.Zero, "TTabSheet", "Text Output");
            //af.APIGetWindow(hWndPlumesTab, GW_CHILD);
            if (hWndTextOutputTab == IntPtr.Zero)
            {
                string ErrorMessage = "hWndTextOutputTab not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            af.APISetForegroundWindow(hWndPlumes);
            SelectTextOutputTab();

            hWndDiffuserTab = af.APIFindWindowEx(hWndPlumesTab, IntPtr.Zero, "TTabSheet", @"Diffuser: AutoRun.vpp.db");
            //af.APIGetWindow(af.APIGetWindow(hWndPlumesTab, GW_CHILD), GW_HWNDNEXT);
            if (hWndDiffuserTab == IntPtr.Zero)
            {
                string ErrorMessage = "hWndDiffuserTab not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            af.APISetForegroundWindow(hWndPlumes);
            SelectDiffuserTab();

            hWndAmbientTab = af.APIFindWindowEx(hWndPlumesTab, IntPtr.Zero, "TTabSheet", @"Ambient: C:\Plumes\AutoRun.001.db");
            //af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumesTab, GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT);
            if (hWndAmbientTab == IntPtr.Zero)
            {
                string ErrorMessage = "hWndAmbientTab not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            af.APISetForegroundWindow(hWndPlumes);
            SelectAmbientTab();

            hWndGraphicOutputTab = af.APIFindWindowEx(hWndPlumesTab, IntPtr.Zero, "TTabSheet", "Graphical Output");
            //af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumesTab, GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT);
            if (hWndGraphicOutputTab == IntPtr.Zero)
            {
                string ErrorMessage = "hWndGraphicOutputTab not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            af.APISetForegroundWindow(hWndPlumes);
            SelectGraphicOutputTab();

            hWndSpecialSettingTab = af.APIFindWindowEx(hWndPlumesTab, IntPtr.Zero, "TTabSheet", "Special Settings");
            //af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumesTab, GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT), GW_HWNDNEXT);
            if (hWndSpecialSettingTab == IntPtr.Zero)
            {
                string ErrorMessage = "hWndSpecialSettingTab not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            af.APISetForegroundWindow(hWndPlumes);
            return true;
        }
        private bool SethWndPlumesToolBar()
        {
            MegaDoEvents();
            hWndPlumesToolBar = IntPtr.Zero;
            hWndPlumesToolBar = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT);

            if (hWndPlumesToolBar == IntPtr.Zero)
            {
                if (CheckIfVPRunning())
                {
                    MegaDoEvents();
                    hWndPlumesToolBar = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT);

                    if (hWndPlumesToolBar == IntPtr.Zero)
                    {
                        string ErrorMessage = "hWndToolBar not found";
                        richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                        return false;
                    }
                }
            }

            return true;
        }
        private bool SethWndTextOutputClearButton()
        {
            SelectTextOutputTab();
            MegaDoEvents();

            hWndTextOutputClearButton = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD);

            if (hWndTextOutputClearButton == IntPtr.Zero)
            {
                string ErrorMessage = "hWndTextOutputClearButton not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            return true;
        }
        private bool SethWndTextOutputResultTextBox()
        {
            SelectTextOutputTab();

            hWndTextOutputResultTextBox = af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(af.APIGetWindow(hWndPlumes, GW_CHILD), GW_HWNDNEXT), GW_CHILD), GW_CHILD), GW_HWNDNEXT), GW_HWNDNEXT), GW_CHILD);

            if (hWndTextOutputResultTextBox == IntPtr.Zero)
            {
                string ErrorMessage = "hWndTextOutputResultTextBox not found";
                richTextBoxStatus.AppendText(ErrorMessage + "\r\n");
                return false;
            }

            return true;
        }
        private void SetTimerToCloseDialogBox(string Caption, string Command, int Interval, int NumberOfTry)
        {
            DialogToClose.Clear();
            CloseCaptionAndCommand ccc = new CloseCaptionAndCommand()
            {
                Caption = Caption,
                Command = Command
            };

            DialogToClose.Add(ccc);
            MegaDoEvents();
            timerCheckForDialogBoxToClose.Enabled = true;
            timerCheckForDialogBoxToClose.Interval = Interval;
            timerCheckForDialogBoxToClose.Start();
            return;
        }
        private void StartVP()
        {
            int CountLoop = 0;
            if (!CopyAutoRunVPFiles())
            {
                String TempErr = "Error while copying Visual Plumes project files.";
                richTextBoxStatus.AppendText(TempErr + "\r\n");
                return;
            }

            FillDesktopWindowsChildrenList(false);
            if (CheckIfVPRunning())
            {
                String TempErr = "Please close the current Visual Plumes that is running.";
                richTextBoxStatus.AppendText(TempErr + "\r\n");
                MessageBox.Show(TempErr);
                return;
            }

            LoadVP();
            for (int i = 0; i < 100; i++)
            {
                MegaDoEvents();
            }

            FillDesktopWindowsChildrenList(false);
            CountLoop = 0;
            if (!CheckIfVPRunning())
            {
                MegaDoEvents();
                FillDesktopWindowsChildrenList(false);
                CountLoop += 1;
                if (CountLoop > 100)
                {
                    MessageBox.Show("ERROR - Visual Plumes could not be started by the application");
                    return;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                MegaDoEvents();
            }

            if (!SethWndPlumesToolBar())
                return;

            if (!LoadAutoRunVPFiles())
                return;

            for (int i = 0; i < 100; i++)
            {
                MegaDoEvents();
            }

            FillDesktopWindowsChildrenList(false);
            if (CheckIfVPRunning())
            {
                processPlumes.CloseMainWindow();
                MegaDoEvents();
            }
            else
            {
                MessageBox.Show("Visual Plumes should be running");
                return;
            }

            LoadVP();
            for (int i = 0; i < 100; i++)
            {
                MegaDoEvents();
            }

            FillDesktopWindowsChildrenList(false);
            if (!CheckIfVPRunning())
            {
                richTextBoxStatus.AppendText("Visual Plumes could not be started.\r\n");
                return;
            }

            af.APISetForegroundWindow(hWndPlumes);

            MegaDoEvents();
            MegaDoEvents();
            MegaDoEvents();

            for (int i = 0; i < 100; i++)
            {
                MegaDoEvents();
            }

            if (!SetHandles())
                return;

            for (int i = 0; i < 100; i++)
            {
                MegaDoEvents();
            }

            af.APISetForegroundWindow(this.Handle);

            richTextBoxStatus.AppendText("Success.\r\n");

            timerCheckDB.Enabled = true;

            return;

        }
        private void StartVPExecuteOfScenario()
        {
            af.APISetForegroundWindow(hWndPlumes);
            while (af.APIGetForegroundWindow() != hWndPlumes)
            {
                MegaDoEvents();
            }
            SelectTextOutputTab();
            ClickClearButton();
            af.APISendMouseClick(hWndDiffuserProjectTextBox, 10, 10);
            MegaDoEvents();
            SendKeys.SendWait("^u"); // running VP scenario quick function Ctrl+U
            MegaDoEvents();

            // this will stop the execution after one second
            timerStopExecutionAfterOneSecond.Enabled = true;
            timerStopExecutionAfterOneSecond.Start();

        }
        private void StopVPExecutionOfScenario()
        {
            af.APISetForegroundWindow(hWndPlumes);
            MegaDoEvents();
            while (af.APIGetForegroundWindow() != hWndPlumes)
            {
                MegaDoEvents();
            }
            SendKeys.SendWait("^q^q^q");

            MegaDoEvents();
            MegaDoEvents();

        }
        #endregion Private Functions

        #region Temp Button Function

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Copy(@"C:\CSSP latest code\VPAuto", @"\\wmon01dtchlebl\c$\VPAuto");
            richTextBoxStatus.Text = "copied ------------";
            button2.Enabled = true;
        }

        private void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        #endregion Temp Button Function


    }
}

#region Public class
public class CloseCaptionAndCommand
{
    public string Caption { get; set; }
    public string Command { get; set; }
}
public class APIFunc
{

    private const uint GW_CHILD = 5;
    private const uint GW_HWNDNEXT = 2;
    private const uint WM_MOUSEMOVE = 0x200;
    private const uint WM_LBUTTONDOWN = 0x201;
    private const uint WM_LBUTTONUP = 0x202;
    private const uint WM_CHAR = 0x102;
    private const uint WM_KEYDOWN = 0x100;
    private const uint WM_KEYUP = 0x101;
    private const uint WM_SYSKEYDOWN = 0x104;
    private const uint WM_SYSKEYUP = 0x105;
    private const uint VK_SHIFT = 0x10;
    private const uint VK_CONTROL = 0x11;


    private static List<WndHandleAndTitle> wht;

    public APIFunc()
    {
        wht = new List<WndHandleAndTitle>();
    }
    private void MegaDoEvents()
    {
        for (int i = 0; i < 20000; i++)
        {
            Application.DoEvents();
        }
    }
    public static POINTMouse APIGetCursorPosition()
    {
        POINTMouse lpPoint;
        GetCursorPos(out lpPoint);
        //bool success = User32.GetCursorPos(out lpPoint);
        // if (!success)

        return lpPoint;
    }
    public void APISendMouseMove(IntPtr hWnd)
    {
        POINTMouse pm = APIGetCursorPosition();
        APISendMessage(hWnd, WM_MOUSEMOVE, (int)0, (uint)(Convert.ToUInt16(pm.X + 10) + (Convert.ToUInt16(pm.Y + 10) << 16)));
        APISendMessage(hWnd, WM_MOUSEMOVE, (int)0, (uint)(Convert.ToUInt16(pm.X) + (Convert.ToUInt16(pm.Y) << 16)));
        MegaDoEvents();
    }
    public void APISendMouseClick(IntPtr hWnd, int x, int y)
    {
        APISendMessage(hWnd, WM_LBUTTONDOWN, (int)0, (uint)(Convert.ToUInt16(x) + (Convert.ToUInt16(y) << 16)));
        APISendMessage(hWnd, WM_LBUTTONUP, (int)0, (uint)(Convert.ToUInt16(x) + (Convert.ToUInt16(y) << 16)));
        MegaDoEvents();
    }
    public void APIPostMouseClick(IntPtr hWnd, int x, int y)
    {
        APIPostMessage(hWnd, (uint)WM_LBUTTONDOWN, (uint)1, (uint)(Convert.ToUInt16(x) + (Convert.ToUInt16(y) << 16)));
        APIPostMessage(hWnd, (uint)WM_LBUTTONUP, (uint)0, (uint)(Convert.ToUInt16(x) + (Convert.ToUInt16(y) << 16)));
        MegaDoEvents();
    }
    public IntPtr APIFindWinow(string lpClassName, string lpWindowName)
    {
        return FindWindow(lpClassName, lpWindowName);
    }
    public IntPtr APIFindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName)
    {
        return FindWindowEx(hwndParent, hwndChildAfter, lpClassName, lpWindowName);
    }
    public bool APISetForegroundWindow(IntPtr hWnd)
    {
        return SetForegroundWindow(hWnd);
    }
    public IntPtr APIGetForegroundWindow()
    {
        return GetForegroundWindow();
    }
    public uint APISendMessage(IntPtr hWnd, uint Msg, int wParam, uint lParam)
    {
        return SendMessage(hWnd, Msg, wParam, lParam);
    }
    public uint APIPostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam)
    {
        return PostMessage(hWnd, Msg, wParam, lParam);
    }
    public IntPtr APIGetDesktopWindow()
    {
        return GetDesktopWindow();
    }
    public IntPtr APIGetWindow(IntPtr hWnd, uint uCmd)
    {
        return GetWindow(hWnd, uCmd);
    }
    public bool APIIsWindowVisible(IntPtr hWnd)
    {
        return IsWindowVisible(hWnd);
    }
    public int APIGetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount)
    {
        return GetWindowText(hWnd, lpString, nMaxCount);
    }
    public int APIInternalGetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount)
    {
        return InternalGetWindowText(hWnd, lpString, nMaxCount);
    }
    public int APIGetWindowTextLength(IntPtr hWnd)
    {
        return GetWindowTextLength(hWnd);
    }
    public IntPtr APISetFocus(IntPtr hWnd)
    {
        return SetFocus(hWnd);
    }
    public IntPtr APIGetFocus()
    {
        return GetFocus();
    }
    public bool APISetWindowText(IntPtr hWnd, StringBuilder WindowText)
    {
        return SetWindowText(hWnd, WindowText);
    }
    public bool APICloseWindow(IntPtr hWnd)
    {
        return CloseWindow(hWnd);
    }
    public bool APIDestroyWindow(IntPtr hWnd)
    {
        return DestroyWindow(hWnd);
    }
    public List<WndHandleAndTitle> GetChildrenWindowsHandleAndTitle(IntPtr hWnd)
    {
        wht.Clear();
        FillWindowHandleAndTitle(hWnd);
        return wht;
    }

    #region DllImport static functions

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTMouse
    {
        public int X;
        public int Y;

        public static implicit operator Point(POINTMouse pointMouse)
        {
            return new Point(pointMouse.X, pointMouse.Y);
        }
    }

    // Get a handle to an application window.
    [DllImport("USER32.DLL")]
    public static extern bool GetCursorPos(out POINTMouse lpPoint);

    [DllImport("USER32.DLL")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("USER32.DLL")]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

    // Activate an application window.
    [DllImport("USER32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    // Get the forground window.
    [DllImport("USER32.DLL")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint SendMessage(IntPtr hWnd, uint Msg, int wParam, uint lParam);

    [DllImport("user32.dll")]
    public static extern uint PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    public static extern int InternalGetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    public static extern IntPtr SetFocus(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    public static extern IntPtr GetFocus();

    [DllImport("USER32.DLL")]
    public static extern bool SetWindowText(IntPtr hWnd, StringBuilder WindowText);

    [DllImport("user32")]
    public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

    [DllImport("user32")]
    public static extern bool CloseWindow(IntPtr window);

    [DllImport("user32")]
    public static extern bool DestroyWindow(IntPtr window);

    public void FillWindowHandleAndTitle(IntPtr hWnd)
    {
        EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
        EnumChildWindows(hWnd, childProc, IntPtr.Zero);
        return;
    }
    private static bool EnumWindow(IntPtr hWnd, IntPtr pointer)
    {
        // get the text from the window
        StringBuilder bld = new StringBuilder(256);
        GetWindowText(hWnd, bld, 256);
        string text = bld.ToString();

        WndHandleAndTitle Tempwht = new WndHandleAndTitle()
        {
            Handle = hWnd,
            Title = text
        };
        wht.Add(Tempwht);
        return true;
    }
    public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);
    #endregion DllImport static functions
}
public class WndHandleAndTitle
{
    public IntPtr Handle { get; set; }
    public string Title { get; set; }
}
//public class VPResValues
//{
//    public double Conc { get; set; }
//    public double Dilu { get; set; }
//    public double FarfieldWidth { get; set; }
//    public double Distance { get; set; }
//    public double TheTime { get; set; }
//    public double Decay { get; set; }
//}
//public class VPModel
//{
//    public VPModel()
//    {
//    }

//    public float AcuteMixZone_m { get; set; }
//    public float ChronicMixZone_m { get; set; }
//    public float EffluentConcentration_MPN_100ml { get; set; }
//    public float EffluentFlow_m3_s { get; set; }
//    public float EffluentSalinity_PSU { get; set; }
//    public float EffluentTemperature_C { get; set; }
//    public float EffluentVelocity_m_s { get; set; }
//    public string Error { get; set; }
//    public float FroudeNumber { get; set; }
//    public float HorizontalAngle_deg { get; set; }
//    public DateTime LastUpdateDate_UTC { get; set; }
//    public int NumberOfPorts { get; set; }
//    public float PortDepth_m { get; set; }
//    public float PortDiameter_m { get; set; }
//    public float PortElevation_m { get; set; }
//    public float PortSpacing_m { get; set; }
//    public string ScenarioName { get; set; }
//    public int Status { get; set; }
//    public int TVItemID { get; set; }
//    public bool UseAsBestEstimate { get; set; }
//    public Nullable<int> UserInfoID { get; set; }
//    public float VerticalAngle_deg { get; set; }
//    public int VPScenarioID { get; set; }
//}

//public class VPFullModel : VPModel
//{
//    public VPFullModel()
//    {
//        AmbientList = new List<VPAmbientModel>();
//        ResultList = new List<VPResultModel>();
//    }
//    public string RawResults { get; set; }
//    public List<VPAmbientModel> AmbientList { get; set; }
//    public List<VPResultModel> ResultList { get; set; }
//}

//public class VPAmbientModel
//{
//    public double AmbientSalinity_PSU { get; set; }
//    public double AmbientTemperature_C { get; set; }
//    public double BackgroundConcentration_MPN_100ml { get; set; }
//    public double CurrentDirection_deg { get; set; }
//    public double CurrentSpeed_m_s { get; set; }
//    public string Error { get; set; }
//    public double FarFieldCurrentDirection_deg { get; set; }
//    public double FarFieldCurrentSpeed_m_s { get; set; }
//    public double FarFieldDiffusionCoefficient { get; set; }
//    public double MeasurementDepth_m { get; set; }
//    public double PollutantDecayRate_per_day { get; set; }
//    public int Row { get; set; }
//}

//public class VPResultModel
//{
//    public int Ordinal { get; set; }
//    public double Concentration { get; set; }
//    public double Dilution { get; set; }
//    public double FarFieldWidth { get; set; }
//    public double DispersionDistance { get; set; }
//    public double TravelTime { get; set; }
//}
//public class VPScenarioIDAndRawResults
//{
//    public int VPScenarioID { get; set; }
//    public string RawResults { get; set; }
//}
#endregion Public class
