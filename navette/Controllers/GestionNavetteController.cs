using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using navette.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Validation;

using Newtonsoft.Json;

namespace navette.Controllers
{
    [Route]
    public class GestionNavetteController : Controller
    {

        NavetteMANAGER_DBEntities DB = new NavetteMANAGER_DBEntities();
        // GET: GestionNavette

        [Route("")]
        public ActionResult Login()
        {
            return View();
        }


        [Route("")]
        [HttpPost]
        public ActionResult Login(UserSociete C)
        {
            if (Session["User"] != null)
            {
                return View();

            }
            if (!ModelState.IsValid)
            {
                return View();
            }
           var f_password = GetMD5(C.USER.Password);
           var data = (from s in DB.USER_APP where s.Email == C.USER.Email
                            && s.Password == f_password
                            select s).FirstOrDefault();
            
            if (data != null)
            {
                //add session
                Session["User"] = data;
                return RedirectToAction("Form");
            }
            else
            {
                //TempData = "Login failed";
                return RedirectToAction("Login");
            }

            

           
        }


        
         [Route("LoginSociete")]
         [HttpPost]
          public ActionResult LoginSociete(UserSociete C)
                {
                    if (Session["User"] != null)
                    {
                        return RedirectToAction("Login");

                    }
                    if (!ModelState.IsValid)
                    {
                        return View("~/Views/GestionNavette/Login.cshtml");
                    }

            var f_password = GetMD5(C.societe.password);
                    var data = (from s in DB.Societe where
                               s.UserName == C.societe.UserName
                               && s.password == C.societe.password
                               select s).FirstOrDefault();

                    if (data != null)
                    {
                        //add session
                        Session["User"] = data;
                        return RedirectToAction("AjouterAbonnement2");
                    }
                    else
                    {
                        ViewBag.error = "Login failed";
                        return RedirectToAction("Login");
                    }
                }





        [Route("LogOut")]
        [HttpPost]
        public ActionResult Logout()
        {
            Session["User"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;



            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");



            }
            return byte2String;
        }




        [Route("AfficherSuggestions")]
        public ActionResult AfficherSuggestions()
        {
            ViewBag.Suggestions = DB.suggestion;
            return View();
        }


        [Route("AjouterAbonnement2")]
        public ActionResult AjouterAbonnement2()

        {

            if (Session["User"] == null)
            {
                return RedirectToAction("Login");

            }
           
            Societe S = (Societe)Session["User"];

            
            var Autocars = DB.Autocar.Where(a => a.ID_Societe == (S.ID_Societe)).ToList();
            ViewBag.Autocars = new SelectList(Autocars, "ID_Autocar", "Matricule");
            ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");

            return View();

        }




        [Route("AjouterAbonnement2")]
        [HttpPost]
        public ActionResult AjouterAbonnement2(AboBus A)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");

            }
            Societe Se = (Societe)Session["User"];
            var Autocars = DB.Autocar.Where(a => a.ID_Societe == (Se.ID_Societe)).ToList();
            ViewBag.Autocars = new SelectList(Autocars, "ID_Autocar", "Matricule");
            ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");
            if (!ModelState.IsValid)
            {
                return View();
            }
           
            A.Abonnement.ID_Societe = Se.ID_Societe;
            TempData["Abonnement ajoutée"] = "Subscribtion added successfully";
            DB.Abonnement.Add(A.Abonnement);
            DB.SaveChanges();

            return View();

        }


        [Route("Form")]
  
        public ActionResult Form()

        {
            if (Session["User"] != null)
            {
                ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");
                return View();
            }
            return RedirectToAction("Login");
            

        }



        [Route("Form")]
        [HttpPost]
        public ActionResult Form(Demande d)
        {
            ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");

            if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    
                    //retouner une liste des abonnenment disponible sous forme d'une liste
                    var Abonnement = (from ab in DB.Abonnement
                                      where ab.Ville_Depart == d.Ville_Depart
                                      && ab.Ville_Arrive == d.Ville_Arrive
                                      && ab.inscrits < ab.Nombre_Passager
                                      && ab.Date_Fin > d.Date_Debut
                                      select ab).ToList();
                    if (Abonnement.Count() == 0)
                    {
                        TempData["Out of subscribtions"] = " There is no subscribtion with the same coordinates";
                        return RedirectToAction("Form");
                    }

                    ViewBag.ville_Depart = Abonnement[0].Ville.Nom_Ville;
                    ViewBag.ville_Arrive = Abonnement[0].Ville1.Nom_Ville;
                    ViewBag.date_choisi = d.Date_Debut;
                    ViewBag.Abonnement = Abonnement;
                    
                    
                    return View("~/Views/GestionNavette/ListAbonnement.cshtml");
                   

                }
                return View();
            } return RedirectToAction("Login");
        }
       [Route("detail/{id}")]
        public ActionResult Detail(int id)
        {
            if (Session["User"] != null) { 
            Abonnement Ab = DB.Abonnement.Find(id);
            Session["ID_Abo"] = id;
            ViewBag.abonnement = Ab;

            
            return View();
            }
            return RedirectToAction("Login");
            
        }


        [Route("Suggest")]

        public ActionResult Suggest()
        {
            if (Session["User"] != null)
            {
                ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");
                return View();
            }
            return RedirectToAction("Login");

          
        }
        [Route("Suggest")]
        [HttpPost]
        public ActionResult Suggest(suggestion S)
        {
            ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");


            if (Session["User"] == null)
            {
                return RedirectToAction("Login");

            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            DB.suggestion.Add(S);
            DB.SaveChanges();
           
            return RedirectToAction("Form");
        }

      
        
        [Route("Confirmation")]
        
        public ActionResult Confirmation()
        {
            if (Session["User"] != null)
            {
                USER_APP C = new USER_APP();
                C = (USER_APP)Session["User"];
                    

            Abo_Client AC = new Abo_Client();
            AC.Id_Abo = (int?)Session["ID_Abo"];
            AC.Id_Client = C.ID_USER;
               


                Abonnement Abo = (from ab in DB.Abonnement
                                  where ab.ID_Abonnement == AC.Id_Abo
                                  select ab).First();
                Abo.inscrits++;
                TempData["test"] = Abo.nombre_inscrit;



            DB.Abo_Client.Add(AC);
            DB.SaveChanges();
            TempData["Validation"] = " Votre demande est validée";


            return RedirectToAction("Form");
            }
            return RedirectToAction("Login");
        }

        
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(UserSociete US)
        {
            if (ModelState.IsValid)
            {

               


                    var check = DB.USER_APP.FirstOrDefault(s => s.Email == US.USER.Email);
                    if (check == null)
                    {
                        US.USER.Password = GetMD5(US.USER.Password);
                        DB.USER_APP.Add(US.USER);
                        DB.SaveChanges();
                        Session["User"] = check;
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.error = "Email already exists";
                        return View();
                    }
                


            }
            return View();
        }


        [Route("RegisterCompany")]
        public ActionResult RegisterCompany()
        {
            return View();
        }

        [Route("RegisterCompany")]
        [HttpPost]
        public ActionResult RegisterCompany(UserSociete US)
        {

            try
            {

                if (ModelState.IsValid)
                {


                    var check = DB.Societe.FirstOrDefault(SO => SO.UserName == US.societe.UserName);
                    if (check == null)
                    {
                        //US.societe.password = GetMD5(US.societe.password);
                        DB.Societe.Add(US.societe);
                        
                        DB.SaveChanges();
                        Session["User"] = check;
                        return View("~/Views/GestionNavette/Register.cshtml");
                    }
                    else
                    {
                        ViewBag.error = "Email already exists";
                        return View("~/Views/GestionNavette/Login.cshtml");
                    }
                }
                return   View("~/Views/GestionNavette/Login.cshtml");
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }





        }

        [Route("Tester")]
        public void tester()
        {

            List<Abonnement> Abonnements = (from a in DB.Abonnement
                                            where a.ID_Societe == 10
                                            //((Societe)Session["User"]).ID_Societe
                                            select a).ToList();
            //AboClient = > ID_Client and ID_Abonnement

            HashSet<int> Id_Clients = new HashSet<int>();
            foreach (var a in Abonnements)
            {
                foreach (var b in DB.Abo_Client)
                {

                    Id_Clients.Add((int)b.Id_Client);
                   
                }
            } 

           
            for(int i = 0; i<Id_Clients.Count(); i++)
            {
                    Response.Write(i);

            }



            
            //List<Abonnement> A = (from a in DB.Abonnement
            //                               where a.ID_Societe == 1
            //                               select a).ToList();

            //var json = JsonConvert.SerializeObject(A); 

            //Response.Write(json);

        }
       
        [Route("ListBus")]
        public ActionResult ListBus()
        {
            Societe S =  ((Societe)Session["User"]);
            var Autocars = (from a in DB.Autocar
                            where a.ID_Societe == S.ID_Societe
                            select a).ToList();
            ViewBag.Autocars = Autocars;

            return View();

        }
        [Route("AjouterAutocar")]
        [HttpPost]
        public ActionResult AjouterAutocar(AboBus T)
        {
            Societe S = (Societe)Session["User"];

            var Autocars = DB.Autocar.Where(a => a.ID_Societe == (S.ID_Societe)).ToList();
            ViewBag.Autocars = new SelectList(Autocars, "ID_Autocar", "Matricule");
            ViewBag.villes = new SelectList(DB.Ville, "ID_Ville", "Nom_Ville");
            if (ModelState.IsValid)
            {
               
            
            var autocars = (from a in DB.Autocar
                            where T.Autocar.Matricule == a.Matricule
                            select a).ToList();
            if(autocars.Count() == 0)
            {
                T.Autocar.ID_Societe = ((Societe)Session["User"]).ID_Societe;
                TempData["AutocarSuccess"] = "The bus is added successfully";
                DB.Autocar.Add(T.Autocar);
                DB.SaveChanges();
                return RedirectToAction("AjouterAbonnement2");

            }
            TempData["AutocarError"] = "This bus exists already";
            return RedirectToAction("AjouterAbonnement2");
            }

        return View("~/Views/GestionNavette/AjouterAbonnement2.cshtml");
        } 
        [Route("AjouterSociete")]
        public ActionResult AjouterSociete()
        {
            return View();
        }

        [Route("AbonnmentClient")]
        public ActionResult AbonnmentClient()
        {
            USER_APP U = ((USER_APP)Session["User"]);

            var abo_client = (from a in DB.Abo_Client
                       where a.Id_Client == U.ID_USER
                        select a).ToList();

            // table = |Id_Abo_Client|Id_client|Id_Abo|

            ViewBag.Abo_of_cli = abo_client;

            return View();
        }


      

       
    }

   
}