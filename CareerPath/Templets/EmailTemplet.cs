namespace CareerPath.Templets
{
    public static class EmailTemplet
    {
        public static string ConfirmEmailTemplate(string userName, string confirmLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='max-width:600px; margin:auto; padding:20px; border:1px solid #eee; border-radius:10px;'>
                        <h2 style='color:#0078D7;'>Confirm Your Email</h2>
                        <p>Hi <strong>{userName}</strong>,</p>
                        <p>Thank you for registering! Please confirm your email address by clicking the button below:</p>
                        <p style='text-align:center;'>
                            <a href='{confirmLink}' 
                               style='background-color:#0078D7; color:white; padding:10px 20px; 
                                      border-radius:5px; text-decoration:none; font-weight:bold;'>
                               Confirm Email
                            </a>
                        </p>
                        <p>If you did not create an account, you can safely ignore this email.</p>
                        <br/>
                        <p>Best regards,<br/>CareerPath Team</p>
                    </div>
                </body>
                </html>";
        }

        public static string ResetPasswordTemplate(string userName, string resetLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='max-width:600px; margin:auto; padding:20px; border:1px solid #eee; border-radius:10px;'>
                        <h2 style='color:#D9534F;'>Reset Your Password</h2>
                        <p>Hi <strong>{userName}</strong>,</p>
                        <p>We received a request to reset your password. You can reset it by clicking the link below:</p>
                        <p style='text-align:center;'>
                            <a href='{resetLink}' 
                               style='background-color:#D9534F; color:white; padding:10px 20px; 
                                      border-radius:5px; text-decoration:none; font-weight:bold;'>
                               Reset Password
                            </a>
                        </p>
                        <p>If you didn’t request this, just ignore this email.</p>
                        <br/>
                        <p>Best regards,<br/>CareerPath Team</p>
                    </div>
                </body>
                </html>";
        }

        public static string WelcomeEmailTemplate(string userName)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='max-width:600px; margin:auto; padding:20px; border:1px solid #eee; border-radius:10px;'>
                        <h2 style='color:#28A745;'>Welcome to CareerPath!</h2>
                        <p>Hi <strong>{userName}</strong>,</p>
                        <p>We’re excited to have you join us! Explore your dashboard and start building your career path today.</p>
                        <p>Good luck 🎯</p>
                        <br/>
                        <p>Best regards,<br/>CareerPath Team</p>
                    </div>
                </body>
                </html>";
        }
    }
}
