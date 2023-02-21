using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// This class is a camera view rotation helper based on the sample code provided by Microsoft:
	/// https://learn.microsoft.com/en-us/windows/uwp/audio-video-camera/handle-device-orientation-with-mediacapture#camerarotationhelper-full-code-listing
	/// </summary>
	public class CameraRotationHelper
	{
		readonly EnclosureLocation cameraEnclosureLocation;
		readonly DisplayInformation displayInformation;
		readonly SimpleOrientationSensor orientationSensor;
		readonly Func<Task> setPreviewRotationAsync;

		public CameraRotationHelper(
			EnclosureLocation cameraEnclosureLocation,
			DisplayInformation displayInformation,
			SimpleOrientationSensor orientationSensor,
			Func<Task> setPreviewRotationAsync)
		{
			this.cameraEnclosureLocation = cameraEnclosureLocation;
			this.displayInformation = displayInformation;
			this.orientationSensor = orientationSensor;
			this.setPreviewRotationAsync = setPreviewRotationAsync;

			displayInformation.OrientationChanged += DisplayInformation_OrientationChangedAsync;
		}

		async void DisplayInformation_OrientationChangedAsync(DisplayInformation sender, object args)
		{
			await setPreviewRotationAsync();
		}

		public static bool IsEnclosureLocationExternal(EnclosureLocation enclosureLocation)
		{
			return enclosureLocation == null || enclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown;
		}

		bool IsCameraMirrored()
		{
			// Front panel cameras are mirrored by default
			return cameraEnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front;
		}

		SimpleOrientation GetCameraOrientationRelativeToNativeOrientation()
		{
			// Get the rotation angle of the camera enclosure
			return ConvertClockwiseDegreesToSimpleOrientation((int)cameraEnclosureLocation.RotationAngleInDegreesClockwise);
		}

		// Gets the rotation to rotate ui elements
		public SimpleOrientation GetUIOrientation()
		{
			if (IsEnclosureLocationExternal(cameraEnclosureLocation))
			{
				// Cameras that are not attached to the device do not rotate along with it, so apply no rotation
				return SimpleOrientation.NotRotated;
			}

			// Return the difference between the orientation of the device and the orientation of the app display
			var deviceOrientation = orientationSensor.GetCurrentOrientation();
			var displayOrientation = ConvertDisplayOrientationToSimpleOrientation(displayInformation.CurrentOrientation);
			return SubOrientations(displayOrientation, deviceOrientation);
		}

		// Gets the rotation of the camera to rotate pictures/videos when saving to file
		public SimpleOrientation GetCameraCaptureOrientation()
		{
			if (IsEnclosureLocationExternal(cameraEnclosureLocation))
			{
				// Cameras that are not attached to the device do not rotate along with it, so apply no rotation
				return SimpleOrientation.NotRotated;
			}

			// Get the device orienation offset by the camera hardware offset
			var deviceOrientation = orientationSensor.GetCurrentOrientation();
			var result = SubOrientations(deviceOrientation, GetCameraOrientationRelativeToNativeOrientation());

			// If the preview is being mirrored for a front-facing camera, then the rotation should be inverted
			if (IsCameraMirrored())
			{
				result = MirrorOrientation(result);
			}
			return result;
		}

		// remove event handler
		public void RemoveEventHandler()
		{
			displayInformation.OrientationChanged -= DisplayInformation_OrientationChangedAsync;
		}

		// Gets the rotation of the camera to display the camera preview
		public SimpleOrientation GetCameraPreviewOrientation()
		{
			if (IsEnclosureLocationExternal(cameraEnclosureLocation))
			{
				// Cameras that are not attached to the device do not rotate along with it, so apply no rotation
				return SimpleOrientation.NotRotated;
			}

			// Get the app display rotation offset by the camera hardware offset
			var result = ConvertDisplayOrientationToSimpleOrientation(displayInformation.CurrentOrientation);
			result = SubOrientations(result, GetCameraOrientationRelativeToNativeOrientation());

			// If the preview is being mirrored for a front-facing camera, then the rotation should be inverted
			if (IsCameraMirrored())
			{
				result = MirrorOrientation(result);
			}
			return result;
		}

		// Converts the sensor device orientation into clockwise degrees
		public static int ConvertSimpleOrientationToClockwiseDegrees(SimpleOrientation? orientation)
		{
			switch (orientation)
			{
				case SimpleOrientation.Rotated90DegreesCounterclockwise:
					return 270;

				case SimpleOrientation.Rotated180DegreesCounterclockwise:
					return 180;

				case SimpleOrientation.Rotated270DegreesCounterclockwise:
					return 90;

				case SimpleOrientation.Faceup:
				case SimpleOrientation.Facedown:
				case SimpleOrientation.NotRotated:
				case null:
				default:
					return 0;
			}
		}

		// Converts the mponitor orientation into counter clockwise degrees
		SimpleOrientation ConvertDisplayOrientationToSimpleOrientation(DisplayOrientations orientation)
		{
			SimpleOrientation result;
			switch (orientation)
			{
				case DisplayOrientations.Landscape:
					result = SimpleOrientation.NotRotated;
					break;

				case DisplayOrientations.PortraitFlipped:
					result = SimpleOrientation.Rotated90DegreesCounterclockwise;
					break;

				case DisplayOrientations.LandscapeFlipped:
					result = SimpleOrientation.Rotated180DegreesCounterclockwise;
					break;

				case DisplayOrientations.Portrait:
				default:
					result = SimpleOrientation.Rotated270DegreesCounterclockwise;
					break;
			}

			// Above assumes landscape; offset is needed if native orientation is portrait
			if (displayInformation?.NativeOrientation == DisplayOrientations.Portrait)
			{
				result = AddOrientations(result, SimpleOrientation.Rotated90DegreesCounterclockwise);
			}

			return result;
		}

		// Mirror the orientation. This only affects the 90 and 270 degree cases, because rotating 0 and 180 degrees is the same clockwise and counter-clockwise
		static SimpleOrientation MirrorOrientation(SimpleOrientation orientation)
		{
			switch (orientation)
			{
				case SimpleOrientation.Rotated90DegreesCounterclockwise:
					return SimpleOrientation.Rotated270DegreesCounterclockwise;

				case SimpleOrientation.Rotated270DegreesCounterclockwise:
					return SimpleOrientation.Rotated90DegreesCounterclockwise;
			}
			return orientation;
		}

		static SimpleOrientation AddOrientations(SimpleOrientation a, SimpleOrientation b)
		{
			var aRot = ConvertSimpleOrientationToClockwiseDegrees(a);
			var bRot = ConvertSimpleOrientationToClockwiseDegrees(b);
			var result = (aRot + bRot) % 360;
			return ConvertClockwiseDegreesToSimpleOrientation(result);
		}

		SimpleOrientation SubOrientations(SimpleOrientation a, SimpleOrientation b)
		{
			var aRot = ConvertSimpleOrientationToClockwiseDegrees(a);
			var bRot = ConvertSimpleOrientationToClockwiseDegrees(b);

			// add 360 to ensure the modulus operator does not operate on a negative
			var result = (360 + (aRot - bRot)) % 360;
			return ConvertClockwiseDegreesToSimpleOrientation(result);
		}

		static SimpleOrientation ConvertClockwiseDegreesToSimpleOrientation(int orientation)
		{
			return orientation switch
			{
				270 => SimpleOrientation.Rotated90DegreesCounterclockwise,
				180 => SimpleOrientation.Rotated180DegreesCounterclockwise,
				90 => SimpleOrientation.Rotated270DegreesCounterclockwise,
				_ => SimpleOrientation.NotRotated,
			};
		}
	}
}