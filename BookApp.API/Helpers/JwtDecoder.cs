using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

public class JwtDecoder {

  // public string CurrentUserId () {

  //   var identity = HttpContext.User.Identity as ClaimsIdentity;

  //   string currentUserId = "";
  //   if (identity != null) {
  //     IEnumerable<Claim> claims = identity.Claims;
  //     // or
  //     // var id = identity.FindFirst ("nameid").Value;

  //     var usernameClaim = claims.Where (x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault ().Value;
  //     // var usernameClaim = claims.Where (x => x.Type == ClaimTypes.).FirstOrDefault ();
  //   }

  //   return currentUserId;
  // }

  public static string Decode () {
    var jwtHandler = new JwtSecurityTokenHandler ();
    var jwtInput = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxNiIsInVuaXF1ZV9uYW1lIjoidGVvZG9yLXVybCIsIm5iZiI6MTU1ODYwMzk5NSwiZXhwIjoxNTU4NjkwMzk1LCJpYXQiOjE1NTg2MDM5OTV9.SyJdoiW7RFJoBcTdi9zriaRUBq4XMrk-KNlh7YUBuDAoyeAdY4xv62xggIUi1SzL2cseuBdiZhjEJgmtY7YWag";
    var outputText = "";
    var readableToken = jwtHandler.CanReadToken (jwtInput);

    if (readableToken != true) {
      outputText = "The token doesn't seem to be in a proper JWT format.";
    }
    if (readableToken == true) {
      var token = jwtHandler.ReadJwtToken (jwtInput);

      //Extract the headers of the JWT
      var headers = token.Header;
      var jwtHeader = "{";
      foreach (var h in headers) {
        jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
      }
      jwtHeader += "}";
      outputText = "Header:\r\n" + JToken.Parse (jwtHeader).ToString ();

      //Extract the payload of the JWT
      var claims = token.Claims;
      var jwtPayload = "{";
      foreach (Claim c in claims) {
        jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
      }

      jwtPayload += "}";
      outputText += "\r\nPayload:\r\n" + JToken.Parse (jwtPayload).ToString ();
    }

    return outputText;
  }
}