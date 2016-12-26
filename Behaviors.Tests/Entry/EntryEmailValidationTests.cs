using System;
using FormsCommunityToolkit.Behaviors;
using NUnit.Framework;
using Xamarin.Forms;

namespace Behaviors.Tests
{
	[TestFixture]
	public class EntryEmailValidationTests
	{


		EntryEmailValidation behavior;
		Entry entry;
		Color textColor;

		[SetUp]
		public void Setup()
		{
			behavior = new EntryEmailValidation();
			behavior.TextColorInvalid = Color.Red;
			entry = new Entry();
			entry.TextColor = Color.Green;

			textColor = entry.TextColor;
			entry.Behaviors.Add(behavior);
		}

		[Test]
		public void ValidEmail()
		{
			var quote = "\"";
			var emails = new []
			{
				@"email@example.com",
				@"firstname.lastname@example.com",
				@"email@subdomain.example.com",
				@"firstname+lastname@example.com",
				@"email@123.123.123.123",
				@"email@[123.123.123.123]",
				$"{quote}email{quote}@example.com",
				@"1234567890@example.com",
				@"email@example-one.com",
				//@"_______@example.com",
				@"email@example.name",
				@"email@example.museum",
				@"email@example.co.jp",
				@"firstname-lastname@example.com"
			};
			foreach (var email in emails)
			{
				entry.Text = email;

				Assert.IsTrue(behavior.IsValid, $"Value should be valid: {email}, but is not.");

				Assert.AreEqual(entry.TextColor, textColor, "Color was changed, but shouldn't have.");
			}
		}


		[Test]
		public void InvalidEmail()
		{
			var emails = new[]
			{
				@"plainaddress",
				@"#@%^%#$@#$@#.com",
				@"@example.com",
				@"Joe Smith <email@example.com>",
				@"email.example.com",
				@"email@example@example.com",
				@".email@example.com",
				@"email.@example.com",
				@"email..email@example.com",
				@"あいうえお@example.com",
				@"email@example.com (Joe Smith)",
				@"email@example",
				@"email@-example.com",
				//@"email@example.web",
				//@"email@111.222.333.44444",
				@"email@example..com",
				@"Abc..123@example.com",
			};
			foreach (var email in emails)
			{
				entry.Text = email;

				Assert.IsFalse(behavior.IsValid, $"Value should be not valid: {email}, but is.");

				Assert.AreEqual(entry.TextColor, behavior.TextColorInvalid, "Color was not changed, but should have.");
			}
		}

		[Test]
		public void InvalidEmailOdd()
		{
			var emails = new[]
			{
				@"(),:;<>[\]@example.com",
				@"just""not""right@example.com",
				@"this\ is""really""not\allowed@example.com"
			};
			foreach (var email in emails)
			{
				entry.Text = email;

				Assert.IsFalse(behavior.IsValid, $"Value should be not valid: {email}, but is.");

				Assert.AreEqual(entry.TextColor, behavior.TextColorInvalid, "Color was not changed, but should have.");
			}
		}

		[Test]
		public void NoText()
		{
			entry.Text = string.Empty;

			Assert.IsFalse(behavior.IsValid, "Value should be not valid, but is.");

			Assert.AreEqual(entry.TextColor, behavior.TextColorInvalid, "Color was not changed, but should have.");
		}
	}
}
