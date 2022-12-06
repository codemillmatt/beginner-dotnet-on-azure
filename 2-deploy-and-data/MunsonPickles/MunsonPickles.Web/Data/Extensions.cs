namespace MunsonPickles.Web.Data;

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var pickleContext = services.GetRequiredService<PickleDbContext>();
        
        pickleContext.Database.EnsureCreated();        

        DBInitializer.InitializeDatabase(pickleContext);        
    }
}
