using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalUtility
{
    public partial class Autoload : MaterialForm
    {
        private MaterialSkinManager materialSkinManager;
        private bool isBackToMenu = false;

        public Autoload()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.BLACK);

            InitializeUI();

            this.FormClosed += Form3_FormClosed;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
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

            // Автозавантаження
            MaterialButton btnAddStartup = new MaterialButton()
            {
                Text = "Додати до автозавантаження",
                Location = new Point(15, 110),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.Aqua,
                ForeColor = Color.White
            };
            btnAddStartup.Click += (s, e) => AddToStartup();
            btnAddStartup.TabStop = false;

            MaterialButton btnRemoveStartup = new MaterialButton()
            {
                Text = "Прибрати з автозавантаження",
                Location = new Point(320, 110),
                Size = new Size(150, 40),
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            btnRemoveStartup.Click += (s, e) => RemoveFromStartup();
            btnRemoveStartup.TabStop = false;

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
                title,
                logo,
                // Вибір фунції
                btnAddStartup, 
                btnRemoveStartup,
                btnBack
            });
        }

        // Автозавантаження
        private void AddToStartup()
        {
            string exePath = Application.ExecutablePath;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue("UniversalUtility", exePath);
            MessageBox.Show("Додано в автозавантаження", "Автозавантаження", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemoveFromStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            rk.DeleteValue("UniversalUtility", false);
            MessageBox.Show("Видалено з автозавантаження", "Автозавантаження", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!isBackToMenu)
            {
                Application.Exit();
            }
        }
    }
}