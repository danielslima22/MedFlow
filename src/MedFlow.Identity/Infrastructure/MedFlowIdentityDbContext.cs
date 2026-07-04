using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedFlow.Identity.Infrastructure;

public class MedFlowIdentityDbContext(DbContextOptions<MedFlowIdentityDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
}
