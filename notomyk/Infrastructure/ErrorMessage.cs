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
        public static string NewsNoDescription = "Podany link nie moze zostać poprawnie odczytany (błędny opis newsa). Być może linki z tej domeny: {0} nie są rozpoznane jako artykuły prasowe. Zostanie to odnotowane w naszych logach i sprawdzimy co było tego przyczyną.";
        public static string NewsNoTitle = "Podany link nie moze zostać poprawnie odczytany (błędny tytuł). Być może linki z tej domeny: {0} nie są rozpoznane jako artykuły prasowe. Zostanie to odnotowane w naszych logach i sprawdzimy co było tego przyczyną.";
        public static string NewsEmptyLink = "Podany link jest nieprawidłowy.";
        public static string NewsRemoveHasComment = "Nie możesz usunąc newsa ponieważ ktoś już go skomentował. Teraz może go usunąc tylko moderator.";
        public static string NewsHasBeenRemoved = "News który próbujesz dodać został zablokowany. Jeżeli uważasz, że spełnia kryteria i powinien zostać odblokowany - proszę skontaktuj się z nami.";
        public static string NewsAlreadyExist = "News który próbujesz dodać został już dodany przez kogoś innego. Zostaniesz do niego  przekierowany.";
        public static string UserNotFound = "Nie ma takiego użytkownika.";
        public static string AdminTableSaveFailed = "Nie udało się zapisać zmian.";
    }
}