using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSP.Models;

namespace TSP.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	    public DbSet<TSP.Models.Attendance> Attendance { get; set; } = default!;
	    public DbSet<TSP.Models.Batch> Batch { get; set; } = default!;
	    public DbSet<TSP.Models.Course> Course { get; set; } = default!;
	    public DbSet<TSP.Models.CourseContent> CourseContent { get; set; } = default!;
	    public DbSet<TSP.Models.CourseCoordinator> CourseCoordinator { get; set; } = default!;
	    public DbSet<TSP.Models.Exam> Exam { get; set; } = default!;
	    public DbSet<TSP.Models.Lab> Lab { get; set; } = default!;
	    public DbSet<TSP.Models.Payment> Payment { get; set; } = default!;
	    public DbSet<TSP.Models.Result> Result { get; set; } = default!;
	    public DbSet<TSP.Models.Schedule> Schedule { get; set; } = default!;
	    public DbSet<TSP.Models.Student> Student { get; set; } = default!;
	    public DbSet<TSP.Models.StudentBatch> StudentBatch { get; set; } = default!;
	    public DbSet<TSP.Models.Trainer> Trainer { get; set; } = default!;
	    public DbSet<TSP.Models.TrainerCourse> TrainerCourse { get; set; } = default!;
	}
}
