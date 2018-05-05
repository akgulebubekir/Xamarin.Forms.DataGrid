using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.DataGrid.UnitTest.MockPlatform
{
	/// <summary>
	/// Mock Deserializer class for Xamarin.Forms inorder to make unit test
	/// Source at: http://brianlagunas.com/mocking-and-unit-testing-the-xamarin-forms-application-class/
	/// </summary>
	internal class MockDeserializer : IDeserializer
	{
		public Task<IDictionary<string, object>> DeserializePropertiesAsync()
		{
			return Task.FromResult<IDictionary<string, object>>(new Dictionary<string, object>());
		}

		public Task SerializePropertiesAsync(IDictionary<string, object> properties)
		{
			return Task.FromResult(false);
		}
	}

}
