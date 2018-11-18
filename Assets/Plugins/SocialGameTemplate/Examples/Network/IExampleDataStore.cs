using System;
using UniRx;

namespace Sandbox.Network
{
	public interface IExampleDataStore
	{
		IObservable<ExampleEntity> Example(string id);
	}
}
