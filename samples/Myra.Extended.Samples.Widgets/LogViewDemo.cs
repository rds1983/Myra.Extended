using Myra.Extended.Widgets;
using Myra.Graphics2D.UI;
using System;

namespace Myra.Extended.Samples.Widgets
{
	class LogViewDemo: DemoT<LogView>
	{
		private readonly LogView _logView = new LogView();
		private DateTime _dt;

		public override Widget Widget => _logView;

		public override void OnSet()
		{
			base.OnSet();
			_logView.ClearLog();

			_dt = DateTime.Now;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			var passed = DateTime.Now - _dt;
			if (passed.TotalSeconds < 1.0)
			{
				return;
			}

			var messagesCount = Utility.Random.Next(1, 4);

			var damage = Utility.Random.Next(1, 10);
			_logView.LogFormat(@"/c[lightBlue]Gandalf/c[white] hits /c[green]a kobold/c[white] with his staff for /c[red]{0}/c[white] damage.", damage);

			if (messagesCount > 1)
			{
				damage = Utility.Random.Next(1, 5);
				_logView.LogFormat(@"/c[green]A kobold/c[white] claws /c[lightBlue]Gandalf/c[white] for /c[red]{0}/c[white] damage.", damage);
			}

			if (messagesCount > 2)
			{
				damage = Utility.Random.Next(1, 15);
				_logView.LogFormat(@"/c[lightBlue]Gandalf/c[white] heals himself for /c[lightgreen]{0}/c[white] hit points.", damage);
			}


			_dt = DateTime.Now;
		}
	}
}
