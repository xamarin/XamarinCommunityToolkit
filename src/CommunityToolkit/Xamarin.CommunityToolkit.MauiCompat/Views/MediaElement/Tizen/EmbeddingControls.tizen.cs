using System;using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using ShapesPath = Xamarin.Forms.Shapes.Path;
using ShapesPathGeometry = Xamarin.Forms.Shapes.PathGeometry;
using XLabel = Xamarin.Forms.Label;
using XTextAlignment = Microsoft.Maui.TextAlignment;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class EmbeddingControls : ContentView
	{
		public Microsoft.Maui.Controls.Compatibility.Grid PlayIcon { get; private set; }

		public Microsoft.Maui.Controls.Compatibility.Grid PauseIcon { get; private set; }

		public EmbeddingControls()
		{
			var iconTapCommand = new AsyncValueCommand(async () =>
			{
				if (BindingContext is not MediaPlayer player)
					throw new InvalidOperationException($"{nameof(BindingContext)} must be {nameof(MediaPlayer)}");

				if (player.State == PlaybackState.Playing)
				{
					player.Pause();
				}
				else
				{
					await player.Start();
				}
			});

			PlayIcon = new Microsoft.Maui.Controls.Compatibility.Grid
			{
				Children =
				{
					new ShapesPath
					{
						Scale = 0.7,
						Data = (ShapesPathGeometry)new Forms.Shapes.PathGeometryConverter().ConvertFromInvariantString("M93.5 52.4019C95.5 53.5566 95.5 56.4434 93.5 57.5981L5 108.694C3 109.848 0.499996 108.405 0.499996 106.096V3.9045C0.499996 1.5951 3 0.151723 5 1.30642L93.5 52.4019Z"),
						Fill = Brush.White,
						Opacity = 0.4,
						Aspect = Stretch.Uniform,
						HorizontalOptions = LayoutOptions.Center
					}
				}
			};

			PlayIcon.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = iconTapCommand
			});
			AbsoluteLayout.SetLayoutFlags(PlayIcon, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(PlayIcon, new Rectangle(0.5, 0.5, 0.25, 0.25));

			PauseIcon = new Microsoft.Maui.Controls.Compatibility.Grid
			{
				HorizontalOptions = LayoutOptions.Center,
				Children =
				{
					new ShapesPath
					{
						Scale = 0.7,
						Data = (ShapesPathGeometry)new Forms.Shapes.PathGeometryConverter().ConvertFromInvariantString("M1 1H36V131H1V1Z"),
						Fill = Brush.White,
						Opacity = 0.4,
						Aspect = Stretch.Uniform,
						HorizontalOptions = LayoutOptions.Start
					},
					new ShapesPath
					{
						Scale = 0.7,
						Data = (ShapesPathGeometry)new Forms.Shapes.PathGeometryConverter().ConvertFromInvariantString("M71 1H106V131H71V1Z"),
						Fill = Brush.White,
						Opacity = 0.4,
						Aspect = Stretch.Uniform,
						HorizontalOptions = LayoutOptions.Start
					}
				}
			};

			PauseIcon.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = iconTapCommand
			});
			AbsoluteLayout.SetLayoutFlags(PauseIcon, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(PauseIcon, new Rectangle(0.5, 0.5, 0.25, 0.25));

			var bufferingLabel = new XLabel
			{
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label), false),
				HorizontalTextAlignment = XTextAlignment.Center,
				TextColor = Color.FromHex("#eeeeeeee")
			};
			bufferingLabel.SetBinding(XLabel.TextProperty, new Binding
			{
				Path = "BufferingProgress",
				StringFormat = "{0:0%}"
			});
			bufferingLabel.SetBinding(IsVisibleProperty, new Binding
			{
				Path = "IsBuffering",
			});
			AbsoluteLayout.SetLayoutFlags(bufferingLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(bufferingLabel, new Rectangle(0.5, 0.5, 0.25, 0.25));

			var progressBoxView = new BoxView
			{
				Color = Color.FromHex($"#4286f4")
			};
			progressBoxView.SetBinding(AbsoluteLayout.LayoutBoundsProperty, new Binding
			{
				Path = "Progress",
				Converter = new ProgressToBoundTextConverter()
			});
			AbsoluteLayout.SetLayoutFlags(progressBoxView, AbsoluteLayoutFlags.All);

			var posLabel = new XLabel
			{
				Margin = new Thickness(10, 0, 0, 0),
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(XLabel)),
				HorizontalTextAlignment = XTextAlignment.Start
			};
			posLabel.SetBinding(XLabel.TextProperty, new Binding
			{
				Path = "Position",
				Converter = new MillisecondToTextConverter()
			});
			AbsoluteLayout.SetLayoutFlags(posLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(posLabel, new Rectangle(0, 0, 1, 1));

			var durationLabel = new XLabel
			{
				Margin = new Thickness(0, 0, 10, 0),
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(XLabel)),
				HorizontalTextAlignment = XTextAlignment.End
			};
			durationLabel.SetBinding(XLabel.TextProperty, new Binding
			{
				Path = "Duration",
				Converter = new MillisecondToTextConverter()
			});
			AbsoluteLayout.SetLayoutFlags(durationLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(durationLabel, new Rectangle(0, 0, 1, 1));

			var progressInnerLayout = new AbsoluteLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 23,
				BackgroundColor = Color.FromHex("#80000000"),
				Children =
				{
					progressBoxView,
					posLabel,
					durationLabel
				}
			};

			var progressLayout = new 	Microsoft.Maui.Controls.StackLayout
			{
				Children =
				{
					new 	Microsoft.Maui.Controls.StackLayout { VerticalOptions = LayoutOptions.FillAndExpand },
					new 	Microsoft.Maui.Controls.StackLayout
					{
						Margin = Device.Idiom == TargetIdiom.Watch ? new Thickness(80, 0, 80, 0) : 20,
						VerticalOptions = LayoutOptions.End,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = Color.FromHex("#50000000"),
						Children = { progressInnerLayout }
					}
				}
			};
			AbsoluteLayout.SetLayoutFlags(progressLayout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(progressLayout, new Rectangle(0, 0, 1, 1));

			Content = new AbsoluteLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children =
				{
					progressLayout,
					PlayIcon,
					PauseIcon,
					bufferingLabel
				}
			};
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext is not IMediaPlayer player)
				throw new InvalidOperationException($"{nameof(BindingContext)} must be {nameof(IMediaPlayer)}");

			player.PlaybackPaused += OnPlaybackStateChanged;
			player.PlaybackStarted += OnPlaybackStateChanged;
			player.PlaybackStopped += OnPlaybackStateChanged;
		}

		async void OnPlaybackStateChanged(object sender, EventArgs e)
		{
			if (BindingContext is not IMediaPlayer player)
				throw new InvalidOperationException($"{nameof(BindingContext)} must be {nameof(IMediaPlayer)}");

			if (player.State == PlaybackState.Playing)
			{
				await Task.WhenAll(PlayIcon.FadeTo(0, 100), PlayIcon.ScaleTo(3.0, 300));

				PlayIcon.IsVisible = false;
				PlayIcon.Scale = 1.0;
				PauseIcon.IsVisible = true;

				await PauseIcon.FadeTo(1, 50);
			}
			else
			{
				await Task.WhenAll(PauseIcon.FadeTo(0, 100), PauseIcon.ScaleTo(3.0, 300));

				PauseIcon.IsVisible = false;
				PauseIcon.Scale = 1.0;
				PlayIcon.IsVisible = true;

				await PlayIcon.FadeTo(1, 50);
			}
		}
	}

	class ProgressToBoundTextConverter : IValueConverter
	{
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			var progress = (double)(value ?? throw new ArgumentNullException(nameof(value)));

			if (double.IsNaN(progress))
			{
				progress = 0d;
			}

			return new Rectangle(0, 0, progress, 1);
		}

		public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			var rect = (Rectangle)(value ?? throw new ArgumentNullException(nameof(value)));
			return rect.Width;
		}
	}

	class MillisecondToTextConverter : IValueConverter
	{
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			var millisecond = (int)(value ?? throw new ArgumentNullException(nameof(value)));
			var second = (millisecond / 1000) % 60;
			var min = (millisecond / 1000 / 60) % 60;
			var hour = millisecond / 1000 / 60 / 60;

			if (hour > 0)
			{
				return string.Format("{0:d2}:{1:d2}:{2:d2}", hour, min, second);
			}
			else
			{
				return string.Format("{0:d2}:{1:d2}", min, second);
			}
		}

		public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotImplementedException();
	}
}