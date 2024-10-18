namespace E_commerce_system.Data.Services
{
    using System;
    using System.Net;
    using System.Net.Mail;

    public class EmailService
    {
        string fromEmail;
        string fromPassword;

        public EmailService()
        {
            fromEmail = "pasanchamikara989@gmail.com";
            fromPassword = "vpzx vzai uusd sflb"; 
        }

        public void SendToCustomerEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Create a new MailMessage object
                MailMessage message = new MailMessage();

                // Set the sender's email address
                message.From = new MailAddress(fromEmail);


                toEmail = "pasanchamikara996@gmail.com";

                // Set the recipient's email address
                message.To.Add(toEmail);

                // Set the subject and body of the email
                message.Subject = subject;
                message.Body = body;

                // If the body contains HTML, set IsBodyHtml to true
                message.IsBodyHtml = true;

                // Create an SmtpClient to send the email
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587); // For Gmail

                // Set the credentials (email and app password)
                smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);

                // Enable SSL for secure connection
                smtpClient.EnableSsl = true;

                // Send the email
                smtpClient.Send(message);

                Console.WriteLine("Email sent successfully to " + toEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }

        public void SendAccountActivationEmail(string toEmail, string customerFirstName)
        {
            // Create the subject and body for the account activation email
            string subject = "Your Account is Now Active – Welcome to Trendify!";

            string body = $@"
                <html>
                <body>
                    <p>Dear <strong>{customerFirstName}</strong>,</p>
                    <p>We are excited to inform you that your account with <strong>Trendify</strong> is now active and ready to use!</p>
                    <p>You can now log in and start exploring our products and services. If you need any assistance or have any questions, feel free to reach out to our support team.</p>
                    <p>Thank you for choosing <strong>Trendify</strong>, and we look forward to serving you!</p>
                    <p>Best regards,<br />
                    The <strong>Trendify</strong> Team</p>
                </body>
                </html>";

            // Send the email
            SendToCustomerEmail(toEmail, subject, body);
        }
    }
}
