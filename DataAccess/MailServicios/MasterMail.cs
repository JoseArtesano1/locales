using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace DataAccess.MailServicios
{
  public abstract  class MasterMail
    {

        private SmtpClient smtpClient;
        protected string senderMail { get; set; }  //correo del remitente  quien envia
        protected string password { get; set; }    // contraseña del remitente
        protected string host { get; set; }
        protected int port { get; set; }
        protected bool ssl { get; set; }

        //PROTOCOLO
        protected void IniciarSmtpCliente()
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(senderMail, password);  //correo y pass
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;   //cifrado ssl (preguntar si tiene)

        }

        //ENVIO MENSAJES
        public void EnvioMail(string subject, string body, List<string> destinatarios)
        {
            var mailMensaje = new MailMessage();
            try
            {
                mailMensaje.From = new MailAddress(senderMail); //de quien lo envia
                foreach (string mail in destinatarios)
                {
                    mailMensaje.To.Add(mail);  //quien lo recibirá
                }
                mailMensaje.Subject = subject;
                mailMensaje.Body = body;
                mailMensaje.Priority = MailPriority.Normal;
                smtpClient.Send(mailMensaje);
            }
            catch(Exception ex)
            {
                
            }
            finally { mailMensaje.Dispose(); smtpClient.Dispose(); } //liberar recursos
        }
    }
}
