using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Deviants.MBSpinner
{
    public partial class MBSpinner : UserControl
    {
        #region PROPERTIES
        [Category("Data")]
        public float AnlgleOuter { get; set; } = 200;
        [Category("Data")]
        public float AngleInner { get; set; } = 200;
        [Category("Data")]
        public float StartAngleOuter { get; set; } = 0;
        [Category("Data")]
        public float StartAngleInner { get; set; } = 360;
        [Category("Data")]
        public float EndAngleOuter { get; set; } = 360;
        [Category("Data")]
        public float EndAngleInner { get; set; } = 0;
        [Category("Data")]
        public Color OuterColor { get; set; } = Color.FromArgb(0xFF, 0x00, 0x4b, 0x99);
        [Category("Data")]
        public Color InnerColor { get; set; } = Color.FromArgb(0xFF, 0xeb, 0x9c, 0x16);
        [Category("Data")]
        public int OuterWidth { get; set; } = 10;
        [Category("Data")]
        public int InnerWidth { get; set; } = 10;
        [Category("Data")]
        public float Size2 { get; set; } = 120;
        [Category("Data")]
        public int SpeedOuter { get; set; } = 5;
        [Category("Data")]
        public int SpeedInner { get; set; } = 5;
        #endregion

        Timer timer;
        BackgroundWorker bgWorker;

        public MBSpinner()
        {
            InitializeComponent();
            Visible = false;
            Dock = DockStyle.None;
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += Tick;
            Paint += new PaintEventHandler(PaintSpinner);

            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        public void Start()
        {
            timer.Start();
            Dock = DockStyle.Fill;
            Visible = true;
            BringToFront();
        }

        public void Start(DoWorkEventHandler toDo, ProgressChangedEventHandler onProgress, RunWorkerCompletedEventHandler whenDone)
        {
            if (bgWorker != null && bgWorker.IsBusy)
                return;

            Start();
            bgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = onProgress != null
            };

            bgWorker.DoWork += toDo;
            bgWorker.ProgressChanged += onProgress;
            bgWorker.RunWorkerCompleted += whenDone;
            bgWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            timer.Stop();
            Dock = DockStyle.None;
            Visible = false;
            bgWorker?.Dispose();
        }

        public void ReportProgress(int val)
        {
            bgWorker?.ReportProgress(val);
        }

        void PaintSpinner(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen p = new Pen(OuterColor, OuterWidth))
            {
                RectangleF r = ClientRectangle;
                r.X = ClientRectangle.X + ClientRectangle.Width / 2 - Size2 / 2;
                r.Y = ClientRectangle.Y + ClientRectangle.Height / 2 - Size2 / 2;
                r.Width = Size2;
                r.Height = Size2;

                r.Inflate(-p.Width, -p.Width);
                e.Graphics.DrawArc(p, r, StartAngleOuter, AnlgleOuter);
                r.Inflate(-p.Width, -p.Width);

                p.Color = InnerColor;
                p.Width = InnerWidth;
                e.Graphics.DrawArc(p, r, StartAngleInner, AngleInner);
            }
        }

        void Tick(object sender, EventArgs e)
        {
            timer.Stop();
            StartAngleOuter += SpeedOuter;
            StartAngleInner -= SpeedInner;

            if (StartAngleOuter == EndAngleOuter)
                StartAngleOuter -= EndAngleOuter;
            if (StartAngleInner == EndAngleInner)
                StartAngleInner += EndAngleInner;

            Invalidate();
            timer.Start();
        }
    }
}