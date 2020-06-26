using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace slnQF.Models
{

    public class accesos_user {

        public string IDPERMISO;
        public string IDACCESO;
    }
  
    public class ModelUser
    {
 
            public int UserID { get; set; }
            public string Nombre { get; set; }
            [Required(ErrorMessage = "Ingrese Usuario", AllowEmptyStrings = false)]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Ingrese Contraseña", AllowEmptyStrings = false)]
            [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            public string Password { get; set; }
      
    }
}