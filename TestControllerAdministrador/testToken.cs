//using emeriTEA_back_end.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Linq;

//namespace TestControllerAdministrador
//{
//    [TestClass]
//    public class testToken
//    {
//        private TokenService _tokenService;

//        public testToken()
//        {
//            _tokenService = new TokenService(); // Asume que TokenService es la clase que contiene el método ExtractUserIdFromToken
//        }

//        [TestMethod]
//        public void ExtractUserIdFromToken_ValidToken_ReturnsUserId()
//        {
//            // Arrange
//            var context = new DefaultHttpContext();
//            context.Request.Cookies.Append("jwtToken", "testToken"); // Asume que "testToken" es un token válido

//            // Act
//            var result = _tokenService.ExtractUserIdFromToken(context);

//            // Assert
//            Assert.IsNotNull(result);
//            // Aquí puedes hacer más afirmaciones, como comprobar que el resultado es igual a un valor esperado.
//        }
//    }
//}
