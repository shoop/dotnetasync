using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DotNetAsync.Lib;
using System;

namespace DotNetAsync.Test;

public class AsyncWorkRunnerTest
{
    private static async Task AsyncWorkRunnerHarness(bool useExpressionBody)
    {
        // Arrange
        var mockaw = new Mock<IAsyncWork>();
        mockaw.Setup(aw => aw.AsyncWork(CancellationToken.None))
            .Returns(() => Task.CompletedTask);
        var doaw = new AsyncWorkRunner(mockaw.Object);

        // Act
        Func<Task> sut = useExpressionBody ?
            () => doaw.StartAsyncTaskExpressionBody(CancellationToken.None)
            : () => doaw.StartAsyncTaskMethod(CancellationToken.None);
        var exception = await Record.ExceptionAsync(sut);

        // Assert
        Assert.Null(exception);
        mockaw.Verify(aw => aw.AsyncWork(CancellationToken.None), Times.Once);
    }

    [Fact]
    public Task AsyncWorkRunsCorrectlyExpressionBody()
        => AsyncWorkRunnerHarness(true);

    [Fact]
    public Task AsyncWorkRunsCorrectlyMethod()
        => AsyncWorkRunnerHarness(false);
}
