using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalUtility
{
    public partial class Clean : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private bool isBackToMenu = false;

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        public Clean()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.BLACK);

            InitializeUI();

            this.FormClosed += Form2_FormClosed;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        // Ініціалізація інтерфейсу
        private void InitializeUI()
        {
            this.Text = String.Empty;
            Label title = new Label();
            title.Text = "UniversalUtility by .../github.com/rrs1se";
            title.Font = new Font("Roboto", 14, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(36, 36, 36);
            title.AutoSize = true;
            title.Location = new Point(50, 32);
            title.BackColor = Color.Transparent;

            this.Size = new Size(600, 200);
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

            // Очищення
            MaterialButton btnCleanTemp = new MaterialButton()
            {
                Text = "Почистити %TEMP%",
                Location = new Point(30, 120),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnCleanTemp.Click += (s, e) => CleanTemp();
            btnCleanTemp.TabStop = false;

            MaterialButton btnCleanAddTemp = new MaterialButton()
            {
                Text = "Почистити TEMP",
                Location = new Point(240, 120),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnCleanAddTemp.Click += (s, e) => CleanAddTemp();
            btnCleanAddTemp.TabStop = false;

            MaterialButton btnMemory = new MaterialButton()
            {
                Text = "Звільнити ОЗП",
                Location = new Point(440, 120),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnMemory.Click += (s, e) => ClearMemory();
            btnMemory.TabStop = false;

            // Повернення до меню
            MaterialButton btnBack = new MaterialButton()
            {
                Text = "Повернутися до меню",
                Location = new Point(200, 180),
                Size = new Size(150, 40),
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
                // Очищення системи
                btnCleanTemp,
                btnCleanAddTemp,
                btnMemory,
                // Повернення до меню
                btnBack,
            });
        }

        // Очищення системи
        private void CleanTemp()
        {
            string tempPath = Path.GetTempPath();
            int deletedFiles = 0;
            int deletedDirs = 0;

            try
            {
                DirectoryInfo di = new DirectoryInfo(tempPath);

                foreach (var file in di.GetFiles())
                {
                    try
                    {
                        RemoveReadOnlyAttributes(file);
                        file.Delete();
                        deletedFiles++;
                    }
                    catch { }
                }

                foreach (var dir in di.GetDirectories())
                {
                    deletedDirs += DeleteDirectoryFully(dir);
                }

                MessageBox.Show($"Видалено файлів: {deletedFiles}\nВидалено папок: {deletedDirs}", "Очищення %TEMP%", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час очищення %TEMP%: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CleanAddTemp()
        {
            string tempPath = Path.GetFullPath("C:\\Windows\\Temp");
            int deletedFiles = 0;
            int deletedDirs = 0;

            try
            {
                DirectoryInfo di = new DirectoryInfo(tempPath);

                foreach (var file in di.GetFiles())
                {
                    try
                    {
                        RemoveReadOnlyAttributes(file);
                        file.Delete();
                        deletedFiles++;
                    }
                    catch { }
                }

                foreach (var dir in di.GetDirectories())
                {
                    deletedDirs += DeleteDirectoryFully(dir);
                }

                MessageBox.Show($"Видалено файлів: {deletedFiles}\nВидалено папок: {deletedDirs}", "Очищення TEMP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час очищення TEMP: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoveReadOnlyAttributes(FileSystemInfo fsi)
        {
            if (fsi.Exists)
            {
                fsi.Attributes = fsi.Attributes & ~FileAttributes.ReadOnly & ~FileAttributes.Hidden & ~FileAttributes.System;
            }
        }

        private int DeleteDirectoryFully(DirectoryInfo dir)
        {
            int deletedCount = 0;

            if (!dir.Exists) return 0;

            try
            {
                // Зняття атрибутів с файлів та їх видалення
                foreach (var file in dir.GetFiles())
                {
                    try
                    {
                        RemoveReadOnlyAttributes(file);
                        file.Delete();
                        deletedCount++;
                    }
                    catch { }
                }

                // Рекурсивна обробка підпапки
                foreach (var subDir in dir.GetDirectories())
                {
                    deletedCount += DeleteDirectoryFully(subDir);
                }

                // Зняття атрибутів с папок та їх видалення
                RemoveReadOnlyAttributes(dir);
                dir.Delete();
                deletedCount++;
            }
            catch
            {
                // Папку занято або вона недоступна - пропускаємо
            }

            return deletedCount;
        }

        private void ClearMemory()
        {
            int cleared = 0;
            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (EmptyWorkingSet(p.Handle) == 1)
                    {
                        cleared++;
                    }
                }
                catch { }
            }
            MessageBox.Show($"Звільнено пам'яті у {cleared} процесів.", "Оперативна пам'ять", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isBackToMenu)
            {
                Application.Exit();
            }
        }
    }
}