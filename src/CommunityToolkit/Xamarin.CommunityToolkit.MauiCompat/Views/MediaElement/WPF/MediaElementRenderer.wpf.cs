using System;using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Xamarin.CommunityToolkit.Core;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.WPF;
using Controls = System.Windows.Controls;
using ToolKitMediaElement = Xamarin.CommunityToolkit.UI.Views.MediaElement;
using ToolKitMediaElementRenderer = Xamarin.CommunityToolkit.UI.Views.MediaElementRenderer;

[assembly: ExportRenderer(typeof(ToolKitMediaElement), typeof(ToolKitMediaElementRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<ToolKitMediaElement, Controls.MediaElement>
	{
		protected virtual void ReleaseControl()
		{
			if (Control == null)
				return;

			Control.BufferingStarted -= ControlBufferingStarted;
			Control.BufferingEnded -= ControlBufferingEnded;
			Control.MediaOpened -= ControlMediaOpened;
			Control.MediaEnded -= ControlMediaEnded;
			Control.MediaFailed -= ControlMediaFailed;

			Element.SeekRequested -= SeekRequested;
			Element.StateRequested -= StateRequested;
			Element.PositionRequested -= PositionRequested;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			ReleaseControl();
		}

		protected override void OnElementChanged(Microsoft.Maui.Controls.Platform.ElementChangedEventArgs<ToolKitMediaElement> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
				ReleaseControl();

			if (e.NewElement != null)
			{
				SetNativeControl(new Controls.MediaElement());
				Control.LoadedBehavior = MediaState.Manual;
				Control.UnloadedBehavior = MediaState.Close;
				Control.HorizontalAlignment = HorizontalAlignment.Stretch;
				Control.VerticalAlignment = VerticalAlignment.Stretch;

				Control.Stretch = Element.Aspect.ToStretch();

				Element.SeekRequested += SeekRequested;
				Element.StateRequested += StateRequested;
				Element.PositionRequested += PositionRequested;
				Control.MediaOpened += ControlMediaOpened;
				Control.MediaEnded += ControlMediaEnded;
				Control.MediaFailed += ControlMediaFailed;

				UpdateSource();
			}
		}

		void PositionRequested(object? sender, EventArgs e)
		{
			if (Control != null)
				Controller.Position = Control.Position;
		}

		MediaElementState requestedState;

		IMediaElementController Controller => Element as IMediaElementController;

		void StateRequested(object? sender, StateRequested e)
		{
			if (Control != null)
			{
				switch (e.State)
				{
					case MediaElementState.Playing:
						Control.Play();
						break;

					case MediaElementState.Paused:
						if (Control.CanPause)
							Control.Pause();
						break;

					case MediaElementState.Stopped:
						Control.Stop();
						break;
				}

				Controller.Position = Control.Position;
			}
		}

		void SeekRequested(object? sender, SeekRequested e)
		{
			if (Control != null)
			{
				Control.Position = e.Position;
				Controller.Position = Control.Position;
				Controller.OnSeekCompleted();
			}
		}

		void ControlBufferingEnded(object sender, RoutedEventArgs e)
		{
			Controller.BufferingProgress = 1.0;
			if (Element.AutoPlay)
			{
				Controller.CurrentState = MediaElementState.Playing;
			}
			else
			{
				Controller.CurrentState = requestedState;
			}
		}

		void ControlBufferingStarted(object sender, RoutedEventArgs e)
		{
			Controller.BufferingProgress = 0.0;
			Controller.CurrentState = MediaElementState.Buffering;
		}

		void ControlMediaFailed(object? sender, ExceptionRoutedEventArgs e) => Controller?.OnMediaFailed();

		void ControlMediaEnded(object? sender, RoutedEventArgs e)
		{
			if (Element.IsLooping)
			{
				Control.Position = TimeSpan.Zero;
				Control.Play();
			}
			else
			{
				requestedState = MediaElementState.Stopped;
				Controller.CurrentState = MediaElementState.Stopped;
				Controller.OnMediaEnded();
			}

			Controller.Position = Control.Position;
		}

		void ControlMediaOpened(object? sender, RoutedEventArgs e)
		{
			Controller.Duration = Control.NaturalDuration.HasTimeSpan ? Control.NaturalDuration.TimeSpan : (TimeSpan?)null;
			Controller.VideoHeight = Control.NaturalVideoHeight;
			Controller.VideoWidth = Control.NaturalVideoWidth;
			Control.Volume = Element.Volume;
			Control.Stretch = Element.Aspect.ToStretch();

			Controller.OnMediaOpened();

			if (Element.AutoPlay)
			{
				Control.Play();
				requestedState = MediaElementState.Playing;
				Controller.CurrentState = MediaElementState.Playing;
			}
			else
			{
				Controller.CurrentState = requestedState;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ToolKitMediaElement.Aspect):
					Control.Stretch = Element.Aspect.ToStretch();
					break;

				case nameof(ToolKitMediaElement.KeepScreenOn):
					if (Element.KeepScreenOn)
					{
						if (Element.CurrentState == MediaElementState.Playing)
						{
							DisplayRequestActive();
						}
					}
					else
					{
						DisplayRequestRelease();
					}
					break;

				case nameof(ToolKitMediaElement.Source):
					UpdateSource();
					break;

				case nameof(ToolKitMediaElement.Volume):
					Control.Volume = Element.Volume;
					break;
			}

			base.OnElementPropertyChanged(sender, e);
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var b = base.GetDesiredSize(widthConstraint, heightConstraint);
			return new SizeRequest(new Forms.Size(Math.Min(120, b.Request.Width), Math.Min(80, b.Request.Height)), new Forms.Size(Math.Min(120, b.Minimum.Width), Math.Min(80, b.Minimum.Height)));
		}

		protected override void UpdateWidth()
		{
			Control.Width = Math.Max(0, Element.Width);
		}

		protected override void UpdateHeight()
		{
			Control.Height = Math.Max(0, Element.Height);
		}

		protected virtual void UpdateSource()
		{
			if (Element.Source is null)
			{
				Control.Stop();
				Control.Source = null;
				return;
			}

			if (Control.Clock != null)
				Control.Clock = null;
			if (Element?.Source != null)
			{
				if (Element.Source is UriMediaSource uriSource)
				{
					if (uriSource.Uri != null)
					{
						if (!uriSource.Uri.IsAbsoluteUri)
						{
							Control.Source = uriSource.Uri;
						}
						else if (uriSource.Uri.Scheme == "ms-appx")
						{
							Control.Source = new Uri(uriSource.Uri.ToString().Replace("ms-appx:///", string.Empty), UriKind.Relative);
						}
						else if (uriSource.Uri.Scheme == "ms-appdata")
						{
							var filePath = string.Empty;

							if (uriSource.Uri.LocalPath.StartsWith("/local"))
							{
								// WPF doesn't have the concept of an app package local folder so using My Documents as a placeholder
								filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), uriSource.Uri.LocalPath.Substring(7));
							}
							else if (uriSource.Uri.LocalPath.StartsWith("/temp"))
							{
								filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), uriSource.Uri.LocalPath.Substring(6));
							}
							else
							{
								throw new ArgumentException("Invalid Uri", "Source");
							}

							Control.Source = new Uri(filePath);
						}
						else if (uriSource.Uri.Scheme == "https")
						{
							throw new ArgumentException("WPF supports only HTTP remote sources and not the HTTPS URI scheme.", "Source");
						}
						else
						{
							Control.Source = uriSource.Uri;
						}
					}
				}
				else if (Element.Source is FileMediaSource fileSource)
					Control.Source = new Uri(fileSource.File ?? string.Empty);

				Controller.CurrentState = MediaElementState.Opening;

				if (Element.AutoPlay)
				{
					Control.Play();
					Controller.CurrentState = MediaElementState.Playing;
				}
			}
		}

		void DisplayRequestActive()
		{
			NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.DISPLAY_REQUIRED | NativeMethods.EXECUTION_STATE.CONTINUOUS);
		}

		void DisplayRequestRelease()
		{
			NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.CONTINUOUS);
		}

		static class NativeMethods
		{
			[DllImport("Kernel32", SetLastError = true)]
			internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

			internal enum EXECUTION_STATE : uint
			{
				/// <summary>
				/// Informs the system that the state being set should remain in effect until the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
				/// </summary>
				CONTINUOUS = 0x80000000,

				/// <summary>
				/// Forces the display to be on by resetting the display idle timer.
				/// </summary>
				DISPLAY_REQUIRED = 0x00000002,
			}
		}

		static System.Windows.Media.Stretch ToStretch(Aspect aspect)
		{
			switch (aspect)
			{
				case Aspect.Fill:
					return System.Windows.Media.Stretch.Fill;
				case Aspect.AspectFill:
					return System.Windows.Media.Stretch.UniformToFill;
				default:
				case Aspect.AspectFit:
					return System.Windows.Media.Stretch.Uniform;
			}
		}
	}
}