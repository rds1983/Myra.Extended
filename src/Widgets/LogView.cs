using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;

namespace Myra.Extended.Widgets
{
	public class LogView : ScrollViewer
	{
		private VerticalStackPanel _logStack;
		private DateTime? _logStarted;
		private int _logPosition;

		public int LogMoveUpInMs
		{
			get; set;
		} = 300;

		public int MaximumStrings
		{
			get; set;
		} = 100;

		public LogView()
		{
			_logStack = new VerticalStackPanel
			{
				VerticalAlignment = VerticalAlignment.Bottom
			};


			Content = _logStack;
		}

		public void Log(string message)
		{
			var oldBounds = _logStack.Bounds;

			// Add to the end
			var textBlock = new Label
			{
				Text = message,
				Wrap = true
			};

			_logStack.Widgets.Add(textBlock);

			// Update sizes of all widgets including LogView
			Desktop.UpdateLayout();

			if (ScrollMaximum.Y == 0)
			{
				// We need to scroll from minus to zero
				var deltaY = oldBounds.Height - _logStack.Bounds.Height;
				ScrollPosition += new Point(0, deltaY);
			}

			// Initiate log movement
			_logStarted = DateTime.Now;
			_logPosition = ScrollPosition.Y;
		}

		public void LogFormat(string message, params object[] args)
		{
			string str;
			try
			{
				if (args != null && args.Length > 0)
				{
					str = string.Format(message, args);
				}
				else
				{
					str = message;
				}
			}
			catch (FormatException)
			{
				str = message;
			}

			Log(str);
		}

		public void ClearLog()
		{
			_logStarted = null;
			_logStack.Widgets.Clear();
		}

		private void ProcessLog()
		{
			if (_logStarted == null)
			{
				return;
			}

			var now = DateTime.Now;
			var elapsed = now - _logStarted.Value;

			if (elapsed.TotalMilliseconds >= LogMoveUpInMs)
			{
				while (_logStack.Widgets.Count > MaximumStrings)
				{
					_logStack.Widgets.RemoveAt(0);
				}

				Desktop.UpdateLayout();

				ScrollPosition = new Point(0, ScrollMaximum.Y);
				_logStarted = null;
				return;
			}

			var y = _logPosition + (int)(elapsed.TotalMilliseconds * (ScrollMaximum.Y - _logPosition) / LogMoveUpInMs);
			ScrollPosition = new Point(0, y);
		}

		public override void InternalRender(RenderContext context)
		{
			base.InternalRender(context);

			ProcessLog();
		}
	}
}