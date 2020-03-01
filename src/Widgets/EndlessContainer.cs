using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace Myra.Extended.Widgets
{
	public class EndlessContainer<T>: SingleItemContainer<T> where T : Widget
	{
		public override void Arrange()
		{
			base.Arrange();

			var bounds = ActualBounds;
			var availableSize = new Point(bounds.Width, bounds.Height);
			var measureSize = InternalChild.Measure(availableSize);

			bounds.Width = measureSize.X;
			bounds.Height = measureSize.Y;
			InternalChild.Layout(bounds);
		}
	}
}
