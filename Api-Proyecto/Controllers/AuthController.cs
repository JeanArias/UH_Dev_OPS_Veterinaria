using Api_Proyecto.Data;
using Api_Proyecto.Dtos;
using Api_Proyecto.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        if (_context.Usuarios.Any(u => u.Username == dto.Username || u.Correo == dto.Correo))
            return BadRequest("Usuario o correo ya existe.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Contraseña);

        var user = new Usuario
        {
            Username = dto.Username,
            Correo = dto.Correo,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Contraseña = hashedPassword,
            FechaRegistro = DateTime.UtcNow,
            Estado = true
        };

        _context.Usuarios.Add(user);
        _context.SaveChanges();

        EnviarCorreo(dto.Correo, "Registro exitoso", $"Hola {dto.Nombre}, tu cuenta fue creada exitosamente.");

        return Ok("Usuario registrado.");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Usuarios.FirstOrDefault(u => u.Username == dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Contraseña, user.Contraseña))
            return Unauthorized("Credenciales incorrectas.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("UniversidadHispanoamericana2025SuperSegura!");


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("correo", user.Correo)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string jwt = tokenHandler.WriteToken(token);

        return Ok(new { token = jwt });
    }

    private void EnviarCorreo(string para, string asunto, string cuerpo)
    {
        var mail = new MailMessage();
        mail.To.Add(para);
        mail.Subject = asunto;
        mail.Body = cuerpo;
        mail.From = new MailAddress("Lover2023Cuc@gmail.com");

        using var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new System.Net.NetworkCredential("Lover2023Cuc@gmail.com", "glplmohnvmynoqsl"),
            EnableSsl = true
        };

        smtp.Send(mail);
    }
    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword(ForgotPasswordDto dto)
    {
        var user = _context.Usuarios.FirstOrDefault(u => u.Correo == dto.Correo);
        if (user == null)
            return NotFound("No existe usuario con ese correo.");

        // Genera token único
        string token = Guid.NewGuid().ToString();

        // Guarda el token en la BD
        var resetToken = new PasswordResetToken
        {
            UsuarioId = user.Id,
            Token = token,
            Expiration = DateTime.Now.AddHours(1), // 1 hora de validez
            Usado = false
        };
        _context.PasswordResetTokens.Add(resetToken);
        _context.SaveChanges();

        // Enviar correo con el enlace para recuperar la contraseña
        string resetLink = $"https://localhost:7084/Home/restablecerPassword?token={token}";
        EnviarCorreo(user.Correo, "Recupera tu contraseña", $"Haz click aquí para restablecer: {resetLink}");

        return Ok(new { message = "Correo de recuperación enviado." });
    }
    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var resetToken = _context.PasswordResetTokens
            .FirstOrDefault(t => t.Token == dto.Token && !t.Usado && t.Expiration > DateTime.Now);
        if (resetToken == null)
            return BadRequest(new { message = "Token inválido o expirado." });

        var user = _context.Usuarios.FirstOrDefault(u => u.Id == resetToken.UsuarioId);
        if (user == null) return NotFound(new { message = "Usuario no encontrado." });

        // Actualiza la contraseña
        user.Contraseña = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);
        resetToken.Usado = true;
        _context.SaveChanges();
        return Ok(new { message = "Contraseña actualizada exitosamente." });
    }
}
