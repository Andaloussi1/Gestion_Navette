//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Demande
    {
        public int ID_Demande { get; set; }
        public Nullable<System.DateTime> Date_Debut { get; set; }
        public Nullable<System.DateTime> Date_Fin { get; set; }
        public Nullable<System.TimeSpan> Heure_Depart { get; set; }
        public Nullable<System.TimeSpan> Heure_Arrive { get; set; }
        public Nullable<int> Nombre_Passager { get; set; }
        public Nullable<int> Ville_Depart { get; set; }
        public Nullable<int> Ville_Arrive { get; set; }
    
        public virtual Ville Ville { get; set; }
        public virtual Ville Ville1 { get; set; }
    }
}
