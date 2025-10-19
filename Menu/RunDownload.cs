using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalUtility
{
    public partial class RunDownload : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private bool isBackToMenu = false;

        private ProgressBar downloadProgress;
        private Label progressLabel;

        private bool isDownloading = false;

        public RunDownload()
        {
            InitializeComponent();

            this.MaximizeBox = false; 
            this.MinimizeBox = false;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.BLACK);

            InitializeUI();

            this.FormClosed += Form4_FormClosed;
        }

        
        private void Form4_Load(object sender, EventArgs e)
        {
            // Розподіл категорій
            Panel Verticalline1 = new Panel();
            Verticalline1.Height = 225;
            Verticalline1.Width = 2;
            Verticalline1.Location = new Point(270, 120);
            Verticalline1.BackColor = Color.LightGray;
            this.Controls.Add(Verticalline1);

            Panel Verticalline2 = new Panel();
            Verticalline2.Height = 225;
            Verticalline2.Width = 2;
            Verticalline2.Location = new Point(570, 120);
            Verticalline2.BackColor = Color.LightGray;
            this.Controls.Add(Verticalline2);

            Panel Horizontalline1 = new Panel();
            Horizontalline1.Height = 2;
            Horizontalline1.Width = 715;
            Horizontalline1.Location = new Point(50, 380);
            Horizontalline1.BackColor = Color.LightGray;
            this.Controls.Add(Horizontalline1);
        }

        // Ініціалізація інтерфейсу
        private void InitializeUI()
        {
            this.Text = string.Empty;
            Label title = new Label();
            title.Text = "UniversalUtility by .../github.com/rrs1se";
            title.Font = new Font("Roboto", 14, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(36, 36, 36);
            title.AutoSize = true;
            title.Location = new Point(50, 32);
            title.BackColor = Color.Transparent;

            this.Size = new Size(820, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SkinManager.BackgroundColor;

            this.BackColor = Color.FromArgb(30, 30, 30);

            PictureBox logo = new PictureBox();
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Size = new Size(32, 32);
            logo.Location = new Point(10, 28);
            logo.BackColor = Color.Transparent;

            string resourceName = "UniversalUtility.service.png";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            if (stream != null)
                {
                    logo.Image = Image.FromStream(stream);
                }

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            // Категорії
            Label BrowsersAndUtilities = new Label()
            {
                Text = "Браузери та утиліти",
                AutoSize = true,
                Location = new Point(20, 80),
                Font = new Font("Tahoma", 13, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.LightGray
            };

            Label GamesAndCommunication = new Label()
            {
                Text = "Ігри та комунікація",
                AutoSize = true,
                Location = new Point(290, 80),
                Font = new Font("Tahoma", 13, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.LightGray
            };

            Label RunSystemPrograms = new Label()
            {
                Text = "Системні програми",
                AutoSize = true,
                Location = new Point(590, 80),
                Font = new Font("Tahoma", 13, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.LightGray
            };

            Label ToolsForSoftwareDevelopment = new Label()
            {
                Text = "Інструменти для розробки ПЗ (для розробників)",
                AutoSize = true,
                Location = new Point(200, 395),
                Font = new Font("Tahoma", 13, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.LightGray
            };

            Label LongDownloading = new Label()
            {
                Text = "!!! Деякі програми можуть мати великий розмір через що завантаження може бути довгим - майте терпіння <3 !!!",
                AutoSize = true,
                Location = new Point(20, 665),
                Font = new Font("Times New Roman", 11, FontStyle.Italic),
                ForeColor = Color.LightGray
            };

            // Завантаження
            progressLabel = new Label()
            {
                Text = "0%",
                Location = new Point(380, 615),
                AutoSize = true,
                Font = new Font("Tahoma", 13, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.Green
            };

            downloadProgress = new MaterialProgressBar()
            {
                Location = new Point(30, 645),
                AutoSize = false,
                Size = new Size(760, 10),
                Maximum = 100,
                Visible = true,
                ForeColor = Color.Green
            };

            // Завантаження. Браузери та утиліти
            MaterialButton btnDownloadChrome = new MaterialButton()
            {
                Text = "Google Chrome",
                Location = new Point(20, 110),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadChrome.Click += (s, e) => DownloadChrome();
            btnDownloadChrome.TabStop = false;

            MaterialButton btnDownloadOperaGX = new MaterialButton()
            {
                Text = "OperaGX",
                Location = new Point(20, 160),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadOperaGX.Click += (s, e) => DownloadOperaGX();
            btnDownloadOperaGX.TabStop = false;

            MaterialButton btnDownloadWinrar = new MaterialButton()
            {
                Text = "Winrar",
                Location = new Point(20, 210),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadWinrar.Click += (s, e) => DownloadWinrar();
            btnDownloadWinrar.TabStop = false;

            MaterialButton btnDownloadGPU_ZCPU_Z = new MaterialButton()
            {
                Text = "GPU-Z/CPU-Z (RAR)",
                Location = new Point(20, 260),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadGPU_ZCPU_Z.Click += (s, e) => DownloadGPU_ZCPU_Z();
            btnDownloadGPU_ZCPU_Z.TabStop = false;

            MaterialButton btnDownloadCrysDiskInfo = new MaterialButton()
            {
                Text = "CrystalDiskInfo 9.7.0 (RAR)",
                Location = new Point(20, 310),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadCrysDiskInfo.Click += (s, e) => DownloadCrysDiskInfo();
            btnDownloadCrysDiskInfo.TabStop = false;
            
            // Завантаження. Ігри та комунікація
            MaterialButton btnDownloadEpicGames = new MaterialButton()
            {
                Text = "Epic Games",
                Location = new Point(290, 110),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadEpicGames.Click += (s, e) => DownloadEpicGames();
            btnDownloadEpicGames.TabStop = false;

            MaterialButton btnDownloadSteam = new MaterialButton()
            {
                Text = "Steam",
                Location = new Point(290, 210),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadSteam.Click += (s, e) => DownloadSteam();
            btnDownloadSteam.TabStop = false;

            MaterialButton btnDownloadDiscord = new MaterialButton()
            {
                Text = "Discord",
                Location = new Point(290, 310),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadDiscord.Click += (s, e) => DownloadDiscord();
            btnDownloadDiscord.TabStop = false;

            // Запуск системних програм
            MaterialButton btnPowerShell = new MaterialButton()
            {
                Text = "Відкрити PowerShell",
                Location = new Point(590, 110),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnPowerShell.Click += (s, e) => PowerShell();
            btnPowerShell.TabStop = false;

            MaterialButton btnCmd = new MaterialButton()
            {
                Text = "Відкрити CMD",
                Location = new Point(590, 160),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnCmd.Click += (s, e) => cmd();
            btnCmd.TabStop = false;

            MaterialButton btnRegistry = new MaterialButton()
            {
                Text = "Відкрити Реєстр",
                Location = new Point(590, 210),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnRegistry.Click += (s, e) => Regedit();
            btnRegistry.TabStop = false;

            MaterialButton btnmsinfo = new MaterialButton()
            {
                Text = "Відкрити msinfo32",
                Location = new Point(590, 260),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnmsinfo.Click += (s, e) => msinfo();
            btnmsinfo.TabStop = false;

            MaterialButton btndevicemanager = new MaterialButton()
            {
                Text = "Відкрити DeviceManager",
                Location = new Point(590, 310),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btndevicemanager.Click += (s, e) => DeviceManager();
            btndevicemanager.TabStop = false;

            // Для розробників
            MaterialButton btnDownloadVisualStudio = new MaterialButton()
            {
                Text = "VisualStudio",
                Location = new Point(30, 450),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadVisualStudio.Click += (s, e) => DownloadVisualStudio();
            btnDownloadVisualStudio.TabStop = false;

            MaterialButton btnDownloadVSCode = new MaterialButton()
            {
                Text = "Visual Studio Code 1.101.2",
                Location = new Point(30, 500),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadVSCode.Click += (s, e) => DownloadVScode();
            btnDownloadVSCode.TabStop = false;

            MaterialButton btnDownloadPycharm = new MaterialButton()
            {
                Text = "Pycharm 2025.1.3",
                Location = new Point(300, 450),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadPycharm.Click += (s, e) => DownloadPycharm();
            btnDownloadPycharm.TabStop = false;

            MaterialButton btnDownloadNotepad = new MaterialButton()
            {
                Text = "Notepad++ v8.8.2(x64)",
                Location = new Point(300, 500),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadNotepad.Click += (s, e) => DownloadNotepad();
            btnDownloadNotepad.TabStop = false;

            MaterialButton btnDownloadWebStorm = new MaterialButton()
            {
                Text = "WebStorm 2025.1.3",
                Location = new Point(560, 450),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadWebStorm.Click += (s, e) => DownloadWebStorm();
            btnDownloadWebStorm.TabStop = false;

            MaterialButton btnDownloadCLion = new MaterialButton()
            {
                Text = "CLion 2025.1.3",
                Location = new Point(560, 500),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnDownloadCLion.Click += (s, e) => DownloadCLion();
            btnDownloadCLion.TabStop = false;

            // Повернення до меню
            MaterialButton btnBack = new MaterialButton()
            {
                Text = "Повернутися до меню",
                Location = new Point(255, 570),
                Size = new Size(280, 40),
                AutoSize = false,
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnBack.Click += (s, e) => Back();
            btnBack.TabStop = false;

            this.Controls.AddRange(new Control[]
            {
                // Інтерфейс
                logo,
                title,
                // Розподіл категорій
                BrowsersAndUtilities,
                GamesAndCommunication,
                RunSystemPrograms,
                ToolsForSoftwareDevelopment,
                // Інформація
                LongDownloading,
                // Завантаження
                downloadProgress,
                progressLabel,
                // Завантаження програм
                btnDownloadCrysDiskInfo,
                btnDownloadOperaGX,
                btnDownloadEpicGames,
                btnDownloadWinrar,
                btnDownloadSteam,
                btnDownloadDiscord,
                btnDownloadChrome,
                btnDownloadGPU_ZCPU_Z,
                // Для розробників
                btnDownloadVisualStudio,
                btnDownloadNotepad,
                btnDownloadPycharm,
                btnDownloadWebStorm,
                btnDownloadCLion,
                btnDownloadVSCode,
                // Запуск програм
                btnPowerShell,
                btnCmd,
                btnRegistry,
                btnmsinfo,
                btndevicemanager,
                // Повернення до меню
                btnBack
            });
        }

        // Запуск програм
        private void cmd()
        {
            Process.Start("cmd.exe");
        }

        private void PowerShell()
        {
            Process.Start("PowerShell.exe");
        }

        private void Regedit()
        {
            Process.Start("regedit.exe");
        }

        private void msinfo()
        {
            Process.Start("msinfo32");
        }

        private void DeviceManager()
        {
            Process.Start("devmgmt.msc");
        }

        // Завантаження програм
        private async void DownloadChrome()
        {
            string url = "https://dl.google.com/tag/s/appguid%3D%7B8A69D345-D564-463C-AFF1-A69D9E530F96%7D%26iid%3D%7BA7537CD6-DA47-9EF7-4FEC-8270F33DB220%7D%26lang%3Dru%26browser%3D4%26usagestats%3D1%26appname%3DGoogle%2520Chrome%26needsadmin%3Dprefers%26ap%3Dx64-statsdef_1%26installdataindex%3Dempty/update2/installers/ChromeSetup.exe";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ChromeSetup.exe"));
                    MessageBox.Show("Chrome завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Chrome: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadOperaGX()
        {
            string url = "https://net.geo.opera.com/opera_gx/stable/windows?utm_source=google&utm_medium=ose&utm_campaign=%28none%29&http_referrer=https%3A%2F%2Fwww.google.com%2F&utm_site=opera_com&utm_lastpage=opera.com%2Fgx&dl_token=77746261";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "OperaGXSetup.exe"));
                    MessageBox.Show("OperaGXSetup - завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження OperaGXSetup" + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadWinrar()
        {
            string url = "https://www.win-rar.com/fileadmin/winrar-versions/winrar/winrar-x64-712ru.exe";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Winrar.exe"));
                    MessageBox.Show("Winrar - завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Winrar: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadGPU_ZCPU_Z()
        {
            string url = "https://drive.usercontent.google.com/download?id=1Fs13jnIaHhnKwhrFmq1HhpgtbR_5ZeEp&export=download";


            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "GPU-Z/CPU-Z.rar"));
                    MessageBox.Show("GPU-Z/CPU-Z - завантажені!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження GPU-Z/CPU-Z: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadCrysDiskInfo()
        {
            string url = "https://altushost-swe.dl.sourceforge.net/project/crystaldiskinfo/9.7.0/CrystalDiskInfo9_7_0.zip?viasf=1";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "CrystalDiskInfo.rar"));
                    MessageBox.Show("CrystalDiskInfo - завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження CrystalDiskInfo" + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadEpicGames()
        {
            string url = "https://epicgames-download1.akamaized.net/Builds/UnrealEngineLauncher/Installers/Win32/EpicInstaller-18.7.0.msi?launcherfilename=EpicInstaller-18.7.0-e43a4046af59427daa3d8d7192fed56b.msi";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Epic Games Installer.msi"));
                    MessageBox.Show("Epic Games - завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Epic Games" + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadSteam()
        {
            string url = "https://cdn.fastly.steamstatic.com/client/installer/SteamSetup.exe";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Steam.exe"));
                    MessageBox.Show("Steam завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Steam: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadDiscord()
        {
            string url = "https://stable.dl2.discordapp.net/distro/app/stable/win/x64/1.0.9198/DiscordSetup.exe";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DiscordSetup.exe"));
                    MessageBox.Show("Discord завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Discord: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        // Для розробників
        private async void DownloadVisualStudio()
        {
            string url = "https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030:c28ad7552a3b429584c46e9f3d972d6e";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "VisualStudioSetup.exe"));
                    MessageBox.Show("VisualStudio - завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження VisualStudio" + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadNotepad()
        {
            string url = "https://github.com/notepad-plus-plus/notepad-plus-plus/releases/download/v8.8.2/npp.8.8.2.Installer.x64.exe";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Notepad++_setup-x64-8.8.2.exe"));
                    MessageBox.Show("Notepad++ v8.8.2 завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Notepad++ v8.8.2: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadPycharm()
        {
            string url = "https://download-cdn.jetbrains.com/python/pycharm-2025.1.3.exe";

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "PycharmSetup.exe"));
                    MessageBox.Show("Pycharm завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Pycharm: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadWebStorm()
        {
            string url = "https://download-cdn.jetbrains.com/webstorm/WebStorm-2025.1.3.exe";
            string destination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "WebStormSetup.exe");

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), destination);
                    MessageBox.Show("WebStorm завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження WebStorm: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadCLion()
        {
            string url = "https://download-cdn.jetbrains.com/cpp/CLion-2025.1.3.exe";
            string destination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "CLionSetup.exe");

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), destination);
                    MessageBox.Show("CLion завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження CLion: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        private async void DownloadVScode()
        {
            string url = "https://download-cdn.jetbrains.com/cpp/CLion-2025.1.3.exe";
            string destination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "VSCodeSetup-x64-1.101.2.exe");

            using (WebClient wc = new WebClient())
            {
                if (isDownloading)
                {
                    MessageBox.Show("Інше завантаження вже виконується!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                isDownloading = true;

                downloadProgress.Value = 0;

                wc.DownloadProgressChanged += (s, e) =>
                {
                    if (downloadProgress.InvokeRequired)
                    {
                        downloadProgress.Invoke(new Action(() =>
                        {
                            downloadProgress.Value = e.ProgressPercentage;
                            progressLabel.Text = e.ProgressPercentage + "%";
                        }));
                    }
                    else
                    {
                        downloadProgress.Value = e.ProgressPercentage;
                        progressLabel.Text = e.ProgressPercentage + "%";
                    }
                };

                try
                {
                    await wc.DownloadFileTaskAsync(new Uri(url), destination);
                    MessageBox.Show("Visual Studio Code завантажений!", "Успішне завантаження!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час завантаження Visual Studio Code: " + ex.Message);
                }
                finally
                {
                    downloadProgress.Value = 0;
                    progressLabel.Text = "0%";
                    isDownloading = false;
                }
            }
        }

        // Плавне повернення до меню
        private async void Back()
        {
            isBackToMenu = true;
            await OpenMenuSmooth();
            this.Close();
        }

        private async Task OpenMenuSmooth()
        {
            // Плавне зникнення поточної форми
            for (double d = 1.0; d >= 0; d -= 0.05)
            {
                this.Opacity = d;
                await Task.Delay(10);
            }

            var menu = MainMenu.Instance;
            menu.Opacity = 0;
            MainMenu.Instance.Show();

            // Плавна поява нової форми
            for (double d = 0; d <= 1.0; d += 0.05)
            {
                this.Hide();
                menu.Opacity = d;
                await Task.Delay(10);
            }

            this.Close();
        }

        // Закриття програми на "X"
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isBackToMenu)
            {
                Application.Exit();
            }
        }
    }
}