using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mogre;

namespace MOWorldEditor
{
    public partial class WorldView : UserControl
    {
        private Root _root;
        private RenderWindow _window;
        private Camera _camera;

        public WorldView()
        {
            InitializeComponent();
            Disposed += WorldView_Disposed;
        }

        private void WorldView_Disposed(object sender, EventArgs e)
        {
            if (_root != null)
                _root.Dispose();
            _root = null;
        }

        public void Init(WorldModel worldModel)
        {
            // Create root object
            _root = new Root();

            // Define Resources
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            String secName, typeName, archName;

            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }

            // Setup RenderSystem
            RenderSystem rs = _root.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
            // or use "OpenGL Rendering Subsystem"
            _root.RenderSystem = rs;
            rs.SetConfigOption("Full Screen", "No");
            rs.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");

            // Create Render Window
            _root.Initialise(false, "Main Ogre Window");
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = Handle.ToString();
            _window = _root.CreateRenderWindow("Main RenderWindow", 800, 600, false, misc);

            // Init resources
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            // Create a Simple Scene
            SceneManager mgr = _root.CreateSceneManager(SceneType.ST_GENERIC);
            mgr.AmbientLight = new ColourValue(0.2f, 0.2f, 0.2f);

            _camera = mgr.CreateCamera("Camera");
            _camera.AutoAspectRatio = true;
            _window.AddViewport(_camera);

            worldModel.Initialize(mgr);
            ManualObject ent = worldModel.WorldObject.Entity;
            mgr.RootSceneNode.CreateChildSceneNode().AttachObject(ent);

            _camera.SetPosition(ent.BoundingBox.Center.x, ent.BoundingBox.Center.y - 300, -250);
            _camera.LookAt(ent.BoundingBox.Center);

            var light = mgr.CreateLight("mainLight");
            light.Type = Light.LightTypes.LT_POINT;
            light.SetPosition(ent.BoundingBox.Center.x, ent.BoundingBox.Center.y, -50);
            light.DiffuseColour = new ColourValue(1, 1, 1);
            light.SpecularColour = new ColourValue(0, 1, 1);
        }

        private void WorldView_Resize(object sender, EventArgs e)
        {
            if (_window != null)
                _window.WindowMovedOrResized();
        }

        public bool Render()
        {
            return _root != null && _root.RenderOneFrame();
        }

        private void WorldView_KeyPress(object sender, KeyPressEventArgs e)
        {
            var multiplier = 4;
            switch (e.KeyChar)
            {
                case 'a':
                    _camera.Move(Vector3.UNIT_X * multiplier);
                    break;
                case 'd':
                    _camera.Move(Vector3.NEGATIVE_UNIT_X * multiplier);
                    break;
                case 'w':
                    _camera.Move(Vector3.UNIT_Y * multiplier);
                    break;
                case 's':
                    _camera.Move(Vector3.NEGATIVE_UNIT_Y * multiplier);
                    break;
                case 'q':
                    _camera.Move(Vector3.NEGATIVE_UNIT_Z * multiplier);
                    break;
                case 'e':
                    _camera.Move(Vector3.UNIT_Z * multiplier);
                    break;
            }
        }
    }
}
