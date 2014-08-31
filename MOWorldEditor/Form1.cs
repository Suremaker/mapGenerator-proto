using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace MOWorldEditor
{
    public partial class WorldEditor : Form
    {
        private readonly WorldModel _worldModel;

        public WorldEditor(WorldModel worldModel)
        {
            _worldModel = worldModel;
            InitializeComponent();
        }

        public void Run()
        {
            worldView.Init(_worldModel);
            Show();

            long frameDelta = 1000 / 60;

            var watch = new Stopwatch();
            watch.Start();
            long last = watch.ElapsedMilliseconds;
            while (worldView.Render())
            {
                Application.DoEvents();
                long now = watch.ElapsedMilliseconds;
                if (now - last < frameDelta)
                    Thread.Sleep((int)(now - last));
                last = now;
            }
        }
    }
}
