using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalUtility
{
    public partial class MainMenu : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private bool isBackToMenu = false;
        public static MainMenu Instance { get; private set; }

        public MainMenu()
        {
            Instance = this;

            InitializeComponent();

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.BLACK);

            InitializeUI();

            this.FormClosed += Form1_FormClosed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Перевірка на запуск від імені адміністратора
            if (!IsRunAsAdmin())
            {
                MessageBox.Show("Для повного доступу рекомендований запуск від імені адміністратора.", "Обмежені права доступу", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsRunAsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
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

            this.Size = new Size(600, 200);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SkinManager.BackgroundColor;

            this.Load += async (s, e) => await OpenMenuSmooth();

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

            // Кнопки в меню
            MaterialButton btnClean = new MaterialButton()
            {
                Text = "Очищення",
                Location = new Point(30, 125),
                Size = new Size(170, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnClean.Click += (s, e) => Clean();
            btnClean.TabStop = false;

            MaterialButton btnAutoload = new MaterialButton()
            {
                Text = "Автозапуск UUtility",
                Location = new Point(165, 125),
                Size = new Size(170, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnAutoload.Click += (s, e) => Autoload();
            btnAutoload.TabStop = false;

            MaterialButton btnPrograms = new MaterialButton()
            {
                Text = "Запуск/Завантаження\nпрограм",
                Location = new Point(380, 125),
                Size = new Size(170, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnPrograms.Click += (s, e) => Programs();
            btnPrograms.TabStop = false;

            this.Controls.AddRange(new Control[]
            {
                // Інтерфейс
                title,
                logo,
                // Вибір розділу
                btnClean,
                btnAutoload,
                btnPrograms,
            });
        }

        // Меню
        private void Clean()
        {
            OpenCleanSmooth();
        }

        private void Autoload()
        {
            OpenAutoloadSmooth();
        }

        private void Programs()
        {
            OpenProgramsSmooth();
        }

        // Анімація
        private async Task OpenMenuSmooth()
        {
            for (double d = 0; d <= 1.0; d += 0.05)
            {
                this.Opacity = d;
                await Task.Delay(10);
            }
        }

        private async void OpenCleanSmooth()
        {
            // Плавне зникнення поточної форми
            for (double d = 1.0; d >= 0; d -= 0.05)
            {
                this.Opacity = d;
                await Task.Delay(10);
            }

            Clean clean = new Clean();
            clean.Opacity = 0;
            clean.Show();

            // Плавна поява нової форми
            for (double d = 0; d <= 1.0; d += 0.05)
            {
                clean.Opacity = d;
                await Task.Delay(10);
            }

            this.Hide();
        }

        private async void OpenAutoloadSmooth()
        {
            // Плавне зникнення поточної форми
            for (double d = 1.0; d >= 0; d -= 0.05)
            {
                this.Opacity = d;
                await Task.Delay(10);
            }

            Autoload autoload = new Autoload();
            autoload.Opacity = 0;
            autoload.Show();

            // Плавна поява нової форми
            for (double d = 0; d <= 1.0; d += 0.05)
            {
                autoload.Opacity = d;
                await Task.Delay(10);
            }

            this.Hide();
        }

        private async void OpenProgramsSmooth()
        {
            // Плавне зникнення поточної форми
            for (double d = 1.0; d >= 0; d -= 0.05)
            {
                this.Opacity = d;
                await Task.Delay(10);
            }

            RunDownload programs = new RunDownload();
            programs.Opacity = 0;
            programs.Show();

            // Плавна поява нової форми
            for (double d = 0; d <= 1.0; d += 0.05)
            {
                programs.Opacity = d;
                await Task.Delay(10);
            }

            this.Hide();
        }

        // Закриття програми на "X"
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isBackToMenu)
            {
                Application.Exit();
            }
        }
    }
}