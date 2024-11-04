using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("Account")]
public class AccountController : Controller
{
    [HttpGet("GoogleLogin")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("signin-google")]
    public async Task<IActionResult> GoogleResponse()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (authenticateResult.Succeeded == false)
            return RedirectToAction("Index", "Home");

        // At this point, the user is authenticated and you have their Google tokens.
        // You can redirect to another page or store the tokens for further actions.

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> SendEmail()
    {
        // Get the access token from the user's authentication properties
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(accessToken))
            return RedirectToAction("GoogleLogin");

        // Set up the Gmail API client
        var credential = GoogleCredential.FromAccessToken(accessToken);
        var gmailService = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Your App Name"
        });

        // Create the email message
        var message = new Message
        {
            Raw = Base64UrlEncodeEmail("to@example.com", "from@example.com", "Subject", "Email body content")
        };

        // Send the email
        await gmailService.Users.Messages.Send(message, "me").ExecuteAsync();

        return Content("Email sent successfully!");
    }

    // Helper function to encode an email in Base64 format
    private static string Base64UrlEncodeEmail(string to, string from, string subject, string body)
    {
        var mimeMessage = $"To: {to}\r\n" +
                          $"From: {from}\r\n" +
                          $"Subject: {subject}\r\n\r\n" +
                          $"{body}";

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage))
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }
}
