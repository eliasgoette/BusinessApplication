namespace BusinessApplication
{
    public class AppDbContextFactory : IDbContextFactory
    {
        public static DbContextFactoryMethod Create()
        {
            return () => new AppDbContext();
        }
    }
}
