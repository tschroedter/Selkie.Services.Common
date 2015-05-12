# Selkie.Services.Common

The Selkie.Services.Common project creates a NuGet package. The package contains common classes used by Selkie services, e.g. ServiceConsole, ServiceMain, ServiceManager... 

Please, check the provided examples for more details.

# Examples:

TestService
```CS
[ProjectComponent(Lifestyle.Singleton)]
public class TestService : BaseService,
						   IService
{
	public TestService([NotNull] IBus bus,
					   [NotNull] ILogger logger,
					   [NotNull] ISelkieManagementClient client)
		: base(bus,
			   logger,
			   client,
			   "TestService")
	{
	}

	protected override void ServiceStart()
	{
		Logger.Info("ServiceStart()");
	}

	protected override void ServiceStop()
	{
		Logger.Info("ServiceStop()");
	}

	protected override void ServiceInitialize()
	{
		Logger.Info("ServiceInitialize()");
	}
}
```

The class which start the service: 
```CS
internal class Program
{
	private static void Main()
	{
		IWindsorContainer container = new WindsorContainer();
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		container.Install(FromAssembly.Instance(executingAssembly));

		ISelkieManagementClient client = container.Resolve <ISelkieManagementClient>();
		client.PurgeQueueForServiceAndMessage("Name",
											  "MessageName");

		IServiceConsole serviceConsole = container.Resolve <IServiceConsole>();
		serviceConsole.Start(false);

		container.Dispose();

		System.Console.ReadLine();
	}
}
```

The Installer class used to register everything in Castle Windsor:
```CS
public class Installer : BasicConsoleInstaller,
						 IWindsorInstaller
{
	public new void Install(IWindsorContainer container,
							IConfigurationStore store)
	{
		base.Install(container,
					 store);

		container.Register(Component.For <IService>()
									.ImplementedBy <TestService>()
									.LifeStyle.Transient);
	}

	protected override void InstallComponents(IWindsorContainer container,
											  IConfigurationStore store)
	{
		base.InstallComponents(container,
							   store);

		container.Register(Component.For <IService>()
									.ImplementedBy <TestService>()
									.LifeStyle.Transient);
	}
}
```

# Selkie
Selkie.Services.Common is part of the Selkie project which is based on Castle Windsor and EasyNetQ. The main goal of the Selkie project is to calculate and displays the shortest path for a boat travelling along survey lines from point A to B. The algorithm takes into account the minimum required turn circle of a vessel required to navigate from one line to another.

The project started as a little ant colony optimization application. Over time the application grew and was split up into different services which communicate via RabbitMQ. The whole project is used to try out TDD, BDD, DRY and SOLID.

### Selkie Projects

* [Selkie](https://github.com/tschroedter/Selkie)
* Selkie ACO
* [Selkie Common](https://github.com/tschroedter/Selkie.Common)
* [Selkie EasyNetQ](https://github.com/tschroedter/Selkie.EasyNetQ)
* [Selkie Geometry] (https://github.com/tschroedter/Selkie.Geometry)
* [Selkie NUnit Extensions](https://github.com/tschroedter/Selkie.NUnit.Extensions)
* Selkie Racetrack
* Selkie Services ACO
* [Selkie Services Common](https://github.com/tschroedter/Selkie.Services.Common)
* Selkie Services Lines
* Selkie Services Monitor
* Selkie Services Racetracks
* Selkie Web
* [Selkie Windsor](https://github.com/tschroedter/Selkie.Windsor)
* Selkie WPF
* [Selkie XUnit Extensions](https://github.com/tschroedter/Selkie.XUnit.Extensions)
