using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using WholesaleAgentPortal.Data;

#nullable disable

namespace WholesaleAgentPortal.Migrations;

[DbContext(typeof(WholesaleDbContext))]
public partial class WholesaleDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.33");
        modelBuilder.UseIdentityColumns();
        InitialSchemaModel.ConfigureModel(modelBuilder);
    }
}
