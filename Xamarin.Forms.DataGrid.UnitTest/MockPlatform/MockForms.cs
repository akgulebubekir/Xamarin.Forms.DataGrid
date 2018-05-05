namespace Xamarin.Forms.DataGrid.UnitTest.MockPlatform
{
	internal static class MockForms
	{
		internal static void Init()
		{
			Device.Info = new MockDeviceInfo();
			Device.PlatformServices = new MockPlatformServices();
			DependencyService.Register<MockResourcesProvider>();
			DependencyService.Register<MockDeserializer>();
		}
	}
}
