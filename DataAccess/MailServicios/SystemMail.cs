using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MailServicios
{
    class SystemMail:MasterMail
    {
        public SystemMail()
        {
            senderMail = "correoempresa";
            password = "";
            host = ""; //nombre del servidor smtp.gmail.com
            port =111;  //587
            ssl = true; //cifrado
            IniciarSmtpCliente();
        }
    }
}
