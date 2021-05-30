using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using CommunityToolkit.Maui.Behaviors.Internals;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Effects;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.UI.Views;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UnitTests
{
	public class Namespace_Tests
	{
		[Test]
		public void MakeSureConvertersAreInTheRightNamespace()
		{
			var allTheTypes = Assembly.GetAssembly(typeof(InvertedBoolConverter))?.GetTypes() ?? throw new NullReferenceException();

			foreach (var type in allTheTypes.Where(t => t.Name.EndsWith("Converter") && t.GetInterface(nameof(IValueConverter)) != null))
			{
				Assert.IsTrue(type?.Namespace?.Equals("CommunityToolkit.Maui.Converters"),
					$"{type?.FullName} not in CommunityToolkit.Maui.Converters namespace");
			}
		}

		[Test]
		public void MakeSureEffectsAreInTheRightNamespace()
		{
			var allTheTypes = Assembly.GetAssembly(typeof(SafeAreaEffect))?.GetTypes() ?? throw new NullReferenceException();

			foreach (var type in allTheTypes.Where(t => t.Name.EndsWith("Effect") && t.IsClass && t.IsSealed && t.IsAbstract))
			{
				Assert.IsTrue(type?.Namespace?.Equals("CommunityToolkit.Maui.Effects"),
					$"{type?.FullName} not in CommunityToolkit.Maui.Effects namespace");
			}
		}

		[Test]
		public void MakeSureMarkupExtensionsAreInTheRightNamespace()
		{
			var allTheTypes = Assembly.GetAssembly(typeof(TranslateExtension))?.GetTypes() ?? throw new NullReferenceException();

			foreach (var type in allTheTypes.Where(t => t.Name.EndsWith("Extension") && t.GetInterface("IMarkupExtension") != null))
			{
				Assert.IsTrue(type?.Namespace?.Equals("CommunityToolkit.Maui.Extensions") is true ||
					type?.Namespace?.Equals("CommunityToolkit.Maui.Extensions.Internals") is true,
					$"{type?.FullName} not in nameof(CommunityToolkit.Maui.Extensions namespace");
			}
		}

		[Test]
		public void MakeSureBehaviorsAreInTheRightNamespace()
		{
			var allTheTypes = Assembly.GetAssembly(typeof(BaseBehavior<>))?.GetTypes() ?? throw new NullReferenceException();

			foreach (var type in allTheTypes.Where(t => t.Name.EndsWith("Behavior") && t.IsSubclassOf(typeof(BaseBehavior<>))))
			{
				Assert.IsTrue(type?.Namespace?.Equals("CommunityToolkit.Maui.Behaviors"),
					$"{type?.FullName} not in CommunityToolkit.Maui.Behaviors namespace");
			}
		}

		[Test]
		public void MakeSureViewsAreInTheRightNamespace()
		{
			var allTheTypes = Assembly.GetAssembly(typeof(AvatarView))?.GetTypes() ?? throw new NullReferenceException();

			foreach (var type in allTheTypes.Where(t => t.IsSubclassOf(typeof(View))))
			{
				Assert.IsTrue(type?.Namespace?.Equals("CommunityToolkit.Maui.UI.Views") is true ||
					type?.Namespace?.Equals("CommunityToolkit.Maui.UI.Views.Internals") is true,
					$"{type?.FullName} not in CommunityToolkit.Maui.UI.Views namespace");
			}
		}
	}
}