namespace ALib.Networking;



using System.Net.Mail;
using System.Net;
using System.Diagnostics;



public static class ALibGMail
{
    private static MailMessage message;
    private static string username; // for your own safety
    private static string password; // for your own safety



    static ALibGMail()
    {
        ALibGMail.message = new MailMessage();
        ALibGMail.username = "abelasnake";
        ALibGMail.password = "CSharpProject2024";
        ALibGMail.message.From = new MailAddress("neserbank@gmail.com");
    }



    public static string Username
    {
        set
        {
            ALibGMail.username = value;
        }
        get
        {
            return ALibGMail.username;
        }
    }
    public static string Password
    {
        set
        {
            ALibGMail.password = value;
        }
        get
        {
            return ALibGMail.password;
        }
    }



    public static bool SendEmail(string username, string password, string toEmail, string emailSubject, string[] emailBody, 
        out string details)
    {
        if(ALibGMail.username != username.Trim() || ALibGMail.password != password.Trim())
        {
            Debug.WriteLine("username or password is not correct!");
            details = "Username or password is not correct!";
            return false;
        }

        // Set up the email message
        try
        {
            ALibGMail.message.To.Add(new MailAddress(toEmail.Trim()));
        }
        catch (Exception)
        {
            details = $"The Gmail `{toEmail}` is invalid; Format Exception!";
            Debug.WriteLine("Gmail Format is invalid!");
            return false;
        }
        ALibGMail.message.Subject = emailSubject;
        string allEmailMessage = "";
        foreach(string e in emailBody)
        {
            allEmailMessage += e + "\n";
        }
        ALibGMail.message.Body = allEmailMessage;

        // Configure the SMTP client
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("neserbank@gmail.com", "wuqr ujfw hwyd dnwq");

        try
        {
            smtpClient.Send(ALibGMail.message); // Send the email
            Debug.WriteLine("CSharp Project: Email sent successfully!");
            details = "Email sent successfully!";
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("CSharp Project: Failed/Exception to send email!");
            details = "Neser Bank: unknown Exception that block the email to be sent!";
            return false;
        }
    }

    public static string GenerateVerificationCode(byte digit)
    {
        Random random = new Random();
        int randomNumber;
        string veriNum = "";

        for (byte i = 0; i < digit; i++)
        {
            randomNumber = random.Next(10);
            veriNum += randomNumber.ToString();
        }

        return veriNum;
    }
}