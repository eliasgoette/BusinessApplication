namespace BusinessApplication
{
    public interface IDbContextFactory
    {
        static abstract DbContextFactoryMethod Create();
    }
}
