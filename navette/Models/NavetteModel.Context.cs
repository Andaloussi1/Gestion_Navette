﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace navette.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NavetteMANAGER_DBEntities : DbContext
    {
        public NavetteMANAGER_DBEntities()
            : base("name=NavetteMANAGER_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Abonnement> Abonnement { get; set; }
        public virtual DbSet<Autocar> Autocar { get; set; }
        public virtual DbSet<Demande> Demande { get; set; }
        public virtual DbSet<Societe> Societe { get; set; }
        public virtual DbSet<USER_APP> USER_APP { get; set; }
        public virtual DbSet<Ville> Ville { get; set; }
        public virtual DbSet<suggestion> suggestion { get; set; }
        public virtual DbSet<Abo_Client> Abo_Client { get; set; }
    }
}