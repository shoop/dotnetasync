namespace DotNetAsync.Lib;

public interface IAsyncWork
{
    Task AsyncWork(CancellationToken cancellationToken);
}

public class AsyncWorkRunner
{
    private readonly IAsyncWork _work;

    public AsyncWorkRunner(IAsyncWork work)
    {
        this._work = work;
    }

    public static Task HandleAsyncTaskError(Task faultedTask)
    {
        // Log something
        throw new Exception($"Fault in task: {faultedTask.Status}");
    }

    public Task StartAsyncTaskExpressionBody(CancellationToken cancellationToken)
        => Task.Factory.StartNew(() => _work.AsyncWork(cancellationToken))
                .ContinueWith(faultedTask => HandleAsyncTaskError(faultedTask),
                    TaskContinuationOptions.OnlyOnFaulted);

    public Task StartAsyncTaskMethod(CancellationToken cancellationToken)
    {
        var task = Task.Factory.StartNew(() => _work.AsyncWork(cancellationToken));
        task.ContinueWith(faultedTask => HandleAsyncTaskError(faultedTask),
                    TaskContinuationOptions.OnlyOnFaulted);
        return task;
    }
}
