using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;

#nullable disable

namespace WholesaleAgentPortal.Migrations;

public partial class InitialWholesaleSchema
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.33");
        modelBuilder.UseIdentityColumns();
        InitialSchemaModel.ConfigureModel(modelBuilder);
    }
}
