using FluentAssertions;
using PactNet;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using SnailRaceKata.Adapters.Fake;
using SnailRaceKata.Adapters.RaceResultProvider;
using Xunit.Abstractions;
using static PactNet.Matchers.Match;

namespace SnailRaceKata.Test.Adapters.RaceResultProvider;

/// <summary>
///     Consumer-driven contract test with Pact
///     Test both:
///     - The <see cref="RaceResultProviderHttp" /> (aka. the consumer)
///     through public behavior like <see cref="RaceResultProviderHttpTestWithStubServer" />
///     by defining the required contract with examples.
///     - The api contract required by the consumer by calling real API (aka. the provider)
///     with more control than <see cref="RaceResultProviderContractTest" />.
///     The two aspects can be separated and verified independently between teams
///     and automatically with a broker that verifies producer contracts.
///     This does not ensure that <see cref="RaceResultProviderFake" /> respect the contract.
/// </summary>
/// <remarks>
///     For developer experience,
///     the test use a <see cref="SnailRaceContainerServer" />
///     to remove requirement of launching test instance manually.
///     In real life, we do not have a container for a real API dependency.
/// </remarks>
public class RaceResultProviderHttpTestWithPact : IClassFixture<SnailRaceContainerServer>
{
    private const string ConsumerName = "SnailRaceConsumer";
    private const string ProviderName = "SnailRaceProvider";

    private readonly IPactBuilderV4 _pact;
    private readonly SnailRaceContainerServer _snailRaceServer;
    private readonly PactVerifierConfig _verifierConfig;

    public RaceResultProviderHttpTestWithPact(
        SnailRaceContainerServer snailRaceServer,
        ITestOutputHelper output)
    {
        _snailRaceServer = snailRaceServer;

        var xunitOutput = new XunitOutput(output);

        _verifierConfig = new PactVerifierConfig { Outputters = [xunitOutput] };
        var configuration = new PactConfig { Outputters = [xunitOutput], PactDir = SpecificationsDirectory() };

        _pact = Pact.V4(ConsumerName, ProviderName, configuration).WithHttpInteractions();

        DefineSnailRaceResultsConsumerContract(_pact);
    }

    private string SpecificationFilePath()
        => Path.Combine(
            SpecificationsDirectory(),
            $"{ConsumerName}-{ProviderName}.json");

    private string SpecificationsDirectory()
        => Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "RaceResultProvider",
            "RaceResultApiContractSpecifications");

    private static void DefineSnailRaceResultsConsumerContract(IPactBuilderV4 pact)
        => pact
            .UponReceiving("A GET request for snail race results")
            .WithRequest(HttpMethod.Get, "/results")
            .WillRespond()
            .WithStatus(200)
            .WithHeader("Content-Type", "application/json; charset=utf-8")
            .WithJsonBody(SnailRacesContract());

    private static object SnailRacesContract()
        => new
        {
            races = MinType(SnailRaceContract(), min: 1)
        };

    private static object SnailRaceContract()
        => new
        {
            raceId = Integer(641290),
            timestamp = Type(1744654103968L),
            snails = new[] // Require at least 3 snails
            {
                SnailContract(2, "Third", 3.3m),
                SnailContract(3, "Second", 2.2m),
                SnailContract(4, "First", 1.1m)
            }
        };

    private static object SnailContract(int number, string name, decimal duration)
        => new
        {
            number = Integer(number),
            name = Type(name),
            duration = Number(duration)
        };

    [Fact]
    public async Task Podium_should_be_composed_of_three_fastest_ordered_snails()
        => await _pact.VerifyAsync(async consumerContext =>
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = consumerContext.MockServerUri;
            var provider = new RaceResultProviderHttp(httpClient);

            // When
            var snailRaces = await provider.Races();

            // Then
            snailRaces.Races[0]
                .Podium.Should()
                .BeEquivalentTo(
                    new Domain.RaceResultProvider.Podium(
                        First: new Domain.RaceResultProvider.Snail(4, "First"),
                        Second: new Domain.RaceResultProvider.Snail(3, "Second"),
                        Third: new Domain.RaceResultProvider.Snail(2, "Third")));
        });

    [Fact]
    public void Ensure_snail_race_api_honours_contract_defined_by_consumer()
    {
        using var verifier = new PactVerifier(ProviderName, _verifierConfig);

        verifier
            .WithHttpEndpoint(_snailRaceServer.GetUrl())
            .WithFileSource(new FileInfo(SpecificationFilePath()))
            .Verify();
    }
}