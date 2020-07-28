using Myra.Graphics2D.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Myra.Extended.Samples.Widgets
{
	public class WidgetsGame : Game
	{
		private readonly GraphicsDeviceManager _graphics;
		private ListBox _listDemos;
		private HorizontalStackPanel _root;
		private Panel _panelDemo;
		private Demo _currentDemo;
		private KeyboardState _lastKeyboard;
		private bool _isDemoFullScreen = false;
		private Desktop _desktop;
		
		public static WidgetsGame Instance { get; private set; }

		public WidgetsGame()
		{
			Instance = this;

			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 800,
				PreferredBackBufferHeight = 600
			};
			Window.AllowUserResizing = true;

			IsMouseVisible = true;
		}

		private static Demo[] CreateDemos()
		{
			return new Demo[]
			{
				new LogViewDemo(),
				new ArrowDemo()
			};
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			MyraEnvironment.Game = this;

			_root = new HorizontalStackPanel();

			_root.Proportions.Add(new Proportion(ProportionType.Auto));
			_root.Proportions.Add(new Proportion(ProportionType.Auto));
			_root.Proportions.Add(new Proportion(ProportionType.Fill));

			_listDemos = new ListBox();

			var demos = CreateDemos();

			foreach (var d in demos)
			{
				var item = new ListItem
				{
					Text = d.Name,
					Tag = d
				};

				_listDemos.Items.Add(item);
			}

			_listDemos.SelectedIndexChanged += ListDemos_SelectedIndexChanged;

			_root.Widgets.Add(_listDemos);

			_root.Widgets.Add(new VerticalSeparator());

			_panelDemo = new Panel();
			_root.Widgets.Add(_panelDemo);

			_desktop = new Desktop();
			_desktop.Root = _root;

#if MONOGAME
			// Inform Myra that external text input is available
			// So it stops translating Keys to chars
			_desktop.HasExternalTextInput = true;

			// Provide that text input
			Window.TextInput += (s, a) =>
			{
				_desktop.OnChar(a.Character);
			};
#endif
		}

		private void ListDemos_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_currentDemo = (Demo)_listDemos.Items[_listDemos.SelectedIndex.Value].Tag;

			_panelDemo.Widgets.Clear();
			_panelDemo.Widgets.Add(_currentDemo.Widget);
			_currentDemo.OnSet();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (_currentDemo == null)
			{
				return;
			}

			var keyboard = Keyboard.GetState();
			if (keyboard.IsKeyDown(Keys.OemTilde) && !_lastKeyboard.IsKeyDown(Keys.OemTilde))
			{
				if (!_isDemoFullScreen)
				{
					_desktop.Root = _currentDemo.Widget;
				}
				else
				{
					_desktop.Root = _root;
				}

				_isDemoFullScreen = !_isDemoFullScreen;
			}

			_lastKeyboard = keyboard;

			_currentDemo.OnUpdate();
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			GraphicsDevice.Clear(Color.Black);
			_desktop.Render();
		}
	}
}