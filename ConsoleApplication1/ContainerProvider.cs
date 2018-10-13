using System.Collections.Generic;
using ConsoleApplication1.Contracts;
using System.ComponentModel.Composition.Hosting;

namespace ConsoleApplication1
{
	internal static class ContainerProvider
	{
		private static CompositionContainer container;

		public static CompositionContainer Container
		{
			get
			{
				if (container == null)
				{
					List<AssemblyCatalog> catalogList = new List<AssemblyCatalog>();
					catalogList.Add(new AssemblyCatalog(typeof(ISorter).Assembly));
					container = new CompositionContainer(new AggregateCatalog(catalogList));
				}

				return container;
			}
		}
	}
}
