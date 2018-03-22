using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace notomyk.Infrastructure
{
    public class ErrorMessage
    {
        public const string EmailNotConfirmedForResetingPassword =  "Nie możesz zresetowac hasła ponieważ nie aktywowałeś konta.";
        public const string EmailAlreadyConfirmed = "Konto zostało juz aktywowane.";
        public const string YouAreNotAdmin = "Nie jesteś zalogowany jako admin.";
        public const string GeneralError = "Podczas przetwarzania żądania wystąpił błąd. Informacje o nim zostały zapisane w naszych logach. Zapoznamy się z nim i postaramy się go naprawić.";
        public const string UserDoesntExist = "Nie ma takiego użytkownika.";
        public const string NewsNoDescription = "Podany link nie moze zostać poprawnie odczytany (błędny opis newsa). Być może linki z tej domeny: {0} nie są rozpoznane jako artykuły prasowe. Zostanie to odnotowane w naszych logach i sprawdzimy co było tego przyczyną.";
        public const string NewsNoTitle = "Podany link nie moze zostać poprawnie odczytany (błędny tytuł). Być może linki z tej domeny: {0} nie są rozpoznane jako artykuły prasowe. Zostanie to odnotowane w naszych logach i sprawdzimy co było tego przyczyną.";
        public const string NewsEmptyLink = "Podany link jest nieprawidłowy.";
        public const string NewsRemoveHasComment = "Nie możesz usunąc newsa ponieważ ktoś już go skomentował. Teraz może go usunąc tylko moderator.";
        public const string NewsHasBeenRemoved = "News który próbujesz dodać został zablokowany. Jeżeli uważasz, że spełnia kryteria i powinien zostać odblokowany - proszę skontaktuj się z nami.";
        public const string NewsAlreadyExist = "News który próbujesz dodać został już dodany przez kogoś innego. Zostaniesz do niego  przekierowany.";
        public const string UserNotFound = "Nie ma takiego użytkownika.";
        public const string AdminTableSaveFailed = "Nie udało się zapisać zmian.";
        public const string ValueIsNotInt = "Musisz podać wartość liczbową dla tych ustawień.";
        public const string ValueTrueOrFalse = "Musisz podać wartość 1 albo 0 dla tych ustawień.";
        public const string EmailIsTaken = "Email jest już używany przez kogoś innego.";
        public const string UserNameIsTaken = "Nazwa użytkownika jest juz używana przez kogos innego.";
        public const string LockedAccount = "Twoje konto zostało zablokowane na {0}.";
        public const string FieldsRequired = "Nie wypelniles wszystkich wymaganych pol.";
        public const string ItemDoesntExist = "Objekt o takim ID nie istnieje.";
        public const string FBnoemail = "Twoje konto Facebook nie posiada przypisanego adresu e-mail. Dlatego też logowanie po przez FB jest niemożliwe. Proszę wybierz inną opcję logowania.";
        public const string ExternalLoginEmailIsTaken = "Email, który próbujesz teraz użyć do zalogowania został już zarejestrowany w sposób standardowy. Jeżeli nie pamiętasz hasła - poproś o jego zresetowanie.";
        public const string ExternalLoginUserNameIsTaken = "Nazwa uzytkownika, któą próbujesz teraz użyć do zalogowania została już zarejestrowany w sposób standardowy. Jeżeli nie pamiętasz hasła - poproś o jego zresetowanie.";
    }

    
}