using Myra.Graphics2D.UI;

namespace Myra.Extended.Samples.Widgets
{
	abstract class Demo
	{
		public abstract string Name
		{
			get;
		}

		public abstract Widget Widget
		{
			get;
		}

		public virtual void OnSet()
		{
		}

		public virtual void OnUpdate()
		{
		}
	}

	abstract class DemoT<T> : Demo where T : Widget
	{
		public override string Name => typeof(T).Name;
	}
}