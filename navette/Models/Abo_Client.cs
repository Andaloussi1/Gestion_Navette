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
    
    public partial class Abo_Client
    {
        public int Id_Abo_Clienta { get; set; }
        public Nullable<int> Id_Abo { get; set; }
        public Nullable<int> Id_Client { get; set; }
    
        public virtual Abonnement Abonnement { get; set; }
        public virtual USER_APP USER_APP { get; set; }
    }
}
