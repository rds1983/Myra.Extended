using Myra.Graphics2D.UI;
using System;

namespace Myra.Extended.Widgets
{
	public class LogView : EndlessContainer<Panel>
	{
		private const int LogMoveUpInMs = 300;

		private VerticalStackPanel _logStack;
		private DateTime? _logStarted;
		private int _moveHeight;

		public LogView()
		{
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
			ClipToBounds = true;

			InternalChild = new Panel();

			_logStack = new VerticalStackPanel();
			InternalChild.Widgets.Add(_logStack);
		}

		private int CalculateTotalHeight(int start = 0)
		{
			var totalHeight = 0;
			for (var i = start; i < _logStack.Widgets.Count; ++i)
			{
				var widget = _logStack.Widgets[i];
				totalHeight += widget.ActualBounds.Height;

				if (i < _logStack.Widgets.Count - 1)
				{
					totalHeight += _logStack.Spacing;
				}
			}

			return totalHeight;
		}

		public void Log(string message)
		{
			// Add to the end
			var textBlock = new Label
			{
				Text = message,
				Wrap = true
			};

			_logStack.Widgets.Add(textBlock);

			Desktop.UpdateLayout();

			// Recalculate total height
			var totalHeight = CalculateTotalHeight();

			if (totalHeight > ActualBounds.Height)
			{
				// Initiate log movement
				if (_logStarted == null)
				{
					_logStarted = DateTime.Now;
				}

				// Determine amount of log strings that needs to be removed
				var start = 1;
				while (start < _logStack.Widgets.Count && totalHeight > ActualBounds.Height)
				{
					totalHeight = CalculateTotalHeight(start);
					if (totalHeight <= ActualBounds.Height)
					{
						break;
					}

					++start;
				}

				// Calculate move height
				_moveHeight = 0;
				for (var i = 0; i < start; ++i)
				{
					var widget = _logStack.Widgets[i];

					_moveHeight += widget.ActualBounds.Height;

					if (i < start - 1)
					{
						_moveHeight += _logStack.Spacing;
					}
				}

			}

			ProcessLog();
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
				while (_logStack.Widgets.Count > 0 && CalculateTotalHeight() > ActualBounds.Height)
				{
					_logStack.Widgets.RemoveAt(0);
				}

				_logStack.Top = 0;
				_logStarted = null;
				return;
			}

			var y = -(int)(elapsed.TotalMilliseconds * _moveHeight / LogMoveUpInMs);
			_logStack.Top = y;
		}

		public override void InternalRender(RenderContext context)
		{
			base.InternalRender(context);

			ProcessLog();
		}
	}
}