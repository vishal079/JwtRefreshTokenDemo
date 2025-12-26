namespace JwtRefreshTokenDemo.Services
{
    public class GetDataService
    {
        public async Task<string> ProcessAsync(
    CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(20000, cancellationToken);
            }

            return "Done";
        }

    }
}
