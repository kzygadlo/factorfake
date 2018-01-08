using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class ErrorMessage
    {
        public static string EmailNotConfirmedForResetingPassword =  "Nie możesz zresetowac hasła ponieważ nie aktywowałeś konta.";
        public static string EmailAlreadyConfirmed = "Konto zostało juz aktywowane.";
        public static string YouAreNotAdmin = "Nie jesteś zalogowany jako admin.";
        public static string GeneralError = "Podczas przetwarzania żądania wystąpił błąd. Informacje o nim zostały zapisane w naszych logach. Zapoznamy się z nim i postaramy się go naprawić.";
        public static string UserDoesntExist = "Nie ma takiego użytkownika.";
    }
}