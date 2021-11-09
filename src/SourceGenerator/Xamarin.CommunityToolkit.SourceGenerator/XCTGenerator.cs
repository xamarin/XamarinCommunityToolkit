using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Xamarin.CommunityToolkit.SourceGenerator
{
	[Generator]
	public class XCTGenerator : ISourceGenerator
	{
		const string code = @"
namespace Xamarin.CommunityToolkit.Initializer
{
	sealed class XCTInitCaller
	{
		public void CallInit()
		{
			Xamarin.CommunityToolkit.Helpers.XCT.Init();
		}
	}
}";

		public void Execute(GeneratorExecutionContext context)
		{
			context.AddSource("InitCaller.g.cs", SourceText.From(code, Encoding.UTF8));
		}

		public void Initialize(GeneratorInitializationContext context)
		{
		}
	}
}
