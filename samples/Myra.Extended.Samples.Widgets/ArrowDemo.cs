using Myra.Extended.Widgets;
using Myra.Graphics2D.UI;

namespace Myra.Extended.Samples.Widgets
{
	class ArrowDemo: DemoT<Arrow>
	{
		private readonly Arrow _arrow = new Arrow();

		public override Widget Widget => _arrow;
	}
}
