using System;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.DataGrid.UnitTest.MockPlatform
{
	/// <summary>
	/// Mock DeviceInfo class for Xamarin.Forms inorder to make unit test
	/// Source at: http://brianlagunas.com/mocking-and-unit-testing-the-xamarin-forms-application-class/
	/// </summary>
	internal class MockDeviceInfo : DeviceInfo
	{
		public override Size PixelScreenSize => throw new NotImplementedException();

		public override Size ScaledScreenSize => throw new NotImplementedException();

		public override double ScalingFactor => throw new NotImplementedException();
	}
}
