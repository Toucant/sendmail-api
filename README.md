#Send Mail Assignment - Connor Neary
## Project Setup
- Open the sendmail-api solution in Visual Studio
- **Modify the appsettings.json file with your email information**
- **Change the savePath variable located in SendMail/EmailService.cs**
- Launch the project (may require you to select your preferred browser)

## Postman Setup
- The app will be reachable at https://localhost:44340/
- Set the request type to POST
- **Change the Body section to type "form-data"**
- The three key-value pairs that are required are ToAddress, Subject, and Body

## Issues and Refinement
There were a few issues I have with the code I am submitting that don't sit well with me. The first which problem I ran into was with creating the appsettings.json file. It makes the most sense to set the file path for logging in this location however, storing paths in JSON doesn't work well and even if it did reading it could be an issue without C# knowing that it is a string literal. The second issue is the amount of bloat in the API/Frontend portion of the project. This was my first time creating a fresh MVC project and it came with a ton of boilerplate code that I had no use for. Additionally when I updated all of the package most of the styling libraries stopped working. If I were to spend more time on this project I think these two areas would be the first things I would look to improve upon.

## Project Information
You are being asked to build a working implement of a “client’s” feature request.
The client has a high volume application which must be able to send emails to customer.
It is critical that a user not be interrupted or delayed while navigating the website simply because an email fails to
send – i.e. other code must be able to call a mail routine without waiting for a result.

## Project Constraints
- Code should be written in C#.
- Send Email Method should be in a dll that can be reused throughout different applications and entry
points.
- Email sender, recipient, subject, and body (not attachments), and date must be logged/stored indefinitely
with status of send attempt.
- If email fails to send it should either be retried until success or a max of 3 times whichever comes first,
and can be sent in succession or over a period of time.
- Please store all credentials in an appsettings instead of hardcoded.
- At minimum that method/dll should be called from a console application.
- Extra Credit if attached to an API that can be called from Postman.
- EXTRA Credit if a front end (wpf/asp.net web application/etc...) calls the API to send the email.
- In any scenario you should be able to take in an input of a recipient email to send a test email.
