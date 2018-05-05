using Xamarin.Forms.Internals;

namespace Xamarin.Forms.DataGrid.UnitTest.MockPlatform
{
	/// <summary>
	/// Mock Ticker class for Xamarin.Forms inorder to make unit test
	/// Source at: http://brianlagunas.com/mocking-and-unit-testing-the-xamarin-forms-application-class/
	/// </summary>
	internal class MockTicker : Ticker
	{
		bool _enabled;

		protected override void EnableTimer()
		{
			_enabled = true;

			while (_enabled)
			{
				SendSignals(16);
			}
		}

		protected override void DisableTimer()
		{
			_enabled = false;
		}
	}

}
