



namespace IBA_Alumni_.NetCore_.Data
{
    public class DataContext: DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring()


        public DbSet<User> Users => Set<User>();
    }
}
