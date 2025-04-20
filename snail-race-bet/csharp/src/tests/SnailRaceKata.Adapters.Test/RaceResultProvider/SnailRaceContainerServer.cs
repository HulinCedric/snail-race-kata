using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

public class SnailRaceContainerServer : IAsyncLifetime
{
    private const ushort SnailRaceServerPort = 8000;

    private readonly IContainer _container = new ContainerBuilder()
        .WithImage("mathieucans/snail-race-server:latest")
        .WithPortBinding(SnailRaceServerPort, true)
        .Build();

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    public Uri GetUrl() => new($"http://localhost:{_container.GetMappedPublicPort(SnailRaceServerPort)}");
}