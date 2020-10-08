namespace Xamarin.CommunityToolkit.Sample.Models
{
	public readonly struct Language
	{
		public Language(string name, string ci)
		{
			Name = name;
			CI = ci;
		}

		public string Name { get; }

		public string CI { get; }
	}
}