using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using MinimalApi.Endpoints;

namespace Endpoints.Tests.Fixtures;

public class OrderEndpointsFixture
{
    public OrderEndpointsFixture()
    {
        var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());


    }
}
