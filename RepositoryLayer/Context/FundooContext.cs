namespace RepositoryLayer.Context
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using RepositoryLayer.Entity;

    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<NoteEntity> Notes { get; set; }

        public DbSet<LabelEntity> Label { get; set; }

        public DbSet<CollaboratorEntity> Collaborator { get; set; }
    }
}
